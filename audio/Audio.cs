using System;
using System.ComponentModel;
using System.Media;
using System.Threading;
using System.Windows.Forms;

namespace audio
{
    public partial class audio : Form
    {
        private static SoundPlayer _soundPlayer = new SoundPlayer();
        private BackgroundWorker _backgroundWorker = new BackgroundWorker();
        private int inputNumber;
        private int playNumber;
        private int correctNumber;
        private int testCount;
        string language;


        public audio()
        {
            InitializeComponent();

            turnOff();
            this.KeyDown += new KeyEventHandler(pressNumber);
            this.langaugeComboBox.Text = "Japanese";

            _backgroundWorker.DoWork += new DoWorkEventHandler(_backgroundWorker_DoWork);
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_backgroundWorker_RunWorkerCompleted);
            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.WorkerSupportsCancellation = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            inputNumber = 1;
            turnOff();  //turn off button and keyDown function.
        }

        private void button2_Click(object sender, EventArgs e)
        {
            inputNumber = 2;
            turnOff();  //turn off button and keyDown function.
        }

        private void button3_Click(object sender, EventArgs e)
        {
            inputNumber = 3;
            turnOff();  //turn off button and keyDown function.
        }

        private void button4_Click(object sender, EventArgs e)
        {
            inputNumber = 4;
            turnOff();  //turn off button and keyDown function.
        }

        private void button5_Click(object sender, EventArgs e)
        {
            inputNumber = 5;
            turnOff();  //turn off button and keyDown function.
        }

        private void button6_Click(object sender, EventArgs e)
        {
            inputNumber = 6;
            turnOff();  //turn off button and keyDown function.
        }

        private void button7_Click(object sender, EventArgs e)
        {
            inputNumber = 7;
            turnOff();  //turn off button and keyDown function.
        }

        private void button8_Click(object sender, EventArgs e)
        {
            inputNumber = 8;
            turnOff();  //turn off button and keyDown function.
        }

        private void button9_Click(object sender, EventArgs e)
        {
            inputNumber = 9;
            turnOff();  //turn off button and keyDown function.
        }

        private void button10_Click(object sender, EventArgs e)
        {
            inputNumber = 10;
            turnOff();  //turn off button and keyDown function.
        }

        private void pressNumber(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D1:
                    inputNumber = 1;
                    turnOff();  //turn off button and keyDown function.
                    break;
                case Keys.D2:
                    inputNumber = 2;
                    turnOff();  //turn off button and keyDown function.
                    break;
                case Keys.D3:
                    inputNumber = 3;
                    turnOff();  //turn off button and keyDown function.
                    break;
                case Keys.D4:
                    inputNumber = 4;
                    turnOff();  //turn off button and keyDown function.
                    break;
                case Keys.D5:
                    inputNumber = 5;
                    turnOff();  //turn off button and keyDown function.
                    break;
                case Keys.D6:
                    inputNumber = 6;
                    turnOff();  //turn off button and keyDown function.
                    break;
                case Keys.D7:
                    inputNumber = 7;
                    turnOff();  //turn off button and keyDown function.
                    break;
                case Keys.D8:
                    inputNumber = 8;
                    turnOff();  //turn off button and keyDown function.
                    break;
                case Keys.D9:
                    inputNumber = 9;
                    turnOff();  //turn off button and keyDown function.
                    break;
                case Keys.D0:
                    inputNumber = 10;
                    turnOff();  //turn off button and keyDown function.
                    break;
            }
        }

        //If there is an error, then display error message.
        //Display result of number correct, after completed backgroundworker
        void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (e.Cancelled)
            {
                //IF cancel
            }

            // Check to see if an error occurred in the background process.
            else if (e.Error != null)
            {
                MessageBox.Show(e.ToString(), "Error Message");
            }
            else
            {
                // Everything completed normally.
                MessageBox.Show(String.Format("You got {0} of {1} correct.", correctNumber, testCount), "Result",MessageBoxButtons.OK);
                langaugeComboBox.Enabled = true;
                startBtn.Enabled = true;
            }
        }

        //Pick random number between one to ten, then play audio.
        //If the user get wrong number after play audio, display message.
        //Then play audio in the english.
        void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _soundPlayer = new SoundPlayer();
            while (!_backgroundWorker.CancellationPending)
            {
                int prevNumber = 0;
                playNumber = (DateTime.Now.Millisecond % 10) + 1;
                while (prevNumber == playNumber)
                {
                    playNumber = (DateTime.Now.Millisecond % 10) + 1;
                }
                prevNumber = playNumber;
                inputNumber = 0;
                language = langaugeComboBox.Text;
                _soundPlayer.SoundLocation = @"sound\" + language + @"\" + playNumber + ".wav";

                while ((inputNumber == 0) && (stopBtn.Enabled))
                {
                    try
                    {
                        turnOn();  //turn on button and keyDown function.
                        _soundPlayer.Play();
                        Thread.Sleep(1500);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "ERROR");
                    }
                }
                if (stopBtn.Enabled)
                {
                    if (inputNumber != playNumber)
                    {
                        MessageBox.Show("Sorry, You have selected wrong number!\nClick OK to hear the correct number.", "Wrong Answer Message");
                        _soundPlayer.SoundLocation = @"sound\english\" + playNumber + ".wav";
                        _soundPlayer.Play();
                        Thread.Sleep(1500);
                    }
                    else
                    {
                        correctNumber++;
                    }
                    testCount++;
                }
            }
            return;
        }

        //Popup message to comfirm, if the user want to close the program
        //Cancel backgroundworker if user is still testing.
        //After backgroundworker completed, then close the program.
        private void userClose(object sender, FormClosingEventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Do you want to close?", "Exit", MessageBoxButtons.YesNo);
                if(result == DialogResult.Yes)
                {
                    this.Enabled = false;   // or this.Hide()
                    _backgroundWorker.CancelAsync();
                    while (_backgroundWorker.IsBusy)
                    {
                        Application.DoEvents();
                    }
                }
                else if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Close Error");
            }
        }

        //Disable at the Button number and Keydown number after user had guess a number
        //To avoid use double click button or keydown while soundPlayer had not finish.
        private void turnOff()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            button10.Enabled = false;                 
            this.KeyPreview = false;
        }

        //Enable at the Button number and Keydown number for user guess a number
        private void turnOn()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = true;
            this.KeyPreview = true;
        }

        //startButton will disable the start and ComboBox for select Language.
        //and Reset the number of user have test and correct.
        //Then call the Backgroundworker function to run the test.
        private void startBtn_Click(object sender, EventArgs e)
        {
            langaugeComboBox.Enabled = false;
            stopBtn.Enabled = true;
            startBtn.Enabled = false;
            correctNumber = 0;
            testCount = 0;
            _backgroundWorker.RunWorkerAsync();
        }

        //startButton will enable the start and ComboBox for select Language.
        //and cancel Backgroundworker to stop the test.
        private void stopBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to stop testing?", "Stop Testing", MessageBoxButtons.YesNo);
            {
                if (result == DialogResult.Yes)
                {
                    stopBtn.Enabled = false;
                    _backgroundWorker.CancelAsync();   
                }
            }
        }
    }
}
