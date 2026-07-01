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
            ComparisonsCount = 0;
            SwapsCount = 0;

            int[] arr = (int[])array.Clone();
            int n = arr.Length;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    ComparisonsCount++;

                    yield return new SortStep
                    {
                        Type = StepType.Comparison,
                        Index1 = j,
                        Index2 = j + 1,
                        CurrentArray = (int[])arr.Clone()
                    };

                    if (arr[j] > arr[j + 1])
                    {
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                        SwapsCount++;

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