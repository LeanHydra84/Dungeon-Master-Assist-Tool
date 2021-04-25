using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Master_Assist_Tool
{

    class ItemComparer<T> : IComparer<T> where T : BindToListBox
    {
        public int Compare(T x, T y)
        {
            return string.Compare(x.PrimarySource, y.PrimarySource);
        }
    }

    class TestComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            if (x == y) return 0;
            if (x < y) return -1;
            if (x > y) return 1;

            return 0;

        }
    }

    class ArrayManagement
    {

        public static void TEST()
        {
            int[] first = { 1, 4, 8, 13, 17, 25 };
            int[] second = { 2, 3, 7, 9, 13, 14, 18 };

            int[] output = CombineAndSort(first, second, new TestComparer());

            foreach (int item in output)
            {
                Debug.WriteLine(item + " ");
            }

        }

        // Preconditions: both arrays are sorted in ascending order
        public static T[] CombineAndSort<T>(T[] first, T[] second, IComparer<T> comparer)
        {
            
            T[] newArray = new T[first.Length + second.Length];

            int a = 0, b = 0, c = 0;

            // a = FIRST array position
            // b = SECOND array position
            // c = NEW array position

            while(a < first.Length && b < second.Length)
            {
                if (comparer.Compare(first[a], second[b]) <= 0) // FIRST; If equal, default to FIRST
                {
                    newArray[c++] = first[a++];
                }
                else // SECOND
                {
                    newArray[c++] = second[b++];
                }
            }

            if(a < first.Length)
            {
                for (; a < first.Length; a++)
                {
                    newArray[c++] = first[a];
                }
            }
            else
            {
                for(; b < second.Length; b++)
                {
                    newArray[c++] = second[b];
                }
            }

            return newArray;

        }

    }
}
