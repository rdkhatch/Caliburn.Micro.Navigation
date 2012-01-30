using System;

namespace Caliburn.Micro.Navigation
{
    /// <summary>
    /// UriEx is a bit ugly and maybe not too safe, but it's working good for now
    /// </summary>
    public class UriEx:Uri
    {
        public UriEx(string uriString) : base(uriString)
        {
        }

        public UriEx(string uriString, UriKind uriKind) : base(uriString, uriKind)
        {
        }

        public UriEx(Uri baseUri, string relativeUri) : base(baseUri, relativeUri)
        {
            TransferTag(baseUri);
        }

        public UriEx(Uri baseUri, Uri relativeUri) : base(baseUri, relativeUri)
        {
            TransferTag((relativeUri is UriEx)?relativeUri:baseUri);
        }

        private void TransferTag(Uri uri)
        {
            if (uri is UriEx)
                Tag = ((UriEx)uri).Tag;
        }

        public object Tag { get; set; }

        public static Uri CreateAbsolute(Uri uri)
        {
            if (!uri.IsAbsoluteUri)
                uri = new UriEx(new Uri("dummy:///"), uri);
            return uri;
        }
    }
}
