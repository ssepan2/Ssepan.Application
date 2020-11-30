//Copyright (C) 2002 Microsoft Corporation
//All rights reserved.
//THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER 
//EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF 
//MERCHANTIBILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//Requires the Trial or Release version of Visual Studio .NET Professional (or greater).

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Ssepan.Application.WinForms
{
	public partial class AboutDialog : Form 
	{
        private AssemblyInfoBase ainfo = default(AssemblyInfoBase);

		public AboutDialog() 
		{
            this.Font = SystemFonts.MessageBoxFont;
            InitializeComponent();
		}

        public AboutDialog(AssemblyInfoBase assemblyInfo) 
		{
            this.Font = SystemFonts.MessageBoxFont;
            InitializeComponent();
            ainfo = assemblyInfo;
		}

		private void frmAbout_Load(Object sender, System.EventArgs e) 
		{
			try 
			{
				// Set this Form's Text + Icon properties by using values from the parent form
				this.Text = "About " + this.Owner.Text;
				this.Icon = this.Owner.Icon;
				
                // Set this Form's Picture Box's image using the parent's icon 
				// However, we need to convert it to a Bitmap since the Picture Box Control
				// will ! accept a raw Icon.
                this.pbIcon.Image = this.Owner.Icon.ToBitmap();

                // Set the labels identitying the Title, Version, and Description by
				// reading Assembly meta-data originally entered in the AssemblyInfo.cs file
				// using the AssemblyInfo class defined in the same file
                //AssemblyInfo ainfo = new AssemblyInfo();
				this.lblTitle.Text = ainfo.Title;
				this.lblVersion.Text = String.Format("Version {0}", ainfo.Version);
				this.lblCopyright.Text = ainfo.Copyright;
				this.lblDescription.Text = ainfo.Description;
				this.lblCodebase.Text = ainfo.CodeBase;
			} 
			catch(System.Exception exp) 
			{
				// This catch will trap any unexpected error.
				MessageBox.Show(exp.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}
	}
}