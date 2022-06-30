using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuizCryptographyProject.Exceptions;
using QuizCryptographyProject.Controllers;
using QuizCryptographyProject.Model;
using QuizCryptographyProject.Utils;

namespace QuizCryptographyProject.Forms
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void lblSignIn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form loginForm = new LoginForm();
            loginForm.ShowDialog();
            this.Close();

        }

        private void chbPassword_CheckedChanged(object sender, EventArgs e)
        {
            if(chbPassword.Checked)
            {
                tbPassword.UseSystemPasswordChar = false;
                tbConfirmPassword.UseSystemPasswordChar = false;
            }
            else
            {
                tbPassword.UseSystemPasswordChar = true;
                tbConfirmPassword.UseSystemPasswordChar = true;
            }
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            lblName.ForeColor = Color.LightSkyBlue;
        }

        private void tbUsername_TextChanged(object sender, EventArgs e)
        {
           lblUsername.ForeColor = Color.LightSkyBlue;
        }
        private void tbEmail_TextChanged(object sender, EventArgs e)
        {
            lblEmail.ForeColor = Color.LightSkyBlue;
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            lblPassword.ForeColor = Color.LightSkyBlue;
        }

        private void tbConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            lblConfirmPassword.ForeColor = Color.LightSkyBlue;
        }

      

        private void btnRegister_KeyPress(object sender, KeyPressEventArgs e)
        {
            char enterChar = (char)13;

            if (e.KeyChar == enterChar)
                btnRegister.PerformClick();
        }

        private void tbName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
                e.Handled = true;
            
        }
       

        private void tbUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!e.KeyChar.Equals('_') && !char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar))
                e.Handled = true;
        }

       

        private bool CheckTextBoxes(string name, string email, string username, string password, string confirmPassword )
        {
            bool textCorrect = true;

            if (string.IsNullOrEmpty(name))
            {
                lblName.ForeColor = Color.Red;
                textCorrect= false;

            }
            if (string.IsNullOrEmpty(username))
            {
                lblUsername.ForeColor = Color.Red;
                textCorrect = false;
            }
            if(string.IsNullOrEmpty(email) || !UserAccountCheck.IsEmailValid(email))
            {
                lblEmail.ForeColor = Color.Red;
                textCorrect = false;
            }
            if (string.IsNullOrEmpty(password) || password.Length<8)
            {
                MessageBox.Show("Lozinka mora imati minimalno 8 karaktera!", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblPassword.ForeColor = Color.Red;
                textCorrect = false;
            }
            if (string.IsNullOrEmpty(confirmPassword))
            {
                lblConfirmPassword.ForeColor = Color.Red;
                textCorrect = false;
            }
            if (!password.Equals(confirmPassword))
            {
                MessageBox.Show("Lozinke se ne podudaraju!", "Greška!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblPassword.ForeColor = Color.Red;
                lblConfirmPassword.ForeColor = Color.Red;
                textCorrect = false; 
            }
             if(UserAccountCheck.IsUserRegistered(username))
             {
                 MessageBox.Show("Korisničko ime je zauzeto!", "Greška!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 lblUsername.ForeColor = Color.Red;
                 textCorrect= false;
             }
            return textCorrect;

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string name = tbName.Text.Trim();
            string email = tbEmail.Text.Trim();
            string usename = tbUsername.Text.Trim();
            string password = tbPassword.Text.Trim();
            string confirmPassword = tbConfirmPassword.Text.Trim();


            bool prepared = CheckTextBoxes(name, email, usename, password, confirmPassword);
           
        
            if (prepared)
            {
                Random rnd = new Random();
                int randomNumber = rnd.Next(1, 3);
                User user = new User( name, usename, UserAccountCheck.HashPassword(password), email,randomNumber, 0);
                try
                {
                  string path =  RegisterController.RegisterUser(user);
                  MessageBox.Show("Uspješno ste se registrovali.\n Putanja do Vašeg sertifikata je:" + path, "Obavještenje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    Form loginForm = new LoginForm();
                    loginForm.ShowDialog();
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
