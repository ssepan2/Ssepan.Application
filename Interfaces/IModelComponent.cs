using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Ssepan.Application
{
    /// <summary>
    /// Interface for ModelComponent, which are implemented by either ModelBase or any complex property used in a descendent of ModelBase.
    /// </summary>
    public interface IModelComponent :
        IDisposable,
        INotifyPropertyChanged,
        IEquatable<IModelComponent>
    {
        #region Properties
        /// <summary>
        /// Support clients that do not handle databinding, but which can subscribe to PropertyChanged.
        /// Value doesn't matter; setting value from controller Refresh fires PropertyChanged event that tells viewer(s) to apply changes
        /// </summary>
        Boolean IsChanged { get; set; }
        #endregion Properties

        #region non-static methods

        /// <summary>
        /// Support clients that do not handle databinding, but which can subscribe to PropertyChanged.
        /// Additionally, while clients can handle PropertyChanged on individual properties, 
        ///  this is a general notification that the client may desire to do a Refresh.
        /// </summary>
        void Refresh();
        #endregion non-static methods

    }
}
