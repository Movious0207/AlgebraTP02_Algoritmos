using System;
using System.Collections.Generic;

public static class SortingAlgorithms
{
    // Auxiliar para intercambiar elementos
    private static void Swap<T>(T[] array, int i, int j)
    {
        T temp = array[i];
        array[i] = array[j];
        array[j] = temp;
    }

    // ========================================================================
    // 1. INTRO SORT
    // Complejidad: O(n log n)
    // ========================================================================
    public static void IntroSort<T>(T[] array) where T : IComparable<T>
    {
        if (array == null || array.Length <= 1) return;
        int maxDepth = (int)(2 * Math.Floor(Math.Log(array.Length) / Math.Log(2)));
        IntroSortRecursive(array, 0, array.Length - 1, maxDepth);
    }

    private static void IntroSortRecursive<T>(T[] array, int left, int right, int depthLimit) where T : IComparable<T>
    {
        int size = right - left + 1;
        if (size <= 16)
        {
            InsertionSortSection(array, left, right);
            return;
        }

        if (depthLimit == 0)
        {
            HeapSortSection(array, left, right);
            return;
        }

        int pivotIndex = Partition(array, left, right);
        IntroSortRecursive(array, left, pivotIndex - 1, depthLimit - 1);
        IntroSortRecursive(array, pivotIndex + 1, right, depthLimit - 1);
    }

    // ========================================================================
    // 2. MERGE SORT
    // Complejidad: O(n log n)
    // ========================================================================
    public static void MergeSort<T>(T[] array) where T : IComparable<T>
    {
        if (array == null || array.Length <= 1) return;
        MergeSortInternal(array, 0, array.Length - 1);
    }

    private static void MergeSortInternal<T>(T[] array, int left, int right) where T : IComparable<T>
    {
        if (left < right)
        {
            int middle = left + (right - left) / 2;
            MergeSortInternal(array, left, middle);
            MergeSortInternal(array, middle + 1, right);
            Merge(array, left, middle, right);
        }
    }

    private static void Merge<T>(T[] array, int left, int middle, int right) where T : IComparable<T>
    {
        int n1 = middle - left + 1;
        int n2 = right - middle;

        T[] leftArray = new T[n1];
        T[] rightArray = new T[n2];

        Array.Copy(array, left, leftArray, 0, n1);
        Array.Copy(array, middle + 1, rightArray, 0, n2);

        int i = 0, j = 0, k = left;

        while (i < n1 && j < n2)
        {
            if (leftArray[i].CompareTo(rightArray[j]) <= 0)
                array[k++] = leftArray[i++];
            else
                array[k++] = rightArray[j++];
        }

        while (i < n1) array[k++] = leftArray[i++];
        while (j < n2) array[k++] = rightArray[j++];
    }

    // ========================================================================
    // 3. ADAPTIVE MERGE SORT (TimSort simplificado)
    // Complejidad: O(n log n) [O(n) en datos ya ordenados]
    // ========================================================================
    public static void AdaptiveMergeSort<T>(T[] array) where T : IComparable<T>
    {
        if (array == null || array.Length <= 1) return;

        const int RUN = 32;
        int n = array.Length;

        for (int i = 0; i < n; i += RUN)
            InsertionSortSection(array, i, Math.Min(i + RUN - 1, n - 1));

        for (int size = RUN; size < n; size = 2 * size)
        {
            for (int left = 0; left < n; left += 2 * size)
            {
                int mid = left + size - 1;
                int right = Math.Min((left + 2 * size - 1), (n - 1));

                if (mid < right)
                {
                    // Adaptación: Si ya está ordenado entre mitades, omitir el merge
                    if (array[mid].CompareTo(array[mid + 1]) > 0)
                        Merge(array, left, mid, right);
                }
            }
        }
    }

    // ========================================================================
    // 4. HEAP SORT
    // Complejidad: O(n log n)
    // ========================================================================
    public static void HeapSort<T>(T[] array) where T : IComparable<T>
    {
        if (array == null || array.Length <= 1) return;
        HeapSortSection(array, 0, array.Length - 1);
    }

    private static void HeapSortSection<T>(T[] array, int left, int right) where T : IComparable<T>
    {
        int n = right - left + 1;

        for (int i = n / 2 - 1; i >= 0; i--)
            Heapify(array, n, i, left);

        for (int i = n - 1; i > 0; i--)
        {
            Swap(array, left, left + i);
            Heapify(array, i, 0, left);
        }
    }

    private static void Heapify<T>(T[] array, int n, int i, int offset) where T : IComparable<T>
    {
        int largest = i;
        int left = 2 * i + 1;
        int right = 2 * i + 2;

        if (left < n && array[offset + left].CompareTo(array[offset + largest]) > 0)
            largest = left;

        if (right < n && array[offset + right].CompareTo(array[offset + largest]) > 0)
            largest = right;

        if (largest != i)
        {
            Swap(array, offset + i, offset + largest);
            Heapify(array, n, largest, offset);
        }
    }

