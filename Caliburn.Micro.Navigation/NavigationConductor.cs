using System;
using System.Collections.Generic;
using Caliburn.Micro.Navigation.Helpers;

namespace Caliburn.Micro.Navigation
{
    public class NavigationConductor : Conductor<IViewAware>
    {

        protected IScreenFactory<IViewAware> ScreenFactory { get; private set; }

        public NavigationConductor(IScreenFactory<IViewAware> factory)
        {
            ScreenFactory = factory;
        }

        public virtual void ActivateItem<TScreen>() where TScreen : Screen
        {
            ActivateItem(typeof(TScreen).GetUri());
        }

        /// <summary>
        /// ActivateItem has to go through the contentLoader, that's why we store temporarily the item so we can fire the CLoader
        /// </summary>
        /// <param name="item"></param>
        public override void ActivateItem(IViewAware item)
        {
            var ur = item.GetUri();
            ur.Tag = item;
            ActivateItem(ur);
        }

        /// <summary>
        /// By setting the FrameSource we are firing the contentLoader
        /// </summary>
        /// <param name="targetUri"></param>
        public void ActivateItem(Uri targetUri)
        {
            FrameSource = targetUri;
        }

        /// <summary>
        /// this one is used by contentLoader to start activation, this is where it activation ACTUALLY happens.
        /// </summary>
        /// <param name="targetUri"></param>
        internal void NavigateToItem(Uri targetUri)
        {
            targetUri = UriEx.CreateAbsolute(targetUri);
            var item = default(IViewAware);
            if (targetUri is UriEx)
                item = ((UriEx)targetUri).Tag as IViewAware;
            if (item == null)
            {
                if (targetUri.GetNavigationName() != ActiveItem.GetNavigationName())
                {
                    item = ScreenFactory.GetScreen(targetUri.GetNavigationName());
                    base.ActivateItem(item);
                }
                else
                    item = ActiveItem;
            }

            if (ActiveItem == item && item is INavigationAware) //activation successful
            {
                var navAwareItem = (INavigationAware)item;

                // Set Query String
                if (!string.IsNullOrEmpty(targetUri.Query))
                    navAwareItem.QueryString = NavUtility.ParseQueryString(targetUri.Query);
                else
                    navAwareItem.QueryString = new Dictionary<string, string>();

                // Call OnNavigatedTo
                navAwareItem.OnNavigatedTo(targetUri);
            }

        }

        private Uri frameSource;

        public Uri FrameSource
        {
            get { return frameSource; }
            set
            {
                frameSource = value;
                NotifyOfPropertyChange(() => FrameSource);
            }
        }
    }
}
