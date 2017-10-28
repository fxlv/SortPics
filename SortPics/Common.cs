using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortPics
{
    class Common
    {
        /// <summary>
        /// Exit and optionally print an error message.
        /// </summary>
        /// <param name="msg"></param>
        public static void Die(string msg = null)
        {
            if (msg != null)
            {
                Console.WriteLine($"ERROR: {msg}");
            }
            System.Environment.Exit(1);
        }
    }
}
