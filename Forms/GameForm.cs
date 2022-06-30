using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuizCryptographyProject.Controllers;
using QuizCryptographyProject.Model;
using System.Diagnostics;
using System.Timers;
//using System.Threading;

namespace QuizCryptographyProject.Forms
{
    public partial class GameForm : Form
    {
      
        private int questionNumber = 1;
        private  string user;
        DateTime _startTime;
        DateTime _endTime;
        private  int score = 0;
        private List<Questions> questions = new List<Questions>();
        private string answer;
        private bool correct = false;

        
       

        

        public GameForm(string username)
        {
            InitializeComponent();
            user = username;

            timer1.Tick += new EventHandler(timer1_Tick);
            _startTime = DateTime.Now;
            timer1.Start();
            questions = GameController.GetQuestions();
           
            ShowQuestions();
        }

        private void ShowQuestions()
        {
           
            lblScore.Text = score.ToString();
            tbAnswer.Text = "";

            if(questionNumber <6)
            {
                gbQuestion.Text = questionNumber.ToString() + ". pitanje: ";
                lblQuestion.Text = questions[questionNumber-1].question.ToString();
                

                if (!questions[questionNumber - 1].answer.Contains("#"))
                {
                    
                    answer = questions[questionNumber - 1].answer;
                    gbAnswers.Visible = false;
                    tbAnswer.Visible = true;
                    btnSave.Visible = true;
                   
                }
                else
                {
                    tbAnswer.Visible = false;
                    btnSave.Visible = false;
                    gbAnswers.Visible = true;
                    string[] answers = questions[questionNumber - 1].answer.ToString().Split('#');
                    answer = answers[0];

                    List<int> randomList = Utils.Game.GenerateNumbers(0, 3);

                    
                    btn1.Text = answers[randomList[0]];
                    btn2.Text = answers[randomList[1]];
                    btn3.Text = answers[randomList[2]];
                    btn4.Text = answers[randomList[3]];
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTimer.Text = (DateTime.Now - _startTime).ToString(@"mm\:ss");
        }
       

        private void btnCancel_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Hide();
            Form f = new MainWindowForm(user);
            f.ShowDialog();
            this.Close();
        
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            string userAnswer = tbAnswer.Text.Trim();
            correct = userAnswer.Equals(answer);
            if (correct)
                score += 20;
            LoadNextQuestion();
        }



        private void LoadNextQuestion()
        {
           
            if (questionNumber == 5)
                EndGame();
            else
            {
                questionNumber++;
                ShowQuestions();
            }
        }

        

        private void EndGame()
        {
            timer1.Stop();
            MessageBox.Show("Završili ste igru.\nVrijeme igranja: " + lblTimer.Text + "\nRezultat: " + score.ToString(), "Obavještenje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            GameResult result = new GameResult(user, lblTimer.Text, score.ToString());
            GameController.SaveResult(result);
            this.Hide();
            Form f = new MainWindowForm(user);
            f.ShowDialog();
            this.Close();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            if (btn1.Text.Equals(answer))
                score += 20;
            LoadNextQuestion();
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            if (btn3.Text.Equals(answer))
                score += 20;
            LoadNextQuestion();
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            if (btn2.Text.Equals(answer))
                score += 20;
            LoadNextQuestion();
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            if (btn4.Text.Equals(answer))
                score += 20;
            LoadNextQuestion();
        }
    }
}
