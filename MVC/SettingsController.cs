using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Ssepan.Application;
using Ssepan.Utility;
using Ssepan.Io;

namespace Ssepan.Application
{
    /// <summary>
    /// Manager for the persisted Settings. 
    /// </summary>
    /// <typeparam name="TSettings"></typeparam>
    public  class SettingsController<TSettings> //static
        where TSettings :
            class,
            ISettings,
            new()
    {
        #region Declarations
        public const String FILE_NEW = "(new)";
        #endregion Declarations

        #region Constructors
        #endregion Constructors

        #region Properties
        private static TSettings _Settings = default(TSettings);
        public static TSettings Settings//TODO:notify here as well?
        {
            get { return _Settings; }
            set 
            {
                if (DefaultHandler != null)
                {
                    if (_Settings != null)
                    {
                        _Settings.PropertyChanged -= DefaultHandler;
                    }
                }

                _Settings = value;

                if (DefaultHandler != null)
                {
                    if (_Settings != null)
                    {
                        _Settings.PropertyChanged += DefaultHandler;
                    }
                }
            }
        }

        private static String _OldPathname = default(String);
        /// <summary>
        /// Previous value of Pathname
        /// Set when filename changes.  Not synchronized here, but client apps may override and sychronize if it suits them.
        /// </summary>
        public static String OldPathname
        {
            get { return _OldPathname; }
            set
            {
                _OldPathname = value;
            }
        }

        private static String _OldFilename = default(String);
        /// <summary>
        /// Previous value of Filename.
        /// Set when filename changes.  Not synchronized here, but client apps may override and sychronize if it suits them.
        /// </summary>
        public static String OldFilename
        {
            get { return _OldFilename; }
            set
            {
                _OldFilename = value;
            }
        }

        private static String _Filename = FILE_NEW; 
        /// <summary>
        /// Filename component of FilePath
        /// </summary>
        public static String Filename
        {
            get { return _Filename; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    //just clear property
                    OldFilename = _Filename;//remember previous name
                    _Filename = String.Empty;
                }
                else if (Path.GetDirectoryName(value) != String.Empty)
                {
                    //send to be split first
                    FilePath = value;
                }
                else
                {
                    //just set property to value
                    OldFilename = _Filename;//remember previous name
                    _Filename = value;
                }
            }
        }

