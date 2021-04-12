using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Ssepan.Utility;

namespace Ssepan.Application.MVC
{
	/// <summary>
    /// Base for the manager for the persisted Settings. 
	/// </summary>
    public class SettingsControllerBase 
    {
        #region Constructors
        static SettingsControllerBase()
        {
            try
            {
                AsStatic = new SettingsControllerBase();
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                throw ex;
            }
        }
        #endregion Constructors

        #region PropertyChanged handlers
        void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                ModelControllerBase.AsStatic.Refresh();

            }
            catch (Exception ex)
            {
                Log.Write(
                    ex,
                    System.Reflection.MethodBase.GetCurrentMethod(),
                    System.Diagnostics.EventLogEntryType.Error,
                        99);
            }
        }
        #endregion PropertyChanged handlers

        #region Properties
        private static SettingsControllerBase _AsStatic;
        public static SettingsControllerBase AsStatic
        {
            get { return _AsStatic; }
            set
            {
                _AsStatic = value;
            }
        }

        private ISettings _ModelSettings;
        public ISettings ModelSettings
        {
            get { return _ModelSettings; }
            set
            {
                if (_ModelSettings != null)
                {
                    _ModelSettings.PropertyChanged -= new PropertyChangedEventHandler(Settings_PropertyChanged);
                }
                _ModelSettings = value;
                if (_ModelSettings != null)
                {
                    _ModelSettings.PropertyChanged += new PropertyChangedEventHandler(Settings_PropertyChanged);
                }
            }
        }

        private String _Filename;
        public String Filename
        {
            get { return _Filename; }
            set { _Filename = value; }
        }

        private Boolean _SkipSettingsCheck;
        public Boolean SkipSettingsCheck
        {
            get { return _SkipSettingsCheck; }
            set { _SkipSettingsCheck = value; }
        }

        public Boolean Dirty
        {
            get 
            {
                //skip if form initializing
                if (!_SkipSettingsCheck)
                {
                    return ModelSettings.Dirty;
                }
                else
                {
                    return false;
                }
            }
            //set { _Dirty = value; }
        }
        #endregion Properties

        #region static methods
        /// <summary>
        /// Assign an instance of a sub-class to the base class static property.
        /// Equivalent to what happens in the static constructor of the sub class, but can be run any time.
        /// </summary>
        /// <param name="newBaseClass"></param>
        public static void NewBase(SettingsControllerBase newBaseClass)
        {
            AsStatic = newBaseClass;
        }

        /// <summary>
        /// New settings
        /// </summary>
        /// <param name="newSettings"></param>
        /// <returns></returns>
        public Boolean New(ISettings newSettings)
        {
            Boolean returnValue = default(Boolean);
            try
            {
                //create new object
                ModelSettings = newSettings;
                Filename = SettingsBase.FILE_NEW;

                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
            return returnValue;
        }

        /// <summary>
        /// Open settings.
        /// </summary>
        /// <returns></returns>
        public Boolean Open()
        {
            Boolean returnValue = default(Boolean);

            try
            {
                //read from XML file
                ModelSettings.Load(Filename);

                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }

            return returnValue;
        }

        /// <summary>
        /// Save settings.
        /// </summary>
        /// <returns></returns>
        public Boolean Save()
        {
            Boolean returnValue = default(Boolean);

            try
            {
                //write to XML file
                ModelSettings.Save(Filename);

                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
             
            return returnValue;
        }
        #endregion

    }
}
