using System;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;
using System.Globalization;

namespace console_speech
{
    class Program
    {
        static SpeechSynthesizer ss = new SpeechSynthesizer();
        static SpeechRecognitionEngine sre;
        static bool done = false;
        static bool speechOn = true;
        
        static void Main(string[] args)
        {
            try
            {
                ss.SetOutputToDefaultAudioDevice();
                Console.WriteLine("\n(Speaking: I am awake)");
                ss.Speak("I am awake");

                // Setting the language to US English
                CultureInfo ci = new CultureInfo("en-us");
                sre = new SpeechRecognitionEngine(ci);
                sre.SetInputToDefaultAudioDevice();
                sre.SpeechRecognized += sre_SpeechRecognized;

                // Setting up start/stop commands
                Choices ch_StartStopCommands = new Choices();
                ch_StartStopCommands.Add("speech on");
                ch_StartStopCommands.Add("speech off");
                ch_StartStopCommands.Add("farewell");

                GrammarBuilder gb_StartStop = new GrammarBuilder();
                gb_StartStop.Append(ch_StartStopCommands);
                Grammar g_StartStop = new Grammar(gb_StartStop);

                Choices ch_Numbers = new Choices();
                ch_Numbers.Add("1");
                ch_Numbers.Add("2");
                ch_Numbers.Add("3");
                ch_Numbers.Add("4");

                GrammarBuilder gb_WhatIsXplusY = new GrammarBuilder();
                gb_WhatIsXplusY.Append("What is");
                gb_WhatIsXplusY.Append(ch_Numbers);
                gb_WhatIsXplusY.Append("plus");
                gb_WhatIsXplusY.Append(ch_Numbers);

                Grammar g_WhatIsXplusY = new Grammar(gb_WhatIsXplusY);

                sre.LoadGrammarAsync(g_StartStop);
                sre.LoadGrammarAsync(g_WhatIsXplusY);
            }
            catch
            {

            }
        }
    }
}
