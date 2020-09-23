using GreyAnatomyFanSite.Models.Persos;
using GreyAnatomyFanSite.ViewModels;
using Moq;
using Xunit;

namespace GreyAnatomyFansSite.Tests.ViewModels
{
    public class ActeurViewModelTests
    {
        [Fact]
        public void GivenActeurPerso_WhenCallSet_ThenReturnNotNull()
        {
            //Arrange
            var acteur = new Mock<Acteur>();
            var perso = new Mock<Personnage>();

            //Act
            var actualValue = new ActeurViewModel { Acteur = acteur.Object, Perso = perso.Object };

            Assert.NotNull(actualValue);
        }

        [Fact]
        public void GivenActeur_WhenCallSet_ThenReturnNotNull()
        {
            //Arrange
            var acteur = new Mock<Acteur>();

            //Act
            var actualValue = new ActeurViewModel { Acteur = acteur.Object };

            Assert.NotNull(actualValue);
        }

        [Fact]
        public void GivenActeurPerso_WhenCallSet_ThenReturnActeurViewModel()
        {
            //Arrange
            var acteur = new Mock<Acteur>();
            var perso = new Mock<Personnage>();

            //Act
            var actualValue = new ActeurViewModel { Acteur = acteur.Object, Perso = perso.Object };

            Assert.IsType<ActeurViewModel>(actualValue);
        }
    }
}