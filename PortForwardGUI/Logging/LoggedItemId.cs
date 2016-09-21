using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortForwardGUI.Logging
{
    public class LoggedItemId
    {
        private readonly int _id;

        public LoggedItemId(int id)
        {
            _id = id;
        }

        public int Id
        {
            get
            {
                return _id;
            }
        }
    }
}
