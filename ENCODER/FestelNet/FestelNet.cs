using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENCODER.FestelNet
{

    interface IFestelNet<T> where T: IConvertible
    {

        IEnumerable<T> OpenValueStream(IEnumerable<T> input);

        IEnumerable<T> CodedValueStream(IEnumerable<T> input);

    }

    interface IFestelNetAndStaticMethod<T> : IFestelNet<T> where T : IConvertible
    {
        static abstract IFestelNet<T> Create<R, Operation>(int count, Func<int, T> Transformer)
            where R : IFestelRoundAndStaticMethod<T>
            where Operation : IOperationBlockAndStaticMethod<T>;
    }


    class FestelNet<T> : IFestelNetAndStaticMethod<T> where T: IConvertible
    {

        public FestelNet(IEnumerable<IFestelRound<T>> roundvalues) 
        {
            round = roundvalues.ToArray();
        }

        protected readonly IFestelRound<T>[] round;

        public static IFestelNet<T> Create<R, Operation>(int count, Func<int, T> Transformer) 
            where R : IFestelRoundAndStaticMethod<T> 
            where Operation : IOperationBlockAndStaticMethod<T>
        {
            var temp = (new IFestelRound<T>[count]).Select(x=>(R.Create<Operation, IEnumerable<T>>(Transformer)));

            return new FestelNet<T>(temp);
        }

        /// <summary>
        /// Агрегирует массив полученныъ значений с функцией
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> OpenValueStream(IEnumerable<T> input)
        {
            var s9 = input.ToArray();

            T s0 = input.First();
            int size = s0 switch
            {
                (bool s1) => (64),
                (int s2) => (2),
                (byte s3) => (8),
                _ => 0
            };

            var temps = input.Chunk(size);

            var finnaly = temps.Select(group => (round.Aggregate(group.Select(x => x), (x, y) => (y.Code(x).ToArray()))));

            return finnaly.SelectMany(x=>x);

            var temp = round.Aggregate(input, (y, x) => (x.Code(y)));

            return (temp.Skip(temp.Count() / 2)).Concat(temp.Take(temp.Count()/2));

        }

        public virtual IEnumerable<T> CodedValueStream(IEnumerable<T> input)
        {

            T s0 = input.First();
            int size = s0 switch
            {
                (bool s1) => (64),
                (int s2) => (2),
                (byte s3) => (8),
                _ => 0
            };

            var temps = input.Chunk(size);

            var finnaly = temps
                .Select(group => (round
                    .Reverse()
                    .Aggregate((group
                        .Skip(group.Count() / 2)
                        .Concat(group.Take(group.Count() / 2)))
                .Select(x => x), (x, y) => (y
                    .Code(x))
                    .ToArray())))
                .Select(i => i.Skip(i.Count() / 2).Concat(i.Take(i.Count() / 2)));
                

            return finnaly.SelectMany(x=>x);


            var temp = round.Reverse().Aggregate(input, (y, x) => (x.Code(y)));

            return (temp.Skip(temp.Count() / 2)).Concat(temp.Take(temp.Count() / 2));

        }
    }




}
