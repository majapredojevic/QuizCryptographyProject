using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace QuizCryptographyProject.Utils
{
    class UserAccountCheck
    {
        private static readonly string USERS_PATH = Application.StartupPath + "\\Users";
        public static string HashPassword(string password)
        {
            string command = "openssl passwd -6 -salt lozinka " + password;
            return CommandPrompt.ExecuteCommandWithResponse(command).Trim();
        }

        public static bool IsUserRegistered(string username)
        {
            bool exists = false;

            DirectoryInfo root = new DirectoryInfo(USERS_PATH);
            var files = root.GetFiles();
            foreach (var file in files)
            {
                if (file.Name.Equals(username + ".txt"))
                {
                    exists = true;
                }
            }
            return exists;
        }
        public static bool IsEmailValid(string email)
        {
            var regex = new Regex(@"^[\w\.-]+@([\w\-]+\.)+[a-z]{2,4}$");
            Match match = regex.Match(email);
            return match.Success;
        }
    }
}
