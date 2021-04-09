using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Dec3
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

            var stopWatch = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                try
                {
                    CalculateRoots(ref variables[i]);
                }
                catch (RootsException e)
                {
                    if (e.Message == "")
                        rootsSum += e.Roots.x1 + e.Roots.x2;
                }
            }
            stopWatch.Stop();
            Console.WriteLine($"Сумма корней: {rootsSum}");
            Console.WriteLine($"Время последовательного выполнения: {stopWatch.ElapsedMilliseconds} мс.");

            rootsSum = default;

            stopWatch = Stopwatch.StartNew();
            Parallel.For(0, count, i =>
            {
                try
                {
                    CalculateRoots(ref variables[i]);
                }
                catch (RootsException e)
                {
                    if (e.Message == "")
                    {
                        double sum = e.Roots.x1 + e.Roots.x2;

                        Add(ref rootsSum, sum);
                    }
                }

            });
            stopWatch.Stop();
            Console.WriteLine($"Сумма корней: {rootsSum}");
            Console.WriteLine($"Время параллельного выполнения: {stopWatch.ElapsedMilliseconds} мс.");
        }

        private static void CalculateRoots(ref Variables variables)
        {
            if (Compare(variables.a, 0))
            {
                if (Compare(variables.b, 0))
                {
                    if (Compare(variables.c, 0))
                    {
                        throw new RootsException("Ошибка при вычислении корней");
                    }
                    throw new RootsException("", new Roots());
                }
                else
                {
                    throw new RootsException("", new Roots(-variables.c / variables.b));
                }

            }

            double d = variables.b * variables.b - 4 * variables.a * variables.c;

            if (d < 0)
            {
                throw new RootsException("", new Roots());
            }

            if (d == 0)
            {
                throw new RootsException("", new Roots(-variables.b / (2 * variables.a)));
            }

            throw new RootsException("", new Roots((-variables.b - Math.Sqrt(d)) / (2 * variables.a),
                                                   (-variables.b + Math.Sqrt(d)) / (2 * variables.a)));
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
