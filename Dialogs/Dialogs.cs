using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ssepan.Io;
using Ssepan.Utility;
//using Microsoft.Data.ConnectionUI;//cannot use until Microsoft.VisualStudio.Data.dll is made redistributable by Microsoft
using System.Reflection;

namespace Ssepan.Application
{
    public class Dialogs
    {
        public const String FilterSeparator = "|";
        public const String FilterDescription = "{0} Files(s)";
        public const String FilterFormat = "{0} (*.{1})|*.{1}";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static Boolean GetFolderPath
        (
            ref String path,
            ref String errorMessage
        )
        {
            Boolean returnValue = default(Boolean);
            FolderBrowserDialog folderBrowserDialog = default(FolderBrowserDialog);

            try
            {
                folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal).WithTrailingSeparator();
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    path = folderBrowserDialog.SelectedPath;
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

            }
            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileMasks"></param>
        /// <param name="path"></param>
        /// <param name="errorMessage"></param>
        /// <param name="defaultPath"></param>
        /// <param name="title"></param>
        /// <param name="defaultExtension"></param>
        /// <returns></returns>
        public static Boolean GetFileSavePath
        (
            String[] fileMasks,
            ref String path,
            ref String errorMessage,
            String defaultPath = null,
            String title = null,
            String defaultExtension = null,
            String initialDirectory = null
        )
        {
            Boolean returnValue = default(Boolean);
            SaveFileDialog saveFileDialog = default(SaveFileDialog);
            String fileDescription = default(String);
            String fileExtension = default(String);
            String fileFilter = default(String);
            String filter = default(String);

            try
            {
                filter = "All files (*.*)|*.*";
                foreach (String fileMask in fileMasks)
                {
                    fileExtension = Path.GetExtension(fileMask).Replace(".", String.Empty);
                    fileDescription = String.Format(FilterDescription, fileExtension.ToUpper());
                    fileFilter = String.Format(FilterFormat, fileDescription, fileExtension);

                    //add to filter
                    filter = String.Join(FilterSeparator, new String[] { fileFilter, "All files (*.*)|*.*" });
                }

                saveFileDialog = new SaveFileDialog();
                saveFileDialog.AddExtension = true;
                saveFileDialog.AutoUpgradeEnabled = true;
                saveFileDialog.CheckPathExists = true;
                saveFileDialog.CheckFileExists = false;
                saveFileDialog.ValidateNames = true;
                saveFileDialog.ShowHelp = true;
                saveFileDialog.DefaultExt = "";
                saveFileDialog.FileName = String.Empty;
                saveFileDialog.Filter = filter;
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.InitialDirectory = initialDirectory; 
                saveFileDialog.FileName =
                    Path.Combine
                    (
                        (defaultPath ?? Environment.GetFolderPath(Environment.SpecialFolder.Personal).WithTrailingSeparator()),
                        fileMasks[0]
                    );
                saveFileDialog.Title = title;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    path = saveFileDialog.FileName;
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

            }
            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileMasks"></param>
        /// <param name="path"></param>
        /// <param name="errorMessage"></param>
        /// <param name="defaultPath"></param>
        /// <returns></returns>
        public static Boolean GetFileOpenPath
        (
            String[] fileMasks,
            ref String path,
            ref String errorMessage,
            String defaultPath = null
        )
        {
            Boolean returnValue = default(Boolean);
            OpenFileDialog openFileDialog = default(OpenFileDialog);
            String fileDescription = default(String);
            String fileExtension = default(String);
            String fileFilter = default(String);
            String filter = default(String);

            try
            {
                filter = "All files (*.*)|*.*";
                foreach (String fileMask in fileMasks)
                {
                    fileExtension = Path.GetExtension(fileMask).Replace(".", String.Empty);
                    fileDescription = String.Format(FilterDescription, fileExtension.ToUpper());
                    fileFilter = String.Format(FilterFormat, fileDescription, fileExtension);

                    //add to filter
                    filter = String.Join(FilterSeparator, new String[] { fileFilter, "All files (*.*)|*.*" });
                }

                openFileDialog = new OpenFileDialog();
                openFileDialog.AddExtension = true;
                openFileDialog.AutoUpgradeEnabled = true;
                openFileDialog.CheckPathExists = true;
                openFileDialog.CheckFileExists = true;
                openFileDialog.ValidateNames = true;
                openFileDialog.ShowHelp = true;
                openFileDialog.DefaultExt = "";
                openFileDialog.FileName = String.Empty;
                openFileDialog.Filter = filter;
                openFileDialog.FilterIndex = 1;
                openFileDialog.Multiselect = false;
                openFileDialog.FileName =
                    Path.Combine
                    (
                        (defaultPath ?? Environment.GetFolderPath(Environment.SpecialFolder.Personal).WithTrailingSeparator()),
                        fileMasks[0]
                    );
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    path = openFileDialog.FileName;
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

            }
            return returnValue;
        }

        /// <summary>
        /// Perform input of connection String and provider name.
        /// Uses MS Data Connections Dialog.
        /// Note: relies on MS-LPL license and code from http://archive.msdn.microsoft.com/Connection
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="providerName"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static Boolean GetDataConnection
        (
            ref String connectionString,
            ref String providerName,
            ref String errorMessage
        )
        {
            Boolean returnValue = default(Boolean);
            //DataConnectionDialog dataConnectionDialog = default(DataConnectionDialog);
            //DataConnectionConfiguration dataConnectionConfiguration = default(DataConnectionConfiguration);

            try
            {
                //dataConnectionDialog = new DataConnectionDialog();

                //DataSource.AddStandardDataSources(dataConnectionDialog);

                //dataConnectionDialog.SelectedDataSource = DataSource.SqlDataSource;
                //dataConnectionDialog.SelectedDataProvider = DataProvider.SqlDataProvider;//TODO:use?

                //dataConnectionConfiguration = new DataConnectionConfiguration(null);
                //dataConnectionConfiguration.LoadConfiguration(dataConnectionDialog);

                //(don't) set to current connection string, because it overwrites previous settings, requiring user to click Refresh in Data COnenction Dialog.
                //dataConnectionDialog.ConnectionString = connectionString;

                if (true/*DataConnectionDialog.Show(dataConnectionDialog) == DialogResult.OK*/)
                {
                    ////extract connection string
                    //connectionString = dataConnectionDialog.ConnectionString;
                    //providerName = dataConnectionDialog.SelectedDataProvider.ViewName;

                    ////writes provider selection to xml file
                    //dataConnectionConfiguration.SaveConfiguration(dataConnectionDialog);

                    ////save these too
                    //dataConnectionConfiguration.SaveSelectedProvider(dataConnectionDialog.SelectedDataProvider.ToString());
                    //dataConnectionConfiguration.SaveSelectedSource(dataConnectionDialog.SelectedDataSource.ToString());

                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

            }
            return returnValue;
        }

        /// <summary>
        /// Get a color.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="allowFullOpen"></param>
        /// <param name="fullOpen"></param>
        /// <param name="anyColor"></param>
        /// <returns></returns>
        public static Boolean GetColor
        (
            ref Color color,
            Boolean allowFullOpen = true,
            Boolean fullOpen = true,
            Boolean anyColor = true
        )
        {
            Boolean returnValue = default(Boolean);
            ColorDialog colorDialog = default(ColorDialog);

            try
            {
                colorDialog = new ColorDialog();
                colorDialog.AllowFullOpen = allowFullOpen;
                colorDialog.FullOpen = fullOpen;
                colorDialog.AnyColor = anyColor;

                colorDialog.Color = color;

                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    //remember alpha channel, and re-set if necessary
                    if ((color.A != colorDialog.Color.A) && (colorDialog.Color.A == 255))
                    {
                        //color dialog likely replaced Alpha with 255 during edit
                        colorDialog.Color = Color.FromArgb(color.A, colorDialog.Color);
                    }
                    color = colorDialog.Color;

                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }

            return returnValue;
        }
    }
}
