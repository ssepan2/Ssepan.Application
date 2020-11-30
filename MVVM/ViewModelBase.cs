using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using Ssepan.Utility;

namespace Ssepan.Application
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TIcon"></typeparam>
    public abstract class ViewModelBase<TIcon> :
        INotifyPropertyChanged,
        IViewModel<TIcon>
        where TIcon : class
    {
        #region Declarations
        #endregion Declarations

        #region Constructors
        #endregion Constructors

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChanged

        #region Properties
        private String _StatusMessage = default(String);
        public String StatusMessage
        {
            get { return _StatusMessage; }
            set
            {
                if (value != _StatusMessage)
                {
                    _StatusMessage = value;
                    OnPropertyChanged("StatusMessage");
                }
            }
        }

        private String _ErrorMessage = default(String);
        public String ErrorMessage
        {
            get { return _ErrorMessage; }
            set
            {
                if (value != _ErrorMessage)
                {
                    _ErrorMessage = value;
                    OnPropertyChanged("ErrorMessage");
                }
            }
        }

        private String _CustomMessage = default(String);
        public String CustomMessage
        {
            get { return _CustomMessage; }
            set
            {
                if (value != _CustomMessage)
                {
                    _CustomMessage = value;
                    OnPropertyChanged("CustomMessage");
                }
            }
        }

        private String _ErrorMessageToolTipText = default(String);
        public String ErrorMessageToolTipText
        {
            get { return _ErrorMessageToolTipText; }
            set
            {
                if (value != _ErrorMessageToolTipText)
                {
                    _ErrorMessageToolTipText = value;
                    OnPropertyChanged("ErrorMessageToolTipText");
                }
            }
        }

        private Int32 _ProgressBarValue = default(Int32);
        public Int32 ProgressBarValue
        {
            get { return _ProgressBarValue; }
            set
            {
                if (value != _ProgressBarValue)
                {
                    _ProgressBarValue = value;
                    OnPropertyChanged("ProgressBarValue");
                }
            }
        }

        private Int32 _ProgressBarMaximum = default(Int32);
        public Int32 ProgressBarMaximum
        {
            get { return _ProgressBarMaximum; }
            set
            {
                if (value != _ProgressBarMaximum)
                {
                    _ProgressBarMaximum = value;
                    OnPropertyChanged("ProgressBarMaximum");
                }
            }
        }

        private Int32 _ProgressBarMinimum = default(Int32);
        public Int32 ProgressBarMinimum
        {
            get { return _ProgressBarMinimum; }
            set
            {
                if (value != _ProgressBarMinimum)
                {
                    _ProgressBarMinimum = value;
                    OnPropertyChanged("ProgressBarMinimum");
                }
            }
        }

        private Int32 _ProgressBarStep = default(Int32);
        public Int32 ProgressBarStep
        {
            get { return _ProgressBarStep; }
            set
            {
                if (value != _ProgressBarStep)
                {
                    _ProgressBarStep = value;
                    OnPropertyChanged("ProgressBarStep");
                }
            }
        }

        private Boolean _ProgressBarIsMarquee = default(Boolean);
        public Boolean ProgressBarIsMarquee
        {
            get { return _ProgressBarIsMarquee; }
            set
            {
                if (value != _ProgressBarIsMarquee)
                {
                    _ProgressBarIsMarquee = value;
                    OnPropertyChanged("ProgressBarIsMarquee");
                }
            }
        }

        private Boolean _ProgressBarIsVisible = default(Boolean);
        public Boolean ProgressBarIsVisible
        {
            get { return _ProgressBarIsVisible; }
            set
            {
                if (value != _ProgressBarIsVisible)
                {
                    _ProgressBarIsVisible = value;
                    OnPropertyChanged("ProgressBarIsVisible");
                }
            }
        }

        private Boolean _ActionIconIsVisible = default(Boolean);
        public Boolean ActionIconIsVisible
        {
            get { return _ActionIconIsVisible; }
            set
            {
                if (value != _ActionIconIsVisible)
                {
                    _ActionIconIsVisible = value;
                    OnPropertyChanged("ActionIconIsVisible");
                }
            }
        }

        private TIcon _ActionIconImage = default(TIcon);
        public TIcon ActionIconImage
        {
            get { return _ActionIconImage; }
            set
            {
                if (value != _ActionIconImage)
                {
                    _ActionIconImage = value;
                    OnPropertyChanged("ActionIconImage");
                }
            }
        }

        private Boolean _DirtyIconIsVisible = default(Boolean);
        public Boolean DirtyIconIsVisible
        {
            get { return _DirtyIconIsVisible; }
            set
            {
                if (value != _DirtyIconIsVisible)
                {
                    _DirtyIconIsVisible = value;
                    OnPropertyChanged("DirtyIconIsVisible");
                }
            }
        }

        private TIcon _DirtyIconImage = default(TIcon);
        public TIcon DirtyIconImage
        {
            get { return _DirtyIconImage; }
            set
            {
                if (value != _DirtyIconImage)
                {
                    _DirtyIconImage = value;
                    OnPropertyChanged("DirtyIconImage");
                }
            }
        }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Use when Marquee-style progress bar is not sufficient, and percentages must be indicated.
        /// WPF.
        /// </summary>
        /// <param name="statusMessage"></param>
        /// <param name="errorMessage"></param>
        /// <param name="objImage">System.Windows.Controls.Image or System.Drawing.Image</param>
        /// <param name="isMarqueeProgressBarStyle"></param>
        /// <param name="progressBarValue"></param>
        /// <param name="doEventsWrapperDelegate"></param>
        /// <typeparam name="TIcon">System.Windows.Controls.Image or System.Drawing.Image</typeparam>
        public void StartProgressBar
        (
            String statusMessage, 
            String errorMessage, 
            TIcon objImage, 
            Boolean isMarqueeProgressBarStyle, 
            Int32 progressBarValue,
            Action doEventsWrapperDelegate = null
        )
        {
            try
            {
                ProgressBarIsMarquee = isMarqueeProgressBarStyle;//set to blocks if actual percentage was used.
                ProgressBarValue = progressBarValue;//set to value if percentage used.
                //if Style is not Marquee, then we are marking either a count or percentage
                if (progressBarValue > ProgressBarMaximum)
                {
                    ProgressBarStep = 1;
                    ProgressBarValue = 1;
                }

                StatusMessage = statusMessage;
                ErrorMessage = errorMessage;
                //this.StatusBarErrorMessage.ToolTipText = errorMessage;

                ProgressBarIsVisible = true;

                ActionIconImage = objImage;
                //ActionIconWpfImage = objImage;
                if (objImage != null)
                {
                    ActionIconIsVisible = true;
                }

                //give the app time to draw the eye-candy, even if its only for an instant
                if (doEventsWrapperDelegate != null)
                {
                    doEventsWrapperDelegate();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                throw;
            }
        }

        /// <summary>
        /// Update percentage changes.
        /// </summary>
        /// <param name="statusMessage"></param>
        /// <param name="progressBarValue"></param>
        /// <param name="doEventsWrapperDelegate"></param>
        public void UpdateProgressBar
        (
            String statusMessage, 
            Int32 progressBarValue,
            Action doEventsWrapperDelegate = null
        )
        {
            try
            {
                StatusMessage = statusMessage;
                //ErrorMessage = errorMessage;
                //this.StatusBarErrorMessage.ToolTipText = errorMessage;

                //if Style is not Marquee, then we are marking either a count or percentage
                //if we are simply counting, the progress bar will periodically need to adjust the Maximum.
                if (progressBarValue > ProgressBarMaximum)
                {
                    ProgressBarMaximum = ProgressBarMaximum * 2;
                }
                ProgressBarValue = progressBarValue;

                //give the app time to draw the eye-candy, even if its only for an instant
                if (doEventsWrapperDelegate != null)
                {
                    doEventsWrapperDelegate();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                throw;
            }
        }

        /// <summary>
        /// Update message(s) only, without changing progress bar. 
        /// Null parameter will leave a message unchanged; 
        /// String.Empty will clear it.
        /// Optional doEvents flag will determine if
        /// messages are processed before continuing.
        /// </summary>
        /// <param name="statusMessage"></param>
        /// <param name="errorMessage"></param>
        /// <param name="customMessage"></param>
        /// <param name="doEventsWrapperDelegate"></param>
        public void UpdateStatusBarMessages
        (
            String statusMessage, 
            String errorMessage,
            String customMessage = null,
            Action doEventsWrapperDelegate = null
        )
        {
            try
            {
                if (statusMessage != null)
                {
                    StatusMessage = statusMessage;
                }
                if (errorMessage != null)
                {
                    ErrorMessage = errorMessage;
                    ErrorMessageToolTipText = errorMessage;
                }
                if (customMessage != null)
                {
                    CustomMessage = customMessage;
                }

                //give the app time to draw the eye-candy, even if its only for an instant
                if (doEventsWrapperDelegate != null)
                {
                    doEventsWrapperDelegate(); 
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                throw;
            }
        }

        /// <summary>
        /// Stop progress bar and display messages.
        /// DoEvents will ensure messages are processed before continuing.
        /// </summary>
        /// <param name="statusMessage"></param>
        /// <param name="errorMessage"></param>
        /// <param name="doEventsWrapperDelegate"></param>
        public void StopProgressBar
        (
            String statusMessage, 
            String errorMessage = null, 
            Action doEventsWrapperDelegate = null
        )
        {
            try
            {
                StatusMessage = statusMessage;
                //do not clear error at end of operation, clear it at start of operation
                if (errorMessage != null)
                {
                    ErrorMessage = errorMessage;
                    //this.StatusBarErrorMessage.ToolTipText = errorMessage;
                }

                ProgressBarIsMarquee = false;//reset back to marquee (default) in case actual percentage was used
                ProgressBarMaximum = 100;//ditto
                ProgressBarStep = 10;//ditto
                ProgressBarValue = 0;//ditto
                ProgressBarIsVisible = false;

                ActionIconIsVisible = false;
                ActionIconImage = default(TIcon);
                //ActionIconWpfImage = null;

                //give the app time to draw the eye-candy, even if its only for an instant
                if (doEventsWrapperDelegate != null)
                {
                    doEventsWrapperDelegate(); 
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                throw;
            }
        }
        #endregion Methods

    }
}
