using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace oneHandleInput
{
    public partial class ConfigForm : Form
    {
        public struct ConfigFormSaveData
        {
            public Guid guid;

            public int reverserPosFront;
            public int reverserPosBack;
            public int reverserAxis;
            public bool reverserAxisNegative;

            public int brakePosEmr;
            public int brakePosMax;
            public int brakePosNeutral;
            public int brakeNotches;
            public int brakeChatter;
            public int brakeAxis;
            public bool brakeAxisNegative;

            public int powerPosNeutral;
            public int powerPosMax;
            public int powerNotches;
            public int powerAxis;
            public bool powerAxisNegative;

            public int ssbPosMax;
            public int ssbPosNeutral;
            public int ssbNotches;
            public int ssbAxis;
            public bool ssbAxisNegative;

            public int switchS;
            public int switchA1;
            public int switchA2;
            public int switchB1;
            public int switchB2;
            public int switchC1;
            public int switchC2;
            public int switchD;
            public int switchE;
            public int switchF;
            public int switchG;
            public int switchH;
            public int switchI;
            public int switchJ;
            public int switchK;
            public int switchL;
            public int switchReverserFront;
            public int switchReverserNeutral;
            public int switchReverserBack;
            public int switchHorn1;
            public int switchHorn2;
            public int switchMusicHorn;
            public int switchConstSpeed;
        };

        public enum AxisType
        {
            axisNothing = 0,
            axisX = 1,
            axisY,
            axisZ,
            axisRx,
            axisRy,
            axisRz,
        }

        private ConfigFormSaveData m_saveData;
        private const string FileName = "oneHandleInput.xml";
        
        private string m_configFilePath;

        public ConfigFormSaveData Configuration
        {
            get
            {
                return m_saveData;
            }
        }

        public void loadConfigurationFile(string path)
        {
            m_configFilePath = path;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ConfigFormSaveData));
                FileStream fs = new FileStream(Path.Combine(path, FileName), FileMode.Open);
                m_saveData = (ConfigFormSaveData)serializer.Deserialize(fs);
                fs.Close();
            }
            catch
            {
                
            }
        }

        public void saveConfigurationFile(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, FileName);

            XmlSerializer serializer = new XmlSerializer(typeof(ConfigFormSaveData));
            FileStream fs = new FileStream(path, FileMode.Create);

            serializer.Serialize(fs, m_saveData);
            fs.Close();
        }

        public ConfigForm()
        {
            InitializeComponent();

            List<string> axisArray = new List<string>();

            axisArray.Add("OFF");
            axisArray.Add("X");
            axisArray.Add("Y");
            axisArray.Add("Z");
            axisArray.Add("Rx");
            axisArray.Add("Ry");
            axisArray.Add("Rz");

            foreach (string s in axisArray)
            {
                cmbAxisBrake.Items.Add(s);
                cmbAxisPower.Items.Add(s);
                cmbAxisReverser.Items.Add(s);
                cmbAxisSsb.Items.Add(s);
            }

            m_saveData = new ConfigFormSaveData();

            m_saveData.switchS = fromSwitchString("OFF");
            m_saveData.switchA1 = fromSwitchString("OFF");
            m_saveData.switchA2 = fromSwitchString("OFF");
            m_saveData.switchB1 = fromSwitchString("OFF");
            m_saveData.switchB2 = fromSwitchString("OFF");
            m_saveData.switchC1 = fromSwitchString("OFF");
            m_saveData.switchC2 = fromSwitchString("OFF");
            m_saveData.switchD = fromSwitchString("OFF");
            m_saveData.switchE = fromSwitchString("OFF");
            m_saveData.switchF = fromSwitchString("OFF");
            m_saveData.switchG = fromSwitchString("OFF");
            m_saveData.switchH = fromSwitchString("OFF");
            m_saveData.switchI = fromSwitchString("OFF");
            m_saveData.switchJ = fromSwitchString("OFF");
            m_saveData.switchK = fromSwitchString("OFF");
            m_saveData.switchL = fromSwitchString("OFF");
            m_saveData.switchReverserFront = fromSwitchString("OFF");
            m_saveData.switchReverserNeutral = fromSwitchString("OFF");
            m_saveData.switchReverserBack = fromSwitchString("OFF");
            m_saveData.switchHorn1 = fromSwitchString("OFF");
            m_saveData.switchHorn2 = fromSwitchString("OFF");
            m_saveData.switchMusicHorn = fromSwitchString("OFF");
            m_saveData.switchConstSpeed = fromSwitchString("OFF");
        }

        private string toSwitchString(int n)
        { 
            switch (n)
            {
                case -1:    return "OFF";
                default:    return toString(n);
            }
        }

        private int fromSwitchString(string s)
        {
            switch (s)
            {
                case "OFF":     return -1;
                default:        return fromString(s);
            }
        }

        private void restoreConfiguration(ConfigFormSaveData saveData)
        {
            txtReverserFront.Text = toString(saveData.reverserPosFront);
            txtReverserBack.Text = toString(saveData.reverserPosBack);
            cmbAxisReverser.SelectedIndex = saveData.reverserAxis;
            chkAxisReverserNegative.Checked = saveData.reverserAxisNegative;

            txtBrakeEmr.Text = toString(saveData.brakePosEmr);
            txtBrakeMax.Text = toString(saveData.brakePosMax);
            txtBrakeNeutral.Text = toString(saveData.brakePosNeutral);
            txtBrakeNotches.Text = toString(saveData.brakeNotches);
            txtBrakeChatter.Text = toString(saveData.brakeChatter);
            cmbAxisBrake.SelectedIndex = saveData.brakeAxis;
            chkAxisBrakeNegative.Checked = saveData.brakeAxisNegative;

            txtPowerNeutral.Text = toString(saveData.powerPosNeutral);
            txtPowerMax.Text = toString(saveData.powerPosMax);
            txtPowerNotches.Text = toString(saveData.powerNotches);
            cmbAxisPower.SelectedIndex = saveData.powerAxis;
            chkAxisPowerNegative.Checked = saveData.powerAxisNegative;

            txtSsbNeutral.Text = toString(saveData.ssbPosNeutral);
            txtSsbMax.Text = toString(saveData.ssbPosMax);
            txtSsbNotches.Text = toString(saveData.ssbNotches);
            cmbAxisSsb.SelectedIndex = saveData.ssbAxis;
            chkAxisSsbNegative.Checked = saveData.ssbAxisNegative;

            txtSwS.Text = toSwitchString(saveData.switchS);
            txtSwA1.Text = toSwitchString(saveData.switchA1);
            txtSwA2.Text = toSwitchString(saveData.switchA2);
            txtSwB1.Text = toSwitchString(saveData.switchB1);
            txtSwB2.Text = toSwitchString(saveData.switchB2);
            txtSwC1.Text = toSwitchString(saveData.switchC1);
            txtSwC2.Text = toSwitchString(saveData.switchC2);
            txtSwD.Text = toSwitchString(saveData.switchD);
            txtSwE.Text = toSwitchString(saveData.switchE);
            txtSwF.Text = toSwitchString(saveData.switchF);
            txtSwG.Text = toSwitchString(saveData.switchG);
            txtSwH.Text = toSwitchString(saveData.switchH);
            txtSwI.Text = toSwitchString(saveData.switchI);
            txtSwJ.Text = toSwitchString(saveData.switchJ);
            txtSwK.Text = toSwitchString(saveData.switchK);
            txtSwL.Text = toSwitchString(saveData.switchL);
            txtSwReverserFront.Text = toSwitchString(saveData.switchReverserFront);
            txtSwReverserNeutral.Text = toSwitchString(saveData.switchReverserNeutral);
            txtSwReverserBack.Text = toSwitchString(saveData.switchReverserBack);
            txtSwHorn1.Text = toSwitchString(saveData.switchHorn1);
            txtSwHorn2.Text = toSwitchString(saveData.switchHorn2);
            txtSwMusicHorn.Text = toSwitchString(saveData.switchMusicHorn);
            txtSwConstSpeed.Text = toSwitchString(saveData.switchConstSpeed);
        }

        private ConfigFormSaveData saveConfiguration()
        {
            ConfigFormSaveData saveData = new ConfigFormSaveData();

            if (directInputApi.currentJoystick != null)
            {
                saveData.guid = directInputApi.currentJoystick.DeviceInformation.ProductGuid;
            }

            saveData.reverserPosFront = fromString(txtReverserFront.Text);
            saveData.reverserPosBack = fromString(txtReverserBack.Text);
            saveData.reverserAxis = cmbAxisReverser.SelectedIndex;
            saveData.reverserAxisNegative = chkAxisReverserNegative.Checked;

            saveData.brakePosEmr = fromString(txtBrakeEmr.Text);
            saveData.brakePosMax = fromString(txtBrakeMax.Text);
            saveData.brakePosNeutral = fromString(txtBrakeNeutral.Text);
            saveData.brakeNotches = fromString(txtBrakeNotches.Text);
            saveData.brakeChatter = fromString(txtBrakeChatter.Text);
            saveData.brakeAxis = cmbAxisBrake.SelectedIndex;
            saveData.brakeAxisNegative = chkAxisBrakeNegative.Checked;

            saveData.powerPosNeutral = fromString(txtPowerNeutral.Text);
            saveData.powerPosMax = fromString(txtPowerMax.Text);
            saveData.powerNotches = fromString(txtPowerNotches.Text);
            saveData.powerAxis = cmbAxisPower.SelectedIndex;
            saveData.powerAxisNegative = chkAxisPowerNegative.Checked;

            saveData.ssbPosNeutral = fromString(txtSsbNeutral.Text);
            saveData.ssbPosMax = fromString(txtSsbMax.Text);
            saveData.ssbNotches = fromString(txtSsbNotches.Text);
            saveData.ssbAxis = cmbAxisSsb.SelectedIndex;
            saveData.ssbAxisNegative = chkAxisSsbNegative.Checked;
 
            saveData.switchS = fromSwitchString(txtSwS.Text);
            saveData.switchA1 = fromSwitchString(txtSwA1.Text);
            saveData.switchA2 = fromSwitchString(txtSwA2.Text);
            saveData.switchB1 = fromSwitchString(txtSwB1.Text);
            saveData.switchB2 = fromSwitchString(txtSwB2.Text);
            saveData.switchC1 = fromSwitchString(txtSwC1.Text);
            saveData.switchC2 = fromSwitchString(txtSwC2.Text);
            saveData.switchD = fromSwitchString(txtSwD.Text);
            saveData.switchE = fromSwitchString(txtSwE.Text);
            saveData.switchF = fromSwitchString(txtSwF.Text);
            saveData.switchG = fromSwitchString(txtSwG.Text);
            saveData.switchH = fromSwitchString(txtSwH.Text);
            saveData.switchI = fromSwitchString(txtSwI.Text);
            saveData.switchJ = fromSwitchString(txtSwJ.Text);
            saveData.switchK = fromSwitchString(txtSwK.Text);
            saveData.switchL = fromSwitchString(txtSwL.Text);
            saveData.switchReverserFront = fromSwitchString(txtSwReverserFront.Text);
            saveData.switchReverserNeutral = fromSwitchString(txtSwReverserNeutral.Text);
            saveData.switchReverserBack = fromSwitchString(txtSwReverserBack.Text);
            saveData.switchHorn1 = fromSwitchString(txtSwHorn1.Text);
            saveData.switchHorn2 = fromSwitchString(txtSwHorn2.Text);
            saveData.switchMusicHorn = fromSwitchString(txtSwMusicHorn.Text);
            saveData.switchConstSpeed = fromSwitchString(txtSwConstSpeed.Text);

            return saveData;
        }

        public void enumerateDevices()
        {
            directInputApi.enumerateJoystick();
            int joyNum = directInputApi.joystickList.Count;

            cmbJoySelect.Items.Clear();

            if (joyNum == 0) return;

            cmbJoySelect.MaxDropDownItems = joyNum;

            for (int i = 0; i < joyNum; ++i)
            {
                cmbJoySelect.Items.Add(directInputApi.joystickList[i].ProductName);

                if (m_saveData.guid == directInputApi.joystickList[i].ProductGuid)
                {
                    directInputApi.selectJoystick(i);
                    cmbJoySelect.SelectedIndex = i;
                }
            }

            if (cmbJoySelect.SelectedIndex == -1)
            {
                cmbJoySelect.SelectedIndex = joyNum - 1;
            }
        }

        private string toString(int n)
        {
            return n.ToString();
        }

        private int fromString(string s)
        {
            int n = 0;

            try
            {
                n = int.Parse(s);
            }
            catch
            {
                n = 0;
            }

            return n;
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            m_saveData = saveConfiguration();
            saveConfigurationFile(m_configFilePath);
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void configurateSwitch()
        {
            int buttonNum = directInputApi.currentJoystick.Caps.NumberButtons;

            byte[] currentButtonState = directInputApi.currentJoystickState.GetButtons();
            byte[] lastButtonState = directInputApi.lastJoystickState.GetButtons();
            
            for (int i = 0; i < buttonNum; ++i)
            {
                if (currentButtonState[i] != lastButtonState[i])
                {
                    if (txtSwS.Focused)
                    {
                        txtSwS.Text = toSwitchString(i);
                    }
                    else if (txtSwA1.Focused)
                    {
                        txtSwA1.Text = toSwitchString(i);
                    }
                    else if (txtSwA2.Focused)
                    {
                        txtSwA2.Text = toSwitchString(i);
                    }
                    else if (txtSwB1.Focused)
                    {
                        txtSwB1.Text = toSwitchString(i);
                    }
                    else if (txtSwB2.Focused)
                    {
                        txtSwB2.Text = toSwitchString(i);
                    }
                    else if (txtSwC1.Focused)
                    {
                        txtSwC1.Text = toSwitchString(i);
                    }
                    else if (txtSwC2.Focused)
                    {
                        txtSwC2.Text = toSwitchString(i);
                    }
                    else if (txtSwD.Focused)
                    {
                        txtSwD.Text = toSwitchString(i);
                    }
                    else if (txtSwE.Focused)
                    {
                        txtSwE.Text = toSwitchString(i);
                    }
                    else if (txtSwF.Focused)
                    {
                        txtSwF.Text = toSwitchString(i);
                    }
                    else if (txtSwG.Focused)
                    {
                        txtSwG.Text = toSwitchString(i);
                    }
                    else if (txtSwH.Focused)
                    {
                        txtSwH.Text = toSwitchString(i);
                    }
                    else if (txtSwI.Focused)
                    {
                        txtSwI.Text = toSwitchString(i);
                    }
                    else if (txtSwJ.Focused)
                    {
                        txtSwJ.Text = toSwitchString(i);
                    }
                    else if (txtSwK.Focused)
                    {
                        txtSwK.Text = toSwitchString(i);
                    }
                    else if (txtSwL.Focused)
                    {
                        txtSwL.Text = toSwitchString(i);
                    }
                    else if (txtSwReverserFront.Focused)
                    {
                        txtSwReverserFront.Text = toSwitchString(i);
                    }
                    else if (txtSwReverserNeutral.Focused)
                    {
                        txtSwReverserNeutral.Text = toSwitchString(i);
                    }
                    else if (txtSwReverserBack.Focused)
                    {
                        txtSwReverserBack.Text = toSwitchString(i);
                    }
                    else if (txtSwHorn1.Focused)
                    {
                        txtSwHorn1.Text = toSwitchString(i);
                    }
                    else if (txtSwHorn2.Focused)
                    {
                        txtSwHorn2.Text = toSwitchString(i);
                    }
                    else if (txtSwMusicHorn.Focused)
                    {
                        txtSwMusicHorn.Text = toSwitchString(i);
                    }
                    else if (txtSwConstSpeed.Focused)
                    {
                        txtSwConstSpeed.Text = toSwitchString(i);
                    }

                    break;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            directInputApi.update();

            if (directInputApi.currentJoystick != null)
            {
                if (!chkInfoNegative.Checked)
                {
                    txtInfoX.Text = toString(directInputApi.currentJoystickState.X);
                    txtInfoY.Text = toString(directInputApi.currentJoystickState.Y);
                    txtInfoZ.Text = toString(directInputApi.currentJoystickState.Z);
                    txtInfoRx.Text = toString(directInputApi.currentJoystickState.Rx);
                    txtInfoRy.Text = toString(directInputApi.currentJoystickState.Ry);
                    txtInfoRz.Text = toString(directInputApi.currentJoystickState.Rz);
                }
                else
                {
                    txtInfoX.Text = toString(0xFFFF - directInputApi.currentJoystickState.X);
                    txtInfoY.Text = toString(0xFFFF - directInputApi.currentJoystickState.Y);
                    txtInfoZ.Text = toString(0xFFFF - directInputApi.currentJoystickState.Z);
                    txtInfoRx.Text = toString(0xFFFF - directInputApi.currentJoystickState.Rx);
                    txtInfoRy.Text = toString(0xFFFF - directInputApi.currentJoystickState.Ry);
                    txtInfoRz.Text = toString(0xFFFF - directInputApi.currentJoystickState.Rz);
                }

                configurateSwitch();
            }
            else
            {
                enumerateDevices();
            }
        }

        private void ConfigForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Enabled = false;
        }

        private void ConfigForm_Shown(object sender, EventArgs e)
        {
            timer1.Enabled = true;

            enumerateDevices();
            restoreConfiguration(m_saveData);
        }

        private void cmbJoySelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbJoySelect.SelectedIndex != -1)
            {
                directInputApi.selectJoystick(cmbJoySelect.SelectedIndex);
            }
        }

        private void deconfigurateSwitch(object sender, KeyEventArgs e)
        {
            TextBox me = (TextBox)sender;

            if (e.KeyCode == Keys.Delete)
            {
                me.Text = "OFF";
            }
        }
    }
}
