using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDash
{
    public class ActionNextView : ActionHandler
    {
        public ActionNextView(MainForm mainForm, string action) : base(mainForm, action)
        {
            this.mainForm = mainForm;
            this.action = action;
        }

        public override void process()
        {
            mainForm.setNextView();
        }
    }
}
