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
	/// Base for component of Settings which are persisted.
	/// </summary>
    [DataContract]
    [Serializable]
    public abstract class SettingsComponentBase : 
        ISettingsComponent
    {
        #region Declarations
        protected Boolean disposed;
        #endregion Declarations

        #region Constructors
        public SettingsComponentBase()
        {
        }
        #endregion Constructors
            
        #region IDisposable
        ~SettingsComponentBase()
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
        //If property of ISettingsComponent object changes, fire OnPropertyChanged, which notifies any subscribed observers by calling PropertyChanged.
        //Called by all 'set' statements in ISettingsComponent object properties.
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
        public virtual Boolean Equals(ISettingsComponent other)
        {
            Boolean returnValue = default(Boolean);
            SettingsComponentBase otherSettings = default(SettingsComponentBase);

            try
            {
                otherSettings = other as SettingsComponentBase;

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
        public virtual void CopyTo(ISettingsComponent destination, Boolean sync)
        {
            ISettingsComponent destinationSettings = default(ISettingsComponent);

            try
            {
                destinationSettings = destination as ISettingsComponent;

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

                if (Dirty)
                {
                    throw new ApplicationException("Sync failed.");
                }
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
