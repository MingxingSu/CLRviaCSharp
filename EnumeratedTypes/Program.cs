using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumeratedTypes
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Enum.GetUnderlyingType(typeof(Color)));
            Console.WriteLine(Enum.GetUnderlyingType(typeof(Color2)));
            Console.WriteLine();

            Color colorBlack = Color.Black;
            Console.WriteLine(colorBlack);
            Console.WriteLine(colorBlack.ToString());
            Console.WriteLine(colorBlack.ToString("G"));//general
            Console.WriteLine(colorBlack.ToString("F"));//flag
            Console.WriteLine(colorBlack.ToString("D"));//decimal
            Console.WriteLine(colorBlack.ToString("X"));//hex, 
            Console.WriteLine();

            Color2 colorBlack2 = Color2.Black;
            Console.WriteLine(colorBlack2.ToString("X"));//digits based on the underlying type
           Console.WriteLine(Enum.Format(typeof(Color),2,"F"));//output 'Yellow'
           Console.WriteLine(Enum.Format(typeof(Color2), (byte)3, "G"));//output 'Yellow'
           Console.WriteLine();

            Color[] colorList = (Color[])Enum.GetValues(typeof (Color));//Method from Enum
            Console.WriteLine(colorList.GetValue(0));
            foreach (Color c in colorList)
            {
                Console.WriteLine("{0:G}\t{0:D}", c);
            }
            Console.WriteLine();

            Type type = typeof (Color2);
            Color2[] colorList2 = (Color2[])type.GetEnumValues();//method from Type
            Console.WriteLine(colorList2.GetValue(0));
            Console.WriteLine();

            Color2[] colorListGoodWay = new Program().GetEnumValues<Color2>();
            foreach (Color2 c in colorListGoodWay)
            {
                Console.WriteLine("{0:G}\t{0:D}", c);
            }
            Console.WriteLine();

            var names = Enum.GetNames(typeof (Color));
            var names2 = type.GetEnumNames();
            Console.WriteLine(names[0]);
            Console.WriteLine(names2[0]);
            Console.WriteLine();

            //Parse String to Enum object
            Color red = (Color) Enum.Parse(typeof (Color), "Red", true);
            Console.WriteLine(red.ToString("G"));

            //Color brown = (Color)Enum.Parse(typeof(Color), "Brown", true);//will throw exception because no brown color defined

            Color red2;
            Enum.TryParse("1", false, out red2);
            Console.WriteLine(red2.ToString());
            Console.WriteLine();

            //IsDfined 经常用于参数验证，是否传入的是有效instance， 这个方法case sensitive using reflection,所以比较慢
            Console.WriteLine(Enum.IsDefined(typeof(Color),"Green"));
            Console.WriteLine(Enum.IsDefined(typeof (Color), 3));

            //何时使用Enum,一定要跟需要使用Enum的class 在同一个level.
            Console.WriteLine();

            

            Console.Read();
        }

        //此泛型方法更好，比Enum和Type自带的要强制转换好
        public TEnum[] GetEnumValues<TEnum>() where TEnum : struct
        {
            return (TEnum[]) Enum.GetValues(typeof (TEnum));
        }
    }
    //value type but have no method, event, property
    internal enum Color
    {
        Blue,//0，
        Red,//1
        Yellow,//2
        White,//3
        Black//4
    }


    //本质上， Color会编译为Struct类型，
    //一个instance会编译一个常量：public const Color Red = (Color)1;
    //枚举类型，只在编译时required：编译时symbol已经被常量的值替换，如果不重新编译，只是

    //value type but have no method, event, property
    internal enum Color2:byte{
        Pink,//0，
        Red,//1
        Yellow,//2
        White,//3
        Black//4
    }



}
