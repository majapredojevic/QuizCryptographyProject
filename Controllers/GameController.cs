using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizCryptographyProject.Model;
using QuizCryptographyProject.Exceptions;
using QuizCryptographyProject.Utils;
using System.Windows.Forms;
using System.IO;

namespace QuizCryptographyProject.Controllers
{
    public class GameController
    {

        private static readonly string QUESTIONS_PATH = Application.StartupPath + "\\Questions";
        


        
        private static List<int> randomList = new List<int>();
        

        public static List<Questions> GetQuestions()
        {
            List<Questions> questions = new List<Questions>();

            randomList = Utils.Game.GenerateNumbers(1, 20); //5 random razlicitih brojeva iz intervala [1,20]

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    string commandExtract = "steghide extract -sf " + QUESTIONS_PATH + $"\\{randomList[i]}.jpg -p kviz";
                CommandPrompt.ExecuteCommand(commandExtract);
                
                    var path = Application.StartupPath + $"\\{randomList[i]}.txt";
                    string[] lines = File.ReadAllLines(path);
                    string[] QandA = lines[0].Split('$');
                    Questions question = new Questions(QandA[0], QandA[1]);
                    Console.WriteLine(QandA[0] + " " + QandA[1]);
                    questions.Add(question);

                    File.Delete(path);
                }
                catch (Exception e)
                {
                    throw new QuizException(e.StackTrace + " : " + e.Message);
                }
            }

            return questions;
        }

        public static List<GameResult> GetResults()
        {
           
                List<GameResult> results = new List<GameResult>();
                string filePath = Application.StartupPath + "\\_result.txt";
                string decryptCommand = "openssl aes-256-ecb -d -in " + Application.StartupPath + "\\result.txt -out " + Application.StartupPath + "/_result.txt -pbkdf2 -k kviz -nosalt -base64";
                CommandPrompt.ExecuteCommand(decryptCommand);

            try
            {

                var allLines = File.ReadAllLines(filePath);
                foreach (var line in allLines)
                {
                    string[] result = line.Split(' ');
                    var gameInfo = new GameResult(result[0], result[1], result[2]);
                    results.Add(gameInfo);
                }
                File.Delete(filePath);
            }

            catch (Exception e)
            {
                throw new QuizException(e.StackTrace + " : " + e.Message);
            }
            return results;
        }
    
        public static void SaveResult(GameResult result)
        {
            string filePath = Application.StartupPath + "\\_result.txt";
            string decryptCommand = "openssl aes-256-ecb -d -in " + Application.StartupPath + "\\result.txt -out " + Application.StartupPath + "\\_result.txt -pbkdf2 -k kviz -nosalt -base64";
            CommandPrompt.ExecuteCommand(decryptCommand);

            try
            {

                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(result.Usename + " " + result.Time + " " + result.Result);
                }
                string encryptCommand = "openssl aes-256-ecb -in " + filePath + " -out " + Application.StartupPath + "\\result.txt -pbkdf2 -k kviz -nosalt -base64";
                CommandPrompt.ExecuteCommand(encryptCommand);
               
                File.Delete(filePath);
            }

            catch (Exception e)
            {
                throw new QuizException(e.StackTrace + " : " + e.Message);
            }
           
        }

    }


}

