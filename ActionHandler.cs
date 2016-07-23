using System.Windows.Forms;

namespace iDash
{
    public abstract class ActionHandler
    {
        public static readonly string[] ACTIONS = { ACT_NEXT_VIEW, ACT_PREVIOUS_VIEW };
        public const string ACT_NEXT_VIEW = "NextView";
        public const string ACT_PREVIOUS_VIEW = "PreviousView";
        public const string ACT_SEND_KEY = "SendKey";

        protected MainForm mainForm;        

        public ActionHandler(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }

        abstract public void process(string action, State key);
    }
}
