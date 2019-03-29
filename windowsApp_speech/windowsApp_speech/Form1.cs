using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;
using System.Globalization;

namespace windowsApp_speech
{
    public partial class Form1 : Form
    {
        static CultureInfo ci = new CultureInfo("en-us");
        static SpeechRecognitionEngine sre = new SpeechRecognitionEngine(ci);

        public Form1()
        {
            InitializeComponent();
            sre.SetInputToDefaultAudioDevice();
            sre.SpeechRecognized += sre_SpeechRecognized;
            Grammar g_HellowGoodbye = GetHelloGoodbyeGrammar();
            Grammar g_SetTextBox = GetTextBox1TextGrammar();
            Grammar g_WebBrowser = GetWebBrowserGrammer();
            sre.LoadGrammarAsync(g_HellowGoodbye);
            sre.LoadGrammarAsync(g_SetTextBox);
            sre.LoadGrammarAsync(g_WebBrowser);
        }

        // Helper method
        static Grammar GetHelloGoodbyeGrammar()
        {
            // Makes choices
            Choices ch_HelloGoodbye = new Choices();
            ch_HelloGoodbye.Add("hello");
            ch_HelloGoodbye.Add("goodbye");

            // Make a Grammar Builder
            GrammarBuilder gb_result = new GrammarBuilder(ch_HelloGoodbye);

            // Make a Grammar out of that, then return it.
            Grammar g_result = new Grammar(gb_result);
            return g_result;
        }

        // Helper method
        // Recognizes "set text box 1 ..."
        static Grammar GetTextBox1TextGrammar()
        {
            Choices ch_Colors = new Choices();
            ch_Colors.Add(new string[] { "red", "white", "blue" });

            GrammarBuilder gb_result = new GrammarBuilder();
            gb_result.Append("set text box 1");
            gb_result.Append(ch_Colors);

            Grammar g_result = new Grammar(gb_result);
            return g_result;
        }

        // Helper method
        //  Changes the browser window
        static Grammar GetWebBrowserGrammer()
        {
            Choices ch_Songs = new Choices();
            ch_Songs.Add(new string[]
            {
                "all star by smash mouth",
                "everybodies circulation",
                "megalovania"
            });

            GrammarBuilder gb_result = new GrammarBuilder();
            gb_result.Append("alexa play");
            gb_result.Append(ch_Songs);

            Grammar g_result = new Grammar(gb_result);
            return g_result;
        }

        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string txt = e.Result.Text;
            float conf = e.Result.Confidence;

            if(conf < 0.65)
            {
                return;
            }

            // Change the text of listBox1 and textBox1
            // A method invoker is needed because the speech recognizer is running
            //  in a different thread from the Windows Forms UI.
            this.Invoke(new MethodInvoker(() =>
            {
                // WinForm specific
                listBox1.Items.Add("I heard you say: " + txt);
            }));

            if (txt.IndexOf("text") >= 0 && txt.IndexOf("box") >= 0
                && txt.IndexOf("1") >= 0)
            {
                string[] words = txt.Split(' ');
                this.Invoke(new MethodInvoker(() =>
                {
                    // WinForm specific
                    textBox1.Text = words[4];
                }));
            }

            // Pull up the youtube videos of the subsequent commands in the web browser
            if(txt.IndexOf("alexa") >= 0 && txt.IndexOf("play") >= 0)
            {
                string url = "";

                if(txt.IndexOf("all star") >= 0)
                {
                    url = "https://youtu.be/L_jWHffIx5E?t=36";
                }           
                else if(txt.IndexOf("everybodies curculation") >= 0)
                {
                    url = "https://www.youtube.com/watch?v=RQmEERvqq70";
                }
                else if(txt.IndexOf("megalovania") >= 0)
                {
                    url = "https://www.youtube.com/watch?v=ZcoqR9Bwx1Y";
                }

                this.Invoke(new MethodInvoker(() =>
                {
                    webBrowser1.Url = new Uri(url);
                }));          
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                sre.RecognizeAsync(RecognizeMode.Multiple);
            }
            else if(!checkBox1.Checked)
            {
                sre.RecognizeAsyncCancel();
            }
        }
    } // Form
} // ns
