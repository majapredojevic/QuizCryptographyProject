using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizCryptographyProject.Model
{
    public class Questions
    {
        public string question;
        public string answer;

        public Questions(string question, string answer)
        {
            this.question = question;
            this.answer = answer;
        }

        

        public override bool Equals(object obj)
        {
            return obj is Questions question &&
                   this.question == question.question &&
                   EqualityComparer<string>.Default.Equals(answer, question.answer);
        }

        public override int GetHashCode()
        {
            int hashCode = -444598804;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(question);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(answer);
            return hashCode;
        }
    }
}
