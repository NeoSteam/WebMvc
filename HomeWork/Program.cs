using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork
{
    class Program
    {
        static void Main(string[] args)
        {
            Bubble_Sort();
            Console.Read();
        }
        /// <summary>
        /// 冒泡排序算法
        /// </summary>
        private static void Bubble_Sort()
        {
            int[] numbers = { 2, 9, 4, 6, 8, 0, 7,3 };
            bool IsRun = false;
            do
            {
                IsRun = false;
                for (int i = 0; i < numbers.Length - 1; i++)
                {
                    if (numbers[i] > numbers[i + 1])
                    {
                        int temp = numbers[i];
                        numbers[i] = numbers[i + 1];
                        numbers[i + 1] = temp;
                        IsRun = true;
                    }
                }
            }
            while (IsRun);
            for (int i = 0; i < numbers.Length; i++)
            {
                Console.Write(numbers[i] + " ");
            }
        }
        
    }
}
