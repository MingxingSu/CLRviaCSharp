#define TEST
#define VERIFY

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

//declarativly annotate code constructs, enable special features
namespace CustomAttributes
{  

    class Program
    {
       
        static void Main(string[] args)
        {
            //Attributes can specify prefix
                //[assembly: SomeAttr] // Applied to assembly
                //[module: SomeAttr] // Applied to module
                //[type: SomeAttr] // Applied to type
                //[field:SomeAttr]
                //[return:SomeAttr]
                //[method:SomeAttr]
                //[param:SomeAttr]
                //[property::SomeAttr]
                //[event ::SomeAttr]


            //CustomAttributes derived from System.Attributes}
            
            TestDectingCustomAttClass.Run();

            Console.Read();


        }

        [method:RequireCredential("MyUser","MyPassowrd#1")]// parenthesis is to call its constructor
        //[DllImport("Kernel32", CharSet = CharSet.Auto, SetLastError = true)]//"Kernel32" passed to constructor, CharSet and SetLastError are public instance fields, they are "named parameters"
        public static void Login(string userid, string password)
        {
        }

          //一个目标可以有多个attributes
        //[Serializable][Flags]
        //[Serializable, Flags]
        //[FlagsAttribute, SerializableAttribute]
        //[FlagsAttribute()][Serializable()]

        //Attributes应该是一个状态容器，除了构造函数外不该有任何公共方法或者事件，只用必要的公共properties来保存option或状态，不推荐公共field，


    }

    //定义特性应用的类型
    [AttributeUsage(AttributeTargets.All,AllowMultiple = false,Inherited = true)]//these are the default attributes usage settings
    public class RequireCredentialAttribute : System.Attribute
    {
        //Attribute has to provide public constructor so t
        public RequireCredentialAttribute(string userid,string passowrd)//parameter type can only accept built in .net types, without arrays, custom class.
        {
            UserID = userid;
            Passowrd = passowrd;
        }
        public string UserID { set; get; }
        public string Passowrd { set; get; }
    }


    //Demo Innerited=true: means derived type no need to mark this flag if base type already marked
    //.NET Framework considers targets only of :classes, methods, properties, events,fields, method return values, and parameters to be inheritable.
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]//Inherited = true
    internal class TastyAttribute : Attribute
    {
    }
    [Tasty]
    [Serializable]
    internal class BaseType//Mark as [Tasty]
    {
        [Tasty]
        protected virtual void DoSomething() { }
    }
    internal class DerivedType : BaseType// should mark a [Tasty] flag, but no Serializable flag because this flag was not inherited.
    {
        protected override void DoSomething() { }
    }


    //Demo  Type[] as parameter
    internal enum Color { Red }
    [AttributeUsage(AttributeTargets.All)]
    internal sealed class SomeAttribute : Attribute
    {
        public SomeAttribute(String name, Object o, Type[] types)
        {
            // 'name' refers to a String
            // 'o' refers to one of the legal types (boxing if necessary)
            // 'types' refers to a 1-dimension, 0-based array of Types
        }
    }
    [Some("Jeff", Color.Red, new Type[] { typeof(Math), typeof(Console) })]// Color.Red will be boxed
    internal sealed class SomeType
    {
    }

    //Now that the custom attribute object is initialized, the compiler serializes the attribute  object’s state out to the target’s metadata table entry, like a json object string.
    //deserialized when being used.

    //Use Reflection to extract attributes object's setting when running codings. so that it will affect target's behavor
    internal class SomeClass
    {
        public override String ToString()
        {
            // Does the enumerated type have an instance of
            // the FlagsAttribute type applied to it?
            if (this.GetType().IsDefined(typeof (FlagsAttribute), false))
            {
                // Yes; execute code treating value as a bit flag enumerated type.
            }
            else
            {
            } // No; execute code treating value as a normal enumerated type.

            return "";


            ////几个有用的方法:位于CustomAttributeExtensions，调用方式如下：
            //this.GetType().GetCustomAttributes(typeof(MyCustomAttributes)),不高效， will  construct attribute instance, cache起来而不是重复调用可提高性能
            //this.GetType().CustomAttributes -- list，不高效， will  construct attribute instances
                //this.GetType().IsDefined(typeof(MyCustomAttributes))，高效， will not construct attribute instance
            
        }
        


        
    }

    //Detecting the Use of a Custom Attribute Without Creating Attribute-Derived Objects
    

    //Conditional Attributes: when want to compile attributes basing on different environment.
    [Conditional("TEST")][Conditional("VERIFY")]
    public sealed class CondAttribute : Attribute {
    }

    [Cond]
    public sealed class SomeProgram
    {
        public static void Run()
        {
            Console.WriteLine("CondAttribute is {0}applied to Program type.",
                Attribute.IsDefined(typeof (Program),
                    typeof (CondAttribute))
                    ? ""
                    : "not ");
        }
    }

}
