using System;
using System.Timers;

namespace MicrosoftSecurityCodeAnalysisTesting.FlawedCode
{
    public class PoorCode   
    {
        public Timer UnDisposedTimer { get; set; }

        public int i, j, k, l = 0;//poor names - will SAST notice?

        public PoorCode()
        {
            UnDisposedTimer = new Timer //will SAST notice if this is not destroyed, and we shoudl use IDisposable?
            {
                Interval = 60000              
            };

            UnDisposedTimer.Elapsed += UnDisposedTimer_Elapsed;
            UnDisposedTimer.Start();
        }

        private static void UnDisposedTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            throw new Exception("Shouldn't use Exception - will this be picked up by SAST?");
        }

        internal double TimerDuration()
        {
            return UnDisposedTimer.Interval;
        }
      
    }
}
