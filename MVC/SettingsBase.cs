using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Microsoft.Win32;//registry/file association
using Ssepan.Application;
using Ssepan.Utility;
   
namespace Ssepan.Application
{
	/// <summary>
	/// Base for Settings which are persisted.
	/// </summary>
    [DataContract(IsReference=true)]
    [Serializable]
    public abstract class SettingsBase : 
        ISettings
    {
        #region Declarations
        protected Boolean disposed;

        private const String FILE_TYPE_EXTENSION = "xml"; 
        private const String FILE_TYPE_NAME = "settingsfile"; 
        private const String FILE_TYPE_DESCRIPTION = "Settings Files";

        public enum SerializationFormat
        {
            Xml,
            DataContract
        }
        #endregion Declarations

        #region Constructors
        public SettingsBase()
        {
        }
        #endregion Constructors
            
        #region IDisposable
        ~SettingsBase()
        {
            Dispose(false);
        }

        public virtual void Dispose()
        {
            // dispose of the managed and unmanaged resources
            Dispose(true);

            // tell the GC that the Finalize process no longer needs
            // to be run for this object.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposeManagedResources)
        {
            // process only if mananged and unmanaged resources have
            // not been disposed of.
            if (!disposed)
            {
                //Resources not disposed
                if (disposeManagedResources)
                {
                    // dispose managed resources
                    //if (_xxx != null)
                    //{
                    //    _xxx = null;
                    //}
                }
                // dispose unmanaged resources
                disposed = true;
            }
        }
        #endregion IDisposable

        #region INotifyPropertyChanged 
        //If property of ISettings object changes, fire OnPropertyChanged, which notifies any subscribed observers by calling PropertyChanged.
        //Called by all 'set' statements in ISettings object properties.
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(String propertyName)
        {
            try
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
#if debug
                    Log.Write(MethodBase.GetCurrentMethod().DeclaringType.Module.Name, MethodBase.GetCurrentMethod() + Log.FormatEntry(String.Format("PropertyChanged: {0}", propertyName), MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name), EventLogEntryType.Information);
#endif
                }

                //any property that can fire OnPropertyChanged can affect the value Dirty, which should be recalculated on demand.
                if (propertyName != "Dirty")
                {
                    OnPropertyChanged("Dirty");
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                //throw;
            }
        }
        #endregion INotifyPropertyChanged 

        #region IEquatable<ISettingsComponent>
        /// <summary>
        /// Compare property values of two specified Settings objects.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual Boolean Equals(/*ISettings*/ISettingsComponent other)
        {
            Boolean returnValue = default(Boolean);
            SettingsBase otherSettings = default(SettingsBase);

            try
            {
                otherSettings = other as SettingsBase;

                if (this == otherSettings)
                {
                    returnValue = true;
                }
                else
                {
                    if (false/*this.Xxx != otherSettings.Xxx*/)
                    {
                        returnValue = false;
                    }
                    else
                    {
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                throw;
            }

            return returnValue;
        }
        #endregion IEquatable<ISettingsComponent>

        #region Properties
        private static String _FileTypeExtension = FILE_TYPE_EXTENSION;
        [XmlIgnore]
        public static String FileTypeExtension
        {
            get { return _FileTypeExtension; }
            set 
            { 
                _FileTypeExtension = value;
                //OnPropertyChanged("FileTypeExtension");
            }
        }

        private static String _FileTypeName = FILE_TYPE_NAME;
        [XmlIgnore]
        public static String FileTypeName
        {
            get { return _FileTypeName; }
            set 
            { 
                _FileTypeName = value;
                //OnPropertyChanged("FileTypeName");
            }
        }

        private static String _FileTypeDescription = FILE_TYPE_DESCRIPTION;
        [XmlIgnore]
        public static String FileTypeDescription
        {
            get { return _FileTypeDescription; }
            set 
            { 
                _FileTypeDescription = value;
                //OnPropertyChanged("FileTypeDescription");
            }
        }

        private static SerializationFormat _SerializeAs = default(SerializationFormat);
        [XmlIgnore]
        public static SerializationFormat SerializeAs
        {
            get { return _SerializeAs; }
            set 
            { 
                _SerializeAs = value;
                //OnPropertyChanged("SerializeAs");
            }
        }

        [XmlIgnore]
        public virtual Boolean Dirty
        {
            get
            {
                Boolean returnValue = default(Boolean);

                try
                {
                    if (false/*_Xxx != __Xxx*/)
                    {
                        returnValue = true;
                    }
                    else
                    {
                        returnValue = false;
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                    throw;
                }

                return returnValue;
            }
        }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Copies property values from source working fields to detination working fields, then optionally syncs destination.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="sync"></param>
        public virtual void CopyTo(/*ISettings*/ISettingsComponent destination, Boolean sync)
        {
            ISettings destinationSettings = default(ISettings);

            try
            {
                destinationSettings = destination as ISettings;

                //destinationSettings.Xxx = this.Xxx;

                if (sync)
                {
                    destinationSettings.Sync();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                throw;
            }
        }

        /// <summary>
        /// Syncs property values by copying from working fields to reference fields.
        /// </summary>
        public virtual void Sync()
        {
            try
            {
                //__Xxx = _Xxx;

                //Note:where we have cloned collections; the collection comparison will never find the orignla items in the cloned collection if it is looking at identity vs content--SJS
                //if (Dirty)
                //{
                //    throw new ApplicationException("Sync failed.");
                //}
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                throw;
            }
        }

        /// <summary>
        /// Update child components (used as properties) to use the passed handler.
        /// </summary>
        public virtual void UpdateHandlers(){}
        #endregion Methods

    }
}
