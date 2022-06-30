using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuizCryptographyProject.Forms
{
    public partial class MainWindowForm : Form
    {
        private readonly string user;
        public MainWindowForm(string username)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            user = username;
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            
            Form f = new GameForm(user);
            f.WindowState = this.WindowState;
            f.ShowDialog();
            
        }

        private void btnResult_Click(object sender, EventArgs e)
        {
            Form f = new Result();
            f.ShowDialog();
        }
    }
}
