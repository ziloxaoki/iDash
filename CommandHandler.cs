using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDash
{
    abstract class CommandHandler
    {
        SerialManager sm;

        public CommandHandler(SerialManager sm)
        {
            this.sm = sm;
        }
    }
}
