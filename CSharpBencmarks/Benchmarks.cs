using System;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpBencmarks
{
    public static class Benchmarks
    {
        private static async Task<T> ReduceAsyncAwait<T>(Func<T, T, T> f, T[] ie, int start, int finish)
        {
            var len = finish - start + 1;
            if (len == 0)
            {
                throw new Exception("Sequence contains no elements");
            }

            if (len == 1)
            {
                return ie[start];
            }

            if (len == 2)
            {
                return f(ie[start], ie[start+1]);
            }

            var h = len / 2 + start;
            var o1Task = ReduceAsyncAwait(f, ie, start, h-1);
            var o2Task = ReduceAsyncAwait(f, ie, h, finish);
            var o1 = await o1Task;
            var o2 = await o2Task;
            return f(o1, o2);
        }
        
        public static T ReduceAsyncAwait<T>(Func<T, T, T> f, T[] ie)
        {
            return ReduceAsyncAwait(f, ie, 0, ie.Length-1).GetAwaiter().GetResult();
        }
        
        private static Task<T> ReduceTPL<T>(Func<T, T, T> f, T[] ie, int start, int finish)
        {
            var len = finish - start + 1;
            if (len == 0)
            {
                throw new Exception("Sequence contains no elements");
            }

            if (len == 1)
            {
                return Task.FromResult(ie[start]);
            }

            if (len == 2)
            {
                return Task.FromResult(f(ie[start], ie[start+1]));
            }

            var h = len / 2 + start;
            var o1Task = ReduceTPL(f, ie, start, h-1);
            var o2Task = ReduceTPL(f, ie, h, finish);
            var o1 = o1Task.GetAwaiter().GetResult();
            var o2 = o2Task.GetAwaiter().GetResult();
            return Task.FromResult(f(o1, o2));
        }
        
        public static T ReduceTPL<T>(Func<T, T, T> f, T[] ie)
        {
            return ReduceTPL(f, ie, 0, ie.Length-1).GetAwaiter().GetResult();
        }
        
        private static ValueTask<T> ReduceVT<T>(Func<T, T, T> f, T[] ie, int start, int finish)
        {
            var len = finish - start + 1;
            if (len == 0)
            {
                throw new Exception("Sequence contains no elements");
            }

            if (len == 1)
            {
                return new ValueTask<T>(ie[start]);
            }

            if (len == 2)
            {
                return new ValueTask<T>(f(ie[start], ie[start+1]));
            }

            var h = len / 2 + start;
            var o1Task = ReduceVT(f, ie, start, h-1);
            var o2Task = ReduceVT(f, ie, h, finish);
            var o1 = o1Task.Result;
            var o2 = o2Task.Result;
            return new ValueTask<T>(f(o1, o2));
        }
        
        public static T ReduceVT<T>(Func<T, T, T> f, T[] ie)
        {
            return ReduceVT(f, ie, 0, ie.Length-1).Result;
        }
        
        private static async ValueTask<T> ReduceVTPattern<T>(Func<T, T, T> f, T[] ie, int start, int finish)
        {
            var len = finish - start + 1;
            if (len == 0)
            {
                throw new Exception("Sequence contains no elements");
            }

            if (len == 1)
            {
                return ie[start];
            }

            if (len == 2)
            {
                return f(ie[start], ie[start+1]);
            }

            var h = len / 2 + start;
            var o1Task = ReduceVTPattern(f, ie, start, h-1);
            var o2Task = ReduceVTPattern(f, ie, h, finish);
            var o1 = o1Task.IsCompletedSuccessfully ? o1Task.Result : await o1Task.AsTask();
            var o2 = o2Task.IsCompletedSuccessfully ? o2Task.Result : await o2Task.AsTask();
            return f(o1, o2);
        }
        
        public static T ReduceVTPattern<T>(Func<T, T, T> f, T[] ie)
        {
            return ReduceVTPattern(f, ie, 0, ie.Length-1).Result;
        }
        
        private static T ReduceTaskRun<T>(Func<T, T, T> f, T[] ie, int start, int finish)
        {
            var len = finish - start + 1;
            if (len == 0)
            {
                throw new Exception("Sequence contains no elements");
            }

            if (len == 1)
            {
                return ie[start];
            }

            if (len == 2)
            {
                return f(ie[start], ie[start + 1]);
            }

            var h = len / 2 + start;
            var o1Task = Task.Factory.StartNew(() => ReduceTaskRun(f, ie, start, h-1), TaskCreationOptions.RunContinuationsAsynchronously);
            var o2Task = Task.Factory.StartNew(() => ReduceTaskRun(f, ie, h, finish), TaskCreationOptions.RunContinuationsAsynchronously);
            var o1 = o1Task.GetAwaiter().GetResult();
            var o2 = o2Task.GetAwaiter().GetResult();
            return f(o1, o2);
        }
        
        public static T ReduceTaskRun<T>(Func<T, T, T> f, T[] ie)
        {
            return Task.Factory.StartNew(() => ReduceTaskRun(f, ie, 0, ie.Length - 1), TaskCreationOptions.RunContinuationsAsynchronously).GetAwaiter().GetResult();
        }
    }
}
