using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrays
{
    public sealed  class DynamicArrays
    {
        public static void Test()
        {
            int[] lowerBounds = {2005,1};
            int[] lengths = {5, 4};
            Decimal[,] qunarterReveue = (Decimal[,])Array.CreateInstance(typeof(Decimal), lengths, lowerBounds);//typeof(decimal)而不是decimal[,]
            Console.WriteLine("{0,4} {1,9} {2,9} {3,9} {4,9}","Year","Q1","Q2","Q3","Q4");
            
            //维度顺序取决于定义时逗号分割的顺序
            int firstYear = qunarterReveue.GetLowerBound(0);
            int lastYear = qunarterReveue.GetUpperBound(0);
            int firstQunarter = qunarterReveue.GetLowerBound(1);
            int lastQunarter = qunarterReveue.GetUpperBound(1);

            for(int year=firstYear; year <= lastYear; year++)
            {
                Console.Write(year+"    ");
                for (int qunarter = firstQunarter; qunarter <= lastQunarter; qunarter++)
                {
                    Console.Write("{0,9:C}",qunarterReveue[year,qunarter]);//格式化字符串时指定重复数量的9应该在冒号前面
                }
                Console.WriteLine();
            }

        }
    }
}
