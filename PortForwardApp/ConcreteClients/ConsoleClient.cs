using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortForward
{
    public class ConsoleClient : Client
    {
        public override void HandleRx(object sender, EventArgs e)
        {
            string message = (string)sender;
            Console.WriteLine("Message received: {0}", message);
        }
    }
}
