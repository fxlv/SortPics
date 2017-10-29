using static System.Console;

namespace SortPics.Common
{
    internal class UserInput
    {
        public static bool ConfirmContinue(string msg = null)
        {
            while (true)
            {
                if (msg != null)
                    WriteLine(msg);
                Write("Please answer with y/n: ");
                var userResponse = ReadLine();
                switch (userResponse)
                {
                    case "y":
                        return true;
                    case "n":
                        return false;
                    default:
                        continue;
                }
            }
        }
    }
}