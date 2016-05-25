using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDash
{

    enum State
    {
        None,
        KeyDown,
        KeyUp,
        KeyHold
    };

    static class StateExtensions
    {
        public static State NextDownState(this State s)
        {
            switch (s)
            {
                case State.None:
                    return State.KeyDown;
                case State.KeyDown:
                    return State.KeyHold;
                case State.KeyHold:
                    return State.KeyHold;
                case State.KeyUp:
                    return State.KeyDown;
                default:
                    return State.None;
            }
        }

        public static State NextUpState(this State s)
        {
            switch (s)
            {
                case State.None:
                    return State.None;
                case State.KeyDown:
                    return State.KeyUp;
                case State.KeyHold:
                    return State.KeyUp;
                case State.KeyUp:
                    return State.None;
                default:
                    return State.None;
            }
        }

    }

    class ButtonHandler : CommandHandler
    {
        List<State> currentStates = new List<State>();

        //----------------------------------------- Events
        public delegate void ButtonStateHandler(byte[] states);
        public ButtonStateHandler ButtonStateSubscribers;

        public ButtonHandler(SerialManager sm) : base(sm) {
            sm.CommandReceivedSubscribers += new SerialManager.CommandReceivedHandler(executeCommand);            
        }

        private void updateKeyState(byte[] bStates)
        {
            for (int i = 0; i < bStates.Length; i++)
            {
                //if button is not pressed
                if (bStates[i] == 0)
                {
                    //if button states is stored in the list
                    if (i < currentStates.Count)
                    {
                        currentStates[i] = currentStates[i].NextUpState();
                    }
                    else
                    {
                        //default button state when not pressed
                        currentStates.Add(State.None);
                    }
                }
                else
                {
                    //if button states is stored in the list
                    if (i < currentStates.Count)
                    {
                        currentStates[i] = currentStates[i].NextDownState();
                    }
                    else
                    {
                        //default button state when pressed
                        currentStates.Add(State.KeyDown);
                    }
                }
            }
            
            NotifyButtonStateReceived(bStates);
        } 

        public void executeCommand(Command command)
        {
            //is the command a status button command
            if(command.getData()[0] == Command.CMD_BUTTON_STATUS)
            {
                this.updateKeyState(Utils.getSubArray(command.getData(), 1, command.getData().Length - 1));
            }
        }

        //----------------------------------------- Events
        //notify subscribers about button state received event
        public void NotifyButtonStateReceived(byte[] args)
        {
            ButtonStateHandler handler = ButtonStateSubscribers;

            if (handler != null)
            {
                handler(args);
            }
        }
    }
}
