using System.Collections.Generic;
using SortingVisualizer.Models;

namespace SortingVisualizer.Algorithms
{
    public interface ISorter
    {
        string Name { get; }

        int ComparisonsCount { get; }
        int SwapsCount { get; }

        IEnumerable<SortStep> Sort(int[] array);
    }
}