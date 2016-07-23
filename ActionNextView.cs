using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDash
{
    public class ActionNextView : ActionHandler
    {
        public ActionNextView(MainForm mainForm) : base(mainForm)
        {
            this.mainForm = mainForm;
        }

        public override void process(string action, State state)
        {
            if (state == State.KeyDown)
                mainForm.setNextView();
        }
    }
}
