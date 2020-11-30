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
using System.Linq;
using System.Text;

using System.Reflection;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ssepan.Application;

namespace Ssepan.Application.WPF
{
	public partial class AboutDialog : Window 
	{
        private Ssepan.Application.WinForms.AssemblyInfoBase ainfo = default(Ssepan.Application.WinForms.AssemblyInfoBase);

		public AboutDialog() 
		{	
			InitializeComponent();

            this.Loaded += new RoutedEventHandler(AboutDialog_Load);
        }

        public AboutDialog(Ssepan.Application.WinForms.AssemblyInfoBase assemblyInfo) :
            this()
		{	
            ainfo = assemblyInfo;
		}

		private void AboutDialog_Load(Object sender, System.EventArgs e) 
		{
			try 
			{
                if (this.Owner != null)
                {
                    // Set this Form's Text + Icon properties by using values from the parent form
                    this.Title = "About " + this.Owner.Title;
                    this.Icon = this.Owner.Icon;

                    // Set this Form's Picture Box's image using the parent's icon 
                    // However, we need to convert it to a Bitmap since the Picture Box Control
                    // will ! accept a raw Icon.
                    this.pbIcon.Source = this.Owner.Icon;
                }

                if (ainfo != null)
                {
                    // Set the labels identitying the Title, Version, and Description by
                    // reading Assembly meta-data originally entered in the AssemblyInfo.cs file
                    // using the AssemblyInfo class defined in the same file
                    //AssemblyInfo ainfo = new AssemblyInfo();
                    this.lblTitle.Content = ainfo.Title;
                    this.lblVersion.Content = string.Format("Version {0}", ainfo.Version);
                    this.lblCopyright.Content = ainfo.Copyright;
                    this.lblDescription.Content = ainfo.Description;
                    this.lblCodebase.Content = ainfo.CodeBase;
                }
			} 
			catch(Exception ex) 
			{
				MessageBox.Show(ex.Message, this.Title, MessageBoxButton.OK, MessageBoxImage.Stop);
			}
		}

        private void OK_Click(Object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
