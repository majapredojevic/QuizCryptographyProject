using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizCryptographyProject.Model
{
    public class User
    {
        private readonly string name;
        private readonly string username;
        private readonly string password;
        private readonly string email;
        private readonly int ca;
        private readonly int numberOfLogin;

        public User(string name, string username, string password, string email,int ca, int numberOfLogin)
        {
            this.name = name;
            this.username = username;
            this.password = password;
            this.email = email;
            this.ca = ca;
            this.numberOfLogin = numberOfLogin;
        }

        public string Name => name;

        public string Username => username;

        public string Password => password;

        public string Email => email;
        public int Ca => ca;

        
        public int NumberOfLogin => numberOfLogin;


        public override bool Equals(object obj)
        {
            return obj is User user &&
                   name == user.name &&
                   username == user.username &&
                   password == user.password &&
                   email == user.email &&
                  ca == user.ca &&
                   numberOfLogin == user.numberOfLogin;
        }

        public override int GetHashCode()
        {
            int hashCode = 91172485;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(username);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(password);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(email);
            hashCode = hashCode * -1521134295 + ca.GetHashCode();
            hashCode = hashCode * -1521134295 + numberOfLogin.GetHashCode();
            return hashCode;
        }
    }
}
