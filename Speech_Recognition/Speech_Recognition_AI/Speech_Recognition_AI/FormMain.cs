using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;

namespace Speech_Recognition_AI
{
    public partial class FormMain : Form
    {
        //Create object to instantiate a speech synthesizer
        SpeechSynthesizer synth = new SpeechSynthesizer();

        public FormMain()
        {
            InitializeComponent();

            //Set voice to female and test initial voice settings
            synth.SelectVoiceByHints(VoiceGender.Female);
            synth.Speak("Hello, It's been awhile since I have seen you. How are you?");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
