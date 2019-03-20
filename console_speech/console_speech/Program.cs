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

                // Make a Choices collection, add numbers (array of strings) to it.
                string[] numbers = new string[] { "1", "2", "3", "4" };
                Choices ch_Numbers = new Choices(numbers);

                // List of specific examples of what we're listening for.
                //  Then determine a corresponding general template.
                //  The template is a GrammarBuilder, and the specific values that go into it are Choices.
                // These are all of the words that this Grammar object will be listening for.
                GrammarBuilder gb_WhatIsXplusY = new GrammarBuilder();
                gb_WhatIsXplusY.Append("What is");
                gb_WhatIsXplusY.Append(ch_Numbers);
                gb_WhatIsXplusY.Append("plus");
                gb_WhatIsXplusY.Append(ch_Numbers);
                Grammar g_WhatIsXplusY = new Grammar(gb_WhatIsXplusY);

                // Here we have a lot of flexibility when defining grammars.
                // We could create new Grammar objects for other commands.

                // Pass them into the speech recognizer once they're all created.
                sre.LoadGrammarAsync(g_StartStop);
                sre.LoadGrammarAsync(g_WhatIsXplusY);
                sre.RecognizeAsync(RecognizeMode.Multiple); // Multiple for when we have more than one Grammar.

                while(done == false)
                {
                    ; // This allows the console app shell to stay alive.
                }

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        } // Main

        // The recognized text is stored in the SpeechRecognizedEventArgs Result here.
        static void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string txt = e.Result.Text;
            // The confidence value (between 0.0 and 1.0) will tend to fall 
            //  as more commands are being listened for.
            float confidence = e.Result.Confidence;
            Console.WriteLine("\nRecognized: " + txt);

            // If the confidence is too low, don't do anything.
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
            // End the program upon hearing "farewell"
            if(txt.IndexOf("farewell") >= 0)
            {
                ((SpeechRecognitionEngine)sender).RecognizeAsyncCancel();
                done = true;
                Console.WriteLine("(Speaking: Farewell)");
                ss.Speak("Farewell");
            }
            // When this specific question is asked, return the sum of the two given numbers.
            if(txt.IndexOf("What") >= 0 && txt.IndexOf("plus") >= 0)
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
