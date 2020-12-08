//#define debug

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ssepan.Utility;
using System.Diagnostics;
using System.Reflection;

namespace Ssepan.Application.WinConsole
{
    public static class ConsoleApplication
    {
        #region Declarations
        public const String DefaultCommandLineSwitchIndicators = "/-";
        public const String DefaultCommandLineSwitchValueSeparator = ":";
        public static Boolean HelpInvoked = default(Boolean);
        #endregion Declarations

        #region Constructors
        static ConsoleApplication()
        {
            //add default Help functionality.
            AddCommandLineSwitch
            (
                new CommandLineSwitch[]  
                { 
                    new CommandLineSwitch("h", "Help. (this feature)", false, Help),
                    new CommandLineSwitch("H", "Help. (this feature)", false, Help),
                    new CommandLineSwitch("?", "Help. (this feature)", false, Help)
                }
            );
        }
        #endregion Constructors

        #region Properties
        private static String _CommandLineSwitchIndicators = DefaultCommandLineSwitchIndicators;
        /// <summary>
        /// Characters that indicated a command-line switch.
        /// </summary>
        public static String CommandLineSwitchIndicators
        {
            get { return _CommandLineSwitchIndicators; }
            set { _CommandLineSwitchIndicators = value; }
        }

        private static String _CommandLineSwitchValueSeparator = DefaultCommandLineSwitchValueSeparator;
        /// <summary>
        /// Characters that separate a command-line switch from a value.
        /// </summary>
        public static String CommandLineSwitchValueSeparator
        {
            get { return _CommandLineSwitchValueSeparator; }
            set { _CommandLineSwitchValueSeparator = value; }
        }

        private static List<CommandLineSwitch> _CommandLineSwitches = new List<CommandLineSwitch>();
        /// <summary>
        /// Formal list of switches allowed.
        /// </summary>
        public static List<CommandLineSwitch> CommandLineSwitches
        {
            get { return _CommandLineSwitches; }
            set { _CommandLineSwitches = value; }
        }

        private static Dictionary<String, String> _Arguments = default(Dictionary<String, String>);
        /// <summary>
        /// Actual list of switches (and values) passed.
        /// </summary>
        public static Dictionary<String, String> Arguments
        {
            get { return _Arguments; }
            set { _Arguments = value; }
        }
        #endregion Properties

        #region Public Methods
        /// <summary>
        /// Load switch definitons and delegates, 
        ///  parse passed arguments into switches and value, 
        ///  and run defined actions for passed switches and values.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="commandLineSwitches"></param>
        public static void DoCommandLineSwitches(String[] args, CommandLineSwitch[] commandLineSwitches)
        {
            //define supported switches
            ConsoleApplication.AddCommandLineSwitch(commandLineSwitches);

            //parse switches
            if (!ConsoleApplication.ParseArguments(args))
            {
                throw new ApplicationException("Unable to parse arguments.");
            }
            
            //run switch actions
            if (!ConsoleApplication.ProcessArguments())
            {
                throw new ApplicationException("Unable to process arguments.");
            }
        }

