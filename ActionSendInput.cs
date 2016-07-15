using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;

namespace iDash
{
    class ActionSendInput : ActionHandler
    {
        public ActionSendInput(MainForm mainForm, string action) : base(mainForm, action)
        {
            this.mainForm = mainForm;
            this.action = action;
        }

        public override void process()
        {
            InputSimulator.SimulateTextEntry(action);
        }
    }
}
