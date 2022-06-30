using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizCryptographyProject.Exceptions
{
    public class QuizException: Exception
    {
        public QuizException(string message):base(message)
        {

        }
    }
}