        private static String _Pathname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).WithTrailingSeparator(); 
        /// <summary>
        /// Path component of FilePath
        /// </summary>
        public static String Pathname
        {
            get { return _Pathname; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    //just clear property
                    OldPathname = _Pathname;
                    _Pathname = value;
                }
                else if (Path.GetFileName(value) != String.Empty)
                {
                    //send to be split first
                    FilePath = value;
                }
                else
                {
                    //just set property to value
                    OldPathname = _Pathname;
                    _Pathname = value;
                }
            }
        }

        /// <summary>
        /// Combined value of Pathname and Filename
        /// </summary>
        public static String FilePath
        {
            get 
            { 
                //retrieve and combine
                return Path.Combine(Pathname, Filename); 
            }
            set
            {
                //split and store
                Filename = Path.GetFileName(value);
                if (!Path.GetDirectoryName(value).EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    //add separator or Pathname setter will think path still contains filename
                    Pathname = Path.GetDirectoryName(value).WithTrailingSeparator();
                }
            }
        }
        
        private static PropertyChangedEventHandler _DefaultHandler = default(PropertyChangedEventHandler);
        /// <summary>
        /// Handler to assigned to Settings; triggered on New, Open.
        /// </summary>
        public static PropertyChangedEventHandler DefaultHandler
        {
            get { return _DefaultHandler; }
            set 
            {
                if (DefaultHandler != null)
                {
                    if (Settings != null)
                    {
                        Settings.PropertyChanged -= DefaultHandler;
                    }
                }

                _DefaultHandler = value;

                if (DefaultHandler != null)
                {
                    if (Settings != null)
                    {
                        Settings.PropertyChanged += DefaultHandler;
                    }
                }
            }
        }
        #endregion Properties

        #region Methods
        /// <summary>
        /// New settings
        /// </summary>
        /// <returns></returns>
        public static Boolean New()
        {
            Boolean returnValue = default(Boolean);
            try
            {//DEBUG:why is settings controller calling new before defaulthandler is set?
                //create new object
                Settings = new TSettings();
                Settings.Sync();
                Filename = FILE_NEW;

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
        public static Boolean Open()
        {
            Boolean returnValue = default(Boolean);

            try
            {
                //read from file
                switch (SettingsBase.SerializeAs)
                //switch (Settings.SerializeAs)
                {
                    case SettingsBase.SerializationFormat.DataContract:
                        {
                            Settings = SettingsController<TSettings>.LoadDataContract(FilePath);

                            break;
                        }
                    case SettingsBase.SerializationFormat.Xml:
                    default:
                        {
                            Settings = SettingsController<TSettings>.LoadXml(FilePath);

                            break;
                        }
                }

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
        public static Boolean Save()
        {
            Boolean returnValue = default(Boolean);

            try
            {
                //write to file
                switch (SettingsBase.SerializeAs)
                //switch (Settings.SerializeAs)
                {
                    case SettingsBase.SerializationFormat.DataContract:
                        {
                            SettingsController<TSettings>.PersistDataContract(Settings, FilePath);

                            break;
                        }
                    case SettingsBase.SerializationFormat.Xml:
                    default:
                        {
                            SettingsController<TSettings>.PersistXml(Settings, FilePath);

                            break;
                        }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
             
            return returnValue;
        }

        /// <summary>
        /// Loads the specified object with data from the specified file, using XML Serializer.
        /// </summary>
        /// <param name="filePath"></param>
        public static TSettings LoadXml(String filePath)
        {
            TSettings returnValue = default(TSettings);
            Type returnValueType = default(Type);

            try
            {
                //XML Serializer of type Settings
                returnValueType = typeof(TSettings);//returnValue.GetType();
                XmlSerializer xs = new XmlSerializer(returnValueType);

                //Stream reader for file
                StreamReader sr = new StreamReader(filePath);

                //de-serialize into Settings
                returnValue = (TSettings)xs.Deserialize(sr);
                returnValue.Sync();

                //close file
                sr.Close();
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw;
            }
            return returnValue;
        }

        /// <summary>
        /// Saves the specified object's data to the specified file, using XML Serializer.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="filePath"></param>
        public static void PersistXml(TSettings settings, String filePath)
        {
            try
            {
                //XML Serializer of type Settings
                XmlSerializer xs = new XmlSerializer(settings.GetType());

                //Stream writer for file
                StreamWriter sw = new StreamWriter(filePath);

                //serialize out of Settings
                xs.Serialize(sw, settings);

                //close file
                sw.Close();

                settings.Sync();
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw;
            }
        }

        /// <summary>
        /// Loads the specified object with data from the specified file, using DataContract Serializer.
        /// </summary>
        /// <param name="filePath"></param>
        public static TSettings LoadDataContract(String filePath)
        {
            TSettings returnValue = default(TSettings);
            Type returnValueType = default(Type);


            try
            {
                //DataContract Serializer of type Settings
                returnValueType = typeof(TSettings);//returnValue.GetType();
                DataContractSerializer xs = new DataContractSerializer(returnValueType, null, Int32.MaxValue, false, true /* preserve object refs */, null);

                //Stream writer for file
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    //de-serialize into Settings
                    returnValue = (TSettings)xs.ReadObject(fs);
                }

                returnValue.Sync();
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw;
            }
            return returnValue;
        }

        /// <summary>
        /// Saves the specified object's data to the specified file, using DataContract Serializer.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="filePath"></param>
        public static void PersistDataContract(TSettings settings, String filePath)
        {
            try
            {
                //DataContract Serializer of type Settings
                DataContractSerializer xs = new DataContractSerializer(settings.GetType(), null, int.MaxValue, false, true /* preserve object refs */, null);

                //Stream writer for file
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    //serialize out of Settings
                    xs.WriteObject(fs, settings);
                }

                settings.Sync();
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw;
            }
        }

        /// <summary>
        /// Display property grid dialog. Changes to properties are reflected immediately.
        /// </summary>
        public static void ShowProperties(Action refreshDelegate)
        {
            try
            {
                //PropertiesViewer pv = new PropertiesViewer(SettingsBase.AsStatic, Refresh);
                PropertyDialog pv = new PropertyDialog(Settings, refreshDelegate);
#if debug
                //pv.Show();//dialog properties grid validation does refresh
#else
                pv.ShowDialog();//dialog properties grid validation does refresh
                pv.Dispose();
#endif
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);


                throw;
            }
        }
        #endregion Methods
    }
}
