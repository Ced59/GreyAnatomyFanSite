using System.Net;

namespace GreyAnatomyFanSite.Web.Infrastructure
{
    public static class TextFormatting
    {
        public static string ToSafeHtml(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            string encoded = WebUtility.HtmlEncode(text);
            return encoded.Replace(Environment.NewLine, "<br/>");
        }
    }
}
