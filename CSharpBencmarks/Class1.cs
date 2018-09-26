using System;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpBencmarks
{
    public static class Benchmarks
    {
        private static async Task<T> Reduce<T>(Func<T, T, T> f, T[] ie, int start, int finish)
        {
            var len = finish - start + 1;
            if (len == 0)
            {
                throw new Exception("Sequence contains no elements");
            }
            else if (len == 1)
            {
                return ie[start];
            }
            else if (len == 2)
            {
                return f(ie[start], ie[start+1]);
            }
            else
            {
                var h = len / 2 + start;
                var o1Task = Reduce(f, ie, start, h-1);
                var o2Task = Reduce(f, ie, h, finish);
                var o1 = await o1Task;
                var o2 = await o2Task;
                return f(o1, o2);
            }
        }
        public static T ReduceParallelTasks<T>(Func<T, T, T> f, T[] ie)
        {
            return Reduce(f, ie, 0, ie.Length-1).Result;
        }
    }
}
