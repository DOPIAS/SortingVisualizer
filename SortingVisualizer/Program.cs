using System;
using System.Collections.Generic;
using System.Threading;
using SortingVisualizer.Algorithms;
using SortingVisualizer.Models;

namespace SortingVisualizer
{
    public class Program
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
                Console.WriteLine("5. Режим 'Гонки' (сравнение алгоритмов)");
                Console.WriteLine("0. Выход");
                Console.Write("\nВыберите действие: ");

                var key = Console.ReadKey(true).KeyChar;
                if (key == '0') break;

                // Если выбрали гонку - запускаем отдельный метод
                if (key == '5')
                {
                    RunRaceMode();
                    continue;
                }

                // Инициализируем выбранный алгоритм через интерфейс
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

        static void RunRaceMode()
        {
            Console.Clear();
            Console.WriteLine("=== РЕЖИМ 'ГОНКИ' АЛГОРИТМОВ ===");
            Console.Write("Введите размер массива (например, 100): ");

            if (!int.TryParse(Console.ReadLine() ?? "", out int size) || size <= 0) size = 100;

            Console.WriteLine("\nВыберите начальное состояние массива:");
            Console.WriteLine("1. Случайный");
            Console.WriteLine("2. Обратный (от большего к меньшему)");
            Console.WriteLine("3. Почти отсортированный");
            var typeKey = Console.ReadKey(true).KeyChar;

            // Генерируем один базовый массив для всех участников гонки
            int[] baseArray = GenerateArray(size, typeKey);

            Console.Clear();
            Console.WriteLine("Гонка началась! Вычисляем результаты...\n");

            // Создаем список всех наших алгоритмов для последовательного запуска
            ISorter[] sorters = { new BubbleSorter(), new InsertionSorter(), new MergeSorter(), new QuickSorter() };

            // Рисуем красивую шапку таблицы
            Console.WriteLine("{0,-25} | {1,-15} | {2,-15}", "Алгоритм", "Сравнения", "Перестановки");
            Console.WriteLine(new string('-', 62));

            // Запускаем каждый алгоритм на точной копии массива
            foreach (var sorter in sorters)
            {
                int[] arrCopy = (int[])baseArray.Clone();

                // Прогоняем алгоритм "вхолостую" (без отрисовки), чтобы просто накрутить счетчики
                foreach (var step in sorter.Sort(arrCopy)) { }

                // Выводим результат алгоритма в таблицу
                Console.WriteLine("{0,-25} | {1,-15} | {2,-15}", sorter.Name, sorter.ComparisonsCount, sorter.SwapsCount);
            }

            Console.WriteLine("\nГонка завершена! Нажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }

        static void RunSingleSort(ISorter sorter)
        {
            Console.Clear();
            Console.WriteLine($"=== {sorter.Name.ToUpper()} ===");
            Console.Write("Введите размер массива (рекомендуется 15-25): ");

            if (!int.TryParse(Console.ReadLine() ?? "", out int size) || size <= 0) size = 20;

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
                // Подсвечиваем активные элементы в зависимости от типа операции
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

                // Рисуем значения и столбик. PadRight(50) стирает артефакты от прошлых кадров
                Console.Write($"{array[i],2} | ");
                Console.WriteLine(new string('█', array[i]).PadRight(50));
            }
            Console.ResetColor(); // Сбрасываем цвет обратно
        }

        public static int[] GenerateArray(int size, char type)
        {
            int[] arr = new int[size];
            Random rand = new Random();

            // Заполняем случайными числами от 1 до 40 (чтобы столбики влезали в экран консоли)
            for (int i = 0; i < size; i++) arr[i] = rand.Next(1, 40);

            if (type == '2') // Обратный порядок
            {
                Array.Sort(arr);
                Array.Reverse(arr);
            }
            else if (type == '3') // Почти отсортированный порядок
            {
                Array.Sort(arr);
                int swaps = Math.Max(1, size / 5); // Ломаем правильный порядок примерно у 20% элементов
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