using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProjectTerraformServer
{
    public partial class Form2 : Form
    {
        public Form2(string[] connected,string[] players)
        {
            InitializeComponent();

            listBox1.Items.AddRange(connected);
            listBox2.Items.AddRange(players);

            this.DialogResult = DialogResult.Abort;
        }

        public string[] GetResault()
        {
            string[] ret = new string[listBox1.Items.Count];

            for (int i = 0; i < ret.Length; i++)
                ret[i] = (string)listBox1.Items[i];

            return ret;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                string[] strs = ((string)listBox1.SelectedItem).Split(new char[] { '[', ']' });
                int num = int.Parse(strs[1]);
                listBox1.Items[listBox1.SelectedIndex] = "[" + (int)numericUpDown1.Value + "]" + strs[2];
            }
            catch { ;}
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label1.Visible = true;
            numericUpDown1.Visible = true;
        }
    }
}
