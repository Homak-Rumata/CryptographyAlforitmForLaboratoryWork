using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENCODER.ExpressionsAlgebry
{
    interface ICycleGroup
    {

    }


    internal class CycleGroup: ICycleGroup
    {


        int _mod;
        int _baseNum;

        public IEnumerable<ICycleGroup> GetSubGroups ()
        {


            yield break;
        }

        public int GetNumBaseElement ()
        {
            return 3;
        }
    }


}
