/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static List<int> sharedList = new List<int>();
        static object lockObj = new object();
        static AutoResetEvent itemAddedEvent = new AutoResetEvent(false);
        static bool addingCompleted = false;

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var producer = new Thread(AddItems);
            var consumer = new Thread(PrintItems);

            producer.Start();
            consumer.Start();

            producer.Join();
            consumer.Join();

            Console.WriteLine("Task 1 is done.");

            Console.ReadLine();
        }

        static void AddItems()
        {
            for (int i = 1; i <= 10; i++)
            {
                Thread.Sleep(300);

                lock (lockObj)
                {
                    sharedList.Add(i);
                }

                itemAddedEvent.Set();
            }

            addingCompleted = true;
            itemAddedEvent.Set();
        }

        static void PrintItems()
        {
            while (true)
            {
                itemAddedEvent.WaitOne();

                lock (lockObj)
                {
                    Console.WriteLine($"[{string.Join(", ", sharedList)}]");
                }

                if (addingCompleted && sharedList.Count == 10)
                    break;
            }
        }
    }
}
