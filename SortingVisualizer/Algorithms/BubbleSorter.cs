using System.Collections.Generic;
using SortingVisualizer.Models;

namespace SortingVisualizer.Algorithms
{
    public class BubbleSorter : ISorter
    {
        public string Name => "Пузырьковая сортировка";

        public int ComparisonsCount { get; private set; }
        public int SwapsCount { get; private set; }

        public IEnumerable<SortStep> Sort(int[] array)
        {
            // Обнуляем счетчики перед началом
            ComparisonsCount = 0;
            SwapsCount = 0;

            // Делаем копию массива, чтобы не испортить исходные данные
            int[] arr = (int[])array.Clone();
            int n = arr.Length;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    ComparisonsCount++;

                    // Передаем в программу шаг сравнения для отрисовки
                    yield return new SortStep
                    {
                        Type = StepType.Comparison,
                        Index1 = j,
                        Index2 = j + 1,
                        CurrentArray = (int[])arr.Clone()
                    };

                    if (arr[j] > arr[j + 1])
                    {
                        // Меняем элементы местами
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                        SwapsCount++;

                        // Передаем в программу шаг перестановки для отрисовки
                        yield return new SortStep
                        {
                            Type = StepType.Swap,
                            Index1 = j,
                            Index2 = j + 1,
                            CurrentArray = (int[])arr.Clone()
                        };
                    }
                }
            }

            // Сообщаем, что сортировка завершена
            yield return new SortStep
            {
                Type = StepType.Done,
                Index1 = -1,
                Index2 = -1,
                CurrentArray = arr
            };
        }
    }
}