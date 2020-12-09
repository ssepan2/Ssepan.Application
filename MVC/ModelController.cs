using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Ssepan.Application;
using Ssepan.Utility;

namespace Ssepan.Application
{
    /// <summary>
    /// Manager for the run-time model. 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class ModelController<TModel>
        where TModel :
            class,
            IModel,
            new()
    {
        #region Declarations
        protected static Boolean _ValueChanging; //used by controller methods that could trigger notifications and refresh while processing
        protected static Boolean _NoUiOnThisThread = default(Boolean); //use with controller operations run from non-UI thread
        #endregion Declarations

        #region Constructors
        #endregion Constructors

        #region Properties
        private static TModel _Model = default(TModel);
        public static TModel Model
        {
            get { return _Model; }
            set { _Model = value; }//TODO:notify here as well?
        }
        #endregion Properties

        #region Methods
        /// <summary>
        /// New settings
        /// </summary>
        /// <returns></returns>
        public static Boolean New()
        {
            Boolean returnValue = default(Boolean);
            try
            {
                //create new object
                Model = new TModel();

                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
            return returnValue;
        }
        #endregion Methods
    }
}
