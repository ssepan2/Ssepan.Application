using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ssepan.Application
{
    public partial class SelectDialog : Form
    {
        public SelectDialog()
        {
            InitializeComponent();
        }

        private void cmdOk_Click(Object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdCancel_Click(Object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
