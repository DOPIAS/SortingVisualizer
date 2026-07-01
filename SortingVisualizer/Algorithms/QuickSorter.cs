using System.Collections.Generic;
using SortingVisualizer.Models;

namespace SortingVisualizer.Algorithms
{
    public class QuickSorter : ISorter
    {
        public string Name => "Быстрая сортировка";

        public int ComparisonsCount { get; private set; }
        public int SwapsCount { get; private set; }

        public IEnumerable<SortStep> Sort(int[] array)
        {
            ComparisonsCount = 0;
            SwapsCount = 0;

            int[] arr = (int[])array.Clone();

            foreach (var step in QuickSortHelper(arr, 0, arr.Length - 1))
            {
                yield return step;
            }

            yield return new SortStep
            {
                Type = StepType.Done,
                Index1 = -1,
                Index2 = -1,
                CurrentArray = arr
            };
        }

        private IEnumerable<SortStep> QuickSortHelper(int[] arr, int low, int high)
        {
            if (low < high)
            {
                int pivotIndex = -1;

                foreach (var step in Partition(arr, low, high))
                {
                    if (step.Type == StepType.Done)
                    {
                        pivotIndex = step.Index1;
                    }
                    else
                    {
                        yield return step;
                    }
                }

                if (pivotIndex != -1)
                {
                    foreach (var step in QuickSortHelper(arr, low, pivotIndex - 1)) yield return step;
                    foreach (var step in QuickSortHelper(arr, pivotIndex + 1, high)) yield return step;
                }
            }
        }

        private IEnumerable<SortStep> Partition(int[] arr, int low, int high)
        {
            int pivot = arr[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                ComparisonsCount++;
                yield return new SortStep { Type = StepType.Comparison, Index1 = j, Index2 = high, CurrentArray = (int[])arr.Clone() };

                if (arr[j] < pivot)
                {
                    i++;
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                    SwapsCount++;

                    yield return new SortStep { Type = StepType.Swap, Index1 = i, Index2 = j, CurrentArray = (int[])arr.Clone() };
                }
            }

            int temp1 = arr[i + 1];
            arr[i + 1] = arr[high];
            arr[high] = temp1;
            SwapsCount++;

            yield return new SortStep { Type = StepType.Swap, Index1 = i + 1, Index2 = high, CurrentArray = (int[])arr.Clone() };

            yield return new SortStep { Type = StepType.Done, Index1 = i + 1, Index2 = -1, CurrentArray = arr };
        }
    }
}