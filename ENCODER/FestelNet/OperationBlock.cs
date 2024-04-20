using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace ENCODER.FestelNet
{

    interface IOperationBlock<T> 
        where T: IConvertible
    {

        public IEnumerable<T> Operation(IEnumerable<T> date);


    }

    interface IOperationBlockAndStaticMethod<T> : IOperationBlock<T> 
        where T : IConvertible
    {
        public abstract static IOperationBlock<T> Create(int key, Func<int, T> Transformer);
    }

    class SOperationBlock<T> : IOperationBlockAndStaticMethod<T>
        where T : IConvertible
        
    {
        public SOperationBlock (int key, Func<int, T> Transformer)
        {

            transformer = (IEnumerable<T> x) => ((_functionToInt(x, key)).Select(x => Transformer(x)));
            _transformator = Transformer;
        }

        protected Func<int, T> _transformator;

        public static IOperationBlock<T> Create(int key, Func<int, T> Transformer)
        {
            return new SOperationBlock<T>(key, Transformer);
        }

        private Func<IEnumerable<T>, IEnumerable<T>> transformer;
        private readonly static Func<IEnumerable<T>, int, IEnumerable<int>> _functionToInt = (value, i) => value
            .Select( x=> ((Convert.ToInt32(x)>>2 | i) & Convert.ToInt32(x)));

        public virtual IEnumerable<T> Operation(IEnumerable<T> date)
        {
            return transformer(date);
        }
    }

    
    
    

    class FestelOperationBlock<T> : SOperationBlock<T>, IOperationBlockAndStaticMethod<T> where T : IConvertible
    {
        public FestelOperationBlock(byte[] key, Func<int, T> Transformer)
            : base(key
                  .Select<byte, (byte value, int num)>((x, y) => (x, y))
                  .Aggregate(0, (x, y)=>(x+(y.value<<(y.num*4)))) ,
                  Transformer)
        {
            if (key.Length < 6)
            {
                throw new Exception("Короткий ключ");
            }
            _key = key;
        }

        public static IOperationBlock<T> Create(byte[] key, Func<int, T> Transformer)
        {
            return new FestelOperationBlock<T>(key, Transformer);
        }

        private byte[] _key;


        //private static int _functionToInt(T value, int key)
        //{

        //}

        /// <summary>
        /// Таблица сужения
        /// </summary>
        readonly static byte[,,] STables =
            {
                {
                    {
                        14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7,
                    },
                    {
                        0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8,
                    },
                    {
                        4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0,
                    },
                    {
                        15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13,
                    },
                },
                {
                    {
                        15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10,
                    },
                    {
                        3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5,
                    },
                    {
                        0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15,
                    },
                    {
                        13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9,
                    },
                },
                {
                    {
                        10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8,
                    },
                    {
                        13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1,
                    },
                    {
                        13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7,
                    },
                    {
                        1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12,
                    },
                },
                {
                    {
                        7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15,
                    },
                    {
                        13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9,
                    },
                    {
                        10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4,
                    },
                    {
                        3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14,
                    },
                },
                {
                    {
                        2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9,
                    },
                    {
                        14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6,
                    },
                    {
                        4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14,
                    },
                    {
                        11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3,
                    },
                },
                {
                    {
                        12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11,
                    },
                    {
                        10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8,
                    },
                    {
                        9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6,
                    },
                    {
                        4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13,
                    },
                },
                {
                    {
                        4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1,
                    },
                    {
                        13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6,
                    },
                    {
                        1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2,
                    },
                    {
                        6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12,
                    },
                },
                {
                    {
                        13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7,
                    },
                    {
                        1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2,
                    },
                    {
                        7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8,
                    },
                    {
                        2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11,
                    },
                } 
        };

        /// <summary>
        /// Функция расширения 4 бита -> 6 бит, меняет порядок и дублирует элементы
        /// </summary>
        /// <param name="args"></param>
        /// <returns>
        /// Возврат массива bool
        /// </returns>
        /// <remarks>
        /// Ремарка
        /// </remarks>
        /// <example>
        /// Пример
        /// </example>
        private static IEnumerable<bool> Generator (bool[] args)
        {
            const int hight = 8;
            const int lenght = 6;
            const int start = 32;

            int accamulator = 2;
            int pointer = 0;
            int tempmemory = 0;

            T buffer;

            //Console.WriteLine();
            //Console.WriteLine((args[31] ? 1 : 0) + " ^ " + 32 + " : " + accamulator);

            yield return args[31];

            while (accamulator < 48)
            {


                //Console.WriteLine((args[pointer]?1:0) + " ^ " + (pointer+1)+ " : "+accamulator);
                yield return args[pointer];

                accamulator++;

                pointer++;

                    

                if (accamulator%lenght==0)
                {
                    if (pointer == 32)
                        break;

                    //Console.WriteLine((args[pointer]?1: 0) + " ^ " + (pointer +1) + " : " + accamulator);
                    accamulator++;
                    //Console.WriteLine((args[tempmemory] ? 1 : 0) + " ^ " + (tempmemory+ 1) + " : " + accamulator);
                    accamulator++;
                    yield return args[pointer];
                    yield return args[tempmemory];
                }
                tempmemory = pointer;
            }


            //Console.WriteLine((args[0]?1: 0) + " ^ " + 1 + " : " + accamulator);

            yield return args[0];
            yield break;

        }

        /// <summary>
        /// Расширяет объект с int 4 в группы по 6
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static bool[] getExpandedArray(int input)
        {
            
           BitVector32 group = new BitVector32(input);

            bool[] bools = new bool[32];
            int[] ints = new int[32];

            ints = ints.Select((x,y)=>(y==0? BitVector32.CreateMask(): 1<<y)).ToArray();
            bools = ints.Select((x, y) => group[x]).ToArray();

            IEnumerable<bool> ExpensainBools = Generator(bools).ToArray();


            return ExpensainBools.ToArray();
        }

        /// <summary>
        /// Складывает ключ и минимизирует аргументы
        /// </summary>
        /// <param name="key"> Первый параметр, ключ, 8 групп по 6 бит</param>
        /// <param name="input"> входное значение 32 бита</param>
        /// <returns>Выходное значение 32 бита</returns>
        private static int Minimazier (byte[] key, int input)
        {

            IEnumerable<byte> valueInBytes = getExpandedArray(input)
                .Chunk(8)
                .Select(x => x
                    .Aggregate(0, (a, b) => ((a << 1) + (b ? 1 : 0)), (num) => ((byte)num)));

            byte[] XorResult = valueInBytes.Zip(key, (x, y) => (byte)(x | y))
                .Select(x => (new bool[8]
                    .Select((i, n) => ((x >> n) % 2 == 1 ? true : false)))
                    .Reverse())
                    .SelectMany(x=>x)
                .Chunk(6).Select(x=>x
                    .Aggregate((byte)0,(s, item)=>(byte)((s<<1)+(item?1:0))))
                .ToArray();

            

            var invalue = XorResult.Select((x, y) => (STables[y, (x >> 5) * 2 + x % 2, ((x >> 4) % 2 * 8) + ((x >> 3) % 2 * 4) + ((x >> 2) % 2 * 2) + ((x >> 1) % 2)])).Chunk(2).Select<byte[], byte>(x => (byte)((x[0] << 4) + x[1])).ToArray();

            int result = invalue
                .Select<byte, (byte value, int number)>((x, y) => (x, y))
                .Aggregate(0, (a, b)=>(a+(b.value<<(b.number*4))));

            return result;
        }

        /// <summary>
        /// Шифратор
        /// </summary>
        /// <param name="date"> Коллекция элементов</param>
        /// <returns>
        /// Половина кодового слова
        /// </returns>
        public override IEnumerable<T> Operation( IEnumerable<T> date)
        {
            IEnumerable<int> temp;


            switch (date.First())
                {
                case int s1:
                    {
                        temp = date.Cast<int>();
                        break;
                    }
                case byte s2:
                    {
                        temp = date.Chunk(4).Select(i => i.Cast<byte>().Select((x, y) => (x<<y*8)).Sum());
                        break;
                    }
                case bool s3:
                    {
                        temp = date.Chunk(32).Select(i=>i.Cast<bool>().Reverse().Select((x, y)=>x?1<<y:0).Sum());
                        break;
                    }
                default:
                    temp = null;
                    throw new Exception("Неизвестный тип "+typeof(T));
                    break;
                }

            /// Элемент превращается в набор битов и складывается с ключом
            IEnumerable<T> result = (temp.Select((x)=>_transformator( Minimazier(_key, x))));

            return result;
        }


    }
}