    // ========================================================================
    // 5. QUICK SORT
    // Complejidad: O(n log n) promedio, O(n^2) peor caso
    // ========================================================================
    public static void QuickSort<T>(T[] array) where T : IComparable<T>
    {
        if (array == null || array.Length <= 1) return;
        QuickSortInternal(array, 0, array.Length - 1);
    }

    private static void QuickSortInternal<T>(T[] array, int left, int right) where T : IComparable<T>
    {
        if (left < right)
        {
            int pivotIndex = Partition(array, left, right);
            QuickSortInternal(array, left, pivotIndex - 1);
            QuickSortInternal(array, pivotIndex + 1, right);
        }
    }

    private static int Partition<T>(T[] array, int left, int right) where T : IComparable<T>
    {
        T pivot = array[right];
        int i = left - 1;

        for (int j = left; j < right; j++)
        {
            if (array[j].CompareTo(pivot) <= 0)
            {
                i++;
                Swap(array, i, j);
            }
        }
        Swap(array, i + 1, right);
        return i + 1;
    }

    // ========================================================================
    // 6. BITONIC SORT
    // Complejidad: O(n log^2 n) — *Requiere un tamaño n potencia de 2*
    // ========================================================================
    public static void BitonicSort<T>(T[] array) where T : IComparable<T>
    {
        if (array == null || array.Length <= 1) return;
        BitonicSortInternal(array, 0, array.Length, true);
    }

    private static void BitonicSortInternal<T>(T[] array, int low, int count, bool dir) where T : IComparable<T>
    {
        if (count > 1)
        {
            int k = count / 2;
            BitonicSortInternal(array, low, k, true);
            BitonicSortInternal(array, low + k, k, false);
            BitonicMerge(array, low, count, dir);
        }
    }

    private static void BitonicMerge<T>(T[] array, int low, int count, bool dir) where T : IComparable<T>
    {
        if (count > 1)
        {
            int k = count / 2;
            for (int i = low; i < low + k; i++)
            {
                if ((dir && array[i].CompareTo(array[i + k]) > 0) ||
                    (!dir && array[i].CompareTo(array[i + k]) < 0))
                {
                    Swap(array, i, i + k);
                }
            }
            BitonicMerge(array, low, k, dir);
            BitonicMerge(array, low + k, k, dir);
        }
    }

    // ========================================================================
    // 7. SHELL SORT
    // Complejidad: ~ O(n^(1.3..1.5))
    // ========================================================================
    public static void ShellSort<T>(T[] array) where T : IComparable<T>
    {
        if (array == null || array.Length <= 1) return;
        int n = array.Length;

        for (int gap = n / 2; gap > 0; gap /= 2)
        {
            for (int i = gap; i < n; i++)
            {
                T temp = array[i];
                int j = i;
                while (j >= gap && array[j - gap].CompareTo(temp) > 0)
                {
                    array[j] = array[j - gap];
                    j -= gap;
                }
                array[j] = temp;
            }
        }
    }

    // ========================================================================
    // 8. RADIX SORT (LSD)
    // Complejidad: O(n * k) — *Implementación para enteros*
    // ========================================================================
    public static void RadixSortLSD(int[] array)
    {
        if (array == null || array.Length <= 1) return;

        int max = array[0];
        for (int i = 1; i < array.Length; i++)
            if (array[i] > max) max = array[i];

        for (int exp = 1; max / exp > 0; exp *= 10)
            CountSortLSD(array, exp);
    }

    private static void CountSortLSD(int[] array, int exp)
    {
        int n = array.Length;
        int[] output = new int[n];
        int[] count = new int[10];

        for (int i = 0; i < n; i++)
            count[(array[i] / exp) % 10]++;

        for (int i = 1; i < 10; i++)
            count[i] += count[i - 1];

        for (int i = n - 1; i >= 0; i--)
        {
            output[count[(array[i] / exp) % 10] - 1] = array[i];
            count[(array[i] / exp) % 10]--;
        }

        for (int i = 0; i < n; i++)
            array[i] = output[i];
    }

    // ========================================================================
    // 9. RADIX SORT (MSD)
    // Complejidad: O(n * k) — *Implementación para enteros*
    // ========================================================================
    public static void RadixSortMSD(int[] array)
    {
        if (array == null || array.Length <= 1) return;

        int max = array[0];
        for (int i = 1; i < array.Length; i++)
            if (array[i] > max) max = array[i];

        int maxDigits = (int)Math.Floor(Math.Log10(max));
        RadixSortMSDInternal(array, 0, array.Length - 1, (int)Math.Pow(10, maxDigits));
    }

