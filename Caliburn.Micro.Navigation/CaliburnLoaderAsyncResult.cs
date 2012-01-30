#region Using Directives

using System;
using System.Threading;

#endregion

namespace Caliburn.Micro.Navigation
{
    public class CaliburnLoaderAsyncResult : IAsyncResult
    {
        public CaliburnLoaderAsyncResult(object asyncState, AsyncCallback callback)
        {

            AsyncState = asyncState;
            Callback = callback;
            Lock = new object();
            AsyncWaitHandle = new AutoResetEvent(false);
        }

        internal bool BeginLoadCompleted { get; set; }
        internal AsyncCallback Callback { get; private set; }
        internal Exception Error { get; set; }
        internal object Lock { get; set; }
        internal object Page { get; set; }
        internal Uri RedirectUri { get; set; }

        #region IAsyncResult Members

        public object AsyncState { get; private set; }

        public WaitHandle AsyncWaitHandle { get; private set; }

        public bool CompletedSynchronously { get; private set; }

        public bool IsCompleted { get; private set; }

        #endregion

        public void Complete()
        {
            CompletedSynchronously = !BeginLoadCompleted;
            IsCompleted = true;
            ((AutoResetEvent) AsyncWaitHandle).Set();
        }
    }
}