using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace Ssepan.Application.Patterns
{
    public class SingleInstanceController<TForm>  : 
        WindowsFormsApplicationBase
        where 
            TForm :
                Form,
                new()

    {
        public SingleInstanceController()
        {
            IsSingleInstance = true;

            StartupNextInstance += this_StartupNextInstance;
        }

        void this_StartupNextInstance(object sender, StartupNextInstanceEventArgs e)
        {
            TForm form = MainForm as TForm; //My derived form type
            //form.LoadFile(e.CommandLine[1]);
        }

        protected override void OnCreateMainForm()
        {
            MainForm = new TForm();
        }
    }
}
