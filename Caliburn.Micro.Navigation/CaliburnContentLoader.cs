#region Using Directives

using System;
using System.Windows;
using System.Windows.Navigation;

#endregion

namespace Caliburn.Micro.Navigation
{
    /// <summary>
    /// A Utility class that simplifies creation of an INavigationContentLoader.
    /// </summary>
    public class CaliburnContentLoader : DependencyObject, INavigationContentLoader
    {

        public static readonly DependencyProperty NavigationConductorProperty = DependencyProperty.Register(
"NavigationConductor", typeof(NavigationConductor), typeof(CaliburnContentLoader), new PropertyMetadata(new PropertyChangedCallback(NavigatorChangedCallBack)));

        public NavigationConductor NavigationConductor
        {
            get { return (NavigationConductor)GetValue(NavigationConductorProperty); }
            set { SetValue(NavigationConductorProperty, value); }
        }

        private static void NavigatorChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var nav = e.OldValue as NavigationConductor;
            if (nav != null)
                nav.ActivationProcessed -= ((CaliburnContentLoader)d).ActivationProcessed;
            nav = e.NewValue as NavigationConductor;
            if (nav != null)
                nav.ActivationProcessed += ((CaliburnContentLoader)d).ActivationProcessed;
        }

        public void ActivationProcessed(object sender, ActivationProcessedEventArgs e)
        {
            if (!e.Success) return;
            var screen = e.Item as IViewAware;
            if (screen == null)
                return;
            if (currentResult != null)
                currentResult.Page = screen.GetView();
            Complete();
        }

        private CaliburnLoaderAsyncResult currentResult;


        internal void Complete()
        {
            lock (currentResult.Lock)
            {
                Execute.OnUIThread(() => currentResult.Callback(currentResult));
                currentResult.Complete();
            }
            currentResult = null;
        }

        #region INavigationContentLoader Members

        /// <summary>
        /// Begins asynchronous loading of the content for the specified target URI.
        /// </summary>
        /// <param name="targetUri">The URI to load content for.</param>
        /// <param name="currentUri">The URI that is currently loaded.</param>
        /// <param name="userCallback">The method to call when the content finishes loading.</param>
        /// <param name="asyncState">An object for storing custom state information.</param>
        /// <returns>An object that stores information about the asynchronous operation.</returns>
        public IAsyncResult BeginLoad(Uri targetUri, Uri currentUri, AsyncCallback userCallback, object asyncState)
        {
            var result = new CaliburnLoaderAsyncResult(asyncState, userCallback) { BeginLoadCompleted = false };
            currentResult = result;
            try
            {
                NavigationConductor.NavigateToItem(targetUri);
            }
            catch (Exception e)
            {
                Error(e, result);
            }
            result.BeginLoadCompleted = true;
            return result;
        }

        /// <summary>
        /// Gets a value that indicates whether the specified URI can be loaded.
        /// </summary>
        /// <param name="targetUri">The URI to test.</param>
        /// <param name="currentUri">The URI that is currently loaded.</param>
        /// <returns>true if the URI can be loaded; otherwise, false.</returns>
        public virtual bool CanLoad(Uri targetUri, Uri currentUri)
        {
            return true;
        }

        /// <summary>
        /// Ends loading with an error, delaying throwing the error until EndLoad() is called on
        /// the INavigationContentLoader.
        /// </summary>
        /// <param name="error">The error that occurred.</param>
        /// <param name="result"></param>
        protected void Error(Exception error, CaliburnLoaderAsyncResult result)
        {
            result.Error = error;
            Complete();
        }

        /// <summary>
        /// Attempts to cancel content loading for the specified asynchronous operation.
        /// </summary>
        /// <param name="asyncResult">An object that identifies the asynchronous operation to cancel.</param>
        public void CancelLoad(IAsyncResult asyncResult)
        {
            return;
        }

        /// <summary>
        /// Completes the asynchronous content loading operation.
        /// </summary>
        /// <param name="asyncResult">An object that identifies the asynchronous operation.</param>
        /// <returns>An object that represents the result of the asynchronous content loading operation.</returns>
        public LoadResult EndLoad(IAsyncResult asyncResult)
        {
            CaliburnLoaderAsyncResult result = (CaliburnLoaderAsyncResult)asyncResult;
            if (result.Error != null)
            {
                throw result.Error;
            }
            result.Complete();
            return result.Page != null ? new LoadResult(result.Page) : new LoadResult(result.RedirectUri);
        }

        #endregion
    }
}
