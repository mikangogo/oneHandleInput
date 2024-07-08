using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace oneHandleInput
{
    public partial class ConfigForm : Form
    {
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

        private readonly FormStringConverter m_converter = new FormStringConverter();
        private string m_configDirectory;
        private ProfileSet m_profiles;

        private bool m_isSaved;

        private const string SubDirectoryName = "oneHandleInput";
        
        public ConfigProfile currentProfile => m_profiles.currentProfile;

        public ConfigForm(string path)
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

            m_configDirectory = Path.Combine(path, SubDirectoryName);
            loadProfiles();
        }

        private void loadProfiles()
        {
            if (!Directory.Exists(m_configDirectory))
            {
                Directory.CreateDirectory(m_configDirectory);
            }
            m_profiles = ProfileSet.load(m_configDirectory);
            string currentKey = m_profiles.currentKey;
            cmbProfileSelect.DataSource = m_profiles.keys.ToList();
            cmbProfileSelect.SelectedItem = currentKey;

            cmbProfileSelect.Enabled = !m_profiles.isInitialSetting;
            btnSave.Enabled = !m_profiles.isInitialSetting;
        }

        private void restoreConfiguration(ConfigProfile profile)
        {
            txtReverserFront.Text = m_converter.toString(profile.reverserPosFront);
            txtReverserBack.Text = m_converter.toString(profile.reverserPosBack);
            cmbAxisReverser.SelectedIndex = profile.reverserAxis;
            chkAxisReverserNegative.Checked = profile.reverserAxisNegative;

            txtBrakeEmr.Text = m_converter.toString(profile.brakePosEmr);
            txtBrakeMax.Text = m_converter.toString(profile.brakePosMax);
            txtBrakeNeutral.Text = m_converter.toString(profile.brakePosNeutral);
            txtBrakeNotches.Text = m_converter.toString(profile.brakeNotches);
            txtBrakeChatter.Text = m_converter.toString(profile.brakeChatter);
            cmbAxisBrake.SelectedIndex = profile.brakeAxis;
            chkAxisBrakeNegative.Checked = profile.brakeAxisNegative;

            txtPowerNeutral.Text = m_converter.toString(profile.powerPosNeutral);
            txtPowerMax.Text = m_converter.toString(profile.powerPosMax);
            txtPowerNotches.Text = m_converter.toString(profile.powerNotches);
            cmbAxisPower.SelectedIndex = profile.powerAxis;
            chkAxisPowerNegative.Checked = profile.powerAxisNegative;

            txtSsbNeutral.Text = m_converter.toString(profile.ssbPosNeutral);
            txtSsbMax.Text = m_converter.toString(profile.ssbPosMax);
            txtSsbNotches.Text = m_converter.toString(profile.ssbNotches);
            cmbAxisSsb.SelectedIndex = profile.ssbAxis;
            chkAxisSsbNegative.Checked = profile.ssbAxisNegative;

            txtSwS.Text = m_converter.toSwitchString(profile.switchS);
            txtSwA1.Text = m_converter.toSwitchString(profile.switchA1);
            txtSwA2.Text = m_converter.toSwitchString(profile.switchA2);
            txtSwB1.Text = m_converter.toSwitchString(profile.switchB1);
            txtSwB2.Text = m_converter.toSwitchString(profile.switchB2);
            txtSwC1.Text = m_converter.toSwitchString(profile.switchC1);
            txtSwC2.Text = m_converter.toSwitchString(profile.switchC2);
            txtSwD.Text = m_converter.toSwitchString(profile.switchD);
            txtSwE.Text = m_converter.toSwitchString(profile.switchE);
            txtSwF.Text = m_converter.toSwitchString(profile.switchF);
            txtSwG.Text = m_converter.toSwitchString(profile.switchG);
            txtSwH.Text = m_converter.toSwitchString(profile.switchH);
            txtSwI.Text = m_converter.toSwitchString(profile.switchI);
            txtSwJ.Text = m_converter.toSwitchString(profile.switchJ);
            txtSwK.Text = m_converter.toSwitchString(profile.switchK);
            txtSwL.Text = m_converter.toSwitchString(profile.switchL);
            txtSwReverserFront.Text = m_converter.toSwitchString(profile.switchReverserFront);
            txtSwReverserNeutral.Text = m_converter.toSwitchString(profile.switchReverserNeutral);
            txtSwReverserBack.Text = m_converter.toSwitchString(profile.switchReverserBack);
            txtSwHorn1.Text = m_converter.toSwitchString(profile.switchHorn1);
            txtSwHorn2.Text = m_converter.toSwitchString(profile.switchHorn2);
            txtSwMusicHorn.Text = m_converter.toSwitchString(profile.switchMusicHorn);
            txtSwConstSpeed.Text = m_converter.toSwitchString(profile.switchConstSpeed);
        }

        private void overwriteConfiguration(ConfigProfile profile)
        {
            if (directInputApi.currentJoystick != null)
            {
                profile.guid = directInputApi.currentJoystick.Information.ProductGuid;
            }

            profile.reverserPosFront = m_converter.fromString(txtReverserFront.Text);
            profile.reverserPosBack = m_converter.fromString(txtReverserBack.Text);
            profile.reverserAxis = cmbAxisReverser.SelectedIndex;
            profile.reverserAxisNegative = chkAxisReverserNegative.Checked;

            profile.brakePosEmr = m_converter.fromString(txtBrakeEmr.Text);
            profile.brakePosMax = m_converter.fromString(txtBrakeMax.Text);
            profile.brakePosNeutral = m_converter.fromString(txtBrakeNeutral.Text);
            profile.brakeNotches = m_converter.fromString(txtBrakeNotches.Text);
            profile.brakeChatter = m_converter.fromString(txtBrakeChatter.Text);
            profile.brakeAxis = cmbAxisBrake.SelectedIndex;
            profile.brakeAxisNegative = chkAxisBrakeNegative.Checked;

            profile.powerPosNeutral = m_converter.fromString(txtPowerNeutral.Text);
            profile.powerPosMax = m_converter.fromString(txtPowerMax.Text);
            profile.powerNotches = m_converter.fromString(txtPowerNotches.Text);
            profile.powerAxis = cmbAxisPower.SelectedIndex;
            profile.powerAxisNegative = chkAxisPowerNegative.Checked;

            profile.ssbPosNeutral = m_converter.fromString(txtSsbNeutral.Text);
            profile.ssbPosMax = m_converter.fromString(txtSsbMax.Text);
            profile.ssbNotches = m_converter.fromString(txtSsbNotches.Text);
            profile.ssbAxis = cmbAxisSsb.SelectedIndex;
            profile.ssbAxisNegative = chkAxisSsbNegative.Checked;
 
            profile.switchS = m_converter.fromSwitchString(txtSwS.Text);
            profile.switchA1 = m_converter.fromSwitchString(txtSwA1.Text);
            profile.switchA2 = m_converter.fromSwitchString(txtSwA2.Text);
            profile.switchB1 = m_converter.fromSwitchString(txtSwB1.Text);
            profile.switchB2 = m_converter.fromSwitchString(txtSwB2.Text);
            profile.switchC1 = m_converter.fromSwitchString(txtSwC1.Text);
            profile.switchC2 = m_converter.fromSwitchString(txtSwC2.Text);
            profile.switchD = m_converter.fromSwitchString(txtSwD.Text);
            profile.switchE = m_converter.fromSwitchString(txtSwE.Text);
            profile.switchF = m_converter.fromSwitchString(txtSwF.Text);
            profile.switchG = m_converter.fromSwitchString(txtSwG.Text);
            profile.switchH = m_converter.fromSwitchString(txtSwH.Text);
            profile.switchI = m_converter.fromSwitchString(txtSwI.Text);
            profile.switchJ = m_converter.fromSwitchString(txtSwJ.Text);
            profile.switchK = m_converter.fromSwitchString(txtSwK.Text);
            profile.switchL = m_converter.fromSwitchString(txtSwL.Text);
            profile.switchReverserFront = m_converter.fromSwitchString(txtSwReverserFront.Text);
            profile.switchReverserNeutral = m_converter.fromSwitchString(txtSwReverserNeutral.Text);
            profile.switchReverserBack = m_converter.fromSwitchString(txtSwReverserBack.Text);
            profile.switchHorn1 = m_converter.fromSwitchString(txtSwHorn1.Text);
            profile.switchHorn2 = m_converter.fromSwitchString(txtSwHorn2.Text);
            profile.switchMusicHorn = m_converter.fromSwitchString(txtSwMusicHorn.Text);
            profile.switchConstSpeed = m_converter.fromSwitchString(txtSwConstSpeed.Text);
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

                if (m_profiles.currentProfile.guid == directInputApi.joystickList[i].ProductGuid)
                {
                    directInputApi.selectJoystick(i, this.Handle);
                    cmbJoySelect.SelectedIndex = i;
                }
            }

            if (cmbJoySelect.SelectedIndex == -1)
            {
                cmbJoySelect.SelectedIndex = joyNum - 1;
            }
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string currentKey = m_profiles.currentKey;
            ConfigProfile currentProfile = m_profiles.currentProfile;

            overwriteConfiguration(currentProfile);
            currentProfile.save(Path.Combine(m_configDirectory, currentKey + ".xml"));

            m_isSaved = true;
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            string newKey;
            using (SaveAsForm dialog = new SaveAsForm(m_configDirectory, m_profiles.isInitialSetting ? "" : m_profiles.currentKey))
            {
                DialogResult result = dialog.ShowDialog();
                if (result != DialogResult.OK) return;

                newKey = dialog.newName;
            }

            ConfigProfile newProfile = new ConfigProfile();

            overwriteConfiguration(newProfile);
            newProfile.save(Path.Combine(m_configDirectory, newKey + ".xml"));
            loadProfiles();

            m_isSaved = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (!m_isSaved)
            {
                DialogResult result = MessageBox.Show("変更は保存されていません。本当に終了してもよろしいですか？", "oneHandleInput",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result != DialogResult.Yes) return;
            }

            m_profiles.save(m_configDirectory);
            this.Close();
        }

        private void configurateSwitch()
        {
            int buttonNum = directInputApi.currentJoystick.Capabilities.ButtonCount;

            var currentButtonState = directInputApi.currentJoystickState.GetButtons();
            var lastButtonState = directInputApi.lastJoystickState.GetButtons();

            for (int i = 0; i < buttonNum; ++i)
            {
                if (currentButtonState[i] != lastButtonState[i])
                {
                    if (txtSwS.Focused)
                    {
                        txtSwS.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwA1.Focused)
                    {
                        txtSwA1.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwA2.Focused)
                    {
                        txtSwA2.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwB1.Focused)
                    {
                        txtSwB1.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwB2.Focused)
                    {
                        txtSwB2.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwC1.Focused)
                    {
                        txtSwC1.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwC2.Focused)
                    {
                        txtSwC2.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwD.Focused)
                    {
                        txtSwD.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwE.Focused)
                    {
                        txtSwE.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwF.Focused)
                    {
                        txtSwF.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwG.Focused)
                    {
                        txtSwG.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwH.Focused)
                    {
                        txtSwH.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwI.Focused)
                    {
                        txtSwI.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwJ.Focused)
                    {
                        txtSwJ.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwK.Focused)
                    {
                        txtSwK.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwL.Focused)
                    {
                        txtSwL.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwReverserFront.Focused)
                    {
                        txtSwReverserFront.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwReverserNeutral.Focused)
                    {
                        txtSwReverserNeutral.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwReverserBack.Focused)
                    {
                        txtSwReverserBack.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwHorn1.Focused)
                    {
                        txtSwHorn1.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwHorn2.Focused)
                    {
                        txtSwHorn2.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwMusicHorn.Focused)
                    {
                        txtSwMusicHorn.Text = m_converter.toSwitchString(i);
                    }
                    else if (txtSwConstSpeed.Focused)
                    {
                        txtSwConstSpeed.Text = m_converter.toSwitchString(i);
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
                    txtInfoX.Text = m_converter.toString(directInputApi.currentJoystickState.X);
                    txtInfoY.Text = m_converter.toString(directInputApi.currentJoystickState.Y);
                    txtInfoZ.Text = m_converter.toString(directInputApi.currentJoystickState.Z);
                    txtInfoRx.Text = m_converter.toString(directInputApi.currentJoystickState.RotationX);
                    txtInfoRy.Text = m_converter.toString(directInputApi.currentJoystickState.RotationY);
                    txtInfoRz.Text = m_converter.toString(directInputApi.currentJoystickState.RotationZ);
                }
                else
                {
                    txtInfoX.Text = m_converter.toString(0xFFFF - directInputApi.currentJoystickState.X);
                    txtInfoY.Text = m_converter.toString(0xFFFF - directInputApi.currentJoystickState.Y);
                    txtInfoZ.Text = m_converter.toString(0xFFFF - directInputApi.currentJoystickState.Z);
                    txtInfoRx.Text = m_converter.toString(0xFFFF - directInputApi.currentJoystickState.RotationX);
                    txtInfoRy.Text = m_converter.toString(0xFFFF - directInputApi.currentJoystickState.RotationY);
                    txtInfoRz.Text = m_converter.toString(0xFFFF - directInputApi.currentJoystickState.RotationZ);
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
            loadProfiles();
            restoreConfiguration(currentProfile);

            m_isSaved = false;
        }

        private void cmbProfileSelect_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbProfileSelect.SelectedIndex != -1)
            {
                m_profiles.currentKey = (string)cmbProfileSelect.SelectedItem;
                restoreConfiguration(currentProfile);
            }
        }

        private void cmbJoySelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbJoySelect.SelectedIndex != -1)
            {
                directInputApi.selectJoystick(cmbJoySelect.SelectedIndex, this.Handle);
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
