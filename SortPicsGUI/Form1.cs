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
            SetStatus("Ready to work.");
        }

        private void SetStatus(string msg)
        {
            status_label.Text = msg;
        }
        private void FindPics_Click(object sender, EventArgs e)
        {
            SetStatus("Looking for pics...");
            var pics =  Program.FindPics();
            foreach (var pic in pics)
            {
                listBox1.Items.Add(pic);
            }
            SetStatus("Search complete.");
            moveButton.Enabled = true;
        }

        private void moveButton_Click(object sender, EventArgs e)
        {
            // todo: provide status report, if success, how many pics moved etc.
            Program.MovePics();
            SetStatus("Pics moved.");
        }
    }
}
