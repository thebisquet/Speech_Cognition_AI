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
using System.Speech.Recognition;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Speech_Recognition_AI
{
    public partial class FormMain : Form
    {
        //Create object to instantiate a speech synthesizer
        readonly SpeechSynthesizer synth = new SpeechSynthesizer();
        readonly Choices list = new Choices();

        //Sleep/Wake mode
        Boolean wake = true;


        public FormMain()
        {
            //Initilize new object to process speech
            SpeechRecognitionEngine rec = new SpeechRecognitionEngine();

            //List of possible sayings to expect from the user
            //WARNING: Must be lowercase, as the computer doesn't correct for case sensitivity
            list.Add(new string[] { "hello", "hello jarvis", "how are you", "what time is it", "what day is it", "open google", "go to sleep",
            "jarvis", "restart", "update", "open spotify", "close spotify" });

            #region Main Form Initilization and Speech Gatherer
            //Gather voice information for processing
            Grammar gramm = new Grammar(new GrammarBuilder(list));
            try
            {
                rec.RequestRecognizerUpdate();
                rec.LoadGrammar(gramm);
                rec.SpeechRecognized += Rec_SpeechRecognized;
                rec.SetInputToDefaultAudioDevice();
                rec.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch
            {
                return;
            }

            //Set voice to Male because Jarvis is a man
            synth.SelectVoiceByHints(VoiceGender.Male);

            InitializeComponent();
            CenterToScreen();
            #endregion
        }

        #region Say and Restart Methods
        /// <summary>
        /// AI Says what is passed as output. Sends output as text to form.
        /// </summary>
        /// <param name="output">The string Jarvis will say.</param>
        public void Say(String output) {
            outputText.Text = output;
            synth.Speak(output);
        }
        
        /// <summary>
        /// Restarts Program. Set full path to executable file.
        /// </summary>
        public void Restart() {
            Process.Start(@"C:\Users\keine\source\repos\Speech_Cognition_AI\Speech_Recognition\Speech_Recognition_AI\Speech_Recognition_AI\bin\Debug\Speech_Recognition_AI.exe");
            Environment.Exit(0);
        }

        /// <summary>
        /// Kill programs by name.
        /// </summary>
        /// <param name="frog">Program name to close.</param>
        public static void KillFrog(String frog)
        {
            System.Diagnostics.Process[] frogs = null;

            try {
                frogs = Process.GetProcessesByName(frog);
                Process deadFrog = frogs[0];

                if ( !deadFrog.HasExited ) {
                    deadFrog.Kill();
                }
            } finally {
                if (frogs != null) {
                    foreach (Process livingFrogs in frogs) {
                        livingFrogs.Dispose();
                    }
                }
            }
        }
        #endregion

        private void Rec_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            //Store user voice results in a string for comparisons
            String user = e.Result.Text;
            inputText.Text = user;

            #region Sleep State Functionality
            //Set wake and sleep functionality
            if (user == "go to sleep")
            {
                Say("Love you bro, goodnight.");
                wake = false;
                sleepText.Text = "Asleep";
            }
            if (user == "jarvis")
            {
                Say("Sup dude?");
                wake = true;
                sleepText.Text = "Awake";
            }
            #endregion

            //If awake, dictate what to respond to
            if (wake == true)
            {
                #region Basic Commands
                if (user == "hello jarvis" || user == "hello") { Say("Sup bro?"); }
                if (user == "how are you") { Say("I'm doing good brother, thanks for asking. Is there anything I can do for you?"); }
                if (user == "what time is it") { Say(DateTime.Now.ToString("h:mm tt")); }
                if (user == "what day is it") { Say(DateTime.Now.ToString("dddd M/d/yyyy")); }
                #endregion

                #region Multi-line Commands
                if (user == "open google") 
                { 
                    Process.Start("http://google.com");
                    Say("I have opened Google for you, my dude.");
                }
                if (user == "open spotify")
                {
                    Process.Start(@"C:\Users\keine\AppData\Roaming\Spotify\Spotify.exe");
                    Say("I have opened Spotify for you, my dude.");
                }
                if (user == "close spotify")
                {
                    KillFrog("Spotify");
                    Say("I have closed Spotify for you sir.");
                }
                if (user == "restart" || user == "update")
                {
                    Say("Restarting bro.");
                    Restart();
                }
                #endregion
            }
        }
        #region Useless Method Declarations
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void inputBox_Enter(object sender, EventArgs e)
        {

        }

        private void outputBox_Enter(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
