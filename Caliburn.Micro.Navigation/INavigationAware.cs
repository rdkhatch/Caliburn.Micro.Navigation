using System;
using System.Collections.Generic;

namespace Caliburn.Micro.Navigation
{
    public interface INavigationAware
    {
        IDictionary<string, string> QueryString { get; set; }
        void OnNavigatedTo(Uri uri);
    }
}
