using GreyAnatomyFanSite.Web.Infrastructure;
using Xunit;

namespace GreyAnatomyFanSite.Tests.Unit
{
    public sealed class TextFormattingTests
    {
        [Fact]
        public void ToSafeHtml_ShouldEncodeHtml_AndPreserveLineBreaks()
        {
            string input = "<b>Bonjour</b>" + Environment.NewLine + "Monde";

            string output = TextFormatting.ToSafeHtml(input);

            Assert.Equal("&lt;b&gt;Bonjour&lt;/b&gt;<br/>Monde", output);
        }
    }
}