    private static void RadixSortMSDInternal(int[] array, int lo, int hi, int exp)
    {
        if (lo >= hi || exp <= 0) return;

        List<int>[] buckets = new List<int>[10];
        for (int i = 0; i < 10; i++) buckets[i] = new List<int>();

        for (int i = lo; i <= hi; i++)
        {
            int digit = (array[i] / exp) % 10;
            buckets[digit].Add(array[i]);
        }

        int index = lo;
        for (int i = 0; i < 10; i++)
        {
            int start = index;
            foreach (var item in buckets[i])
            {
                array[index++] = item;
            }
            int end = index - 1;
            RadixSortMSDInternal(array, start, end, exp / 10);
        }
    }

    // ========================================================================
    // 10. INSERTION SORT
    // Complejidad: O(n^2)
    // ========================================================================
    public static void InsertionSort<T>(T[] array) where T : IComparable<T>
    {
        if (array == null || array.Length <= 1) return;
        InsertionSortSection(array, 0, array.Length - 1);
    }

    private static void InsertionSortSection<T>(T[] array, int left, int right) where T : IComparable<T>
    {
        for (int i = left + 1; i <= right; i++)
        {
            T key = array[i];
            int j = i - 1;

            while (j >= left && array[j].CompareTo(key) > 0)
            {
                array[j + 1] = array[j];
                j--;
            }
            array[j + 1] = key;
        }
    }

    // ========================================================================
    // 11. SELECTION SORT
    // Complejidad: O(n^2)
    // ========================================================================
    public static void SelectionSort<T>(T[] array) where T : IComparable<T>
    {
        if (array == null || array.Length <= 1) return;
        int n = array.Length;

        for (int i = 0; i < n - 1; i++)
        {
            int minIdx = i;
            for (int j = i + 1; j < n; j++)
            {
                if (array[j].CompareTo(array[minIdx]) < 0)
                    minIdx = j;
            }
            if (minIdx != i)
                Swap(array, i, minIdx);
        }
    }

    // ========================================================================
    // 12. BUBBLE SORT
    // Complejidad: O(n^2)
    // ========================================================================
    public static void BubbleSort<T>(T[] array) where T : IComparable<T>
    {
        if (array == null || array.Length <= 1) return;
        int n = array.Length;
        bool swapped;

        for (int i = 0; i < n - 1; i++)
        {
            swapped = false;
            for (int j = 0; j < n - i - 1; j++)
            {
                if (array[j].CompareTo(array[j + 1]) > 0)
                {
                    Swap(array, j, j + 1);
                    swapped = true;
                }
            }
            if (!swapped) break;
        }
    }

    // ========================================================================
    // 13. COCKTAIL SHAKER SORT
    // Complejidad: O(n^2)
    // ========================================================================
    public static void CocktailShakerSort<T>(T[] array) where T : IComparable<T>
    {
        if (array == null || array.Length <= 1) return;

        bool swapped = true;
        int start = 0;
        int end = array.Length - 1;

        while (swapped)
        {
            swapped = false;

            for (int i = start; i < end; ++i)
            {
                if (array[i].CompareTo(array[i + 1]) > 0)
                {
                    Swap(array, i, i + 1);
                    swapped = true;
                }
            }

            if (!swapped) break;

            swapped = false;
            end--;

            for (int i = end - 1; i >= start; --i)
            {
                if (array[i].CompareTo(array[i + 1]) > 0)
                {
                    Swap(array, i, i + 1);
                    swapped = true;
                }
            }
            start++;
        }
    }

    // ========================================================================
    // 14. GNOME SORT
    // Complejidad: O(n^2)
    // ========================================================================
    public static void GnomeSort<T>(T[] array) where T : IComparable<T>
    {
        if (array == null || array.Length <= 1) return;

        int index = 0;
        int n = array.Length;

        while (index < n)
        {
            if (index == 0 || array[index].CompareTo(array[index - 1]) >= 0)
            {
                index++;
            }
            else
            {
                Swap(array, index, index - 1);
                index--;
            }
        }
    }

    // ========================================================================
    // 15. BOGO SORT
    // Complejidad: O((n+1)!)
    // ========================================================================
    public static void BogoSort<T>(T[] array) where T : IComparable<T>
    {
        if (array == null || array.Length <= 1) return;
        Random rng = new Random();

        while (!IsSorted(array))
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int k = rng.Next(i + 1);
                Swap(array, i, k);
            }
        }
    }

    private static bool IsSorted<T>(T[] array) where T : IComparable<T>
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            if (array[i].CompareTo(array[i + 1]) > 0)
                return false;
        }
        return true;
    }
}