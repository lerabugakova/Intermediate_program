/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            StartThreadRecursive(10);
            ThreadWork(10);

            Console.ReadLine();
        }

        static void StartThreadRecursive(int value)
        {
            if (value <= 0)
                return;

            var thread = new Thread(state =>
            {
                int current = (int)state;
                int next = current - 1;
                Console.WriteLine($"Thread with value: {current}");

                if (next > 0)
                {
                    StartThreadRecursive(next);
                }
            });

            thread.Start(value);
            thread.Join(); 
        }

        static SemaphoreSlim semaphore = new SemaphoreSlim(0);
        static void ThreadWork(object state)
        {
            int value = (int)state;
            Console.WriteLine($"ThreadPool thread with value: {value}");

            if (value > 1)
            {
                ThreadPool.QueueUserWorkItem(ThreadWork, value - 1);
            }
            else
            {
                semaphore.Release();
            }
        }
    }
}
