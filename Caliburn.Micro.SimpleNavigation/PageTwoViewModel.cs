using System.Collections.Generic;
using Caliburn.Micro.Navigation;

namespace Caliburn.Micro.SimpleNavigation
{
    public class PageTwoViewModel : Screen, INavigationAware
    {
        public PageTwoViewModel()
        {
            FirstQuery = "Page Two";
        }

        private IDictionary<string, string> queryString;
        public IDictionary<string, string> QueryString
        {
            get { return queryString; }
            set
            {
                queryString = value;
                foreach (var kv in queryString)
                {
                    FirstQuery = "FirstQueryValue:" + kv.Value;
                    break;
                }
            }
        }

        private string firstQuery;

        public string FirstQuery
        {
            get { return firstQuery; }
            set
            {
                firstQuery = value;
                NotifyOfPropertyChange(() => FirstQuery);
            }
        }
    }
}