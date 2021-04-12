using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Ssepan.Utility;

namespace Ssepan.Application.MVC
{
    /// <summary>
    /// Base type for components of run-time Model.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class ModelComponentBase : 
        IModelComponent
    {
        #region Declarations
        protected Boolean disposed;
        #endregion Declarations

        #region Constructors
        public ModelComponentBase()
        {
        }
        #endregion Constructors
            
        #region IDisposable
        ~ModelComponentBase()
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
        //If property of IModelComponent object changes, fire OnPropertyChanged, which notifies any subscribed observers by calling PropertyChanged.
        //Called by all 'set' statements in IModelComponent object properties.
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

        #region IEquatable<IModelComponent>
        /// <summary>
        /// Compare property values of two specified Model objects.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual Boolean Equals(IModelComponent other)
        {
            Boolean returnValue = default(Boolean);
            ModelComponentBase otherModel = default(ModelComponentBase);

            try
            {
                otherModel = other as ModelComponentBase;

                if (this == otherModel)
                {
                    returnValue = true;
                }
                else
                {
                    if (false/*this.Xxx != otherModel.Xxx*/)
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
        #endregion IEquatable<IModelComponent>

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
        #endregion Methods

    }
}
