using GreyAnatomyFanSite.Models.Persos;
using System;
using Xunit;

namespace GreyAnatomyFansSite.Tests.Models.Persos
{
    public class ActeurTests
    {
        [Fact]
        public void GivenDate_ThenCallGet_ThenReturnActeurWithDateNaissance()
        {
            //Arrange
            var actor = new Acteur();
            actor.DateNaissance = new DateTime(1990, 12, 25);

            Assert.IsType<DateTime>(actor.DateNaissance);
        }
    }
}