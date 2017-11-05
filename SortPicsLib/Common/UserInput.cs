using System;

namespace SortPicsLib.Common
{
    public class UserInput
    {
        public static bool ConfirmContinue(string msg = null)
        {
            while (true)
            {
                if (msg != null)
                    Console.WriteLine(msg);
                Console.Write("Please answer with y/n: ");
                var userResponse = Console.ReadLine();
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