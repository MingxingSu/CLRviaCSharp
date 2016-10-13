using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomAttributes
{
    [assembly: CLSCompliant(true)]
    [Serializable]
    [DefaultMember("Run")]
    [DebuggerDisplay("Richter", Name = "Jeff", Target = typeof(TestDectingCustomAttClass))]
    public sealed class TestDectingCustomAttClass
    {
        [Conditional("Debug")]
        [Conditional("Release")]
        public void DoSomething()
        {
        }

        public TestDectingCustomAttClass()
        {
        }

        [CLSCompliant(true)]
        public static void Run()
        {
            // Show the set of attributes applied to this type
            ShowAttributes(typeof(TestDectingCustomAttClass));
                // Get the set of methods associated with the type
            MemberInfo[] members = typeof(TestDectingCustomAttClass).FindMembers(
                MemberTypes.Constructor | MemberTypes.Method,
                BindingFlags.DeclaredOnly | BindingFlags.Instance |
                BindingFlags.Public | BindingFlags.Static,
                Type.FilterName, "*");
            foreach (MemberInfo member in members)
            {
                // Show the set of attributes applied to this member
                ShowAttributes(member);
            }
        }

        private static void ShowAttributes(MemberInfo attributeTarget)
        {
            IList<CustomAttributeData> attributes =
                CustomAttributeData.GetCustomAttributes(attributeTarget);
            Console.WriteLine("Attributes applied to {0}: {1}",
                attributeTarget.Name, (attributes.Count == 0 ? "None" : String.Empty));
            foreach (CustomAttributeData attribute in attributes)
            {
                // Display the type of each applied attribute
                Type t = attribute.Constructor.DeclaringType;
                Console.WriteLine(" {0}", t.ToString());
                Console.WriteLine(" Constructor called={0}", attribute.Constructor);
                IList<CustomAttributeTypedArgument> posArgs = attribute.ConstructorArguments;
                Console.WriteLine(" Positional arguments passed to constructor:" +
                                  ((posArgs.Count == 0) ? " None" : String.Empty));
                foreach (CustomAttributeTypedArgument pa in posArgs)
                {
                    Console.WriteLine(" Type={0}, Value={1}", pa.ArgumentType, pa.Value);
                }
                IList<CustomAttributeNamedArgument> namedArgs = attribute.NamedArguments;
                Console.WriteLine(" Named arguments set after construction:" +
                                  ((namedArgs.Count == 0) ? " None" : String.Empty));
                foreach (CustomAttributeNamedArgument na in namedArgs)
                {
                    Console.WriteLine(" Name={0}, Type={1}, Value={2}",
                        na.MemberInfo.Name, na.TypedValue.ArgumentType, na.TypedValue.Value);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
