using GreyAnatomyFanSite.Tools;
using System;
using Xunit;

namespace GreyAnatomyFansSite.Tests
{
    public class PassConnectionTests
    {
        [Fact]
        public void WhenCallMailPassword_ThenReturnString()
        {
            //Act
            var actualValue = PassConnection.MailPassWord();

            //Assert
            Assert.IsType<String>(actualValue);
        }

        [Fact]
        public void WhenCallSmtpConfig_ThenReturnString()
        {
            //Act
            var actualValue = PassConnection.SmtpConfig();

            //Assert
            Assert.IsType<String>(actualValue);
        }

        [Fact]
        public void WhenCallConnectionBddSerie_ThenReturnString()
        {
            //Act
            var actualValue = PassConnection.ConnectionBddSerie();

            //Assert
            Assert.IsType<String>(actualValue);
        }

        [Fact]
        public void WhenCallConnectionBddSMembres_ThenReturnString()
        {
            //Act
            var actualValue = PassConnection.ConnectionBddMembres();

            //Assert
            Assert.IsType<String>(actualValue);
        }

        [Fact]
        public void WhenCallConnectionBddSurveys_ThenReturnString()
        {
            //Act
            var actualValue = PassConnection.ConnectionBddSurveys();

            //Assert
            Assert.IsType<String>(actualValue);
        }

        [Fact]
        public void WhenCallConnectionTheMovieDb_ThenReturnString()
        {
            //Act
            var actualValue = PassConnection.connectionTheMovieDB();

            //Assert
            Assert.IsType<String>(actualValue);
        }
    }
}