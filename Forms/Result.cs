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

namespace QuizCryptographyProject.Forms
{
    public partial class Result : Form
    {
        public Result()
        {
            InitializeComponent();
            ShowData();
        }
        private void ShowData()
        {
            dgvResults.Rows.Clear();
            var results = GameController.GetResults();
            foreach (var r in results)
            {

                DataGridViewRow row = new DataGridViewRow()
                {
                    Tag = r
                };
                row.CreateCells(dgvResults, r.Usename,
                                                 r.Time,
                                                 r.Result
                                                );
                dgvResults.Rows.Add(row);
            }
        }
    }
}
