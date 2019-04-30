using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.Tools
{
    public static class ConvertIntBool
    {
        public static int ConvertBoolToInt(bool b)
        {
            if (b)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static bool ConvertIntToBool(int a)
        {
            if (a == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
