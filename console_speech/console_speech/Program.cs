using System;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;
using System.Globalization;

namespace console_speech
{
    class Program
    {
        // SpeechSynthesizer gives the application the ability to speak.
        static SpeechSynthesizer ss = new SpeechSynthesizer();
        // The engine object allows the app to listen for and recognize spoken words or phrases.
        static SpeechRecognitionEngine sre;
        static bool done = false;
        static bool speechOn = true;
        
        static void Main(string[] args)
        {
            try
            {
                // Sends output to the speakers (can also be sent to a file).
                ss.SetOutputToDefaultAudioDevice();
                Console.WriteLine("\n(Speaking: I am awake)");
                // Speak method accepts a string and speaks it aloud.
                ss.Speak("I am awake");

                // Setting the language to US English
                CultureInfo ci = new CultureInfo("en-us");
                sre = new SpeechRecognitionEngine(ci);
                // Picks up from the mic.
                sre.SetInputToDefaultAudioDevice();
                sre.SpeechRecognized += sre_SpeechRecognized; // event handler

                // Setting up start/stop commands
                Choices ch_StartStopCommands = new Choices();
                ch_StartStopCommands.Add("speech on");
                ch_StartStopCommands.Add("speech off");
                ch_StartStopCommands.Add("farewell");

                GrammarBuilder gb_StartStop = new GrammarBuilder();
                gb_StartStop.Append(ch_StartStopCommands);
                Grammar g_StartStop = new Grammar(gb_StartStop);

                // Make a Choices collection, add numbers (strings) to it.
                Choices ch_Numbers = new Choices();
                ch_Numbers.Add("1");
                ch_Numbers.Add("2");
                ch_Numbers.Add("3");
                ch_Numbers.Add("4");

                // List of specific examples of what we're listening for.
                //  Then determine a corresponding general template.
                //  The template is a GrammarBuilder, and the specific values that go into it are Choices.
                GrammarBuilder gb_WhatIsXplusY = new GrammarBuilder();
                gb_WhatIsXplusY.Append("What is");
                gb_WhatIsXplusY.Append(ch_Numbers);
                gb_WhatIsXplusY.Append("plus");
                gb_WhatIsXplusY.Append(ch_Numbers);

                Grammar g_WhatIsXplusY = new Grammar(gb_WhatIsXplusY);

                sre.LoadGrammarAsync(g_StartStop);
                sre.LoadGrammarAsync(g_WhatIsXplusY);
                sre.RecognizeAsync(RecognizeMode.Multiple);

                while(done == false)
                {
                    ;
                }

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        } // Main

        static void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string txt = e.Result.Text;
            float confidence = e.Result.Confidence;
            Console.WriteLine("\nRecognized: " + txt);

            if(confidence < 0.60)
            {
                return;
            }
            if(txt.IndexOf("speech on") >= 0)
            {
                Console.WriteLine("Speech is now ON");
                speechOn = true;
            }
            if(txt.IndexOf("speech off") >= 0)
            {
                Console.WriteLine("Speech is now OFF");
                speechOn = false;
            }
            if (speechOn == false)
            {
                return;
            }

            if(txt.IndexOf("farewell") >= 0)
            {
                ((SpeechRecognitionEngine)sender).RecognizeAsyncCancel();
                done = true;
                Console.WriteLine("(Speaking: Farewell)");
                ss.Speak("Farewell");
            }

            if(txt.IndexOf("what") >= 0 && txt.IndexOf("plus") >= 0)
            {
                string[] words = txt.Split(' ');
                int num1 = int.Parse(words[2]);
                int num2 = int.Parse(words[4]);
                int sum = num1 + num2;
                Console.WriteLine("(Speaking: " + words[2] + " plus " + 
                    words[4] + " equals " + sum + ")");
                ss.SpeakAsync(words[2] + " plus " +
                    words[4] + " equals " + sum);
            }
        } // sre_SpeechRecognized
    } // Program
} // ns
