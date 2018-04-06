using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SimuMod
{
    class CDF
    {
        int N;
        List<double> Line;
        List<double> a;
        List<double> b;

        public CDF(String path)
        {
            FileStream stream = File.OpenRead(path);

            const int BUF_SIZE = 10000;
            byte[] buf = new byte[BUF_SIZE];
            stream.Read(buf, 0, BUF_SIZE);
            char[] buf2 = new char[BUF_SIZE];
            for (int i = 0; i < BUF_SIZE; i++) buf2[i] = (char)buf[i];
            String txt = new String(buf2);
            String[] output = System.Text.RegularExpressions.Regex.Split(txt, "[\n]", System.Text.RegularExpressions.RegexOptions.None);
            String[] output2 = System.Text.RegularExpressions.Regex.Split(output[0], "=", System.Text.RegularExpressions.RegexOptions.None);
            int.TryParse(output2[1], out int res);

            N = res;
            Line = new List<double>();
            a = new List<double>();
            b = new List<double>();
            for (int i = 0; i < N+1; i++)
            {
                a.Add(0);
                b.Add(0);
                Line.Add(0);
            }
            ClearLine();

            for (int i = 0; i < res; i++)
            {
                output[i + 1] = output[i + 1].Trim();
                output[i + 1] = output[i + 1].Replace('.', ',');
                a[i] = double.Parse(output[i + 1], System.Globalization.NumberStyles.Float);
            }
        }

        public void ClearLine()
        {
            for (int i = 0; i < N; i++)
            {
                Line[i] = 0;
            }
        }

        public void InLine(double x)
        {
            for(int i = N - 1; i > 0; i--)
            {
                Line[i] = Line[i - 1];
            }
            Line[0] = x;
        }

        public double Out(double x)
        {
            double sum1 = x;
            for (int i = 1; i <= N; i++)
                sum1 += b[i] * Line[i - 1];
            double sum2 = a[0] * sum1;
            for (int i = 1; i <= N; i++)
                sum2 += a[i] * Line[i - 1];
            InLine(sum1);
            return sum2;
        }
    }
}
