using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableValueTypes
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Datetime is a value type in .NET, Nullable type is needed for nullable database columns
            Nullable<int> x = 5; //int?
            int? y = null;
            Console.WriteLine("x:HasValue={0}, Value={1}", x.HasValue, x.Value);
            Console.WriteLine("y:HasValue={0}, Value={1}", y.HasValue, y.GetValueOrDefault());

            //Create nullable instances and casting
            int? a = 5;
            int? b = null;
            double? c = 5;
            double? d = b;
            int e = (int) a;

            //Nullable对自定义的类型也有用，只要重载了==, !=
            Point? p1 = new Point(1,1);
            Point? p2 = new Point(2, 2);
            Console.WriteLine("P1==P2? {0}",p1==p2);
            Console.WriteLine("P1!=P2? {0}", p1 != p2);

            //Null-coalecing operator ??
            int? age = GetMyAge("Max");

            int myAge = age.HasValue ? age.Value : 31;
            int myAge2 = age ?? 31; 
            Console.WriteLine("Max's age is :"+myAge2);
            age = GetMyAge("Diana");
            myAge2 = age ?? 31; 
            Console.WriteLine("Diana's age is :" + myAge2);

            //Greate syntax!
            int myAge3 = GetMyAge("Jessica") ?? GetMyAgeV2("Diana") ?? 31;

            //Though Int32? not implement ICompareable, CLR still allow it to run
            Int32? n = 5;
            Int32 result = ((IComparable)n).CompareTo(5); // Compiles & runs OK, no need to write like this: (IComparable)(int)n
            Console.WriteLine(result); // 0

            Console.Read();
        }

        private static int? GetMyAge(string name)
        {
            if (name == "Jessica")
                return 3;
            return null;
        }
        private static int? GetMyAgeV2(string name)
        {
            if (name == "Diana")
                return 27;
            return null;
        }
    }

    internal struct Point
    {
        private int m_x;
        private int m_y;
        public Point(int x, int y)
        {
            m_x = x;
            m_y = y;
        }
        //自定义操作符
        public static bool operator==(Point p1, Point p2)
        {
            return p1.m_x == p2.m_x && p1.m_y == p2.m_y;
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }
    }



}
