using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppWindows.Domain.Service
{
    public class CommandService
    {
        public static void Process(String command)
        {
            switch (command)
            {
                case "GetInput":
                    GetInput();
                    break;
                default:
                    App.instance.Message(command);
                    break;
            }
        }

        public static void GetInput()
        {
            App.instance.GetInput();
        }

    }
}
