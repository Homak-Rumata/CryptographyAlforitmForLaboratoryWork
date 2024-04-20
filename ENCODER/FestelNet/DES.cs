using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace ENCODER.FestelNet
{

    interface IDES<T>: IFestelNet<T> where T : IConvertible
    {

    }

    interface IDESAndStaticMethod<T> : IDES<T>, IFestelNetAndStaticMethod<T> where T : IConvertible
    {

    }


    class DES<T> : FestelNet<T>, IDESAndStaticMethod<T> where T : IConvertible
    {

        /// <summary>
        /// Конструктор 
        /// </summary>
        /// <param name="roundvalues"></param>
        /// <param name="netral"></param>
        public DES(IEnumerable<IFestelRound<T>> roundvalues, T netral, Func<int, T> translator)
            : base(roundvalues)
        {
            this.translator = translator;
            this.netral = netral;
        }

        static DES ()
        {
            ReversedFirstTabbleDictionary = Enumerable.Range(0, 64).Select((i, n) => {
                return new KeyValuePair<int, int>((((7 - (n % 8)) * 8) + (((n / 8) % 4 + 1) * 2 - (n / 32))), n+1);
            }).ToDictionary();

            FistTabbleDictionary = ReversedFirstTabbleDictionary.Select(x => new KeyValuePair<int, int>(x.Value, x.Key)).ToDictionary();
        }

        private Func<int, T> translator;

       /// <summary>
       /// Нейтральный элемент
       /// </summary>
        private T netral;

        static private Dictionary<int, int> ReversedFirstTabbleDictionary;
        static private Dictionary<int, int> FistTabbleDictionary;

        private static int counter = 0;

        /// <summary>
        /// Таблица миксер
        /// </summary>
        private static IEnumerable<R> ReplaceFunction<R> (IEnumerable<R> x, R netral) 
                {

                    if (x.Count() < 64)
                    {
                        x = Enumerable.Repeat(netral, 64 - x.Count()).Concat(x);
                    }
                    var tempDictionary = x.Select((x, num) => new KeyValuePair<int, R>(num+1, x)).ToDictionary();
            
                var result = Enumerable.Range(0, tempDictionary.Count).Select(x => tempDictionary[FistTabbleDictionary[x + 1]]).ToArray();
                return result;

                }

        private static IEnumerable<R> ReversedReplaceFunction<R>(IEnumerable<R> x, R netral)
        {
            if (x.Count() < 64)
            {
                x = Enumerable.Repeat(netral, 64 - x.Count()).Concat(x);
            }

            var tempDictionary = x.Select((x, num) => new KeyValuePair<int, R>(num+1, x)).ToDictionary();
            return Enumerable.Range(0, tempDictionary.Count).Select(x => tempDictionary[ReversedFirstTabbleDictionary[x + 1]]);
        }


        /// <summary>
        /// Шифратор
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override IEnumerable<T> CodedValueStream(IEnumerable<T> input)
        {
            Console.WriteLine("\n____________________\n");
            T[] AddedArray;

            IEnumerable<T> compaarateInput;

            Func<int, int, bool[]> BoolConverter = (item, size) =>
            {
                return Enumerable.Range(0, size).Select(x => ((item >> x) % 2) == 1 ? true : false).Reverse().ToArray();
            };

            /// <summary>
            /// Число в массив битов
            /// </summary>
            /// <param name="intvalue"></param>
            /// <param name="lenght"></param>
            /// <param name="size"></param>
            /// <returns></returns>
            Func<IEnumerable<T>, int, IEnumerable<bool[]>> converter = (IEnumerable<T> intvalue, int size) =>
            {

                return intvalue
                    .Chunk(size)
                    .Select(x => x.Cast<int>()
                        .Select<int, IEnumerable<bool>>((i) => BoolConverter(i, 64 / size)).SelectMany(x => x).ToArray());

            };


            switch (input.First())
            {



                case bool t1:
                    {
                        compaarateInput = input.Chunk(64).Select(x => ReversedReplaceFunction(x, netral)).SelectMany(x => x);
                        break;
                    }
                case int t2:
                    {

                        ///Добавить выход из таблицы
                        var temp = converter(input, 2).SelectMany(x => x);
                        compaarateInput = temp
                            .Chunk(64)
                            .Select(x => x.Chunk(32).Select((item) => (item.Aggregate(0, (a, b) => ((a << 1) + (b ? 1 : 0))))))
                            .SelectMany(x => x)
                            .Select(item => translator(item));
                        break;
                    }
                case byte t2:
                    {
                        var temp = converter(input, 8).Select(x => ReversedReplaceFunction(x, false)).SelectMany(x => x);
                        compaarateInput = temp
                            .Chunk(64)
                            .Select(x => x.Chunk(8).Select((item) => (item.Aggregate(0, (a, b) => ((a << 1) + (b ? 1 : 0))))))
                            .SelectMany(x => x).Select(item => translator(item));
                        break;
                    }

                default:
                    {
                        throw new Exception("Недопустимый тип в выражении");
                    }

            }



            var val1 = base.CodedValueStream(compaarateInput).ToArray();
            var val2 = converter(val1, 2).ToArray();
            var val3 = val2.Select(item => ReversedReplaceFunction<bool>(item, false).ToArray());
            var val4 = val3.Select(item => item.Chunk(32).ToArray().Select(num => translator(num.Aggregate(0, (x, y) => (x << 1) + (y ? 1 : 0))))).SelectMany(x => x).ToArray();
            return val4;

            
        }

        /// <summary>
        /// Дешифратор
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override IEnumerable<T> OpenValueStream(IEnumerable<T> input)
        {
            T[] AddedArray;

            IEnumerable<T> compaarateInput;

            Func<int, int, bool[]> BoolConverter = (item, size) =>
            {
                return Enumerable.Range(0, size).Select(x => ((item >> x) % 2) == 1 ? true : false).Reverse().ToArray();
            };

            /// <summary>
            /// Число в массив битов
            /// </summary>
            /// <param name="intvalue"></param>
            /// <param name="lenght"></param>
            /// <param name="size"></param>
            /// <returns></returns>
            Func<IEnumerable<T>, int, IEnumerable<bool[]>> converter = (IEnumerable<T> intvalue, int size) =>
            {

                var result = intvalue
                    .Chunk(size)
                    .Select(x => x.Cast<int>()
                        .Select<int, IEnumerable<bool>>((i) => BoolConverter(i, 64 / size)).SelectMany(x => x).ToArray());

                return result;

            };



            switch (input.First())
            {
                


                case bool t1:
                    {
                        compaarateInput = input.Chunk(64).Select(x => ReplaceFunction(x, netral)).SelectMany(x => x);
                        break;
                    }
                case int t2:
                    {
                        var temp2 = converter(input, 2);
                        var temp1 = temp2.Select(x => ReplaceFunction(x, false).ToArray()).ToArray();
                        var temp = temp1.SelectMany(x => x);
                        compaarateInput = temp
                            .Chunk(64)
                            .Select(x => x.Chunk(32).Select((item) => (item.Aggregate(0, (a, b) => ((a << 1) + (b ? 1 : 0))))))
                            .SelectMany(x => x)
                            .Select(item => translator(item)).ToArray();

                        break;
                    }

                case byte t2:
                    {
                        var temp = converter(input, 8).Select(x => ReplaceFunction(x, false)).SelectMany(x => x);
                        compaarateInput = temp
                            .Chunk(64)
                            .Select(x => x.Chunk(8).Select((item) => (item.Aggregate(0, (a, b) => ((a << 1) + (b ? 1 : 0))))))
                            .SelectMany(x => x).Select(item => translator(item));
                        break;
                    }

                default:
                    {
                        throw new Exception("Недопустимый тип в выражении");
                    }

            }

            

            return base.OpenValueStream(compaarateInput);

        }



    }
}
