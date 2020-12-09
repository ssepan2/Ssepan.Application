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
    /// <typeparam name="TIcon"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public class ViewModelController<TIcon, TViewModel>
        where TViewModel :
            class,
            IViewModel<TIcon>,
            new()
        where TIcon : class
    {
        #region Declarations
        #endregion Declarations

        #region Constructors
        #endregion Constructors

        #region Properties
        private static Dictionary<String, TViewModel> _ViewModel = new Dictionary<String, TViewModel>();
        /// <summary>
        /// Allow for named viewmodels of a given type.
        /// </summary>
        public static Dictionary<String, TViewModel> ViewModel
        {
            get { return _ViewModel; }
            set { _ViewModel = value; }//TODO:notify here as well?
        }        
        #endregion Properties

        #region Methods
        /// <summary>
        /// New settings
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static Boolean New
        (
            String viewName, 
            TViewModel viewModel
        )
        {
            Boolean returnValue = default(Boolean);
            try
            {
                //create new object
                ViewModel.Add(viewName, viewModel);

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
