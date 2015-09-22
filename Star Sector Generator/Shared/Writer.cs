using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public static class Writer
    {
        public static void WriteLine(string line)
        {
#if CONSOLE
            Console.WriteLine(line);
#endif
            Debug.WriteLine(line);
        }

        public static void Write(string text)
        {
#if CONSOLE
            Console.Write(text);
#endif
            Debug.Write(text);
        }
    }
}
