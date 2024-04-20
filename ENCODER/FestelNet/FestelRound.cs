using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ENCODER.FestelNet
{

    interface IFestelRoundAndStaticMethod<T>: IFestelRound<T> where T : IConvertible
    {
        static abstract IFestelRound<T> Create<Operation, R>(Func<int, T> Transformer)
            where R : IEnumerable<T>
            where Operation : IOperationBlockAndStaticMethod<T>;
    }

    interface IFestelRound<T> 
        where T :IConvertible
    {
        public IEnumerable<T> Code(IEnumerable<T> input);

    }


    class FestelRound<T>: IFestelRoundAndStaticMethod<T> 
        where T : IConvertible
    {

        public FestelRound(Func<int, T> Transformer, IOperationBlock<T> Function)
        {
            transformer = Transformer;
            operation = Function;
        }

        static public IFestelRound<T> Create<Operation, R>(Func<int, T> Transformer) 
            where R: IEnumerable<T> 
            where Operation: IOperationBlockAndStaticMethod<T>
        {
            IOperationBlock<T> block = Operation.Create(3, Transformer);

            return new FestelRound<T>(Transformer, block);
        }

        readonly private IOperationBlock<T> operation;

        readonly private Func<int, T> transformer;

        public virtual IEnumerable<T> Code (IEnumerable<T> input)
        {
            var first = input.Take(input.Count() / 2);
            var second = input.Skip(input.Count() / 2);

            var tempsecond = operation.Operation(second);

            //Console.WriteLine($"{first.Aggregate("First: ", (a, b) => (a + Convert.ToString(b) + " "))} \t {second.Aggregate("Second:", (a, b) => (a + Convert.ToString(b) + " "))}");

            //Console.WriteLine($"{tempsecond.Aggregate("Temp: ", (a, b) => (a + Convert.ToString(b) + " "))}");

            ///Xor Block
            first = first.Zip(tempsecond, (x, y) => (transformer(Convert.ToInt32(x) ^ Convert.ToInt32(y)))).ToArray();

            //Console.WriteLine($"First {first.Aggregate("First: ",(a,b)=>(a+Convert.ToString(b)+" "))} \t {second.Aggregate("Second: ", (a, b) => (a + Convert.ToString(b) + " "))}");

            return second.Concat(first);
        }


    }
}
