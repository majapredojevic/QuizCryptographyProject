using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuizCryptographyProject.Forms;
using QuizCryptographyProject.Model;
using QuizCryptographyProject.Controllers;
using QuizCryptographyProject.Exceptions;

namespace QuizCryptographyProject
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void lblRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form registerForm = new RegisterForm();
            registerForm.ShowDialog();
            this.Close();
        }

        private void chbPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chbPassword.Checked)
            {
                tbPassword.UseSystemPasswordChar = false;
            }
            else
            {
                tbPassword.UseSystemPasswordChar = true;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool prepared = true;
            string username = tbUsername.Text.Trim();
            string password = tbPassword.Text.Trim();
            

            if (string.IsNullOrEmpty(username))
            {
                lblUsername.ForeColor = Color.Red;
                prepared = false;
            }
            if (string.IsNullOrEmpty(password))
            {
                lblPassword.ForeColor = Color.Red;
                prepared = false;
            }
            if(prepared)
            {
                try
                {
                    User user = LoginController.LoginUser(username, password);
                    this.Hide();
                    Form f = new GameForm(username);
                    f.ShowDialog();
                    this.Close();
                }
                catch (QuizException ex)
                {
                    MessageBox.Show(ex.Message, "Greška!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
    }
}


