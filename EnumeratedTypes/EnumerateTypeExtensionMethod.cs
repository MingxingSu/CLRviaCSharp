using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumeratedTypes
{
    internal static class EnumerateTypeExtensionMethod
    {
        public static bool IsSet(this FileAttributes flags, FileAttributes attToTest)
        {
            if(attToTest ==0)
                throw new ArgumentOutOfRangeException("Value must not be 0");

            return (flags & attToTest) == attToTest;
        }

        public static bool IsClear(this FileAttributes flags, FileAttributes attToTest)
        {
            if (attToTest == 0)
                throw new ArgumentOutOfRangeException("Value must not be 0");

            return !IsSet(flags, attToTest);
        }

        public static bool AnyFlagsSet(this FileAttributes flags, FileAttributes testFlags)
        {
            return (flags & testFlags) != 0;
        }

        public static FileAttributes ClearFlags(this FileAttributes flags, FileAttributes clearFlags)
        {

            return flags & ~clearFlags;
        }
    }
}
