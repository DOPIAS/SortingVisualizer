using System.Collections.Generic;
using SortingVisualizer.Models;

namespace SortingVisualizer.Algorithms
{
    // Базовый интерфейс для всех алгоритмов сортировки.
    // Нужен, чтобы можно было легко добавлять новые сортировки, 
    // не переписывая основную логику программы.
    public interface ISorter
    {
        // Название сортировки для вывода в консольном меню
        string Name { get; }

        // Переменные для сбора статистики (чтобы потом сравнить эффективность)
        int ComparisonsCount { get; }
        int SwapsCount { get; }

        // Основной метод сортировки. 
        // Используем IEnumerable, чтобы не просто отсортировать массив разом, 
        // а отдавать его состояния пошагово для отрисовки анимации.
        IEnumerable<SortStep> Sort(int[] array);
    }
}