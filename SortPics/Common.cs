using static System.Console;

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
                WriteLine($"ERROR: {msg}");
            }
            System.Environment.Exit(1);
        }
    }
}
