using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Ssepan.Utility;

namespace Ssepan.Application.MVC
{
    /// <summary>
    /// Base for run-time Model. Use this to represent the top-most object in the model heirarchy
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class ModelBase :
        ModelComponentBase,
        IModel 
    {
        #region Declarations
        protected new Boolean disposed;
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

        #region IEquatable<IModelComponent>

        #endregion IEquatable<IModel>

        #region Properties

        #endregion Properties

        #region Methods

        #endregion public Methods


    }
}