        /// <summary>
        /// Add one or more CommandLineSwitches to list
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Boolean AddCommandLineSwitch(CommandLineSwitch[] commandLineSwitches)
        {
            Boolean returnValue = default(Boolean);
            try
            {
                foreach (CommandLineSwitch commandLineSwitch in commandLineSwitches)
                {
                    AddCommandLineSwitch(commandLineSwitch);
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                        
                //throw;
            }
            return returnValue;
        }

        /// <summary>
        /// Add a CommandLineSwitch to list
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Boolean AddCommandLineSwitch(CommandLineSwitch commandLineSwitch)
        {
            Boolean returnValue = default(Boolean);
            try
            {
                //check for duplicates first
                if (CommandLineSwitches.Contains(CommandLineSwitches.Find(cls => cls.SwitchCharacter == commandLineSwitch.SwitchCharacter)))
                {
                    RemoveCommandLineSwitch(commandLineSwitch);
                }
                CommandLineSwitches.Add(commandLineSwitch);

                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                        
                //throw;
            }
            return returnValue;
        }

        /// <summary>
        /// Remove one or more CommandLineSwitch from list
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Boolean RemoveCommandLineSwitch(CommandLineSwitch[] commandLineSwitches)
        {
            Boolean returnValue = default(Boolean);
            try
            {
                foreach (CommandLineSwitch commandLineSwitch in commandLineSwitches)
                {
                    RemoveCommandLineSwitch(commandLineSwitch);
                }
                
                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                        
                //throw;
            }
            return returnValue;
        }

        /// <summary>
        /// Remove a CommandLineSwitch from list
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Boolean RemoveCommandLineSwitch(CommandLineSwitch commandLineSwitch)
        {
            Boolean returnValue = default(Boolean);
            try
            {
                returnValue = CommandLineSwitches.Remove(CommandLineSwitches.Find(cls => cls.SwitchCharacter == commandLineSwitch.SwitchCharacter));
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                        
                //throw;
            }
            return returnValue;
        }

        /// <summary>
        /// Parse arguments into switches and values.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Boolean ParseArguments
        (
            String[] args 
        )
        {
            Boolean returnValue = default(Boolean);
            String argument = String.Empty;
            String argumentWithoutSwitchIndicator = String.Empty;
            String[] argumentFieldsArray = default(String[]);
            String key = String.Empty;
            String value = String.Empty;
            CommandLineSwitch commandLineSwitch = default(CommandLineSwitch);
            Arguments = new Dictionary<String, String>();
            
            try
            {
                #if debug
                Console.WriteLine(args.Length.ToString());
                #endif

                for (Int32 i = args.GetLowerBound(0); i <= args.GetUpperBound(0); i++)
                {
                    argument = args[i].Trim();
                    
                    //check for missing argument
                    if (argument == String.Empty)
                    {
                        throw new ArgumentException("Unexpected empty argument.");
                    }

                    //check for switch indicator
                    if (!CommandLineSwitchIndicators.Contains(argument[0]))
                    {
                        throw new ArgumentException(String.Format("Unexpected argument switch indicator: '{0}'", argument[0]));
                    }

                    //remove switch indicator
                    argumentWithoutSwitchIndicator = argument.Substring(1, argument.Length - 1);

                    #if debug
                    Console.WriteLine(argumentWithoutSwitchIndicator);
                    #endif

                    if (argumentWithoutSwitchIndicator.Contains(CommandLineSwitchValueSeparator))
                    {
                        //a compund switch
                        argumentFieldsArray = argumentWithoutSwitchIndicator.Split(CommandLineSwitchValueSeparator.ToCharArray());

                        if (argumentFieldsArray.Length < 1 || argumentFieldsArray.Length > 2)
                        {
                            throw new ArgumentException(String.Format("Unexpected argument format: '{0}'", argumentWithoutSwitchIndicator));
                        }
                        
                        key = argumentFieldsArray[0].Trim();
                        if (key == String.Empty)
                        {
                            throw new ArgumentException(String.Format("Empty argument key. Argument: '{0}'", argument));
                        }
                        if (key.Length > 1)
                        {
                            throw new ArgumentException(String.Format("Unexpected argument key format; key should be single character. Argument: '{0}'", argument));
                        }
                        
                        value = argumentFieldsArray[1].Trim();
                        if (value == String.Empty)
                        {
                            throw new ArgumentException(String.Format("Empty argument value. Argument: '{0}'", argument));
                        }
                    }
                    else
                    { 
                        //simple switch
                        if (argumentWithoutSwitchIndicator.Length != 1)
                        {
                            //length greater than 1; not a valid simple switch
                            throw new ArgumentException(String.Format("Unexpected argument format. Separator not found: '{0}'", argumentWithoutSwitchIndicator));
                        }
                        else
                        { 
                            //length is 1; valid simple switch
                            key = argumentWithoutSwitchIndicator;
                            value = String.Empty;
                        }
                    }

                    #if debug
                    Console.WriteLine(key);
                    Console.WriteLine(value);
                    #endif

                    //check for switch 
                    commandLineSwitch = CommandLineSwitches.Find(cls => cls.SwitchCharacter == key);
                    if (commandLineSwitch == null)
                    {
                        throw new ArgumentException(String.Format("Unexpected argument switch: '{0}'", key[0]));
                    }

                    //validate against UsesValue connectionString
                    if (commandLineSwitch.UsesValue)
                    {
                        if (value == String.Empty || value == null)
                        { 
                            throw new ArgumentException(String.Format("Switch '{0}' is missing a value.", key[0]));
                        }
                    }
                    else
                    { 
                        if (value != String.Empty && value != null)
                        { 
                            throw new ArgumentException(String.Format("Switch '{0}' does not take a value.", key[0]));
                        }
                    }

                    //add item to dictionary
                    Arguments.Add(key, value);

                    //clean up this iteration
                    argument = String.Empty;
                    argumentWithoutSwitchIndicator = String.Empty;
                    argumentFieldsArray = default(String[]);
                    key = String.Empty;
                    value = String.Empty;
                }

                #if debug
                foreach (KeyValuePair<String, String> kvp in arguments)
                {
                    Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                }
                #endif
                
                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                        
            }
            return returnValue;
        }

        /// <summary>
        /// For each argument actually passed, run defined action.
        /// </summary>
        /// <returns></returns>
        public static Boolean ProcessArguments()
        {
            Boolean returnValue = default(Boolean);
            CommandLineSwitch commandLineSwitch = default(CommandLineSwitch);

            try
            {
                //process arguments passed to app
                foreach (KeyValuePair<String, String> argument in Arguments)
                {
                    commandLineSwitch = CommandLineSwitches.Find(cls => cls.SwitchCharacter == argument.Key);
                    if (commandLineSwitch != null)
                    {
                        if (commandLineSwitch.ActionDelegate != null)
                        {
                            commandLineSwitch.ActionDelegate(argument.Value, ConsoleApplication.defaultOutputDelegate/*messageBoxWrapperOutputDelegate*/);
                        }
                        else
                        { 
                            throw new ApplicationException(String.Format("Switch has no action defined: {0}", argument.Key));
                        }
                    }
                    else
                    {
                        throw new ApplicationException(String.Format("Switch not recognized: {0}", argument.Key));
                    }
                }
                
                #if debug
                foreach (CommandLineSwitch cls in CommandLineSwitches)
                {
                    if (cls.ActionDelegate != null)
                    {
                        cls.ActionDelegate("test", ConsoleApplication.defaultOutputDelegate/*messageBoxWrapperOutputDelegate*/);
                    }
                }
                #endif

                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                        
                //throw;
            }
            return returnValue;
        }

        #region CommandLineSwitch Action Delegates
        /// <summary>
        /// Instance of an action conforming to delegate Action<TStruct>, where TStruct is String.
        /// Built-in Help function for switches. Can be overridden.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="outputDelegate"></param>
        public static void Help
        (
            String value, 
            Action<String> outputDelegate
        )
        {
            StringBuilder stringBuilder = default(StringBuilder);

            try
            {
                stringBuilder = new StringBuilder();

                if (outputDelegate == null)
                {
                    outputDelegate = defaultOutputDelegate;
                }

                stringBuilder.AppendLine(String.Format("\r\nHELP\r\n"));
                foreach (CommandLineSwitch commandLineSwitch in CommandLineSwitches)
                {
                    HelpInvoked = true;

                    stringBuilder.AppendLine(String.Format("\t{0}:\t{1}", commandLineSwitch.SwitchCharacter, commandLineSwitch.Description));
                }
                outputDelegate(stringBuilder.ToString());
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                        
                //throw;
            }
        }
        #endregion CommandLineSwitch Action Delegates
        #region Output Delegates
        //default
        public static Action<String> defaultOutputDelegate = writeLineWrapperOutputDelegate;

        //wrapper for writeline
        public static Action<String> writeLineWrapperOutputDelegate = Console.WriteLine;

        //wrapper for messagebox
        public static Action<String> messageBoxWrapperOutputDelegate = (s) => { System.Windows.Forms.MessageBox.Show(s); };

        //wrapper for log
        public static Action<String> eventLogWrapperOutputDelegate = (s) => { Ssepan.Utility.Log.Write(s, EventLogEntryType.Information); };
        #endregion Output Delegates

        #region DoEvents Delegates
        //default
        public static Action defaultDoEventsDelegate = consoleWrapperDoEventsDelegate;

        //wrapper for null action
        public static Action consoleWrapperDoEventsDelegate = 
            () => 
            { };

        ////wrapper for Invoke
        //public static Action wpfWrapperDoEventsDelegate = 
        //    () => 
        //    {
        //        System.Windows.Application.Current.Dispatcher.Invoke
        //        (
        //            System.Windows.Threading.DispatcherPriority.Background,
        //            new Action(delegate { })
        //        );
        //    };

        //wrapper for DoEvents
        public static Action winFormsWrapperDoEventsDelegate = 
            () => 
            {
                System.Windows.Forms.Application.DoEvents();
            };
        #endregion DoEvents Delegates

        #endregion Public Methods

        #region Private Methods
        #endregion Private Methods
    }
}
