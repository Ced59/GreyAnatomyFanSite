using GreyAnatomyFanSite.Models.Persos;
using System.Collections.Generic;

namespace GreyAnatomyFanSite.Interfaces
{
    public interface IActeur
    {
        List<Acteur> GetAllActeurs();

        Acteur GetActeurById();
    }
}