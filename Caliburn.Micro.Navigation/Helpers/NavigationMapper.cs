using System;

namespace Caliburn.Micro.Navigation.Helpers
{
    public static class NavigationMapper
    {
        public static UriEx GetUri(this object model, string queryString = null)
        {
            if (model == null)
                return null;
            var modelName = model is Type ? ((Type)model).Name : model.GetType().Name;
            return new UriEx("/" + modelName + (queryString != null ? "?" + queryString : ""), UriKind.Relative);
        }

        public static string GetNavigationName(this Uri uri)
        {
            if (uri == null)
                return null;
            if (!uri.IsAbsoluteUri)
                return uri.OriginalString;
            return uri.LocalPath;
        }

        /// <summary>
        /// For registration in Ioc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetNavigationName(this object model)
        {
            return model.GetUri().GetNavigationName();
        }
    }
}
