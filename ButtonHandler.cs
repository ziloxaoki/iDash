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
        public delegate void ButtonStateHandler(List<State> states, int arduinoId);
        public ButtonStateHandler buttonStateHandler;

        public ButtonHandler(SerialManager sm) : base(sm) {
            sm.CommandReceivedSubscribers += new SerialManager.CommandReceivedHandler(executeCommand);
        }

        private void updateKeyState(byte[] bStates)
        {
            //bStates[0] is the arduino id
            //bStates[1] is the command id
            for (int i = 2; i < bStates.Length - 2; i++)
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
            
            NotifyButtonStateReceived(currentStates, bStates[0]);
        } 

        public override void executeCommand(Command command)
        {
            //is the command a status button command
            if(command.getData()[1] == Command.CMD_BUTTON_STATUS)
            {
                this.updateKeyState(command.getData());
            }
        }

        //----------------------------------------- Events
        //notify subscribers about button state received event
        public void NotifyButtonStateReceived(List<State> args, int arduinoId)
        {
            ButtonStateHandler handler = buttonStateHandler;

            if (handler != null)
            {
                handler(args, arduinoId);
            }
        }
    }
}
