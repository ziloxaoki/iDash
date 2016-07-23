using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDash
{
    public class ActionPreviousView : ActionHandler
    {
        public ActionPreviousView(MainForm mainForm) : base(mainForm)
        {
            this.mainForm = mainForm;
        }

        public override void process(string action, State state)
        {
            if(state == State.KeyDown)
                mainForm.setPreviousView();
        }
    }
}
