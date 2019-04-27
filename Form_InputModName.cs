using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SekiroModManager
{
    public partial class Form_InputModName : Form
    {
        public Form_InputModName()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            if(textBox.Text != String.Empty)
            {
                string illegalString = "/`~@#;,.!#$%^&*()+{}|\\:\"<>?-=/,\' ";
                foreach(char iC in illegalString)
                {
                    textBox.Text = textBox.Text.Replace(iC.ToString(),"");
                }
                DialogResult = DialogResult.OK;
            } else
            {
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}
