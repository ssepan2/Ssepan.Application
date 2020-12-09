using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ssepan.Application
{
    public partial class PropertyDialog : Form
    {
        private Action refreshAction = default(Action);
        private Object settingsObject = default(Object);
        
        public PropertyDialog()
        {
            InitializeComponent();
        }

        public PropertyDialog(Object settingsControllerModelSettings, Action modelControllerRefreshAction)
        {
            InitializeComponent();

            refreshAction = modelControllerRefreshAction;
            settingsObject = settingsControllerModelSettings;
        }

        private void propertyGrid_Validated(Object sender, EventArgs e)
        {
            //re-display view
            refreshAction();
        }

        private void PropertyDialog_Load(Object sender, EventArgs e)
        {
            propertyGrid.SelectedObject = settingsObject;
        }

        private void PropertyDialog_FormClosing(Object sender, FormClosingEventArgs e)
        {
            SeeChangesNow.Focus();

        }

        private void SeeChangesNow_Click(Object sender, EventArgs e)
        {
            //Button does little but force focus away from propertygrid, 
            //which triggers validation, and if valid will trigger re-display of plot.
            //However it is also helpful to refresh grid to change inter-property changes that happen in Settings object.
            propertyGrid.Refresh();
        }

        private void Close_Click(Object sender, EventArgs e)
        {
            //Button forces focus away from propertygrid (then closes form).
            //which triggers validation, and if valid will trigger re-display of plot.
            propertyGrid.Refresh();
            Close();

        }

    }
}