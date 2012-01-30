using System.Collections.Generic;

namespace Caliburn.Micro.Navigation
{
    public interface INavigationAware
    {
        IDictionary<string, string> QueryString { get; set; }
    }
}
