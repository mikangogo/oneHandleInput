using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oneHandleInput
{
    public class oneHandleInput : Mackoy.Bvets.IInputDevice
    {
        public event Mackoy.Bvets.InputEventHandler KeyDown;
        public event Mackoy.Bvets.InputEventHandler KeyUp;
        public event Mackoy.Bvets.InputEventHandler LeverMoved;

        private ConfigForm m_configForm;
        private bool m_pauseTick;
        private bool m_first;

        private int m_lastReverserPos;
        private int m_lastPowerPos;
        private int m_lastBrakePos;
        private int m_lastSsbPos;

        public void Load(string settingsPath)
        {
            m_first = true;
            directInputApi.init();

            m_configForm = new ConfigForm();
            m_configForm.loadConfigurationFile(settingsPath);
            m_configForm.Hide();

            m_lastReverserPos = 0xFFFF;
            m_lastPowerPos = 0xFFFF;
            m_lastBrakePos = 0xFFFF;
            m_lastSsbPos = 0xFFFF;
        }

        public void Dispose()
        {
            m_configForm.Dispose();
            directInputApi.term();
        }

        public void Configure(System.Windows.Forms.IWin32Window owner)
        {
            m_first = false;
            m_pauseTick = true;
            m_configForm.ShowDialog(owner);
            m_pauseTick = false;
        }

        public void SetAxisRanges(int[][] ranges)
        {
        }

        public void Tick()
        {
            if (m_pauseTick)
            {
                return;
            }

            if (m_first)
            {
                m_configForm.enumerateDevices();
                m_first = false;
            }

            directInputApi.update();

            setReverserPos();
            setBrakePos();
            setPowerPos();
            setSsbPos();
            setSwitchState();
        }

        private void setSwitchState()
        {
            byte[] currentButtonState = directInputApi.currentJoystickState.GetButtons();
            byte[] lastButtonState = directInputApi.lastJoystickState.GetButtons();
            int buttonNum = directInputApi.currentJoystick.Caps.NumberButtons;

            for (int i = 0; i < buttonNum; ++i)
            {
                if (currentButtonState[i] != lastButtonState[i])
                {
                    if (currentButtonState[i] != 0)
                    {
                        int keyIdx = getKeyIdx(i);
                        if (keyIdx != -1)
                        {
                            if (keyIdx < 100)
                            {
                                onKeyDown(-2, keyIdx);
                            }
                            else
                            {
                                switch (keyIdx)
                                {
                                    case 100:
                                        onLeverMoved(0, 1);
                                        break;
                                    case 101:
                                        onLeverMoved(0, 0);
                                        break;
                                    case 102:
                                        onLeverMoved(0, -1);
                                        break;
                                    case 103:
                                        onKeyDown(-1, 0);
                                        break;
                                    case 104:
                                        onKeyDown(-1, 1);
                                        break;
                                    case 105:
                                        onKeyDown(-1, 3);
                                        break;
                                    case 106:
                                        onKeyDown(-1, 2);
                                        break;
                                }
                            }
                        }
                    }
                    else if (currentButtonState[i] == 0)
                    {
                        int keyIdx = getKeyIdx(i);
                        if (keyIdx != -1)
                        {
                            if (keyIdx < 100)
                            {
                                onKeyUp(-2, keyIdx);
                            }
                            else
                            {
                                switch (keyIdx)
                                {
                                    case 103:
                                        onKeyUp(-1, 0);
                                        break;
                                    case 104:
                                        onKeyUp(-1, 1);
                                        break;
                                    case 105:
                                        onKeyUp(-1, 3);
                                        break;
                                    case 106:
                                        onKeyUp(-1, 2);
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private int getKeyIdx(int i)
        {
            int keyIdx = -1;
            ConfigForm.ConfigFormSaveData config = m_configForm.Configuration;

            if (config.switchS == i)
            {
                keyIdx = 0;
            }
            else if (config.switchA1 == i)
            {
                keyIdx = 1;
            }
            else if (config.switchA2 == i)
            {
                keyIdx = 2;
            }
            else if (config.switchB1 == i)
            {
                keyIdx = 3;
            }
            else if (config.switchB2 == i)
            {
                keyIdx = 4;
            }
            else if (config.switchC1 == i)
            {
                keyIdx = 5;
            }
            else if (config.switchC2 == i)
            {
                keyIdx = 6;
            }
            else if (config.switchD == i)
            {
                keyIdx = 7;
            }
            else if (config.switchE == i)
            {
                keyIdx = 8;
            }
            else if (config.switchF == i)
            {
                keyIdx = 9;
            }
            else if (config.switchG == i)
            {
                keyIdx = 10;
            }
            else if (config.switchH == i)
            {
                keyIdx = 11;
            }
            else if (config.switchI == i)
            {
                keyIdx = 12;
            }
            else if (config.switchJ == i)
            {
                keyIdx = 13;
            }
            else if (config.switchK == i)
            {
                keyIdx = 14;
            }
            else if (config.switchL == i)
            {
                keyIdx = 15;
            }
            else if (config.switchReverserFront == i)
            {
                keyIdx = 100;
            }
            else if (config.switchReverserNeutral == i)
            {
                keyIdx = 101;
            }
            else if (config.switchReverserBack == i)
            {
                keyIdx = 102;
            }
            else if (config.switchHorn1 == i)
            {
                keyIdx = 103;
            }
            else if (config.switchHorn2 == i)
            {
                keyIdx = 104;
            }
            else if (config.switchMusicHorn == i)
            {
                keyIdx = 105;
            }
            else if (config.switchConstSpeed == i)
            {
                keyIdx = 106;
            }

            return keyIdx;
        }

        private void setPowerPos()
        {
            ConfigForm.ConfigFormSaveData config = m_configForm.Configuration;
            int notchPos = 0;

            if (config.powerAxis == (int)ConfigForm.AxisType.axisNothing)
            {
                return;
            }

            int pos = getAxisValue(config.powerAxis);

            if (config.powerAxisNegative)
            {
                pos = 0xFFFF - pos;
            }

            if (pos >= config.powerPosNeutral)
            {
                int notchStep = (config.powerPosMax - config.powerPosNeutral) / config.powerNotches;
                int currentNotch = 1;

                for (int i = config.powerNotches - 1; i >= 0; --i)
                {
                    int currentStep = (i * notchStep) + config.powerPosNeutral;
                    if (pos >= currentStep)
                    {
                        currentNotch += i;
                        break;
                    }
                }

                notchPos = currentNotch;
            }
            else
            {
                notchPos = 0;
            }

            if (notchPos != m_lastPowerPos)
            {
                onLeverMoved(1, notchPos);
            }

            m_lastPowerPos = notchPos;
        }

        private void setSsbPos()
        {
            ConfigForm.ConfigFormSaveData config = m_configForm.Configuration;
            int notchPos = 0;

            if (config.ssbAxis == (int)ConfigForm.AxisType.axisNothing)
            {
                return;
            }

            int pos = getAxisValue(config.ssbAxis);

            if (config.ssbAxisNegative)
            {
                pos = 0xFFFF - pos;
            }

            if (pos >= config.ssbPosNeutral)
            {
                int notchStep = (config.ssbPosMax - config.ssbPosNeutral) / config.ssbNotches;
                int currentNotch = 1;

                for (int i = config.ssbNotches - 1; i >= 0; --i)
                {
                    int currentStep = (i * notchStep) + config.ssbPosNeutral;
                    if (pos >= currentStep)
                    {
                        currentNotch += i;
                        break;
                    }
                }

                notchPos = currentNotch;
            }
            else
            {
                notchPos = 0;
            }

            if (notchPos != m_lastSsbPos)
            {
                onLeverMoved(1, -notchPos);
            }

            m_lastSsbPos = notchPos;
        }

        private void setBrakePos()
        {
            ConfigForm.ConfigFormSaveData config = m_configForm.Configuration;
            int notchPos = 0;
            bool force_move = false;

            if (config.brakeAxis == (int)ConfigForm.AxisType.axisNothing)
            {
                return;
            }

            int pos = getAxisValue(config.brakeAxis);

            if (config.brakeAxisNegative)
            {
                pos = 0xFFFF - pos;
            }

            if (pos >= config.brakePosEmr)
            {
                force_move = true;
                notchPos = config.brakeNotches + 1;
            }
            else if (pos >= config.brakePosNeutral)
            {
                int notchStep = (config.brakePosMax - config.brakePosNeutral) / config.brakeNotches;
                int currentNotch = 1;

                for (int i = config.brakeNotches - 1; i >= 0; --i)
                {
                    int currentStep = (i * notchStep) + config.brakePosNeutral;
                    if (pos >= currentStep)
                    {
                        currentNotch += i;
                        break;
                    }
                }

                notchPos = currentNotch;
            }
            else
            {
                force_move = true;
                notchPos = 0;
            }

            {
                int diff = Math.Abs(m_lastBrakePos - notchPos);

                if (diff > config.brakeChatter)
                {
                    onLeverMoved(2, notchPos);
                    m_lastBrakePos = notchPos;
                }
                else if (force_move)
                {
                    if (m_lastBrakePos != notchPos)
                    {
                        onLeverMoved(2, notchPos);
                    }

                    m_lastBrakePos = notchPos;
                }
            }
        }

        private void setReverserPos()
        {
            ConfigForm.ConfigFormSaveData config = m_configForm.Configuration;
            int notchPos = 0;

            if (config.reverserAxis == (int)ConfigForm.AxisType.axisNothing)
            {
                return;
            }

            int pos = getAxisValue(config.reverserAxis);

            if (config.reverserAxisNegative)
            {
                pos = 0xFFFF - pos;
            }

            if (pos >= config.reverserPosFront)
            {
                notchPos = 1;
            }
            else if (pos <= config.reverserPosBack)
            {
                notchPos = -1;
            }
            else
            {
                notchPos = 0;
            }

            if (m_lastReverserPos != notchPos)
            {
                onLeverMoved(0, notchPos);
            }

            m_lastReverserPos = notchPos;
        }

        private int getAxisValue(int axisType)
        {
            int axisValue = 0;
            switch ((ConfigForm.AxisType)axisType)
            {
                case ConfigForm.AxisType.axisX:
                    axisValue = directInputApi.currentJoystickState.X;
                    break;
                case ConfigForm.AxisType.axisY:
                    axisValue = directInputApi.currentJoystickState.Y;
                    break;
                case ConfigForm.AxisType.axisZ:
                    axisValue = directInputApi.currentJoystickState.Z;
                    break;
                case ConfigForm.AxisType.axisRx:
                    axisValue = directInputApi.currentJoystickState.Rx;
                    break;
                case ConfigForm.AxisType.axisRy:
                    axisValue = directInputApi.currentJoystickState.Ry;
                    break;
                case ConfigForm.AxisType.axisRz:
                    axisValue = directInputApi.currentJoystickState.Rz;
                    break;
            }

            return axisValue;
        }
    
        private void onLeverMoved(int axis, int notch)
        {
            if (LeverMoved != null)
            {
                LeverMoved(this, new Mackoy.Bvets.InputEventArgs(axis, notch));
            }
        }

        private void onKeyDown(int axis, int keyCode)
        {
            if (LeverMoved != null)
            {
                KeyDown(this, new Mackoy.Bvets.InputEventArgs(axis, keyCode));
            }
        }

        private void onKeyUp(int axis, int keyCode)
        {
            if (LeverMoved != null)
            {
                KeyUp(this, new Mackoy.Bvets.InputEventArgs(axis, keyCode));
            }
        }
    }
}
