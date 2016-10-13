using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chars
{
    class Program
    {
        static void Main(string[] args)
        {

            //Char static method
            Char c = '\r';
            Console.WriteLine(c);
            Console.WriteLine(Char.IsControl(c));
            c = 'A';
            Console.WriteLine(c);
            Console.WriteLine(Char.ToLowerInvariant(c));
            Console.WriteLine(Char.IsLetter(c));
            c = '1';
            Console.WriteLine(c);
            Console.WriteLine(Char.IsDigit(c));
            c = ',';
            Console.WriteLine(c);
            Console.WriteLine(Char.IsPunctuation(c));
            c = ' ';
            Console.WriteLine(c);
            Console.WriteLine(Char.IsSeparator(c));

            Console.WriteLine(Char.MinValue.ToString());
            Console.WriteLine(Char.MaxValue.ToString());

            Double d;
            d = Char.GetNumericValue('3');
            Console.WriteLine(d.ToString());
            d = Char.GetNumericValue('\u00bc');
            Console.WriteLine(d.ToString());

            //Convert char to numeric: Way#1 casting, the most efficient way
            int n = (int) 'A';
            Console.WriteLine(n);//65
            char charA = (char) 88;//'X'
            Console.WriteLine(charA);

            // Way#2: Convert
            char c2 = Convert.ToChar(88);
            Console.WriteLine(c2);
            n = Convert.ToInt32(c2);
            Console.WriteLine(n);

            try
            {
                c2 = Convert.ToChar(80000);//too big,
                Console.WriteLine(c2);
            }
            catch (OverflowException)
            {
                
                Console.WriteLine("the int is too big for char conversion");
            }

            //Way#3 IConvertible
            // Convert number <-> character using IConvertible, 慎用ToChar,以及ToUInt16,会产生装箱
            c = ((IConvertible)65).ToChar(null);
            Console.WriteLine(c); // Displays "A"
            n = ((IConvertible)c).ToInt32(null);
            Console.WriteLine(n); // Displays "65"


            

            Console.Read();
        }
    }
}
