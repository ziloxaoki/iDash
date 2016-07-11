using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDash
{
    abstract class CommandHandler
    {
        private MainForm mainForm;
        private SerialManager sm;

        public CommandHandler(SerialManager sm, MainForm mainForm)
        {
            this.mainForm = mainForm;
            this.sm = sm;
        }
    }
}
