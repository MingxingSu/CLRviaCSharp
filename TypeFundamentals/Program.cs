using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using MingxingProgram = SuMingXing.Text.Program;

namespace TypeFundamentals
{
    // This type is implicitly derived from System.Object.
    class Program
    {
        private static void Main(string[] args)
        {
            //Every type derived from System.Object

            //New operator: 
            //1.caculate size of object (instance fiedls, type object pointer, sync block index)
            //2.Allocate memory in managed heap for it, value types set to zero.
            //3.Init type object pointer and sync block index
            //4.Call type's constuctor, in which base type's constructor will be called until Object type.
            Program p = new Program();

            //Get object's type
            Type t = p.GetType();

            //Implicitly casting
            Object o = new Program();

            //Explicitly casting
            Program p2 = (Program) o;

            //Type safety is therefore an extremely important part of the CLR

            //'Is' operator to check type compatitable, 'As' type casting
            Employee employee = new Employee();
            if (employee is Manager)
            {//do sth only manager can do
            }
            Manager manager = employee as Manager;
            if (manager != null)
            {
            }

            //namespaces:CRL know's nothing about namesspace, the short type will add their namepaces to be full type name for compiler
            //using can introduce namespaces
            //When checking for a type’s definition, the compiler must be told which assemblies to examine by using the /reference, will scan all referenced assemblies
            //MSCorLib.dll contain FCL, all base type definitions, has to be refrenced.

            //If there are types sharing the same name among different namespaces, you should use 'using' to define a short name for them, then ambiguation will disappear, or reference them with full name
            



        




    }
    }

    internal class Employee
    {
    }
    internal class Manager: Employee
    {
    }
}
