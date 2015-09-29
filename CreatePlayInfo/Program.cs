using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using MongoDB.Bson.Serialization.Attributes;
using NeoSteam.Model;
using NeoSteam.BLL;
using System.Diagnostics;
using System.Collections;

namespace CreatePlayInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.Title = "创建用户";
            //Stack sk = new Stack();
            //Stack sk2 = new Stack();
            //foreach (int i in new int[4] { 1, 2, 3, 4 })
            //{
            //    sk.Push(i);//入栈 
            //    sk2.Push(i);
            //}
            //foreach (int i in sk)
            //{
            //    Console.WriteLine(i);//遍历 
            //}
            //sk.Pop();//出栈 
            //Console.WriteLine("Pop");
            //foreach (int i in sk)
            //{
            //    Console.WriteLine(i);
            //}
            //sk2.Peek();//弹出最后一项不删除 
            //Console.WriteLine("Peek");
            //foreach (int i in sk2)
            //{
            //    Console.WriteLine(i);
            //} 
            string i = "60";
            string j = "100";

            Console.Write(float.Parse(i) / float.Parse(j) *100);
            Console.Read();
        }
    }
}