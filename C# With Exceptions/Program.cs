using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Desc2
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();

            Console.Write("Количество уровнений: ");
            int count = Convert.ToInt32(Console.ReadLine());

            const int minValue = -1000, maxValue = 1000;

            Variables[] variables = new Variables[count];


            for (int i = 0; i < count; i++)
            {
                variables[i] = new Variables(Convert.ToDouble(random.Next(minValue, maxValue)),
                                             Convert.ToDouble(random.Next(minValue, maxValue)),
                                             Convert.ToDouble(random.Next(minValue, maxValue)));
            }



            double rootsSum = default;
            int countExceptions = default;

            var stopWatch = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                try
                {
                    Roots roots = CalculateRoots(ref variables[i]);

                    rootsSum += roots.x1 + roots.x2;
                }
                catch (RootsException)
                {
                    countExceptions++;
                }
            }
            stopWatch.Stop();
            Console.WriteLine($"Сумма корней: {rootsSum}");
            Console.WriteLine($"количество исключений: {countExceptions}");
            Console.WriteLine($"Время последовательного выполнения: {stopWatch.ElapsedMilliseconds} мс.");

            rootsSum = default;
            countExceptions = default;


            stopWatch = Stopwatch.StartNew();
            Parallel.For(0, count, i =>
            {
                try
                {
                    Roots roots = CalculateRoots(ref variables[i]);

                    double sum = roots.x1 + roots.x2;

                    Add(ref rootsSum, sum);
                }
                catch (RootsException)
                {
                    Interlocked.Increment(ref countExceptions);
                }
            });
            stopWatch.Stop();
            Console.WriteLine($"Сумма корней: {rootsSum}");
            Console.WriteLine($"количество исключений: {countExceptions}");
            Console.WriteLine($"Время параллельного выполнения: {stopWatch.ElapsedMilliseconds} мс.");
        }

        private static Roots CalculateRoots(ref Variables variables)
        {
            if (Compare(variables.a, 0))
            {
                if (Compare(variables.b, 0))
                {
                    if (Compare(variables.c, 0))
                    {
                        throw new RootsException("Ошибка при вычислении корней");
                    }
                    return new Roots();
                }
                else
                {
                    return new Roots(-variables.c / variables.b);
                }
            }
            double d = variables.b * variables.b - 4 * variables.a * variables.c;

            if (Compare(d, 0))
            {
                return new Roots(-variables.b / (2 * variables.a));
            }

            if (d < 0)
            {
                return new Roots();
            }

            return new Roots((-variables.b - Math.Sqrt(d)) / (2 * variables.a),
                             (-variables.b + Math.Sqrt(d)) / (2 * variables.a));
        }
        private static double Add(ref double location, double value)
        {
            double newCurrentValue = location;
            while (true)
            {
                double currentValue = newCurrentValue;
                double newValue = currentValue + value;
                newCurrentValue = Interlocked.CompareExchange(ref location, newValue, currentValue);

                if (newCurrentValue == currentValue)
                    return newValue;
            }
        }
        static bool Compare(double a, double b, double eps = 0.0000001)
        {
            return Math.Abs(a - b) < eps;
        }
    }
}
