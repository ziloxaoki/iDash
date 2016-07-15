namespace iDash
{
    public abstract class CommandHandler : ICommandHandler
    {
        private SerialManager sm;

        public CommandHandler(SerialManager sm)
        {
            this.sm = sm;
        }

        public abstract void executeCommand(Command command);
    }
}
