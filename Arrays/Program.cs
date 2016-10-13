using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Arrays
{
    class Program
    {
        static void Main(string[] args)
        {
            //All Arrays derived from System.Array, is reference type, array of reference types is allowed, the ref type will be initiated as null. 
            //MS support non-zero based array, but no encourage it.
            //Single-dimension, zero-base: SZ array or Vector,
            //Multi dimension arrays [9,9,9] cube 9*9*9
            //array of arrays []][]

            //JIT check the index before loop executes instead of at each loop iteration, to improve it a bit, unsafe can be used if neccessary
            int[] nums = new int[10];
            string[] names = new string[] {"Ming", "Max"}; //{}is array intializer
            var nullCanBeInferredAsString = new string[] { "Ming", "Max" ,null}; //Good

            //bad example : because common super class for string and int is object, which not match with string[]
            //var numberCanNotBeInferredAsString = new string[] { "Ming", "Max", 123 }; 

            string[] strName = { "Ming", "Max" };

            // Using C#’s implicitly typed local, implicitly typed array, and anonymous type features:
            var kids = new[] { new { Name = "Aidan" }, new { Name = "Grant" } };
            // Sample usage (with another implicitly typed local variable):
            foreach (var kid in kids)
                Console.WriteLine(kid.Name);

            //Copy array elements
            //1. Array.Copy, 2.Buffer.BlockCopy 3,Array.ConstrainedCopy
            //1.Array.Copy support type casting
            //Buffer.BlockCopy(src,secoffSet,dst, dstOffset,count); only support primitive types, bytes offset not the same as element index, able to treat array as block of memory
            //Array.ConstrainedCopy:Reliable copy, source array type should be the same or derived from destination's type
            

            //Array implement:IEnumable, ICollection, IList, instead of the generic versioned interface due to multi-dimension array.
            //MS trick: for reference type, and Single-dimension, zero-based array created, it did implement the generic interface
            FileStream[] fsArrays = new FileStream[2];
            M1(fsArrays);//Array implment IEnumable, but can pass into IEnumable<T>
            M2(fsArrays);
            M3(fsArrays);

            //array is reference type, thus when pass as parameter to a method, any change to the array will be effective
            //Array.Copy()

           //always return zero count of array, instead of null
            DynamicArrays.Test();


            Array a;
            a = new String[0];// Create a 1-dim, 0-based array, with no elements in it
            Console.WriteLine(a.GetType()); // "System.String[]"

            a = Array.CreateInstance(typeof(String), // Create a 1-dim, 0-based array, with no elements in it
            new Int32[] { 0 }, new Int32[] { 0 });
            Console.WriteLine(a.GetType()); // "System.String[]"

            a = Array.CreateInstance(typeof(String), // Create a 1-dim, 1-based array, with no elements in it
            new Int32[] { 0 }, new Int32[] { 1 });
            Console.WriteLine(a.GetType()); // "System.String[*]" <-- INTERESTING!
            Console.WriteLine();

            //In Loop, Array.Length property will be optimized as a temp variable, single dimension and zero-based array's low and upper bound check will be skipped in the loop execution as long as it's valide during the check before loop
            //But for multi-dimension, it will not skip. Comparing to it,Please choose arrays of array firstly when performance concerned

            //CLR forbid running codes in reduced security environment, i.e:MS Silverlight

            //Unsafe way to manipulate multi-dimension arrays
            Int32[,] arrInts = new int[10000,10000];
            Unsafe2DimArrayAccess(arrInts);

            //Fixed-sized inline arrays in unsafe value types
            InlineArrayDemo();

            Console.Read();



        }

        private static void M1(IEnumerable<FileStream> files)
        {
           
        }

        private static void M2(ICollection<FileStream> files)
        {
        }

        private static void M3(IList<FileStream> files)
        {
        }

        //enable unsafe in project ->build settings
        private static unsafe Int32 Unsafe2DimArrayAccess(Int32[,] a)
        {
            Int32 sum = 0;
            const Int32 c_numElements = 10000;
            fixed (Int32* pi = a)//Fixed keyword make code complicated
            {
                for (Int32 x = 0; x < c_numElements; x++)
                {
                    Int32 baseOfDim = x * c_numElements;//memory address caculation should be carefull, otherwise will corrupt system, or security hole,type-safty violation
                    for (Int32 y = 0; y < c_numElements; y++)
                    {
                        sum += pi[baseOfDim + y];
                    }
                }
            }
            return sum;
        }

        private static void StackAllocDemo()
        {
            unsafe
            {
                const int width = 20;
                char* pc = stackalloc Char[width];//分配在stack上，必须是single-dimension, zero-based, value type element, and value type not contain reference type

                String s = "MingXing SU";

                for (int i = 0; i < width; i++)
                {
                    pc[width - i - 1] = i < s.Length ? s[i] : '.';
                }

                Console.WriteLine(new string(pc, 0, width));
            }
        }

        private static unsafe void InlineArrayDemo()
        {
            UnsafeStructCharArray ca;

            int widthInBytes = sizeof (UnsafeStructCharArray);
            int width = widthInBytes/2;

            String s = "MingXing SU";

            for (int i = 0; i < width; i++)
            {
                ca.Characters[width - i - 1] = i < s.Length ? s[i] : '.';
            }

            Console.WriteLine(new string(ca.Characters,0,width));
        }

    }

    //Have to be usafe and struct
    public unsafe struct UnsafeStructCharArray
    {
        //Inline Arrays
        public fixed Char Characters [20];//has to be Fixed，分配在堆上
    }


}
