using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SInt = ENCODER.NumAlgoritm.SpecialInt;

namespace ENCODER.AsymetrikEncoder
{
    class BackPack
    {
        public BackPack(IEnumerable<int> Sizes, int p) 
        {
            if (p < Sizes.Sum())
                throw new Exception();

            this.p = p;
            Random random = new Random();

            r = 588; //random.Next(2, p);
            openSizes = Sizes.Select(x => (x * r)%p);
            originalSizes = Sizes.Order().Reverse();
        }

        int r, p;
        IEnumerable<int> openSizes;
        IEnumerable<int> originalSizes;

        public IEnumerable<int> GetKey()
        {
            return openSizes;
        }

        public int Code (IEnumerable<bool> input)
        {
            var function = (bool x, int element) =>
            {
                return element * (x ? 1 : 0);
            };

            return input.Zip(openSizes, function).Sum();
        }

        public IEnumerable<bool> UnCode ( int input)
        {

            int reverseR = new SInt(r, p).Reverse.GetNum.Value;

            int ReversedInput = (input * reverseR) % p;

            return originalSizes
                .Aggregate<int, (int value, IEnumerable<bool> bollArray)>(
                (ReversedInput, new List<bool>()), (a, b) => (a.value >= b ? 
                (a.value - b, (new bool[] { true }).Concat(a.bollArray).ToArray()):
                (a.value, (new bool[] { false }).Concat(a.bollArray).ToArray()))).bollArray;

        }
    }
}
