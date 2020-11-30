using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Ssepan.Application
{
    /// <summary>
    /// Interface for SettingsComponent, which are implemented by either SettingsBase or any complex property used in a descendent of SettingsBase.
    /// </summary>
    public interface ISettingsComponent :
        IDisposable,
        INotifyPropertyChanged,
        IEquatable<ISettingsComponent>
    {
        #region Properties
        [XmlIgnore]
        Boolean Dirty
        {
            get;
        }
        #endregion Properties

        #region non-static methods
        /// <summary>
        /// Copies property values from source working fields to detination working fields, then optionally syncs destination.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="sync"></param>
        void CopyTo(ISettingsComponent destination, Boolean sync);

        /// <summary>
        /// Syncs property values by copying from working fields to reference fields.
        /// </summary>
        void Sync();

        /// <summary>
        /// Update child components (used as properties) to use the passed handler.
        /// </summary>
        void UpdateHandlers();
        #endregion non-static methods

    }
}
