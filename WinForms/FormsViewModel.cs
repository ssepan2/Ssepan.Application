using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Ssepan.Utility;
using Ssepan.Application;
using Ssepan.Application.MVC;
using Ssepan.Application.WinForms;
using Ssepan.Io;

namespace Ssepan.Application.WinForms
{
    /// <summary>
    /// Note: this class can be subclassed without type parameters in the client.
    /// </summary>
    /// <typeparam name="TIcon"></typeparam>
    /// <typeparam name="TSettings"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TView"></typeparam>
    public class FormsViewModel
    <
        TIcon, 
        TSettings, 
        TModel,
        TView
    > :
        ViewModelBase<TIcon>
        where TIcon : class
        where TSettings : class, ISettings, new()
        where TModel : class, IModel, new()
        where TView : Form
    {
        #region Declarations
        //public delegate Boolean DoWork_WorkDelegate(BackgroundWorker worker, DoWorkEventArgs e, ref String errorMessage);
        public delegate TReturn DoWork_WorkDelegate<TReturn>(BackgroundWorker worker, DoWorkEventArgs e, ref String errorMessage);

        protected static FileDialogInfo _settingsFileDialogInfo = default(FileDialogInfo);
                       
        protected Dictionary<String, TIcon> _actionIconImages = default(Dictionary<String, TIcon>);

        #region Commands
        //public ICommand FileNewCommand { get; private set; }
        //public ICommand FileOpenCommand { get; private set; }
        //public ICommand FileSaveCommand { get; private set; }
        //public ICommand FileSaveAsCommand { get; private set; }
        //public ICommand FilePrintCommand { get; private set; }
        //public ICommand FileExitCommand { get; private set; }
        //public ICommand EditCopyToClipboardCommand { get; private set; }
        //public ICommand EditPropertiesCommand { get; private set; }
        //public ICommand HelpAboutCommand { get; private set; }
        #endregion Commands
        #endregion Declarations

        #region Constructors
        public FormsViewModel()
        {
            if (SettingsController<TSettings>.Settings == null)
            {
                SettingsController<TSettings>.New();
            }

            #region Commands
            //ActionIconWinformsImage = 
            #endregion Commands
        }

        public FormsViewModel
        (
            PropertyChangedEventHandler propertyChangedEventHandlerDelegate,
            Dictionary<String, TIcon> actionIconImages,
            FileDialogInfo settingsFileDialogInfo
        ) :
            this()
        {
            try
            {
                //(and the delegate it contains
                if (propertyChangedEventHandlerDelegate != default(PropertyChangedEventHandler))
                {
                    this.PropertyChanged += new PropertyChangedEventHandler(propertyChangedEventHandlerDelegate);
                }

                _actionIconImages = actionIconImages;

                _settingsFileDialogInfo = settingsFileDialogInfo;

                ActionIconImage = _actionIconImages["Save"];
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
        }

        public FormsViewModel
        (
            PropertyChangedEventHandler propertyChangedEventHandlerDelegate,
            Dictionary<String, TIcon> actionIconImages,
            FileDialogInfo settingsFileDialogInfo,
            TView view //= default(TView) //(In VB 2010, )VB caller cannot differentiate between members which differ only by an optional param--SJS
        ) :
            this(propertyChangedEventHandlerDelegate, actionIconImages, settingsFileDialogInfo)
        {
            try
            {
                View = view;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
        }
        #endregion Constructors

        #region Properties
        private TView _View = default(TView);
        public TView View 
        {
	        get { return _View; } 
	        set { _View = value; } 
        }
        #endregion Properties

        #region Methods
        #region Menus
        #region menu feature boilerplate
        //may be used as-is, or overridden to handle differently

        public virtual void FileNew()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar("New...", null, _actionIconImages["New"], true, 33);

                if (SettingsController<TSettings>.Settings.Dirty)
                {
                    //prompt before saving
                    DialogResult messageBoxResult = MessageBox.Show("Save changes?", SettingsController<TSettings>.FilePath, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    switch (messageBoxResult)
                    {
                        case DialogResult.Yes:
                            {
                                //SAVE
                                FileSaveAs();

                                break;
                            }
                        case DialogResult.No:
                            {
                                break;
                            }
                        default:
                            {
                                throw new InvalidEnumArgumentException();
                            }
                    }
                }

                //NEW
                if
                (
                    !SettingsController<TSettings>.New()
                )
                {
                    throw new ApplicationException(String.Format("Unable to get New settings.\r\nPath: {0}", SettingsController<TSettings>.FilePath));
                }

                ModelController<TModel>.Model.Refresh();

                StopProgressBar("New completed.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar("", String.Format("{0}", ex.Message));
            }
        }

        /// <summary>
        /// Open object at SettingsController<TSettings>.FilePath.
        /// </summary>
        /// <param name="forceDialog">If false, just use SettingsController<TSettings>.FilePath</param>
        public virtual void FileOpen(Boolean forceDialog = true)
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Opening...",
                    null,
                    _actionIconImages["Open"], 
                    true,
                    33
                );

                if (SettingsController<TSettings>.Settings.Dirty)
                {
                    //prompt before saving
                    DialogResult messageBoxResult = System.Windows.Forms.MessageBox.Show("Save changes?", SettingsController<TSettings>.FilePath, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    switch (messageBoxResult)
                    {
                        case DialogResult.Yes:
                            {
                                //SAVE
                                FileSave();

                                break;
                            }
                        case DialogResult.No:
                            {
                                break;
                            }
                        default:
                            {
                                throw new InvalidEnumArgumentException();
                            }
                    }
                }

                if (forceDialog)
                {
                    _settingsFileDialogInfo.Filename = SettingsController<TSettings>.FilePath;
                    if (FileDialogInfo.GetPathForLoad(_settingsFileDialogInfo))
                    {
                        SettingsController<TSettings>.FilePath = _settingsFileDialogInfo.Filename;
                    }
                    else
                    {
                        StopProgressBar("Open cancelled.");
                        return; //if open was cancelled
                    }
                }

                //OPEN
                if (!SettingsController<TSettings>.Open())
                {
                    throw new ApplicationException(String.Format("Unable to Open settings.\r\nPath: {0}", SettingsController<TSettings>.FilePath));
                }

                ModelController<TModel>.Model.Refresh();

                StopProgressBar("Opened.");

            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("{0}", ex.Message));
            }
        }

