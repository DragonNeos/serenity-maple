using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity
{
    public class CommandProcessor
    {
        public void Process(string pCommandString)
        {
            string[] Splitted = pCommandString.Split(' ');

            switch (Splitted[0].ToLower())
            {
                case "test":
                    Console.WriteLine("test");
                    break;
                default:
                    Console.WriteLine("Command {0} doesn't exist.", Splitted[0]);
                    break;
            }
        }
    }
}
