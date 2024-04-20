using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENCODER.NumAlgoritm
{
    interface IShifr
    {
        string Code(string InputString);
        string UnCode (string InputString);
    }
}
