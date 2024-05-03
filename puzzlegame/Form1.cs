using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace puzzlegame
{
    public partial class Form1 : Form
    {
        int inNullSliceIndex, inmoves = 0;
        List<Bitmap> lstOriginalPictureList = new List<Bitmap>();
        System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();


        public Form1()
        {
            InitializeComponent();
            lstOriginalPictureList.AddRange(new Bitmap[] { Properties.Resources._1, Properties.Resources._2, Properties.Resources._3, Properties.Resources._4, Properties.Resources._5, Properties.Resources._6, Properties.Resources._7, Properties.Resources._8, Properties.Resources._9, Properties.Resources._null });
            lblMovesMade.Text += inmoves;
            lblTime.Text = "00:00:00";
        }
        public class PuzzleSolver
{
    public List<int> Solve(int[,] puzzle)
    {
       
        List<int> moves = new List<int>();

      
        moves.Add(1);
        moves.Add(2);
        moves.Add(3);

        return moves;
    }
}
        private void Form1_Load(object sender, EventArgs e)
        {
            Shuffle();
        }
        void Shuffle()
        {
            do
            {
                int j;
                List<int> Indexes = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 9 });
                Random r = new Random();
                for (int i = 0; i < 9; i++)
                {
                    Indexes.Remove((j = Indexes[r.Next(0, Indexes.Count)]));
                    ((PictureBox)gpPuzzleBox.Controls[i]).Image = lstOriginalPictureList[j];
                    if (j == 9) inNullSliceIndex = i;
                }
            } while (CheckWin());
        }


     

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult YesOrNo = new DialogResult();
            if (lblTime.Text != "00:00:00")
            {
                YesOrNo = MessageBox.Show("Are You Sure To RESTART ?", "Rabbit Puzzle", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            if (YesOrNo == DialogResult.Yes || lblTime.Text == "00:00:00")
            {
                Shuffle();
                timer.Reset();
                lblTime.Text = "00:00:00";
                inmoves = 0;
                lblMovesMade.Text = "Moves Made : 0";
            }
        }

        private void AskPermissionBeforeQuite(object sender, FormClosingEventArgs e)
        {
            DialogResult YesOrNO = MessageBox.Show("Are You Sure To Quit ?", "Rabbit Puzzle", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sender as Button != button3 && YesOrNO == DialogResult.No) e.Cancel = true;
            if (sender as Button == button3 && YesOrNO == DialogResult.Yes) Environment.Exit(0);
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            AskPermissionBeforeQuite(sender, e as FormClosingEventArgs);
        }

        private void SwitchPictureBox(object sender, EventArgs e)
        {
            if (lblTime.Text == "00:00:00")
                timer.Start();
            int inPictureBoxIndex = gpPuzzleBox.Controls.IndexOf(sender as Control);
            if (inNullSliceIndex != inPictureBoxIndex)
            {
                List<int> FourBrothers = new List<int>(new int[] { ((inPictureBoxIndex % 3 == 0) ? -1 : inPictureBoxIndex - 1), inPictureBoxIndex - 3, (inPictureBoxIndex % 3 == 2) ? -1 : inPictureBoxIndex + 1, inPictureBoxIndex + 3 });
                if (FourBrothers.Contains(inNullSliceIndex))
                {
                    ((PictureBox)gpPuzzleBox.Controls[inNullSliceIndex]).Image = ((PictureBox)gpPuzzleBox.Controls[inPictureBoxIndex]).Image;
                    ((PictureBox)gpPuzzleBox.Controls[inPictureBoxIndex]).Image = lstOriginalPictureList[9];
                    inNullSliceIndex = inPictureBoxIndex;
                    lblMovesMade.Text = "Moves Made : " + (++inmoves);
                    if (CheckWin())
                    {
                        timer.Stop();
                        (gpPuzzleBox.Controls[8] as PictureBox).Image = lstOriginalPictureList[8];
                        MessageBox.Show("Congratulations...\nYour Rabbit Is Happy\nTime Elapsed : " + timer.Elapsed.ToString().Remove(8) + "\nTotal Moves Made : " + inmoves, "Rabbit Puzzle");
                        inmoves = 0;
                        lblMovesMade.Text = "Moves Made : 0";
                        lblTime.Text = "00:00:00";
                        timer.Reset();
                        Shuffle();
                    }
                }
            }
        }

        private void UpdateTimeElapsed(object sender, EventArgs e)
        {
            if (timer.Elapsed.ToString() != "00:00:00")
                lblTime.Text = timer.Elapsed.ToString().Remove(8);
            if (timer.Elapsed.ToString() == "00:00:00")
                button2.Enabled = false;
            else
                button2.Enabled = true;
            if (timer.Elapsed.Minutes.ToString() == "1")
            {
                timer.Reset();
                lblMovesMade.Text = "Moves Made : 0";
                lblTime.Text = "00:00:00";
                inmoves = 0;
                button2.Enabled = false;
                MessageBox.Show("Time Is Up\nTry Again", "Rabbit Puzzle");
                Shuffle();
            }
        }

        bool CheckWin()
        {
            int i;
            for (i = 0; i < 8; i++)
            {
                if ((gpPuzzleBox.Controls[i] as PictureBox).Image != lstOriginalPictureList[i]) break;
            }
            if (i == 8) return true;
            else return false;
        }

        private void gpOriginal_Enter(object sender, EventArgs e)
        {

        }

       
      

        private void button4_Click(object sender, EventArgs e)
        {

            timer.Stop();
            gpPuzzleBox.Visible = true;
            button2.Text = "Pause";


            PuzzleSolver solver = new PuzzleSolver();


            int[,] puzzleState = new int[3, 3];
            int index = 0;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (index == inNullSliceIndex)
                    {
                        puzzleState[row, col] = 0;
                    }
                    else
                    {
                        puzzleState[row, col] = index + 1;
                    }
                    index++;
                }
            }


            List<int> solution = solver.Solve(puzzleState);


            foreach (int move in solution)
            {

                int sliceIndex = -1;
                for (int i = 0; i < 9; i++)
                {
                    if (puzzleState[i / 3, i % 3] == move)
                    {
                        sliceIndex = i;
                        break;
                    }
                }


                SwitchPictureBox(gpPuzzleBox.Controls[sliceIndex], EventArgs.Empty);
                Application.DoEvents();
                System.Threading.Thread.Sleep(500);
            }

        }

        private void PauseOrResume(object sender, EventArgs e)
        {
            if (button2.Text == "Pause")
            {
                timer.Stop();
                gpPuzzleBox.Visible = false;
                button2.Text = "Resume";
            }
            else
            {
                timer.Start();
                gpPuzzleBox.Visible = true;
                button2.Text = "Pause";
            }
        }
    }
}