        public virtual void FileSave()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Saving...",
                    null,
                    _actionIconImages["Save"], 
                    true,
                    33
                );

                _settingsFileDialogInfo.Filename = SettingsController<TSettings>.FilePath;
                if (FileDialogInfo.GetPathForSave(_settingsFileDialogInfo, false))
                {
                    SettingsController<TSettings>.FilePath = _settingsFileDialogInfo.Filename;

                    //SAVE
                    if (!SettingsController<TSettings>.Save())
                    {
                        throw new ApplicationException(String.Format("Unable to Save settings.\r\nPath: {0}", SettingsController<TSettings>.FilePath));
                    }

                    ModelController<TModel>.Model.Refresh();
                
                    StopProgressBar("Saved completed.");
                }
                else
                {
                    StopProgressBar("Save cancelled.");
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("{0}", ex.Message));
            }
        }

        public virtual void FileSaveAs()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Saving As...",
                    null,
                    _actionIconImages["Save"], 
                    true,
                    33
                );

                _settingsFileDialogInfo.Filename = SettingsController<TSettings>.FilePath;
                if (FileDialogInfo.GetPathForSave(_settingsFileDialogInfo))
                {
                    SettingsController<TSettings>.FilePath = _settingsFileDialogInfo.Filename;

                    //SAVE
                    if (!SettingsController<TSettings>.Save())
                    {
                        throw new ApplicationException(String.Format("Unable to Save settings.\r\nPath: {0}", SettingsController<TSettings>.FilePath));
                    }

                    ModelController<TModel>.Model.Refresh();
                    
                    StopProgressBar("Save As completed.");
                }
                else
                {
                    StopProgressBar("Save As cancelled.");
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("{0}", ex.Message));
            }
        }

        public virtual void FilePrint()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Printing...",
                    null,
                    _actionIconImages["Print"], 
                    true,
                    33
                );

                if (Print())
                {
                    StopProgressBar("Printed.");
                }
                else
                {
                    StopProgressBar("Print cancelled.");
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("Print failed: '{0}'", ex.Message));
            }
        }

        public virtual void FileExit()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                //Note: form will handle final cleanup

                //this.Close();
                System.Windows.Forms.Application.OpenForms[0].Close();//App.Current.MainWindow.Close();
                System.Windows.Forms.Application.Exit();

                StatusMessage = String.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = String.Format("{0}", ex.Message);

                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
        }

        public virtual void EditUndo()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Undoing...",
                    null,
                    _actionIconImages["Undo"],
                    true,
                    33
                );

                if (!Undo())
                {
                    throw new ApplicationException("'Undo' error");
                }

                StopProgressBar("Undo completed.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("Undo failed: {0}", ex.Message));
            }
        }

        public virtual void EditRedo()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Redoing...",
                    null,
                    _actionIconImages["Redo"],
                    true,
                    33
                );

                if (!Redo())
                {
                    throw new ApplicationException("'Redo' error");
                }

                StopProgressBar("Redo completed.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("Redo failed: {0}", ex.Message));
            }
        }

        public virtual void EditSelectAll()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Selecting All...",
                    null,
                    null,//_actionIconImages["SelectAll"],
                    true,
                    33
                );

                if (!SelectAll())
                {
                    throw new ApplicationException("'Select All' error");
                }

                StopProgressBar("Select All completed.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("Select All failed: {0}", ex.Message));
            }
        }

        public virtual void EditCut()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Cutting...",
                    null,
                    _actionIconImages["Cut"],
                    true,
                    33
                );

                if (!Cut())
                {
                    throw new ApplicationException("'Cut' error");
                }

                StopProgressBar("Cut completed.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("Cut failed: {0}", ex.Message));
            }
        }

        public virtual void EditCopy()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Copying...",
                    null,
                    _actionIconImages["Copy"],
                    true,
                    33
                );

                if (!Copy())
                {
                    throw new ApplicationException("'Copy' error");
                }

                StopProgressBar("Copied.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("Copy failed: {0}", ex.Message));
            }
        }

        public virtual void EditPaste()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Pasting...",
                    null,
                    _actionIconImages["Paste"],
                    true,
                    33
                );

                if (!Paste())
                {
                    throw new ApplicationException("'Paste' error");
                }

                StopProgressBar("Pasted.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("Paste failed: {0}", ex.Message));
            }
        }

        public virtual void EditDelete()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Deleting...",
                    null,
                    _actionIconImages["Delete"],
                    true,
                    33
                );

                if (!Delete())
                {
                    throw new ApplicationException("'Delete' error");
                }

                StopProgressBar("Deleted.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("Delete failed: {0}", ex.Message));
            }
        }

        public virtual void EditFind()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Finding...",
                    null,
                    _actionIconImages["Find"],
                    true,
                    33
                );

                if (!Find())
                {
                    throw new ApplicationException("'Find' error");
                }

                StopProgressBar("Find completed.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("Find failed: {0}", ex.Message));
            }
        }

        public virtual void EditReplace()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Replacing...",
                    null,
                    _actionIconImages["Replace"],
                    true,
                    33
                );

                if (!Replace())
                {
                    throw new ApplicationException("'Replace' error");
                }

                StopProgressBar("Replace completed.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("Replace failed: {0}", ex.Message));
            }
        }

        public virtual void EditRefresh()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Refreshing...",
                    null,
                    _actionIconImages["Refresh"],
                    true,
                    33
                );

                if (!Refresh())
                {
                    throw new ApplicationException("'Refresh' error");
                }

                StopProgressBar("Refreshed.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("Refresh failed: {0}", ex.Message));
            }
        }

        public virtual void EditPreferences()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Preferences...",
                    null,
                    _actionIconImages["Preferences"],
                    true,
                    33
                );

                if (!Preferences())
                {
                    throw new ApplicationException("'Preferences' error");
                }

                StopProgressBar("Preferences completed.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("Preferences failed: {0}", ex.Message));
            }
        }

        public virtual void EditProperties()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Edit Properties...",
                    null,
                    _actionIconImages["Properties"],
                    true,
                    33
                );

                //Note:pass model, not settings, or changes will not trigger buliness logic in model properties
                PropertyDialog pv = new PropertyDialog(ModelController<TModel>.Model/*SettingsController<TSettings>.Settings*/, ModelController<TModel>.Model.Refresh);
                pv.Owner = System.Windows.Forms.Application.OpenForms[0];//App.Current.MainWindow;
                pv.ShowDialog();

                StopProgressBar("Edit Properties closed.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("{0}", ex.Message));
            }
        }

        public virtual void HelpContents()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Help Contents...",
                    null,
                    _actionIconImages["Contents"],
                    true,
                    33
                );
                //System.Windows.Forms.Help
                if (!Contents())
                {
                    throw new ApplicationException("'Help Contents' error");
                }

                StopProgressBar("Help Contents completed.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("Help Contents failed: {0}", ex.Message));
            }
        }

        public virtual void HelpIndex()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Help Index...",
                    null,
                    null,//_actionIconImages["Index"],
                    true,
                    33
                );
                //System.Windows.Forms.Help
                if (!Index())
                {
                    throw new ApplicationException("'Help Index' error");
                }

                StopProgressBar("Help Index completed.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("Help Index failed: {0}", ex.Message));
            }
        }

        public virtual void HelpOnHelp()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Help On Help...",
                    null,
                    null,//_actionIconImages["Help On Help"],
                    true,
                    33
                );

                //System.Windows.Forms.Help
                if (!OnHelp())
                {
                    throw new ApplicationException("'Help On Help' error");
                }

                StopProgressBar("Help On Help completed.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("Help On Help failed: {0}", ex.Message));
            }
        }

        public virtual void HelpLicenceInformation()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Licence Information...",
                    null,
                    null,//_actionIconImages["Licence Information"],
                    true,
                    33
                );

                if (!LicenceInformation())
                {
                    throw new ApplicationException("'Licence Information' error");
                }

                StopProgressBar("Licence Information completed.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("Licence Information failed: {0}", ex.Message));
            }
        }

        public virtual void HelpCheckForUpdates()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Check For Updates...",
                    null,
                    null,//_actionIconImages["Check For Updates"],
                    true,
                    33
                );

                if (!CheckForUpdates())
                {
                    throw new ApplicationException("'Check For Updates' error");
                }

                StopProgressBar("Check For Updates completed.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("Check For Updates failed: {0}", ex.Message));
            }
        }

        public virtual void HelpAbout<TAssemblyInfo>()
            where TAssemblyInfo :
            //class,
            AssemblyInfoBase,
            new()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar("About...", null, _actionIconImages["About"], true, 33);

                // Open the About form in Dialog Mode
                AboutDialog frm = new AboutDialog(new TAssemblyInfo());
                frm.Owner = System.Windows.Forms.Application.OpenForms[0];//App.Current.MainWindow;
                frm.ShowDialog();
                frm = null;
                
                StopProgressBar("About completed.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("{0}", ex.Message));
            }
        }
        #endregion menu feature boilerplate

        #region menu feature implementation
        //likely to need overridden to handle feature specifics

        /// <summary>
        /// placeholder and example, but this must be overridden to actually do anything
        /// with the print dialog, such as sendign a document to the printer
        /// </summary>
        /// <returns>Boolean</returns>
        protected virtual Boolean Print()
        {
            Boolean returnValue = default(Boolean);
            System.Windows.Forms.PrintDialog printDialog = new System.Windows.Forms.PrintDialog();
            Boolean? dialogResult = (printDialog.ShowDialog() == DialogResult.OK);

            if (dialogResult.HasValue && dialogResult.Value)
            {
                try
                {//TODO:some part of this process needs to be 'protected virtual' and overridden?
                    //printDialog.PrintVisual(/*_view.ChartControl*/, "app_name");
                    returnValue = true;
                }
                catch (Exception ex)
                {
                    Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                    throw ex;
                }
            }

            return returnValue;
        }

        protected virtual Boolean Undo()
        {
            Boolean returnValue = default(Boolean);

            try
            {
                //TODO:some part of this process may need to overridden
                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw ex;
            }

            return returnValue;
        }

        protected virtual Boolean Redo()
        {
            Boolean returnValue = default(Boolean);

            try
            {
                //TODO:some part of this process may need to overridden
                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw ex;
            }

            return returnValue;
        }

        protected virtual Boolean SelectAll()
        {
            Boolean returnValue = default(Boolean);

            try
            {
                //TODO:some part of this process may need to overridden
                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw ex;
            }

            return returnValue;
        }

        protected virtual Boolean Cut()
        {
            Boolean returnValue = default(Boolean);

            try
            {
                //TODO:some part of this process may need to overridden
                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw ex;
            }

            return returnValue;
        }

        protected virtual Boolean Copy()
        {
            Boolean returnValue = default(Boolean);

            try
            {
                //TODO:some part of this process may need to overridden
                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw ex;
            }

            return returnValue;
        }

        protected virtual Boolean Paste()
        {
            Boolean returnValue = default(Boolean);

            try
            {
                //TODO:some part of this process may need to overridden
                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw ex;
            }

            return returnValue;
        }

        protected virtual Boolean Delete()
        {
            Boolean returnValue = default(Boolean);

            try
            {
                //TODO:some part of this process may need to overridden
                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw ex;
            }

            return returnValue;
        }

        protected virtual Boolean Find()
        {
            Boolean returnValue = default(Boolean);

            try
            {
                //TODO:some part of this process may need to overridden
                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw ex;
            }

            return returnValue;
        }

        protected virtual Boolean Replace()
        {
            Boolean returnValue = default(Boolean);

            try
            {
                //TODO:some part of this process may need to overridden
                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw ex;
            }

            return returnValue;
        }

        protected virtual Boolean Refresh()
        {
            Boolean returnValue = default(Boolean);

            try
            {
                //TODO:some part of this process may need to overridden
                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw ex;
            }

            return returnValue;
        }

        protected virtual Boolean Preferences()
        {
            Boolean returnValue = default(Boolean);

            try
            {
                //TODO:some part of this process may need to overridden
                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw ex;
            }

            return returnValue;
        }

        protected virtual Boolean Contents()
        {
            Boolean returnValue = default(Boolean);

            try
            {
                //TODO:some part of this process may need to overridden
                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw ex;
            }

            return returnValue;
        }

        protected virtual Boolean Index()
        {
            Boolean returnValue = default(Boolean);

            try
            {
                //TODO:some part of this process may need to overridden
                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw ex;
            }

            return returnValue;
        }

        protected virtual Boolean OnHelp()
        {
            Boolean returnValue = default(Boolean);

            try
            {
                //TODO:some part of this process may need to overridden
                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw ex;
            }

            return returnValue;
        }

        protected virtual Boolean LicenceInformation()
        {
            Boolean returnValue = default(Boolean);

            try
            {
                //TODO:some part of this process may need to overridden
                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw ex;
            }

            return returnValue;
        }

        protected virtual Boolean CheckForUpdates()
        {
            Boolean returnValue = default(Boolean);

            try
            {
                //TODO:some part of this process may need to overridden
                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw ex;
            }

            return returnValue;
        }
        #endregion menu feature implementation

        #endregion Menus

        #region Controls

        /// <summary>
        /// Handle DoWork event.
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        /// <param name="workDelegate"></param>
        /// <param name="resultNullDescription"></param>
        public virtual void BackgroundWorker_DoWork<TReturn>
        (
            BackgroundWorker worker, 
            DoWorkEventArgs e, 
            DoWork_WorkDelegate<TReturn> workDelegate,
            String resultNullDescription = "No result was returned."
        )
        {
            String errorMessage = default(String);

            try
            {
                //run process
                if (workDelegate != null)
                {
                    e.Result =
                        workDelegate
                        (
                            worker,
                            e,
                            ref errorMessage
                        );
                }

                //look for specific problem
                if (!String.IsNullOrEmpty(errorMessage))
                {
                    throw new Exception(errorMessage);
                }

                //warn about unexpected result
                if (e.Result == null)
                {
                    throw new Exception(resultNullDescription);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                //re-throw and let RunWorkerCompleted event handle and report error.
                throw;
            }
        }

        /// <summary>
        /// Handle ProgressChanged event.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="userState">Object, specifically a String.</param>
        /// <param name="progressPercentage"></param>
        public virtual void BackgroundWorker_ProgressChanged
        (
            String description, 
            Object userState, 
            Int32 progressPercentage
        )
        {
            String message = String.Empty;

            try
            {
                if (userState != null)
                {
                    message = userState.ToString();
                }
                UpdateProgressBar(String.Format("{0} ({1})...{2}%", description, message, progressPercentage.ToString()), progressPercentage);
                System.Windows.Forms.Application.DoEvents();
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Handle RunWorkerCompleted event.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="worker"></param>
        /// <param name="e">RunWorkerCompletedEventArgs</param>
        /// <param name="errorDelegate">Replaces default behavior of displaying the exception message.</param>
        /// <param name="cancelledDelegate">Replaces default behavior of displaying a cancellation message. Handles the display message only; differs from cancelDelegate, which handles view-level behavior not specific to this worker.</param>
        /// <param name="completedDelegate">Extends default behavior of refreshing the display; execute prior to Refresh().</param>
        public virtual void BackgroundWorker_RunWorkerCompleted
        (
            String description,
            BackgroundWorker worker,
            RunWorkerCompletedEventArgs e,
            Action<Exception> errorDelegate = null,
            Action cancelledDelegate = null,
            Action completedDelegate = null
        )
        {
            try
            {
                Exception error = e.Error;
                Boolean isCancelled = e.Cancelled;
                Object result = e.Result;

                // First, handle the case where an exception was thrown.
                if (error != null)
                {
                    if (errorDelegate != null)
                    {
                        errorDelegate(error);
                    }
                    else
                    {
                        // Show the error message
                        StopProgressBar(null, error.Message);
                    }
                }
                else if (isCancelled)
                {
                    if (cancelledDelegate != null)
                    {
                        cancelledDelegate();
                    }
                    else
                    {
                        // Handle the case where the user cancelled the operation.
                        StopProgressBar(null, "Cancelled.");

                        //if (View.cancelDelegate != null)
                        //{
                        //    View.cancelDelegate();
                        //}
                    }
                }
                else
                {
                    // Operation completed successfully, so display the result.
                    if (completedDelegate != null)
                    {
                        completedDelegate();
                    }

                    //backgroundworker calls New/Save without UI refresh; refresh UI explicitly here.
                    ModelController<TModel>.Model.Refresh();
                }

                // Do post completion operations, like enabling the controls etc.      
                View.Activate();

                // Inform the user we're done
                StopProgressBar(description, null);
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("{0}", ex.Message));
            }
            finally
            {
                ////clear cancellation hook
                //View.cancelDelegate = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void BindingSource_PositionChanged<TItem>(BindingSource bindingSource, EventArgs e, Action<TItem> itemDelegate)
        {
            try
            {
                TItem current = (TItem)bindingSource.Current;

                if (current != null)
                {
                    if (itemDelegate != null)
                    {
                        itemDelegate(current);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Handle DataError event; specifically, catch combobox cell data errors.
        /// This allows validation message to be displayed 
        ///  and still allows the user to fix and save the settings.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="e"></param>
        /// <param name="cancel"></param>
        /// <param name="throwException"></param>
        /// <param name="logException"></param>
        public void Grid_DataError
        (
            DataGridView grid, 
            DataGridViewDataErrorEventArgs e,
            Boolean? cancel,
            Boolean? throwException,
            Boolean logException
        )
        {
            try
            {
                if (cancel.HasValue)
                {
                    //cancel to prevent default DataError dialogs
                    e.Cancel = true;
                }

                if (throwException.HasValue)
                {
                    //there are cases where you do not want to throw exception; because it will drop application immediately with only a log
                    e.ThrowException = throwException.Value;
                }

                if (logException)
                {
                    Log.Write(e.Exception, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
        }
        #endregion Controls

        #region Utility

        /// <summary>
        /// Manage buttons' state while processes are running.
        /// Usage:
        ///     View.permitEnabledStateXxx = ButtonEnabled(enabledFlag, View.permitEnabledStateXxx, View.cmdXxx, View.menuFileXxx, View.buttonFileXxx);
        /// </summary>
        /// <param name="enabledFlag"></param>
        /// <param name="permitEnabledState"></param>
        /// <param name="button"></param>
        /// <param name="menuItem"></param>
        /// <param name="buttonItem"></param>
        /// <returns>updated remembered state</returns>
        public Boolean ButtonEnabled
        (
            Boolean enabledFlag,
            Boolean permitEnabledState,
            Button button,
            ToolStripMenuItem menuItem = null,
            ToolStripButton buttonItem = null
        )
        {
            Boolean returnValue = permitEnabledState; // default(Boolean);
            try
            {
                if (enabledFlag)
                {
                    //ENABLING
                    //recall state
                    if (button != null)
                    {
                        button.Enabled = permitEnabledState;
                    }
                    if (menuItem != null)
                    {
                        menuItem.Enabled = permitEnabledState;
                    }
                    if (buttonItem != null)
                    {
                        buttonItem.Enabled = permitEnabledState;
                    }
                }
                else
                {
                    //DISABLING
                    //remember state
                    if (button != null)
                    {
                        returnValue = button.Enabled;
                    }
                    else if (menuItem != null)
                    {
                        returnValue = menuItem.Enabled;
                    }
                    else if (buttonItem != null)
                    {
                        returnValue = buttonItem.Enabled;
                    }

                    //disable
                    if (button != null)
                    {
                        button.Enabled = enabledFlag;
                    }
                    if (menuItem != null)
                    {
                        menuItem.Enabled = enabledFlag;
                    }
                    if (buttonItem != null)
                    {
                        buttonItem.Enabled = enabledFlag;
                    }
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw;
            }
        }
        #endregion Utility
        #endregion Methods

    }
}
