using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ENCODER.NumAlgoritm
{
    internal class Matrix
    {
        public Matrix(int[,] Date, int num)
        {
            var c = Date
                .Cast<int>()
                .Chunk(Date
                .GetLength(1))
                .Select(temp => temp
                .Select(item => new SpecialInt(item, num))
                .ToArray())
                .ToArray();

            module = num;

            _date = c.Select(item => new SpecialVector(item)).ToArray();

        }

        int module = 37;

        private Matrix(IEnumerable<SpecialVector> Date)
        {
            _date = Date;
        }

        private Matrix(IEnumerable<SpecialVector> Date, int num)
        {
            module = num;
            _date = Date;
        }

        IEnumerable<SpecialVector> _date;

        public static Matrix operator + (Matrix x, Matrix y)
        {
            return new Matrix(x._date.Zip(y._date, (a, b) => (a + b)));
        }

        public static Matrix operator * (Matrix x, Matrix y)
        {
            Matrix ytemp = y.Trans();

            return new Matrix (x._date.Select(a => new SpecialVector (ytemp._date.Select(b => a ^ b).ToArray())));
        }

        public static Matrix operator * (SpecialInt item, Matrix matrix)
        {
            return new Matrix(matrix._date.Select(vector => new SpecialVector(vector._date.Select(value => value * item))));
        }

        /// <summary>
        /// Транспонирование
        /// </summary>
        /// <returns></returns>
        public Matrix Transponet()
        {
            return _date.Aggregate(tempVoidListFunction(_date),
                (x, y) => tempAgregateFunction(x,y), 
                date => new Matrix(date.Select(x => new SpecialVector(x))));

            IEnumerable<List<SpecialInt>> tempAgregateFunction (IEnumerable<List<SpecialInt>> x, SpecialVector y)
            {
                IEnumerable<SpecialInt> temp = y._date;
                return x.Zip(temp, (a,b)=> { a.Add(b); return a; });
            }

            IEnumerable<List<SpecialInt>> tempVoidListFunction(IEnumerable<SpecialVector> values)
            {
                return values.Max()._date.Select(x=>new List<SpecialInt>());
            }



        }

        /// <summary>
        /// Транспонирование 2 вариант
        /// </summary>
        /// <returns></returns>
        public Matrix TransponentTwo()
        {
            (SpecialInt, int, int) tempFunction (SpecialInt SPint, int vectorNum, int num)
            {
                return (SPint, vectorNum, num);
            }


            return new Matrix (this._date
                .Select((vector, vectornum) => vector._date
                    .Select((specilanum, num) => tempFunction(specilanum, vectornum, num)))
                .SelectMany((x) => (x))
                .GroupBy((x)=>(x.Item3))
                .Select((x)=>(new SpecialVector (x.Select(y=>y.Item1)))));
        }

        /// <summary>
        /// Транспонирование в императивном стиле
        /// </summary>
        /// <returns></returns>
        public Matrix Trans()
        {
            SpecialInt[][] var = _date.Select(item => item._date.Select(i => i).ToArray()).ToArray();
            SpecialInt[][] temp = new SpecialInt[var[0].Length][];
            temp = temp.Select(x=>new SpecialInt[var.Length]).ToArray();

            for (int i = 0; i < var.Length; i++)
            {
                for (int j = 0; j < var[i].Length; j++)
                {
                    temp[j][i] = var[i][j];
                }
            }

            return new Matrix(temp.Select(x=>new SpecialVector(x)));
        }


        /// <summary>
        /// Определитель матрицы
        /// </summary>
        /// <returns></returns>
        public SpecialInt Determination ()
        {
            return determinantor().Result;
        }


        /// <summary>
        /// Получение обратного числа
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private SpecialInt expression(int i)
        {
            return i % 2 == 0 ? new SpecialInt(1, module) : new SpecialInt(0, module) - new SpecialInt(1, module);
        }


        /// <summary>
        /// Приватный метод для получения определителя
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Рекурсивная функция
        /// </remarks>
        private async Task<SpecialInt> determinantor()
        {
            
            

            Matrix matrix = this;

            /*if (matrix._date.Count() <= 3 && matrix._date.First()._date.Count() <= 3)
            {
                var first = (matrix._date.First()._date.First() * matrix._date.Last()._date.Last()) * matrix._date.ElementAt(1)._date.ElementAt(1) ;
                var second = (matrix._date.First()._date.Last() * matrix._date.Last()._date.First());
                var therd = 
                var result = first - second;
                Console.WriteLine(result.GetNum);
                return result;
            }*/

                if (matrix._date.Count()<=2 || matrix._date.First()._date.Count()<=2)
            {
                if ((matrix._date.Count() < 2) && matrix._date.First()._date.Count() <= 1)
                        return matrix._date.First()._date.First();
                else if (matrix._date.Count() < 2)
                {
                    return matrix._date.First()._date.First() - matrix._date.First()._date.Last();
                }
                else if (matrix._date.First()._date.Count() < 2)
                    return matrix._date.First()._date.First() - matrix._date.Last()._date.First();
                else {
                    var first = (matrix._date.First()._date.First() * matrix._date.Last()._date.Last());
                    var second = (matrix._date.First()._date.Last() * matrix._date.Last()._date.First());
                    var result = first - second;
                    //Console.WriteLine(result.GetNum);
                    return result;
                }
            }
            else
            {
                
                SpecialInt Agregator(SpecialInt a, SpecialInt b)
                {
                    return a + b;
                }

                async Task<SpecialInt> resultexpression((Matrix a, SpecialInt b) matrix, int b, int y)
                {
                    return (await matrix.a.determinantor() * matrix.b * expression(b) * expression(y));
                }   

                IEnumerable<IEnumerable<T>> Expression<T>(IEnumerable<T> items)
                {
                    return items.Select((x, y) => items
                                    .Count() > 2 ? items
                                    .Where((a, b) => b != y) : items).ToArray();
                }

                return await matrix._date                                                           ///Вектора матрицы
                    .ElementAt(0)._date                                                             ///Взять первый вектор
                    .Select(async (x, y) => (expression(y) * x) * await (new Matrix(matrix._date    ///Составить матрицы по число элементов
                        .Where((a, b) => (b != 0))                                                  ///Все вектора кроме первого
                        .Select((a) => (new SpecialVector(a._date                                   ///Поменять конфигурацию векторов
                            .Where((b, c) => (c != y)))))).determinantor()))                        ///Исключить столбец элемента с номером y
                    .Aggregate(async (x, y) => ((await x) +  (await y)));                           ///Суммирование значений







                /*var tempStruct =*/
                return Expression(matrix._date)
                .Select((matrix) => matrix
                   .Select((sourcevector) => Expression(sourcevector._date)
                   .Select((vector) => (new SpecialVector(vector)))))

                .Select((matrix) => matrix
                    .ElementAt(0)
                    .Select((vector, position) => new Matrix(matrix
                        .Select((vectorCollection) => vectorCollection
                            .ElementAt(position)), module)))
                .Zip(matrix._date
                    .Select(x => x._date), (x, y) => (x
                        .Zip(y, (a, b) => (a, b))))
                .Select((x, y) => (x
                    .Select((matrix, b) => resultexpression(matrix, b, y))))
                .Aggregate(new SpecialInt(0, module), (x, y) => (y
                    .Aggregate(x, ( a, b) => Agregator(a, b.Result))));






                /*var matrixcomplect = tempStruct
                                        .Select((matrix) => matrix
                                            .ElementAt(0)
                                            .Select((vector, position)=> new Matrix (matrix
                                                .Select((vectorCollection) => vectorCollection
                                                    .ElementAt(position)), module)));


                var zippedMatrix = matrixcomplect
                                    .Zip(matrix._date
                                        .Select(x => x._date), (x, y) => (x
                                            .Zip(y, (a, b) => (a, b))));

                

                

                var temp = zippedMatrix
                                    .Select((x, y)=> (x.Select((matrix,b)=>resultexpression(matrix,b,y))));



                

                var summ = temp.Aggregate(new SpecialInt(0, module), (x, y) => (y.Aggregate(x, (a, b) => Agregator(a,b))));


                return summ;*/

            }

            

        }


        /// <summary>
        /// Получения двумерного массива
        /// </summary>
        /// <returns></returns>
        public int[,] GetNumArray ()
        {
            
                int[,] OutNumArray;

                int height = _date.Count();

                int width = _date.FirstOrDefault()._date.Count();

                OutNumArray = new int[height, width];

                var expression = (int[,] array, Matrix matrix) =>
                {
                    var a = matrix._date.Select((vector, num) => (vector._date
                                        .Select((value, vectornum) => (array[num, vectornum] = value.GetNum.Value)).ToArray())).ToArray();
                    return array;
                };

                return expression(OutNumArray, this);
            
        }
        public Matrix Printf(string message)
        {
            Console.WriteLine(message + ":");
            return Printf();
        }
        public Matrix Printf()
        {
            
            foreach (var i in _date)
            {
                foreach (var j in i._date)
                {
                    Console.Write(j.GetNum+" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            return this;
        }

        /// <summary>
        /// Возвращает матрицу обратных значений
        /// </summary>
        public Matrix ReversedMatrix
        {
            
            get
            {
                return new Matrix(_date.Select((item) => (new SpecialVector(item._date.Select((temp) => (temp.Reverse))))));
            }
        }

        /// <summary>
        /// Возвращает матрицу алгебраических дополнений
        /// </summary>
        public Matrix MatrixAlgebryAdded
        {
            get
            {
                return new Matrix(_date
                    .Select((vector, vectornum) => new SpecialVector(vector._date                       ///Получение вектора и номера вектора
                        .Select((element, elnum) => expression(vectornum + elnum) * (new Matrix(_date         ///Получение элемента и номера элемента, умножение новой матрицы на её определитель
                            .Where((a, b) => (b != vectornum))                           ///Удаление выбранного вектора
                        .Select((a) => (new SpecialVector(a._date                     ///
                            .Where((item, b) => b != elnum)))))).Determination()))));

                return new Matrix(_date.Select((item, vectornum) =>
                    (new SpecialVector(item._date.Select((temp, itemnum) =>
                        ((vectornum + itemnum) % 2 == 0 ? temp : (new SpecialInt(module, module) - temp))).Reverse())
                    )).Reverse());
            }
        }


        private SpecialInt PR (SpecialInt a, string message)
        {
            Console.WriteLine(message+" : " + a.GetNum);
            return a;
        }

        
    }

    /// <summary>
    /// Горизонтальная строка зчений матрицы, числовой вектор в остаточной арифметике
    /// </summary>
    
    struct SpecialVector : IComparable<SpecialVector> 
    {
        public SpecialVector(int[] Date, int n)
        {
            _date = Date.Select(temp => (new SpecialInt(temp, n)));
        }

        public SpecialVector(IEnumerable<SpecialInt> date)
        {
            _date = date;
        }

        public IEnumerable<SpecialInt> _date { get; }

        public static SpecialVector operator +(SpecialVector x, SpecialVector y)
        {
            return new SpecialVector(x._date.Zip(y._date, (temp1, temp2) => (temp1 + temp2)));
        }

        public static SpecialVector operator - (SpecialVector x, SpecialVector y)
        {
            return new SpecialVector(x._date.Zip(y._date, (temp1, temp2) => (temp1 - temp2)));
        }

        public int CompareTo(SpecialVector other)
        {
            return _date.Count() - other._date.Count();
            
        }

        public static SpecialInt operator ^ (SpecialVector x, SpecialVector y)
        {
            return x._date.Zip(y._date, (a, b) => a * b).Aggregate((a, b) => a + b);
        }

        
    }
}
