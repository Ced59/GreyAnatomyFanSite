using GreyAnatomyFanSite.Tools;
using System;
using Xunit;

namespace GreyAnatomyFansSite.Tests.Tools
{
    public class ConvertIntBoolTests
    {
        [Fact]
        public void GivenBoolTrue_WhenCallConvertBoolToInt_ThenReturn1()
        {
            //Arrange
            var entry = true;

            //Act
            var actualValue = ConvertIntBool.ConvertBoolToInt(entry);
            var expectedValue = 1;

            //Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GivenBoolFalse_WhenCallConvertBoolToInt_ThenReturn0()
        {
            //Arrange
            var entry = false;

            //Act
            var actualValue = ConvertIntBool.ConvertBoolToInt(entry);
            var expectedValue = 0;

            //Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GivenBool0_WhenCallConvertIntToBool_ThenReturnFalse()
        {
            //Arrange
            var entry = 0;

            //Act
            var actualValue = ConvertIntBool.ConvertIntToBool(entry);
            var expectedValue = false;

            //Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GivenBool1_WhenCallConvertIntToBool_ThenReturnTrue()
        {
            //Arrange
            var entry = 1;

            //Act
            var actualValue = ConvertIntBool.ConvertIntToBool(entry);
            var expectedValue = true;

            //Assert
            Assert.Equal(expectedValue, actualValue);
        }

    }
}
