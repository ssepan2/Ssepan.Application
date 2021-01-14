using System;
using System.ComponentModel;
using System.Configuration;

namespace Ssepan.Application
{
    /// <summary>
    /// Interface for run-time Model.
    /// </summary>
    public interface IModel :
        IModelComponent//,
        //IDisposable,    
        //INotifyPropertyChanged,
        //IEquatable<IModel>
    {
        ///// <summary>
        ///// Support clients that do not handle databinding, but which can subscribe to PropertyChanged.
        ///// Value doesn't matter; setting value from controller Refresh fires PropertyChanged event that tells viewer(s) to apply changes
        ///// </summary>
        //Boolean IsChanged { get; set; }

        ///// <summary>
        ///// Support clients that do not handle databinding, but which can subscribe to PropertyChanged.
        ///// Additionally, while clients can handle PropertyChanged on individual properties, 
        /////  this is a general notification that the client may desire to do a Refresh.
        ///// </summary>
        //void Refresh();
    }
}
