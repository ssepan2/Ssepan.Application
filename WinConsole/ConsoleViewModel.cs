using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Ssepan.Utility;
using Ssepan.Application;
using Ssepan.Io;

namespace Ssepan.Application
{
    /// <summary>
    /// Note: this class can be subclassed without type parameters in the client.
    /// </summary>
    /// <typeparam name="TIcon"></typeparam>
    /// <typeparam name="TSettings"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    public class ConsoleViewModel
    <
        TIcon, 
        TSettings, 
        TModel
    > :
        ViewModelBase<TIcon>
        where TIcon : class
        where TSettings : class, ISettings, new()
        where TModel : class, IModel, new()
    {
        #region Declarations
        //private static FileDialogInfo _settingsFileDialogInfo = default(FileDialogInfo);
                       
        private Dictionary<String, TIcon> _actionIconImages = default(Dictionary<String, TIcon>);

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
        public ConsoleViewModel()
        {
            if (SettingsController<TSettings>.Settings == null)
            {
                SettingsController<TSettings>.New();
            }

            #region Commands
            //this.FileNewCommand = new FileNewCommand(this);
            //this.FileOpenCommand = new FileOpenCommand(this);
            //this.FileSaveCommand = new FileSaveCommand(this);
            //this.FileSaveAsCommand = new FileSaveAsCommand(this);
            //this.FilePrintCommand = new FilePrintCommand(this);
            //this.FileExitCommand = new FileExitCommand(this);
            //this.EditCopyToClipboardCommand = new EditCopyToClipboardCommand(this);
            //this.EditPropertiesCommand = new EditPropertiesCommand(this);
            //this.HelpAboutCommand = new HelpAboutCommand(this);
            //ActionIconWinformsImage = 
            #endregion Commands
        }

        public ConsoleViewModel
        (
            PropertyChangedEventHandler propertyChangedEventHandlerDelegate,
            Dictionary<String, TIcon> actionIconImages
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

                ActionIconImage = _actionIconImages["Save"];

                //load user and application settings 
                //Properties.Settings.Default.Reload();

            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
        }
        #endregion Constructors

        #region Properties
        #endregion Properties

        #region Methods
        public void FileNew()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "New...",
                    null,
                    _actionIconImages["New"], 
                    true,
                    33
                );

                if (SettingsController<TSettings>.Settings.Dirty)
                {
                    //prompt before saving
                    DialogResult messageBoxResult = new DialogResult();//TODO:implement console prompt--System.Windows.Forms.MessageBox.Show("Save changes?", SettingsController<TSettings>.FilePath, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

        public void FileOpen()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    String.Format("Opening {0}...", SettingsController<TSettings>.FilePath),
                    null,
                    _actionIconImages["Open"], 
                    true,
                    33
                );

                if (SettingsController<TSettings>.Settings.Dirty)
                {
                    //prompt before saving
                    DialogResult messageBoxResult = new DialogResult();//TODO:implement console prompt--System.Windows.Forms.MessageBox.Show("Save changes?", SettingsController<TSettings>.FilePath, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

                //_settingsFileDialogInfo.Filename = SettingsController<TSettings>.FilePath;
                //if (FileDialogInfo.GetPathForLoad(false, _settingsFileDialogInfo))
                //{
                //    SettingsController<TSettings>.FilePath = _settingsFileDialogInfo.Filename;

                    //OPEN
                    if (!SettingsController<TSettings>.Open())
                    {
                        throw new ApplicationException(String.Format("Unable to Open settings.\r\nPath: {0}", SettingsController<TSettings>.FilePath));
                    }

                    ModelController<TModel>.Model.Refresh();
                    
                    StopProgressBar("Opened.");
                //}
                //else
                //{
                //    StopProgressBar("Open cancelled.");
                //}

            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("{0}", ex.Message));
            }
        }

        public void FileSave()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    String.Format("Saving {0}...", SettingsController<TSettings>.FilePath),
                    null,
                    _actionIconImages["Save"], 
                    true,
                    33
                );

                //_settingsFileDialogInfo.Filename = SettingsController<TSettings>.FilePath;
                //if (FileDialogInfo.GetPathForSave(false, _settingsFileDialogInfo))
                //{
                //    SettingsController<TSettings>.FilePath = _settingsFileDialogInfo.Filename;

                    //SAVE
                    if (!SettingsController<TSettings>.Save())
                    {
                        throw new ApplicationException(String.Format("Unable to Save settings.\r\nPath: {0}", SettingsController<TSettings>.FilePath));
                    }

                    ModelController<TModel>.Model.Refresh();
                
                    StopProgressBar("Saved completed.");
                //}
                //else
                //{
                //    StopProgressBar("Save cancelled.");
                //}
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("{0}", ex.Message));
            }
        }

        public void FileSaveAs()
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

                //_settingsFileDialogInfo.Filename = SettingsController<TSettings>.FilePath;
                //if (FileDialogInfo.GetPathForSave(true, _settingsFileDialogInfo))
                //{
                //    SettingsController<TSettings>.FilePath = _settingsFileDialogInfo.Filename;

                    //SAVE
                    if (!SettingsController<TSettings>.Save())
                    {
                        throw new ApplicationException(String.Format("Unable to Save settings.\r\nPath: {0}", SettingsController<TSettings>.FilePath));
                    }

                    ModelController<TModel>.Model.Refresh();
                    
                    StopProgressBar("Save As completed.");
                //}
                //else
                //{
                //    StopProgressBar("Save As cancelled.");
                //}
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("{0}", ex.Message));
            }
        }

        public void FilePrint()
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

        public void FileExit()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                //Note: app will handle final cleanup

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

        public void EditCopy()
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

                //Copy();
                
                StopProgressBar("Copied.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("Copy failed: {0}", ex.Message));
            }
        }

        public void EditProperties()
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

                ////Note:pass model, not settings, or changes will not trigger buliness logic in model properties
                //PropertyDialog pv = new PropertyDialog(ModelController<TModel>.Model/*SettingsController<TSettings>.Settings*/, ModelController<TModel>.Model.Refresh);
                //pv.Owner = System.Windows.Forms.Application.OpenForms[0];//App.Current.MainWindow;
                //pv.ShowDialog();
                
                StopProgressBar("Edit Properties closed.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("{0}", ex.Message));
            }
        }

        public void HelpAbout<TAssemblyInfo>()
            where TAssemblyInfo :
            //class,
            AssemblyInfoBase,
            new()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar("About...", null, (TIcon)null, true, 33);

                //TODO:create text-mode About feature
                //// Open the About form in Dialog Mode
                //AboutDialog frm = new AboutDialog(new TAssemblyInfo());
                //frm.Owner = System.Windows.Forms.Application.OpenForms[0];//App.Current.MainWindow;
                //frm.ShowDialog();
                //frm = null;
                
                StopProgressBar("About completed.");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("{0}", ex.Message));
            }
        }

        private Boolean Print()
        {
            Boolean returnValue = default(Boolean);
            
            System.Windows.Forms.PrintDialog printDialog = new System.Windows.Forms.PrintDialog();
            Boolean? dialogResult = (printDialog.ShowDialog() == DialogResult.OK);

            if (dialogResult.HasValue && dialogResult.Value)
            {
                try
                {
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
        #endregion Methods

    }
}
