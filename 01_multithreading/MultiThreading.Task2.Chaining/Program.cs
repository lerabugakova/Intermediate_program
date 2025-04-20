/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            Chain();

            Console.ReadLine();
        }

        private static void Chain()
        {
            var rand = new Random();

            var task1 = Task.Run(() =>
            {
                var numbers = new int[10];
                for (int i = 0; i < numbers.Length; i++)
                {
                    numbers[i] = rand.Next(1, 101);
                }

                Console.WriteLine("Task 1 - Initial Array: " + string.Join(", ", numbers));
                return numbers;
            });

            var task2 = task1.ContinueWith(prevTask =>
            {
                int[] numbers = prevTask.Result;
                int multiplier = rand.Next(2, 11); 

                int[] multiplied = numbers.Select(n => n * multiplier).ToArray();
                Console.WriteLine($"Task 2 - Multiplied by {multiplier}: " + string.Join(", ", multiplied));
                return multiplied;
            });

            var task3 = task2.ContinueWith(prevTask =>
            {
                int[] numbers = prevTask.Result;
                Array.Sort(numbers);
                Console.WriteLine("Task 3 - Sorted Array: " + string.Join(", ", numbers));
                return numbers;
            });

            var task4 = task3.ContinueWith(prevTask =>
            {
                int[] numbers = prevTask.Result;
                double average = numbers.Average();
                Console.WriteLine($"Task 4 - Average Value: {average:F2}");
            });

            task4.Wait();

            Console.WriteLine("All tasks completed.");
        }
    }
}
