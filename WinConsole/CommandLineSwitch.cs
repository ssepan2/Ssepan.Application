using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace Ssepan.Application
{
    public class CommandLineSwitch
    {
        #region Declarations
        #endregion Declarations

        #region Constructors
        //public CommandLineSwitch()
        //{ 
        //}

        public CommandLineSwitch
        (
            String switchCharacter, 
            String description, 
            Boolean usesValue, 
            Action<String, Action<String>> actionDelegate
        )
        {
            SwitchCharacter = switchCharacter;
            Description = description;
            UsesValue = usesValue;
            ActionDelegate = actionDelegate;
        }
        #endregion Constructors

        #region Properties
        private String _SwitchCharacter = default(String);
        public String SwitchCharacter
        {
            get { return _SwitchCharacter; }
            set { _SwitchCharacter = value; }
        }

        private String _Description = default(String);
        public String Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        private Boolean _UsesValue = default(Boolean);
        public Boolean UsesValue
        {
            get { return _UsesValue; }
            set { _UsesValue = value; }
        }

        private Action<String, Action<String>> _ActionDelegate = default(Action<String, Action<String>>);
        public Action<String, Action<String>> ActionDelegate
        {
            get { return _ActionDelegate; }
            set { _ActionDelegate = value; }
        }
        #endregion Properties

        #region Methods
        #endregion Methods
    }
}
