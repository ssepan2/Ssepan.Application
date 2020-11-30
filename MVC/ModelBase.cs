using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using Ssepan.Utility;

namespace Ssepan.Application
{
    
    /// <summary>
    /// Base for run-time Model. 
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class ModelBase :
        IModel 
    {
        #region Declarations
        protected Boolean disposed;
        #endregion Declarations

        #region Constructors
        public ModelBase() 
        {
        }
        #endregion Constructors

        #region IDisposable
        ~ModelBase()
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
        //If property of IModel object changes, fire OnPropertyChanged, which notifies any subscribed observers by calling PropertyChanged.
        //Called by all 'set' statements in ISettings object properties.
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

            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                //throw;
            }
        }
        #endregion INotifyPropertyChanged 

        #region IEquatable<IModel>
        /// <summary>
        /// Compare property values of two specified Model objects.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual Boolean Equals(IModel other)
        {
            Boolean returnValue = default(Boolean);
            ModelBase otherModel = default(ModelBase);

            try
            {
                otherModel = other as ModelBase;

                if (this == otherModel)
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
        #endregion IEquatable<IModel>

        #region Properties

        private Boolean _IsChanged = default(Boolean);
        /// <summary>
        /// Used when binding is not available.
        /// Value doesn't matter; setting value from controller Refresh fires PropertyChanged event that tells viewer(s) to apply changes
        /// </summary>
        public Boolean IsChanged
        {
            get { return _IsChanged; }
            set
            {
                _IsChanged = value;
                OnPropertyChanged("IsChanged");
            }
        }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Support clients that do not handle databinding, but which can subscribe to PropertyChanged.
        /// Additionally, while clients can handle PropertyChanged on individual properties, 
        ///  this is a general notification that the client may desire to update bindings.
        /// </summary>
        public virtual void Refresh()
        {
            IsChanged = true;//Value doesn't matter; fire a changed event;
        }
        #endregion public Methods


    }
}
