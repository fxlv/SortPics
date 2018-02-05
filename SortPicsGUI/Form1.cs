using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SortPicsGUI
{
    public partial class SortPics : Form
    {
        public SortPics()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           var pics =  Program.FindPics();
            foreach (var pic in pics)
            {
                listBox1.Items.Add(pic);
            }
            moveButton.Enabled = true;
        }

        private void moveButton_Click(object sender, EventArgs e)
        {
            Program.MovePics();
        }
    }
}
