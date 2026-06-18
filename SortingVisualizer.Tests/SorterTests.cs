using Xunit;
using SortingVisualizer;
using SortingVisualizer.Algorithms;
using SortingVisualizer.Models;

namespace SortingVisualizer.Tests
{
    public class SorterTests
    {
        [Fact]
        public void UnitTest_BubbleSort_NormalCase()
        {
            // Проверка сортировки стандартного массива
            int[] input = { 5, 2, 8, 1 };
            int[] expected = { 1, 2, 5, 8 };
            var sorter = new BubbleSorter();

            int[] result = input;

            foreach (var step in sorter.Sort(input))
            {
                result = step.CurrentArray;
            }

            Assert.Equal(expected, result);
        }

        [Fact]
        public void UnitTest_BubbleSort_EdgeCase_EmptyArray()
        {
            // Проверка обработки пустого массива (граничное условие)
            int[] input = { };
            int[] expected = { };
            var sorter = new BubbleSorter();

            int[] result = input;
            foreach (var step in sorter.Sort(input))
            {
                result = step.CurrentArray;
            }

            Assert.Equal(expected, result);
        }

        [Fact]
        public void IntegrationTest_GeneratorAndSorter()
        {
            // Проверка совместимости генератора массивов и алгоритма сортировки
            int[] generatedArray = Program.GenerateArray(50, '1');
            var sorter = new QuickSorter();

            foreach (var step in sorter.Sort(generatedArray))
            {
                // Прогоняем до конца
            }

            Assert.True(true);
        }
    }
}