using System.Collections.Generic;
using SortingVisualizer.Models;

namespace SortingVisualizer.Algorithms
{
    public class InsertionSorter : ISorter
    {
        public string Name => "Сортировка вставками";

        public int ComparisonsCount { get; private set; }
        public int SwapsCount { get; private set; }

        public IEnumerable<SortStep> Sort(int[] array)
        {
            ComparisonsCount = 0;
            SwapsCount = 0;

            int[] arr = (int[])array.Clone();
            int n = arr.Length;

            for (int i = 1; i < n; i++)
            {
                int key = arr[i];
                int j = i - 1;

                // Показываем текущее сравнение
                yield return new SortStep
                {
                    Type = StepType.Comparison,
                    Index1 = i,
                    Index2 = Math.Max(0, j),
                    CurrentArray = (int[])arr.Clone()
                };

                while (j >= 0 && arr[j] > key)
                {
                    ComparisonsCount++;

                    // Сдвигаем элемент
                    arr[j + 1] = arr[j];
                    SwapsCount++;

                    yield return new SortStep
                    {
                        Type = StepType.Swap,
                        Index1 = j,
                        Index2 = j + 1,
                        CurrentArray = (int[])arr.Clone()
                    };
                    j--;
                }

                // Учитываем последнее сравнение, когда цикл прервался
                if (j >= 0) ComparisonsCount++;

                arr[j + 1] = key;
            }

            // Завершение работы
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