using System;
using System.Collections.Generic;
using vJoyInterfaceWrap;

namespace iDash
{
    public class VJoyFeeder
    {
        // Declaring one joystick (Device id 1) and a position structure. 
        private static uint CENTER = 16384;
        private static uint AXIS_OFFSET = 4;
        public vJoy joystick;
        public uint jID = 1;
        private uint axisX = 0, axisY = 0;

        //events
        public delegate void StatusMessageHandler(string m);
        public StatusMessageHandler StatusMessageSubscribers;        


        public VJoyFeeder(ButtonHandler bh)
        {
            bh.buttonStateHandler += ButtonStateReceived;
        }        

        public void InitializeJoystick()
        {
            joystick = new vJoy();           

            // Get the driver attributes (Vendor ID, Product ID, Version Number)
            if (!joystick.vJoyEnabled())
            {
                NotifyStatusMessage("vJoy driver not enabled: Failed Getting vJoy attributes.");
                return;
            }
            else
                NotifyStatusMessage(String.Format("Vendor: {0} Product :{1} Version Number:{2}", joystick.GetvJoyManufacturerString(), joystick.GetvJoyProductString(), joystick.GetvJoySerialNumberString()));

            // Get the state of the requested device
            VjdStat status = joystick.GetVJDStatus(jID);
            switch (status)
            {
                case VjdStat.VJD_STAT_OWN:
                    NotifyStatusMessage(String.Format("vJoy Device {0} is already owned by this feeder", jID));
                    break;
                case VjdStat.VJD_STAT_FREE:
                    NotifyStatusMessage(String.Format("vJoy Device {0} is free", jID));
                    break;
                case VjdStat.VJD_STAT_BUSY:
                    NotifyStatusMessage(String.Format("vJoy Device {0} is already owned by another feeder. Cannot continue", jID));
                    return;
                case VjdStat.VJD_STAT_MISS:
                    NotifyStatusMessage(String.Format("vJoy Device {0} is not installed or disabled. Cannot continue", jID));
                    return;
                default:
                    NotifyStatusMessage(String.Format("vJoy Device {0} general error. Cannot continue", jID));
                    return;
            };

            // Check which axes are supported
            bool AxisX = joystick.GetVJDAxisExist(jID, HID_USAGES.HID_USAGE_X);
            bool AxisY = joystick.GetVJDAxisExist(jID, HID_USAGES.HID_USAGE_Y);
            bool AxisZ = joystick.GetVJDAxisExist(jID, HID_USAGES.HID_USAGE_Z);
            bool AxisRX = joystick.GetVJDAxisExist(jID, HID_USAGES.HID_USAGE_RX);
            bool AxisRZ = joystick.GetVJDAxisExist(jID, HID_USAGES.HID_USAGE_RZ);
            // Get the number of buttons and POV Hat switchessupported by this vJoy device
            int nButtons = joystick.GetVJDButtonNumber(jID);
            int ContPovNumber = joystick.GetVJDContPovNumber(jID);
            int DiscPovNumber = joystick.GetVJDDiscPovNumber(jID);

            // Print results
            NotifyStatusMessage(String.Format("vJoy Device {0} capabilities:", jID));
            NotifyStatusMessage(String.Format("Numner of buttons\t{0}", nButtons));
            NotifyStatusMessage(String.Format("Numner of Continuous POVs\t{0}", ContPovNumber));
            NotifyStatusMessage(String.Format("Numner of Descrete POVs\t{0}", DiscPovNumber));
            NotifyStatusMessage(String.Format("Axis X\t\t{0}", AxisX ? "Yes" : "No"));
            NotifyStatusMessage(String.Format("Axis Y\t\t{0}", AxisX ? "Yes" : "No"));
            NotifyStatusMessage(String.Format("Axis Z\t\t{0}", AxisX ? "Yes" : "No"));
            NotifyStatusMessage(String.Format("Axis Rx\t\t{0}", AxisRX ? "Yes" : "No"));
            NotifyStatusMessage(String.Format("Axis Rz\t\t{0}", AxisRZ ? "Yes" : "No"));

            // Test if DLL matches the driver
            UInt32 DllVer = 0, DrvVer = 0;
            bool match = joystick.DriverMatch(ref DllVer, ref DrvVer);
            if (match)
                NotifyStatusMessage(String.Format("Version of Driver Matches DLL Version ({0:X})", DllVer));
            else
                NotifyStatusMessage(String.Format("Version of Driver ({0:X}) does NOT match DLL Version ({1:X})", DrvVer, DllVer));


            // Acquire the target
            if ((status == VjdStat.VJD_STAT_OWN) || ((status == VjdStat.VJD_STAT_FREE) && (!joystick.AcquireVJD(jID))))
            {
                NotifyStatusMessage(String.Format("Failed to acquire vJoy device number {0}.", jID));
                return;
            }
            else
                NotifyStatusMessage(String.Format("Acquired: vJoy device number {0}.", jID));

        }

        //----------------------------------------- Events

        public void NotifyStatusMessage(string args)
        {
            StatusMessageHandler handler = StatusMessageSubscribers;

            if (handler != null)
            {
                handler(args + "\n");
            }
        }

        //direction: 0=not pressed, +1=up or right pressed, -1=down or left pressed
        private uint calculateAxisPosition(uint previous, int direction)
        {
            if (direction == 0) return CENTER;
            if (previous > 0 && direction < 0)
            {
                return CENTER - 1000;
            }

            if (previous < 0 && direction > 0)
            {
                return 1000 + CENTER;
            }

            if (direction > 0 && previous < 31767)
            {
                return previous += 1000;
            }
            if (direction < 0 && previous > 1000)
            {
                return previous -= 1000;
            }

            return previous;
        }

        private void setAxis(List<State> states)
        {
            int directionX = 0, directionY = 0;
            //0=up, 1=down, 2=right, 3=left
            if (states[0] == State.KeyDown || states[0] == State.KeyHold)
            {
                directionX = 1;
            } else if(states[1] == State.KeyDown || states[1] == State.KeyHold)
            {
                directionX = -1;
            }
            if (states[2] == State.KeyDown || states[2] == State.KeyHold)
            {
                directionX = 1;
            }
            else if (states[3] == State.KeyDown || states[3] == State.KeyHold)
            {
                directionX = -1;
            }

            axisX = calculateAxisPosition(axisX, directionX);
            axisY = calculateAxisPosition(axisY, directionY);

            joystick.SetAxis((int)axisX, jID, HID_USAGES.HID_USAGE_X);
            joystick.SetAxis((int)axisY, jID, HID_USAGES.HID_USAGE_Y);
        }

        public void ButtonStateReceived(List<State> states)
        {
            
            setAxis(states);

            for (uint i = AXIS_OFFSET; i < states.Count; i++)
            {
                joystick.SetBtn(states[(int)i] == State.KeyDown || states[(int)i] == State.KeyHold, jID, i + 1 - AXIS_OFFSET);                
            }
        }

    }
}
