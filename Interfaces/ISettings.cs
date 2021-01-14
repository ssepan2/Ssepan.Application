using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
    
namespace Ssepan.Application
{
	/// <summary>
    /// Interface for Settings which are implemented by a descendent of SettingsBase.
	/// </summary>
    public interface ISettings :
        ISettingsComponent
        //IDisposable,    
        //INotifyPropertyChanged,
        //IEquatable<ISettings>
    {
        #region Properties
        //[XmlIgnore]
        //Boolean Dirty
        //{
        //    get;
        //}

        //[XmlIgnore]
        //SettingsBase.SerializationFormat SerializeAs
        //{
        //    get;
        //    set;
        //}
        #endregion Properties

        #region non-static methods
        ///// <summary>
        ///// Copies property values from source working fields to detination working fields, then optionally syncs destination.
        ///// </summary>
        ///// <param name="destination"></param>
        ///// <param name="sync"></param>
        //void CopyTo(ISettings destination, Boolean sync);

        ///// <summary>
        ///// Syncs property values by copying from working fields to reference fields.
        ///// </summary>
        //void Sync();
        #endregion non-static methods

    }
}
