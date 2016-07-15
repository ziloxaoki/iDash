using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDash
{
    public class ActionPreviousView : ActionHandler
    {
        public ActionPreviousView(MainForm mainForm, string action) : base(mainForm, action)
        {
            this.mainForm = mainForm;
            this.action = action;
        }

        public override void process()
        {
            mainForm.setPreviousView();
        }
    }
}
