using System;
using System.Collections.Generic;
using System.Threading;
using SortingVisualizer.Algorithms;
using SortingVisualizer.Models;

namespace SortingVisualizer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Скрываем мигающий курсор для красоты анимации
            Console.CursorVisible = false;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ВИЗУАЛИЗАТОР АЛГОРИТМОВ СОРТИРОВКИ ===");
                Console.WriteLine("1. Пузырьковая сортировка");
                Console.WriteLine("2. Сортировка вставками");
                Console.WriteLine("3. Сортировка слиянием");
                Console.WriteLine("4. Быстрая сортировка");
                Console.WriteLine("0. Выход");
                Console.Write("\nВыберите действие: ");

                var key = Console.ReadKey(true).KeyChar;
                if (key == '0') break;

                ISorter sorter = key switch
                {
                    '1' => new BubbleSorter(),
                    '2' => new InsertionSorter(),
                    '3' => new MergeSorter(),
                    '4' => new QuickSorter(),
                    _ => null
                };

                if (sorter != null)
                {
                    RunSingleSort(sorter);
                }
            }
        }

        static void RunSingleSort(ISorter sorter)
        {
            Console.Clear();
            Console.WriteLine($"=== {sorter.Name.ToUpper()} ===");
            Console.Write("Введите размер массива (рекомендуется 15-25): ");

            if (!int.TryParse(Console.ReadLine(), out int size) || size <= 0) size = 20;

            Console.WriteLine("\nВыберите начальное состояние массива:");
            Console.WriteLine("1. Случайный");
            Console.WriteLine("2. Обратный (от большего к меньшему)");
            Console.WriteLine("3. Почти отсортированный");
            var typeKey = Console.ReadKey(true).KeyChar;

            int[] array = GenerateArray(size, typeKey);

            Console.Clear();
            Console.WriteLine($"Подготовка... {sorter.Name}");
            Thread.Sleep(500);
            Console.Clear();

            // Запоминаем верхнюю строчку, чтобы перерисовывать кадры на одном месте (без мерцания экрана)
            int startTop = Console.CursorTop;

            // Запускаем алгоритм и перехватываем каждый шаг для отрисовки
            foreach (var step in sorter.Sort(array))
            {
                Console.SetCursorPosition(0, startTop);
                DrawArray(step.CurrentArray, step.Index1, step.Index2, step.Type, sorter.ComparisonsCount, sorter.SwapsCount);

                // Скорость анимации (в миллисекундах)
                Thread.Sleep(40);
            }

            Console.WriteLine("\nСортировка завершена! Нажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }

        static void DrawArray(int[] array, int idx1, int idx2, StepType type, int compCount, int swapCount)
        {
            Console.WriteLine($"Операции -> Сравнений: {compCount,-5} | Перестановок: {swapCount,-5}\n");

            for (int i = 0; i < array.Length; i++)
            {
                // Подсвечиваем активные элементы
                if (i == idx1 || i == idx2)
                {
                    if (type == StepType.Comparison) Console.ForegroundColor = ConsoleColor.Red;
                    else if (type == StepType.Swap) Console.ForegroundColor = ConsoleColor.Green;
                    else Console.ForegroundColor = ConsoleColor.Yellow; // Для финального шага Done
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                // Рисуем значения и столбик
                Console.Write($"{array[i],2} | ");
                Console.WriteLine(new string('█', array[i]));
            }
            Console.ResetColor(); // Сбрасываем цвет обратно
        }

        static int[] GenerateArray(int size, char type)
        {
            int[] arr = new int[size];
            Random rand = new Random();

            // Заполняем случайными числами от 1 до 40 (чтобы столбики влезали в экран)
            for (int i = 0; i < size; i++) arr[i] = rand.Next(1, 40);

            if (type == '2') // Обратный
            {
                Array.Sort(arr);
                Array.Reverse(arr);
            }
            else if (type == '3') // Почти отсортированный
            {
                Array.Sort(arr);
                int swaps = Math.Max(1, size / 5); // Ломаем порядок примерно у 20% элементов
                for (int i = 0; i < swaps; i++)
                {
                    int idx1 = rand.Next(size);
                    int idx2 = rand.Next(size);
                    (arr[idx1], arr[idx2]) = (arr[idx2], arr[idx1]);
                }
            }
            return arr;
        }
    }
}