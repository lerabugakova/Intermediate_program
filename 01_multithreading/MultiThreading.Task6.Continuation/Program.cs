/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            Case1_AlwaysRuns();
            Case2_OnlyIfNotSuccessful();
            Case3_OnlyOnFault_ReuseThread();
            Case4_OnlyOnCanceled_OutsideThreadPool();

            Console.ReadLine();
        }

        static void Case1_AlwaysRuns()
        {
            Console.WriteLine("Case 1: Continuation always runs");

            var parent = Task.Run(() =>
            {
                Console.WriteLine("  [Case1] Parent running");
                Thread.Sleep(300);
            });

            parent.ContinueWith(t =>
            {
                Console.WriteLine("  [Case1] Continuation ran regardless of result");
            }, TaskContinuationOptions.None);
        }

        static void Case2_OnlyIfNotSuccessful()
        {
            Console.WriteLine("\nCase 2: Continuation runs only if parent NOT successful");

            var parent = Task.Run(() =>
            {
                Console.WriteLine("  [Case2] Parent running");
                throw new Exception("Error");
            });

            parent.ContinueWith(t =>
            {
                Console.WriteLine("  [Case2] Continuation triggered due to failure or cancellation");
            }, TaskContinuationOptions.NotOnRanToCompletion);
        }

        static void Case3_OnlyOnFault_ReuseThread()
        {
            Console.WriteLine("\nCase 3: Continuation on failure and reuse parent thread");

            Task parent = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"  [Case3] Parent running on thread {Thread.CurrentThread.ManagedThreadId}");
                throw new InvalidOperationException("Error");
            });

            parent.ContinueWith(t =>
            {
                Console.WriteLine($"  [Case3] Continuation ran on thread {Thread.CurrentThread.ManagedThreadId} (reused parent thread)");
            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
        }

        static void Case4_OnlyOnCanceled_OutsideThreadPool()
        {
            Console.WriteLine("\nCase 4: Continuation on cancellation (outside thread pool)");

            var cts = new CancellationTokenSource();

            Task parent = Task.Run(() =>
            {
                Console.WriteLine("  [Case4] Parent started");
                Thread.Sleep(200);
                cts.Token.ThrowIfCancellationRequested();
            }, cts.Token);

            Task continuation = parent.ContinueWith(t =>
            {
                Console.WriteLine($"  [Case4] Continuation on cancellation ran on thread {Thread.CurrentThread.ManagedThreadId} (LongRunning)");
            },
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning,
            TaskScheduler.Default);

            cts.Cancel();
        }
    }
}
