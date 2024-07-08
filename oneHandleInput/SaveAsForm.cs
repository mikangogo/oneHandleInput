using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace oneHandleInput
{
    public partial class SaveAsForm : Form
    {
        private readonly string m_directory;

        public bool isValid { get; private set; } = false;
        public string newName => isValid ? txtName.Text : null;

        public SaveAsForm(string directory, string defaultName)
        {
            InitializeComponent();

            m_directory = directory;
            txtName.Text = defaultName;
            txtName.SelectAll();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            string newName = txtName.Text;
            if (newName == string.Empty)
            {
                SetErrorText("このファイル名は使用できません。");
                isValid = false;
            }
            else if (!FileNameValidator.IsValid(newName))
            {
                SetErrorText("ファイル名に使用できない文字が含まれています。");
                isValid = false;
            }
            else if (File.Exists(Path.Combine(m_directory, newName + ".xml")))
            {
                SetErrorText("このファイル名は既に使用されています。");
                isValid = false;
            }
            else
            {
                SetInfoText("このファイル名は使用可能です。");
                isValid = true;
            }

            btnSave.Enabled = isValid;
        }

        private void SetInfoText(string text)
        {
            lblInfo.Text = text;
            lblInfo.ForeColor = SystemColors.ControlText;
        }

        private void SetErrorText(string text)
        {
            lblInfo.Text = text;
            lblInfo.ForeColor = Color.Red;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!isValid) return;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
