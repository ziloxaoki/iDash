using System.Windows.Forms;

namespace iDash
{
    public abstract class ActionHandler
    {
        public static readonly string[] ACTIONS = { ACT_NEXT_VIEW, ACT_PREVIOUS_VIEW };
        public const string ACT_NEXT_VIEW = "NextView";
        public const string ACT_PREVIOUS_VIEW = "PreviousView";

        protected MainForm mainForm;
        protected string action;
        abstract public void process();

        public ActionHandler(MainForm mainForm, string action)
        {
            this.mainForm = mainForm;
            this.action = action;
        }
    }
}
