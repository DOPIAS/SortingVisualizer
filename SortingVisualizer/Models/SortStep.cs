namespace SortingVisualizer.Models
{
    // Описание, что делает алгоритм в данный момент
    public enum StepType
    {
        Comparison, // Сравнивает
        Swap,       // Меняет местами
        Done        // Закончил работу
    }

    // Состояние массива на каждом шаге
    public class SortStep
    {
        public StepType Type { get; set; }
        public int Index1 { get; set; }
        public int Index2 { get; set; }
        public int[] CurrentArray { get; set; } = [];
    }
}