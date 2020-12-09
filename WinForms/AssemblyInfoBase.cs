using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ssepan.Application
{

    #region " Helper class to get information for the About form. "
    // This class uses the System.Reflection.Assembly class to
    // access assembly meta-data
    // This class is ! a normal feature of AssemblyInfo.cs
    public class AssemblyInfoBase
    {
        // Used by Helper Functions to access information from Assembly Attributes
        protected Type myType;

        public AssemblyInfoBase()
        {
            myType = typeof(System.Windows.Forms.Form); //typeof(ClickOnceTestForm.Form1);
        }

        public String AsmName
        {
            get
            {
                return myType.Assembly.GetName().Name.ToString();
            }
        }

        public String AsmFQName
        {
            get
            {
                return myType.Assembly.GetName().FullName.ToString();
            }
        }

        public String CodeBase
        {
            get
            {
                return myType.Assembly.CodeBase;
            }
        }

        public String Copyright
        {
            get
            {
                Type at = typeof(AssemblyCopyrightAttribute);
                object[] r = myType.Assembly.GetCustomAttributes(at, false);
                AssemblyCopyrightAttribute ct = (AssemblyCopyrightAttribute)r[0];
                return ct.Copyright;
            }
        }

        public String Company
        {
            get
            {
                Type at = typeof(AssemblyCompanyAttribute);
                object[] r = myType.Assembly.GetCustomAttributes(at, false);
                AssemblyCompanyAttribute ct = (AssemblyCompanyAttribute)r[0];
                return ct.Company;
            }
        }

        public String Description
        {
            get
            {
                Type at = typeof(AssemblyDescriptionAttribute);
                object[] r = myType.Assembly.GetCustomAttributes(at, false);
                AssemblyDescriptionAttribute da = (AssemblyDescriptionAttribute)r[0];
                return da.Description;
            }
        }

        public String Product
        {
            get
            {
                Type at = typeof(AssemblyProductAttribute);
                object[] r = myType.Assembly.GetCustomAttributes(at, false);
                AssemblyProductAttribute pt = (AssemblyProductAttribute)r[0];
                return pt.Product;
            }
        }

        public String Title
        {
            get
            {
                Type at = typeof(AssemblyTitleAttribute);
                object[] r = myType.Assembly.GetCustomAttributes(at, false);
                AssemblyTitleAttribute ta = (AssemblyTitleAttribute)r[0];
                return ta.Title;
            }
        }

        public String Version
        {
            get
            {
                return myType.Assembly.GetName().Version.ToString();
            }
        }
    }
    #endregion

}