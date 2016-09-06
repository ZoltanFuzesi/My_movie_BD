using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VIdeoLibray
{
    public partial class Progress : Form
    {
        private Boolean checkprogress = false;
        public Progress(string what,string name)
        {
            InitializeComponent();
            this.Text = what + " " +name;
            setLabel(what,name);
            StartPosition = FormStartPosition.CenterScreen;
        }
        
        public void updateLabel(string str)
        {
            label1.Text = str;
        }
        private void progressBar1_Click(object sender, EventArgs e)
        {
            
        }
        private void setLabel(string what,string label)
        {
            label1.Text = "Please wait ";
        }
     
        public void progress()
        {
                this.timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.progressBar1.Increment(1);
        }

        private void Progress_Load(object sender, EventArgs e)
        {

        }
    }
}

