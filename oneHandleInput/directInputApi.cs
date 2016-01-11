using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.DirectX.DirectInput;

namespace oneHandleInput
{
    public class directInputApi
    {
        static private Device m_currentJoystick;
        static private List<DeviceInstance> m_joyInstance;
        static private JoystickState m_joyState, m_lastJoyState;

        public static void init()
        {
            m_currentJoystick = null;
            m_joyInstance = new List<DeviceInstance>();
            m_joyState = new JoystickState();
            m_lastJoyState = new JoystickState();
        }

        public static void term()
        {
            if (m_currentJoystick != null)
            {
                m_currentJoystick.Dispose();
            }

            m_joyInstance.Clear();
        }

        public static void enumerateJoystick()
        {
            m_joyInstance.Clear();

            DeviceList joyDeviceList = Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);
            
            foreach (DeviceInstance joyInstance in joyDeviceList)
            {
                m_joyInstance.Add(joyInstance);
            }
        }

        public static void selectJoystick(int idx)
        {
            if (m_currentJoystick != null)
            {
                m_currentJoystick.Dispose();
            }

            m_currentJoystick = new Device(m_joyInstance[idx].InstanceGuid);
            m_currentJoystick.SetCooperativeLevel(null, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
            m_currentJoystick.SetDataFormat(DeviceDataFormat.Joystick);

            m_currentJoystick.Acquire();
        }

        public static void update()
        {
            m_lastJoyState = m_joyState;

            if (m_currentJoystick == null)
            {
                return;
            }

            try
            {
                m_currentJoystick.Poll();
                m_currentJoystick.Acquire();

                m_joyState = m_currentJoystick.CurrentJoystickState;
            }
            catch
            {
                m_currentJoystick.Dispose();
                m_currentJoystick = null;
            }
        }

        public static Device currentJoystick
        {
            get
            {
                return m_currentJoystick;
            }
        }

        public static List<DeviceInstance> joystickList
        {
            get
            {
                return m_joyInstance;
            }
        }

        public static JoystickState currentJoystickState
        {
            get
            {
                return m_joyState;
            }
        }

        public static JoystickState lastJoystickState
        {
            get
            {
                return m_lastJoyState;
            }
        }
    }
}
