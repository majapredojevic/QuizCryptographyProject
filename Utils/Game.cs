using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizCryptographyProject.Utils
{
    public class Game
    {
        public static List<int> GenerateNumbers(int firstNumber,int lastNumber)
        {
            Random random = new Random();
            List<int> randomList = new List<int>();

            while (randomList.Count < (lastNumber-firstNumber+1))
            {
                int number = random.Next(firstNumber, lastNumber+1);
                if (!randomList.Contains(number))
                    randomList.Add(number);
            }
            return randomList;
        }
    }

}
