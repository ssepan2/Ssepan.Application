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
    public interface IViewModel<TIcon> :
        INotifyPropertyChanged
        where TIcon : class
    {
        #region Declarations
        #endregion Declarations

        #region Constructors
        #endregion Constructors

        #region INotifyPropertyChanged
        //event PropertyChangedEventHandler PropertyChanged;
        #endregion INotifyPropertyChanged

        #region Properties
        String StatusMessage { get; set; }
        String ErrorMessage { get; set; }
        String ErrorMessageToolTipText { get; set; }
        Int32 ProgressBarValue { get; set; }
        Int32 ProgressBarMaximum { get; set; }
        Int32 ProgressBarMinimum { get; set; }
        Int32 ProgressBarStep { get; set; }
        Boolean ProgressBarIsMarquee { get; set; }
        Boolean ProgressBarIsVisible { get; set; }
        Boolean ActionIconIsVisible { get; set; }
        TIcon ActionIconImage { get; set; }
        Boolean DirtyIconIsVisible { get; set; }
        TIcon DirtyIconImage { get; set; }
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
        void StartProgressBar
        (
            String statusMessage, 
            String errorMessage, 
            TIcon objImage, 
            Boolean isMarqueeProgressBarStyle, 
            Int32 progressBarValue,
            Action doEventsWrapperDelegate = null
        );

        /// <summary>
        /// Update percentage changes.
        /// </summary>
        /// <param name="statusMessage"></param>
        /// <param name="progressBarValue"></param>
        /// <param name="doEventsWrapperDelegate"></param>
        void UpdateProgressBar
        (
            String statusMessage, 
            Int32 progressBarValue,
            Action doEventsWrapperDelegate = null
        );

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
        void UpdateStatusBarMessages
        (
            String statusMessage, 
            String errorMessage,
            String customMessage = null,
            Action doEventsWrapperDelegate = null
        );

        /// <summary>
        /// Stop progress bar and display messages.
        /// DoEvents will ensure messages are processed before continuing.
        /// </summary>
        /// <param name="statusMessage"></param>
        /// <param name="errorMessage"></param>
        /// <param name="doEventsWrapperDelegate"></param>
        void StopProgressBar
        (
            String statusMessage, 
            String errorMessage = null, 
            Action doEventsWrapperDelegate = null
        );
        #endregion Methods

    }
}
