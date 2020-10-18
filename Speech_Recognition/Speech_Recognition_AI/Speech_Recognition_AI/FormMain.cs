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

namespace Speech_Recognition_AI
{
    public partial class FormMain : Form
    {
        //Create object to instantiate a speech synthesizer
        SpeechSynthesizer synth = new SpeechSynthesizer();
        Choices list = new Choices();

        //Sleep/Wake mode
        Boolean wake = true;

        public FormMain()
        {
            //Initilize new object to process speech
            SpeechRecognitionEngine rec = new SpeechRecognitionEngine();

            //List of possible sayings to expect from the user
            //WARNING: Must be lowercase, as the computer doesn't correct for case sensitivity
            list.Add(new string[] { "hello", "hello jarvis", "how are you", "what time is it", "what day is it", "open google", "go to sleep",
            "jarvis", "restart", "update" });

            //Gather voice information for processing
            Grammar gramm = new Grammar(new GrammarBuilder(list));
            try
            {
                rec.RequestRecognizerUpdate();
                rec.LoadGrammar(gramm);
                rec.SpeechRecognized += rec_SpeechRecognized;
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
        }

        public void say(String output) {
            outputText.Text = output;
            synth.Speak(output);
        }
        
        public void restart() {
            Process.Start(@"C:\Users\keine\source\repos\Speech_Cognition_AI\Speech_Recognition\Speech_Recognition_AI\Speech_Recognition_AI\bin\Debug\Speech_Recognition_AI.exe");
            Environment.Exit(0);
        }

        private void rec_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            //Store user voice results in a string for comparisons
            String user = e.Result.Text;
            inputText.Text = user;

            //Set wake and sleep functionality
            if (user == "go to sleep")
            {
                say("Love you bro, goodnight.");
                wake = false;
                sleepText.Text = "Asleep";
            }
            if (user == "jarvis")
            {
                say("Sup dude?");
                wake = true;
                sleepText.Text = "Awake";
            }

            //If awake, dictate what to respond to
            if (wake == true)
            {
                if (user == "hello jarvis" || user == "hello") { say("Sup bro?"); }
                if (user == "how are you") { say("I'm doing good brother, thanks for asking. Is there anything I can do for you?"); }
                if (user == "what time is it") { say(DateTime.Now.ToString("h:mm tt")); }
                if (user == "what day is it") { say(DateTime.Now.ToString("dddd M/d/yyyy")); }
                if (user == "open google") 
                { 
                    Process.Start("http://google.com");
                    say("I have opened Google for you, my dude.");
                }
                if (user == "restart" || user == "update")
                {
                    say("Restarting bro.");
                    restart();
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void inputBox_Enter(object sender, EventArgs e)
        {

        }

        private void outputBox_Enter(object sender, EventArgs e)
        {

        }
    }
}
