using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.View
{
    public class ReturnMenu
    {
        public ReturnMenu()
        {

        }

        public void ReturnWelcome(string message)
        {
            Console.WriteLine(message);
            Console.ReadKey();
        }
    }
}
