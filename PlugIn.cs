using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Ssepan.Utility;

namespace Ssepan.Application
{
    public class PlugIn
    {
        public const String PLUGIN_FILE_MASK = "*.dll";

        public static List<TInterface> GetInterfacesImplementedAtPath<TInterface>
        (
            String plugInPath,
            String plugInFilter,
            String[] ignoreCommonLibraries
        )
            where TInterface : class
        {
            List<TInterface> returnValue = default(List<TInterface>);
            String[] filepaths = default(String[]);
            IEnumerable<TInterface> matchingClasses = default(IEnumerable<TInterface>);
            try
            {
                returnValue = new List<TInterface>();

                filepaths = Directory.GetFiles(plugInPath, plugInFilter);
                foreach (String filepath in filepaths)
                {
                    //try-catch individual attempts, so one bad plugin does not prevent loading of good plugins
                    try
                    {
                        //Note: Trap references to common assembly before calling this method. 
                        // Calls to IsAssignableFrom will fail for valid implementation classes if previously called 
                        // with class containing interface *definition* and *base class*. (Or maybe its because its a 
                        // referenced class already loaded by host.)
                        if (!ignoreCommonLibraries.Contains(Path.GetFileNameWithoutExtension(filepath)))
                        {
                            Assembly assembly = GetAssembly(filepath);
                            matchingClasses = InstancesOf<TInterface>(assembly);
                            returnValue.AddRange(matchingClasses);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
            return returnValue;
        }

        /// <summary>
        /// 
        /// based on code by 'wrack' on http://www.neowin.net/forum/topic/1110005-c-better-more-efficient-way-to-load-dll-dynamically-get-interface-instance/page__pid__595224399#entry595224399
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static Assembly GetAssembly(String filepath)
        {
            Assembly returnValue = default(Assembly);

            try
            {
                if (Path.IsPathRooted(filepath)) //suggested by Aethec on http://www.neowin.net/forum/topic/1110005-c-better-more-efficient-way-to-load-dll-dynamically-get-interface-instance/page__pid__595224399#entry595224399
                {
                    returnValue = Assembly.LoadFrom(filepath);
                }
                else
                {
                    returnValue = Assembly.LoadFrom(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), filepath));
                }

                if (returnValue == null)
                {
                    throw new ApplicationException(String.Format("Unable to load assembly: {0}", filepath));
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }

            return returnValue;
        }

        /// <summary>
        /// Trap references to common assembly before calling this method.
        /// *based on* code by Matt Hamilton on http://madprops.org/about-matt-hamilton/
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TInterface> InstancesOf<TInterface>(Assembly assembly)
            where TInterface : class
        {
            IEnumerable<TInterface> returnValue = default(IEnumerable<TInterface>);
            try
            {
                Type interfaceType = typeof(TInterface);
                returnValue =
                    from exportedType in assembly.GetExportedTypes()
                    where
                    (
                        exportedType.IsClass
                        &&
                        !exportedType.IsInterface
                        &&
                        !exportedType.IsAbstract
                        &&
                        interfaceType.IsAssignableFrom(exportedType) //Note:will fail for valid implementation classes if previously called with class containing interface *definition* and *base class*. (Or maybe its because its a referenced class already loaded by host.) Trap references to common assembly before calling this method.
                        &&
                        exportedType.GetConstructor(Type.EmptyTypes) != null
                    )
                    select Activator.CreateInstance(exportedType) as TInterface;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
            return returnValue;
        }
    }
}
