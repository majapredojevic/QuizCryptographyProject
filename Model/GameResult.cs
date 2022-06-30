using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizCryptographyProject.Model
{
    public class GameResult
    {
        private string username;
        private string time;
        private string result;

        public GameResult(string usename, string time, string result)
        {
            this.username = usename;
            this.time = time;
            this.result = result;
        }

        public string Usename { get => username; set => username = value; }
        public string Time { get => time; set => time = value; }
        public string Result { get => result; set => result = value; }

        public override bool Equals(object obj)
        {
            return obj is GameResult result &&
                   username == result.username &&
                   time == result.time &&
                   this.result == result.result;
        }

        public override int GetHashCode()
        {
            int hashCode = 2146523399;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(username);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(time);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(result);
            return hashCode;
        }
    }
}
