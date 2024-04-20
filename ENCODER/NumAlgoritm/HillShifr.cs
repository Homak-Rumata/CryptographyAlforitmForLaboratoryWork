using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace ENCODER.NumAlgoritm
{
    delegate string ToChars(string value);
    delegate int GetNum(char input);
    delegate char GetSymbol(int num);
    class HillShifr : IShifr
    {
        public HillShifr(string CodeString, GetNum MethodConverter, GetSymbol MethodUnconverter, int num, int size)
        {
            _methodConverter = MethodConverter;
            _methodUnConverter = MethodUnconverter;
            _num = num;
            _size = size;
            _codeMatrix = GetNumMatrix(CodeString, size, num, MethodConverter);
            if (_codeMatrix.Determination().GetNum==0)
            {
                throw new Exception("Определитель матрицы не может быть равен 0");
            }
            _keyMatrix = _codeMatrix.Determination().Reverse * _codeMatrix.MatrixAlgebryAdded.Trans();
        }

        protected readonly Matrix _keyMatrix;
        protected readonly Matrix _codeMatrix;
        private readonly GetNum _methodConverter;
        private readonly GetSymbol _methodUnConverter;
        private readonly int _num;
        protected readonly int _size;

        private string BuildString(int[,] array)
        {
            string tempString = "";

            foreach (int item in array)
            {
                tempString += _methodUnConverter(item);
            }
            return tempString;
        }

        static readonly Func<int[,], int, Matrix> GetMatrix = (values, num) => (new Matrix(values, num));

        static readonly Func<Matrix, int[,]> GetNumArray = (matrix) => (matrix.GetNumArray());

        protected readonly Func<string, int, int, GetNum, Matrix> GetNumMatrix = (string inputstring, int size, int num, GetNum getNumDelegate) =>
            {
                int[,] array  = new int[inputstring.Length/size+(inputstring.Length%size!=0?1:0), size];

                int[] inputStringArray = inputstring.Select(symbol => getNumDelegate(symbol)).ToArray();

                

                for (int i = 0; i < inputstring.Length; i++)
                {
                    array[ i / size, i % size] = inputStringArray[i]; 
                }
                 

                return new Matrix(array, num).Trans();
            };

        public virtual string Code (string InputString)
        {
            return Code(InputString, _codeMatrix);            
        }


        private int _aa = 0;
        private int _bb = 0;

        protected string Code (string InputString, Matrix _codeNumMatrix)
        {
            Matrix inputMatrix = GetNumMatrix(InputString, _size, _num, _methodConverter);

            Matrix resultMatrix = (_codeNumMatrix * inputMatrix);            

            return BuildString(resultMatrix.TransponentTwo().GetNumArray());
        }

        public virtual string UnCode (string InputString)
        {
            return UnCode(InputString, _keyMatrix);
        }

        protected string UnCode (string InputString, Matrix _keyMatrix) 
        {
            Matrix inputMatrix = GetNumMatrix(InputString, _size, _num, _methodConverter);

            Matrix resultMatrix = _keyMatrix * inputMatrix;

            return BuildString(resultMatrix.TransponentTwo().GetNumArray());
        }




    }

    class PolyHillShifr: HillShifr, IShifr
    {
        public PolyHillShifr(string CodeString1,string CodeString2, GetNum MethodConverter, GetSymbol MethodUnconverter, int num, int size)
            :base (CodeString1, MethodConverter, MethodUnconverter, num, size)
        {
            _nextMatrix = GetNumMatrix(CodeString2, size, num, MethodConverter);
        }

        private Matrix _nextMatrix;

        private IEnumerable<Matrix> CodeMatrixStep (Matrix CodeMatrix, Matrix NextMatrix)
        {
            while (true)
            {
                yield return CodeMatrix;
                (CodeMatrix, NextMatrix) = (NextMatrix, CodeMatrix * NextMatrix);                
            }
            yield break;
        }

        private IEnumerable<Matrix> KeyMatrixStep (Matrix CodeMatrix, Matrix NextMatrix)
        {
            Matrix KeyMatrix = _keyMatrix;
            while (true)
            {               
                yield return KeyMatrix;
                (CodeMatrix, NextMatrix) = (NextMatrix, CodeMatrix * NextMatrix);
                KeyMatrix = (CodeMatrix.Determination().Reverse * CodeMatrix.MatrixAlgebryAdded.Trans());
            }
            yield break;
        }

        private static Func<string, int, string[]> StringParse = (string InputString, int size) =>
            (InputString.Chunk(size).Select(item => new string(item)).ToArray());

        private delegate IEnumerable<Matrix> MatrixGenerator (Matrix CodeMatrix, Matrix NextMatrix);

        private static Func<string[], Func<string, Matrix, string>, IEnumerable<Matrix>, string> StringOperation = 
            (string[] InputArrayString, Func<string, Matrix, string> Action, IEnumerable<Matrix> generator) =>
            (InputArrayString.Zip(generator, (a, b) => ((a, b)))
                .Select(item => Action(item.a, item.b))
                .Aggregate((x, y) => x + y));


        public override string Code(string InputString)
        {
            
            string resultAccamulater = StringOperation(StringParse(InputString, _size),
                base.Code, CodeMatrixStep(_codeMatrix, _nextMatrix));

            return resultAccamulater;

        }

        public override string UnCode (string InputString)
        {
            
            string resultAccamulater = StringOperation(StringParse(InputString, _size),
                base.UnCode, KeyMatrixStep(_codeMatrix, _nextMatrix));

            return resultAccamulater;
        }



    }
}
