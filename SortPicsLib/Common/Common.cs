using System;

namespace SortPicsLib.Common
{
    public class Common
    {
        /// <summary>
        ///     Exit and optionally print an error message.
        /// </summary>
        /// <param name="msg"></param>
        public static void Die(string msg = null)
        {
            if (msg != null)
                Console.WriteLine($"ERROR: {msg}");
            Environment.Exit(1);
        }
    }
}