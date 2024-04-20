using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENCODER.NumAlgoritm
{
    class Viginer: IShifr
    {
        public Viginer (GetNum MethodConverter, GetSymbol MethodUnConverter, string codestring, int num)
        {
            _methodConverter = MethodConverter;
            _methodUnConverter = MethodUnConverter;
            _codestring = codestring.Select(symbol => new SpecialInt(MethodConverter(symbol), num)).ToArray();
            _num = num;
        }

        private readonly GetNum _methodConverter;
        private readonly GetSymbol _methodUnConverter;

        private readonly IEnumerable<SpecialInt> _codestring;

        private readonly int _num;

        private static Func<string, GetNum, int, IEnumerable<SpecialInt>> ConverterToSpecialInt =
            (string InputString, GetNum Action, int num) =>
            (InputString.Select(symbol => new SpecialInt(Action(symbol), num)));

        private static Func<IEnumerable<SpecialInt>, GetSymbol, string> ConvertToString = (IEnumerable<SpecialInt> Collection, GetSymbol Action) =>
            (new string (Collection.Select(item=>Action(item.GetNum.Value)).ToArray()));

        public string Code (string InputString)
        {
            int lenght = _codestring.Count();

            IEnumerable<SpecialInt> IEnumerableSpecialInt = ConverterToSpecialInt(InputString, _methodConverter, _num);

            IEnumerable<SpecialInt> result = IEnumerableSpecialInt.Select((item, num)=>item+_codestring.ElementAt(num%lenght));

            return ConvertToString(result, _methodUnConverter);
        }

        public string UnCode(string InputString)
        {
            int lenght = _codestring.Count();

            IEnumerable<SpecialInt> IEnumerableSpecialInt = ConverterToSpecialInt(InputString, _methodConverter, _num);

            IEnumerable<SpecialInt> result = IEnumerableSpecialInt.Select((item, num) => item - _codestring.ElementAt(num % lenght));

            return ConvertToString(result, _methodUnConverter);
        }

    }
}
