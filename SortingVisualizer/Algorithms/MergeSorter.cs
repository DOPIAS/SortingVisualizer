using System.Collections.Generic;
using SortingVisualizer.Models;

namespace SortingVisualizer.Algorithms
{
    public class MergeSorter : ISorter
    {
        public string Name => "Сортировка слиянием";

        public int ComparisonsCount { get; private set; }
        public int SwapsCount { get; private set; }

        public IEnumerable<SortStep> Sort(int[] array)
        {
            ComparisonsCount = 0;
            SwapsCount = 0;

            int[] arr = (int[])array.Clone();

            // Запускаем основную логику сортировки
            foreach (var step in MergeSortHelper(arr, 0, arr.Length - 1))
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

        // Рекурсивное разделение массива
        private IEnumerable<SortStep> MergeSortHelper(int[] arr, int left, int right)
        {
            if (left < right)
            {
                int mid = left + (right - left) / 2;

                foreach (var step in MergeSortHelper(arr, left, mid)) yield return step;
                foreach (var step in MergeSortHelper(arr, mid + 1, right)) yield return step;
                foreach (var step in Merge(arr, left, mid, right)) yield return step;
            }
        }

        // Само слияние разделенных частей
        private IEnumerable<SortStep> Merge(int[] arr, int left, int mid, int right)
        {
            int n1 = mid - left + 1;
            int n2 = right - mid;

            int[] L = new int[n1];
            int[] R = new int[n2];

            for (int i = 0; i < n1; ++i) L[i] = arr[left + i];
            for (int j = 0; j < n2; ++j) R[j] = arr[mid + 1 + j];

            int k = left;
            int idx1 = 0, idx2 = 0;

            while (idx1 < n1 && idx2 < n2)
            {
                ComparisonsCount++;
                yield return new SortStep { Type = StepType.Comparison, Index1 = left + idx1, Index2 = mid + 1 + idx2, CurrentArray = (int[])arr.Clone() };

                if (L[idx1] <= R[idx2])
                {
                    arr[k] = L[idx1];
                    idx1++;
                }
                else
                {
                    arr[k] = R[idx2];
                    idx2++;
                }
                SwapsCount++;

                // В слиянии мы не меняем элементы местами, а перезаписываем их
                yield return new SortStep { Type = StepType.Swap, Index1 = k, Index2 = k, CurrentArray = (int[])arr.Clone() };
                k++;
            }

            while (idx1 < n1)
            {
                arr[k] = L[idx1];
                idx1++;
                k++;
                SwapsCount++;
                yield return new SortStep { Type = StepType.Swap, Index1 = k - 1, Index2 = k - 1, CurrentArray = (int[])arr.Clone() };
            }

            while (idx2 < n2)
            {
                arr[k] = R[idx2];
                idx2++;
                k++;
                SwapsCount++;
                yield return new SortStep { Type = StepType.Swap, Index1 = k - 1, Index2 = k - 1, CurrentArray = (int[])arr.Clone() };
            }
        }
    }
}