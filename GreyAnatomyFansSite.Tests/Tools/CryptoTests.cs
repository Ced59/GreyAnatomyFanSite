using GreyAnatomyFanSite.Tools;
using System;
using Xunit;

namespace GreyAnatomyFansSite.Tests.Tools
{
    public class CryptoTests
    {
        [Theory]
        [InlineData("a")]
        [InlineData("test")]
        [InlineData("azerrtyyuuuiiiiiii")]
        [InlineData("password987")]
        [InlineData("aaaaaaaaaaaaannnnnnnnnnnnnnnnnnneeeeeeeeeeeeeeeeeeeeeeeeeeeeekkkkkkkkkkkkkkkkkkkkkkkkkkkkkkssssssssssssssssssssssssssfffffffffffffffffffffff")]
        public void GivenString_WhenCallHashMdp_ThenReturnString(String entry)
        {
            //Act
            var actualValue = Crypto.HashMdp(entry);

            //Assert
            Assert.IsType<String>(actualValue);
        }

        [Fact]
        public void GivenNull_WhenCallHashMdp_ThenThrowException()
        {
            //Arrange
            var expectedMessage = "Value cannot be null.";

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => Crypto.HashMdp(null));

            //Assert
            Assert.Contains(expectedMessage, exception.Message);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("test")]
        [InlineData("azerrtyyuuuiiiiiii")]
        [InlineData("password987")]
        [InlineData("aaaaaaaaaaaaannnnnnnnnnnnnnnnnnneeeeeeeeeeeeeeeeeeeeeeeeeeeeekkkkkkkkkkkkkkkkkkkkkkkkkkkkkkssssssssssssssssssssssssssfffffffffffffffffffffff")]
        public void GivenString_WhenCallHashMdp_ThenEntryDifferentOfOutput(String entry)
        {
            //Act
            var actualValue = Crypto.HashMdp(entry);

            //Assert
            Assert.DoesNotContain(actualValue, entry);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("test")]
        [InlineData("azerrtyyuuuiiiiiii")]
        [InlineData("password987")]
        [InlineData("aaaaaaaaaaaaannnnnnnnnnnnnnnnnnneeeeeeeeeeeeeeeeeeeeeeeeeeeeekkkkkkkkkkkkkkkkkkkkkkkkkkkkkkssssssssssssssssssssssssssfffffffffffffffffffffff")]
        public void GivenDifferentLenghtsString_WhenCallHashMdp_OutputLenghtEqualOf20(String entry)
        {
            //Act
            var actualValue = Crypto.HashMdp(entry);

            //Assert
            Assert.Equal(32, actualValue.Length);
        }
    }
}