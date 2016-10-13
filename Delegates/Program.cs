using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Delegates
{

    // Declare a delegate type; instances refer to a method that
    // takes an Int32 parameter and returns void.
    internal delegate void Feedback(Int32 value);

    //Delegate support covariance and contra-variance for reference type, not for Value Type due to different memory structure.
        //1.Covariance means that a method can return a type that is derived from the delegate’s return type. 
        //2.Contra-variance means that a method can take a parameter that is a base of the delegate’s parameter type.
    // 记忆技巧：父进子出；宽进严出；入逆，出协；参逆，返协

    //Delegate's essentials, please refer to picture named DelegateDefinition_IL.JPG in this project
    //internal class Feedback2 :MulticastDelegate
    //{    
    //    public Feedback2(Object @object, IntPtr method);// Constructor
    //    public virtual void Invoke(Int32 value);
    //}

    //You should be aware that delegate types can be defined within a type (nested within another type) or at global scope. Basically, because delegates are classes, a delegate can be defined anywhere a class can be defined.

    //Delegate object's 3 important fieds:_target: instance method's owner, null for static method; _methodPtr:point to the address to be called back; _invocationList: to store the chain of methods

    //Delegates Chain:Delegate.Combine() == '+='; Delegate.Remove() == '-='
    //Limitation for chains: Only last return result can be returned; Exception throwing will block the invoking. solution:GetInvocationList, then manipulate everyone by iteration. 

    //Generic Delegate : support for at most 16 parameters overloads
    //No return values:Action,
        //public delegate void Action();
        //public delegate void Action<T>(T obj);
    // Has Return Values: Func 
        //public delegate TResult Func<T1, T2, TResult>(T1 obj1, T2 obj2); 

    //Has to customize delegates for : ref, out, params parameters

    //Lambda表达式的意义是：remove a level of indirection，去掉中间环节的方法定义等等。避免太绕了。是匿名方法(c# 2.0)的升级，不再推荐匿名方法。

    //syntax sugar#1: no need to construct a delegate object. like button1.Clicked = button1_click, instead of new EventHandler(button1_click)
    //syntax sugar#2: no need to define a callback method(lambda expression): =>, it's WaitCallback type: it must take object as parameter,void as return value, if there was return clause, wil be ignored, it can reference any static field or methods.
    //Syntactical Shortcut #3: No Need to Wrap Local Variables in a Class Manually to Pass Them to a Callback Method
    
    //超过三行就要自定义方法，而不是lambda，以下应用很自然：
    //names = Array.ConvertAll(names, name => name.ToUpper());
    //Array.ForEach(names, Console.WriteLine);

    //Dynamic create delegate instance and invoke
        //Type deleType = Type.GetType("MyDelegateName");
        //MethodInfo mi =
        //typeof (ClassThatContainMethodDefinition).GetTypeInfo().GetDeclaredMethod("CallbackMethodName");
        //Delegate d = mi.CreateDelegate(deleType);//d is the delegate instance wrapper the static method:CallbackMethodName



    public sealed class Program
    {
        public static void Main()
        {
            Func<int, string> getStatusAction = new Func<int, string>(GetStatusMethod);
            NotifyStatusChanged(1,getStatusAction);//normal instantiate way
            NotifyStatusChanged(2,GetStatusMethod);//syntax sugar#1: no need to construct a delegate object. like button1.Clicked = button1_click, instead of new EventHandler(button1_click)

            StaticDelegateDemo();
            InstanceDelegateDemo();
            ChainDelegateDemo1(new Program());
            ChainDelegateDemo2(new Program());


            Console.Read();
        }

        private static void StaticDelegateDemo()
        {
            Console.WriteLine("----- Static Delegate Demo -----");
            Counter(1, 3, null);
            Counter(1, 3,FeedbackToConsole);
            Counter(1, 3, new Feedback(Program.FeedbackToConsole));//Method name can be the instance object reference, no delegate constructor needed.
            Counter(1, 3, new Feedback(FeedbackToMsgBox)); // "Program." is optional

            Console.WriteLine();
        }

        private static void InstanceDelegateDemo()
        {
            Console.WriteLine("----- Instance Delegate Demo -----");
            Program p = new Program();
            Counter(1, 3, new Feedback(p.FeedbackToFile));
            Console.WriteLine();
        }

        private static void ChainDelegateDemo1(Program p)
        {
            Console.WriteLine("----- Chain Delegate Demo 1 -----");

            Feedback fb1 = new Feedback(FeedbackToConsole);
            Feedback fb2 = new Feedback(FeedbackToMsgBox);
            //Feedback fb3 = new Feedback(p.FeedbackToFile);
            Action<int> fb3 = new Action<int>(FeedbackToConsole);
            Feedback fbChain = null;
            fbChain = (Feedback) Delegate.Combine(fbChain, fb1);
            fbChain = (Feedback) Delegate.Combine(fbChain, fb2);
            fbChain = (Feedback) Delegate.Combine(fbChain, fb3);
            Counter(1, 2, fbChain);
            Console.WriteLine();
            fbChain = (Feedback)
                Delegate.Remove(fbChain, new Feedback(FeedbackToMsgBox));
            Counter(1, 2, fbChain);
        }

        private static void ChainDelegateDemo2(Program p)
        {
            Console.WriteLine("----- Chain Delegate Demo 2 -----");
            Feedback fb1 = new Feedback(FeedbackToConsole);
            Feedback fb2 = new Feedback(FeedbackToMsgBox);
            Feedback fb3 = new Feedback(p.FeedbackToFile);
            Feedback fbChain = null;
            fbChain += fb1;
            fbChain += fb2;
            fbChain += fb3;
            Counter(1, 2, fbChain);
            Console.WriteLine();
            fbChain -=FeedbackToMsgBox;
            Counter(1, 2, fbChain);
        }

        //Accept a delegate instance as a parameter to callback fb linked methods
        private static void Counter(Int32 from, Int32 to, Feedback fb)
        {
            for (Int32 val = from; val <= to; val++)
            {
                // If any callbacks are specified, call them
                if (fb != null)
                    fb(val);//invoke methods,in IL: callvirt void Feedback::Invoke(int32)
                //fb.Invoke();fb.GetInvocationList()
                
            }
        }

        private static void FeedbackToConsole(Int32 value)
        {
            Console.WriteLine("Item=" + value);
        }

        private static void FeedbackToMsgBox(Int32 value)
        {
            MessageBox.Show("Item=" + value);
        }

        private void FeedbackToFile(Int32 value)
        {
            using (StreamWriter sw = new StreamWriter("Status", true))
            {
                sw.WriteLine("Item=" + value);
            }
        }

        private static void NotifyStatusChanged(int statusCode, Func<int, string> getStatus)
        {
            string result = getStatus.Invoke(statusCode);
            Console.WriteLine(result);
        }

        private static string GetStatusMethod(int statusCode)
        {
            return String.Format("Status displayed in UI:{0} ", statusCode);
        }

        private static void TestLambdaExpression()
        {
            Func<string>  f= ()=> "Max";//无参数就只有括号
            Func<int, string> f1 = (int age) => "Max:" + age;//指定参数类型
            Func<int, int, string> f2 = (int n1, int n2) => (n1 + n2).ToString();//逗号分隔参数

            // If the delegate takes 1 argument, you can omit the ()s
            Func<Int32, String> f6 = n => n.ToString();//可以省略（）

            // If the delegate has ref/out arguments, you must explicitly specify ref/out and the type
            Bar b = (out Int32 n) => n = 5;//out

            Func<Int32, Int32, String> f7 = (n1, n2) => { Int32 sum = n1 + n2; return sum.ToString(); };//注意括号和返回值

        }
        delegate void Bar(out Int32 z);

        //Demo for syntax sugar#3:No Need to Wrap Local Variables in a Class Manually to Pass Them to a Callback Method
        //不错，这样可以直接更新操作的容器中的数据，不用把context中的局部变量当做入参传入lambda表达式
        public static void UsingLocalVariablesInTheCallbackCode(Int32 numToDo)
        {
            // Some local variables
            Int32[] squares = new Int32[numToDo];
            AutoResetEvent done = new AutoResetEvent(false);
            // Do a bunch of tasks on other threads： CLR wraper all loca var to fields of a annoymous class, and as a parameter of waitcallback object
            for (Int32 n = 0; n < squares.Length; n++)
            {
                ThreadPool.QueueUserWorkItem(
                    obj =>
                    {
                        Int32 num = (Int32) obj;
                         // This task would normally be more time consuming
                        squares[num] = num*num;
                        // If last task, let main thread continue running
                        if (Interlocked.Decrement(ref numToDo) == 0)
                            done.Set();
                    },
                    n);
            }
            // Wait for all the other threads to finish
            done.WaitOne();
            // Show the results
            for (Int32 n = 0; n < squares.Length; n++)
                Console.WriteLine("Index {0}, Square={1}", n, squares[n]);
        }
    }
}
