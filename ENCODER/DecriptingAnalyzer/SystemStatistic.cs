using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ENCODER.DecriptingAnalyzer
{

    abstract class StatisticTools
    {

        private class MyComparer<T> : IEqualityComparer<T[]> where T: IComparable<T>
        {
            
            public bool Equals(T[] x, T[] y)
            {


                foreach (var item in x.Zip(y, (a, b) => ((a, b))))
                {
                    if (item.a.CompareTo(item.b) < 0)
                        return false;
                    else 
                        return true;
                }

                return false;
                
            }

            public int GetHashCode(T[] obj)
            {
                return obj.GetHashCode();
            }
        }

        static public Dictionary<T[], int> GetElemnts<T>(T[] input, int size) where T : IComparable<T>
        {
            var re = input.Chunk(size).GroupBy(x => x, new MyComparer<T>()).Select(x=> KeyValuePair.Create(x.Key, x.Count())).ToDictionary();

            return re;

            var result = (from value in input.Chunk(size)
                          group value by value into symbolgroups 
                          orderby symbolgroups.Count() descending
                          select KeyValuePair.Create(symbolgroups.Key, symbolgroups.Count())).ToDictionary();

            return result;
        }

        static public Dictionary<string, int> GetElemnts(string input, int size)
        {
            var result = (from value in input.Chunk(size).Select(x=>new string(x))
                          group value by value into symbolgroups
                          orderby symbolgroups.Count() descending
                          select KeyValuePair.Create(symbolgroups.Key, symbolgroups.Count())).ToDictionary();

            return result;
        }



        static public Dictionary<R[], int> ConvertToResultDictionary<T, R>(Dictionary<T[], int> inputDictionary, Func<T, R> function)
        {
            return inputDictionary
                .Select(x=>KeyValuePair
                    .Create(x.Key
                        .Select(a=>function(a))
                        .ToArray(), x.Value))
                .ToDictionary();
        }

        static public Dictionary<R, int> ConvertToResultDictionary<T, R>(Dictionary<T[], int> inputDictionary, Func<T[], R> function)
        {
            return inputDictionary.Select(x => KeyValuePair.Create(function(x.Key), x.Value)).ToDictionary();
        }
    }

    class SystemStatistic<T,R>: StatisticTools 
        where T : IEnumerable<char> 
        where R : IEnumerable<int>
    {
        public SystemStatistic(Func<T, R> function)
            :this((GetStatistic)(x=>function(x)))
        {

        }

        private SystemStatistic(GetStatistic function)
        {
            _function = function;
        }

        

        delegate R GetStatistic(T input);

        private GetStatistic _function { get; }

    }
}
