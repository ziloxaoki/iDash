using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iDash
{
    public class ActionHandlerFactory
    {
        protected MainForm mainForm;

        public ActionHandlerFactory(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }

        public ActionHandler getInstance(string action)
        {
            switch (action)
            {
                case ActionHandler.ACT_NEXT_VIEW:
                    return new ActionNextView(mainForm, action);
                case ActionHandler.ACT_PREVIOUS_VIEW:
                    return new ActionPreviousView(mainForm, action);
                default:
                    return new ActionSendInput(mainForm, action);
            }
        }
    }
}
