using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BigFlags
{
    class Program
    {
        static void Main(string[] args)
        {
            String file = Assembly.GetEntryAssembly().Location;
            FileAttributes att = File.GetAttributes(file);
            bool isHidden = (att & FileAttributes.Hidden) !=0;//位运算
            
            Console.WriteLine("Is file {0} hidden? {1}",file,isHidden);
            Console.WriteLine("Is file {0} read-only? {1}", file, att.HasFlag(FileAttributes.ReadOnly));//HasFlag method will box the parameter

            //Change file to hidden and readonly
            //File.SetAttributes(file, FileAttributes.Hidden | FileAttributes.ReadOnly);

            LiftStatus repair = LiftStatus.StopAtFloor | LiftStatus.Closed;
            Console.WriteLine(repair.ToString());

            LiftStatusWithoutFlag danger = LiftStatusWithoutFlag.Running | LiftStatusWithoutFlag.Open;
            Console.WriteLine(danger.ToString("F"));

            //Parse
            LiftStatus statusList =(LiftStatus) Enum.Parse(typeof (LiftStatus), "Running, StopAtFloor", false);
            Console.WriteLine(statusList.ToString());

            //Parse
            LiftStatus statusList2 = (LiftStatus)Enum.Parse(typeof(LiftStatus), "1", false);
            Console.WriteLine(statusList2.ToString());

            //Never use IsDefined for  bit-flags enums. 
            //  1.comma seperated string will be treated as a whole one 
            //  2. always return false for inputted number


            //使用扩展方法扩展Enum
            Console.WriteLine(att.IsSet(FileAttributes.System));

            Console.Read();

        }
    }


    [Flags]
    public enum LiftStatus
    {
        Open = 0x00001,
        Closed = 0x00002,
        Running = 0x00004,
        StopAtFloor = 0x00010,
        Repairing = LiftStatus.Closed | LiftStatus.StopAtFloor
    }

    public enum LiftStatusWithoutFlag
    {
        Open = 0x00001,
        Closed = 0x00002,
        Running = 0x00004,
        StopAtFloor = 0x00010,
        Repairing = LiftStatus.Closed | LiftStatus.StopAtFloor
    }
}
