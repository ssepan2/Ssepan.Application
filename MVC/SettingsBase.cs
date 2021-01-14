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
using Ssepan.Utility;
   
namespace Ssepan.Application
{
	/// <summary>
	/// Base for Settings which are persisted.
	/// </summary>
    [DataContract(IsReference=true)]
    [Serializable]
    public abstract class SettingsBase : 
        SettingsComponentBase,
        ISettings
    {
        #region Declarations
        protected new Boolean disposed;

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

        public new virtual void Dispose()
        {
            // dispose of the managed and unmanaged resources
            Dispose(true);

            // tell the GC that the Finalize process no longer needs
            // to be run for this object.
            GC.SuppressFinalize(this);
        }

        protected new virtual void Dispose(bool disposeManagedResources)
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
        #endregion INotifyPropertyChanged 

        #region IEquatable<ISettingsComponent>

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
        #endregion Properties

        #region Methods
        #endregion Methods

    }
}
