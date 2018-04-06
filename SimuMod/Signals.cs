using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuMod
{
    class Signals
    {
        public static double[] SineWave(double Freq,double TimeSpacing)
        {
            double p = 1 / Freq;
            int samples = (int)(p / TimeSpacing);

            double[] sinus = new double[samples];
            double step = Math.PI * 2 / samples;
            for (int i = 0; i < samples; i++)
            {
                //radians
                double arg = (step * i);
                sinus[i]=(Math.Sin(arg));
            }
            return sinus;
        }

        public static List<double> Square(double Fd, double Freq, int Periods, double time)
        {
            List<double> square = new List<double>();
            if (Freq >= Fd / 2) return square;

            int onepulse = (int)((Fd / Freq) * (time / Fd));
            //int cnt = (int)(time / onepulse);

            for (int i = 0; i < time && i < Fd; i++)
            {
                int temp = i % onepulse;
                if (temp < onepulse / 2)
                {
                    square.Add(1);
                }
                else
                {
                    square.Add(0);
                }
            }

            return square;
        }
    }
}
