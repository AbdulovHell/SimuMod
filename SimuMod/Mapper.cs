using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuMod
{
    class Mapper
    {
        static double Dist(double x, double y) => Math.Sqrt(x * x + y * y);

        public static void BPSK(byte bit, ref double Re, ref double Im)
        {
            Re = Math.Cos(Math.PI * bit);
            Im = Math.Sin(Math.PI * bit);
        }

        public static void QPSK(byte number, ref double Re, ref double Im)
        {
            switch (number)
            {
                case 0:
                    Re = (1 * Math.Cos(225.0 * (Math.PI / 180.0)));
                    Im = (1 * Math.Sin(225.0 * (Math.PI / 180.0)));
                    break;
                case 1:
                    Re = (1 * Math.Cos(135.0 * (Math.PI / 180.0)));
                    Im = (1 * Math.Sin(135.0 * (Math.PI / 180.0)));
                    break;
                case 2:
                    Re = (1 * Math.Cos(315.0 * (Math.PI / 180.0)));
                    Im = (1 * Math.Sin(315.0 * (Math.PI / 180.0)));
                    break;
                case 3:
                    Re = (1 * Math.Cos(45.0 * (Math.PI / 180.0)));
                    Im = (1 * Math.Sin(45.0 * (Math.PI / 180.0)));
                    break;
                default:
                    Re = (0);
                    Im = (0);
                    break;
            }
        }

        public static void QAM16(byte bits4, ref double Re, ref double Im)
        {
            int[][] xk = {new int[]{ -3, -1, +1, +3 },
                          new int[]{ -3, -1, +1, +3 },
                          new int[]{ -3, -1, +1, +3 },
                          new int[]{ +3, -1, +1, +3 }};

            int[][] yk = {new int[]{  3,  3,  3,  3 },
                          new int[]{  1,  1,  1,  1 },
                          new int[]{ -1, -1, -1, -1 },
                          new int[]{ -3, -3, -3, -3 }};

            double a = 0.3163;



            Re = a*xk[0][0];
            Im = a*yk[0][0];
        }
    }
}
