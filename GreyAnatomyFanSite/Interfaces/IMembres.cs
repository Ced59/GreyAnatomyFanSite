using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.Interfaces
{
    public interface IMembres
    {
        void AddCommentaire ();
        void EditInfo ();
        void EditOwnCommentaires ();
        bool VerifPseudoExist();


    }
}
