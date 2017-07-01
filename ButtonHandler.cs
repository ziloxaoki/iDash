using System;
using System.Collections.Generic;

namespace iDash
{

    public enum State
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

    public class ButtonHandler : CommandHandler
    {
        List<State> currentStates = new List<State>();

        //----------------------------------------- Events
        public delegate void ButtonStateHandler(List<State> states);
        public ButtonStateHandler buttonStateHandler;
        private uint vJoyID = 1;

        public ButtonHandler(SerialManager sm, uint vJoyID) : base(sm) {
            sm.CommandReceivedSubscribers += new SerialManager.CommandReceivedHandler(executeCommand);
            this.vJoyID = vJoyID;
        }

        public uint getVJoyID()
        {
            return vJoyID;
        }

        private void updateKeyState(byte[] bStates)
        {
            for (int i = 0; i < bStates.Length; i++)
            {
                //button is not pressed or was released
                if (bStates[i] == 0)
                {
                    //if button is valid update the state to the next up state
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
                    //if button is valid update the state to the next down state
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
            
            NotifyButtonStateReceived(currentStates);
        } 

        public override void executeCommand(Command command)
        {
            //is the command a status button command
            if(command.getData()[0] == Command.CMD_BUTTON_STATUS)
            {
                this.updateKeyState(Utils.getSubArray(command.getData(), 1, command.getData().Length - 1));
            }
        }

        //----------------------------------------- Events
        //notify subscribers about button state received event
        public void NotifyButtonStateReceived(List<State> args)
        {
            ButtonStateHandler handler = buttonStateHandler;

            if (handler != null)
            {
                handler(args);
            }
        }
    }
}
