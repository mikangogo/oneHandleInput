using System;
using System.Collections.Generic;

using SlimDX.DirectInput;

namespace oneHandleInput
{
    public class directInputApi
    {
        static private DirectInput DirectInputManager { get; set; }
        static private Joystick m_currentJoystick;
        static private List<DeviceInstance> m_joyInstance;
        static private JoystickState m_joyState, m_lastJoyState;

        public static void init()
        {
            DirectInputManager = new DirectInput();

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

            var joyDeviceList = DirectInputManager.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly);

            if (joyDeviceList != null)
            {
                foreach (DeviceInstance joyInstance in joyDeviceList)
                {
                    m_joyInstance.Add(joyInstance);
                }
            }
        }

        public static void selectJoystick(int idx, IntPtr ownerWindow)
        {
            if (m_currentJoystick != null)
            {
                m_currentJoystick.Dispose();
            }

            m_currentJoystick = new Joystick(DirectInputManager, m_joyInstance[idx].InstanceGuid);
            m_currentJoystick.SetCooperativeLevel(ownerWindow, CooperativeLevel.Background | CooperativeLevel.Nonexclusive);

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

                m_joyState = m_currentJoystick.GetCurrentState();
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
