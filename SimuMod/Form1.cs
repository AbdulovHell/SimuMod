using System;
//using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace SimuMod
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        double Interpolate(double Left, double Right, double Ind)
        {
            return (Right - Left) * Ind + Left;
        }

        /*List<double> ReDiscr(ref List<double> input, Double NewSampleRate)
        {
            int cnt = input.Count;
            List<double> output = new List<double>();

            UInt64 steps = (UInt64)NewSampleRate;

            for (UInt64 i = 0; i < steps - 1; i++)
            {
                if (i == 0) output.Add(input[0]);
                Double temp = i / NewSampleRate * (cnt - 1);
                if ((int)temp + 1 > cnt - 1)
                {
                    output.Add(input[cnt - 1]);
                }
                else
                {
                    output.Add(Interpolate(input[(int)temp], input[(int)temp + 1], temp - (int)temp));
                }
            }

            return output;
        }*/

        void Exp3()
        {
            //this.trackBar1.ValueChanged -= new System.EventHandler(this.trackBar1_ValueChanged);
            //trackBar1.Value = 1;
            //this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);


            double SimulTime = 100 * Math.Pow(10, -3); //10 ms
            double SampleRate = 5 * Math.Pow(10, 6); //MHz

            double TimeSpacing = 1 / SampleRate;  //us

            double SamplesCount = Fourier.FFT.GetCompatFdiscr((ulong)(SimulTime / TimeSpacing));

            SimulTime = TimeSpacing * SamplesCount;
            //double SamplesCount = SimulTime / TimeSpacing * 1000;
            double FreqRes = SampleRate / SamplesCount;

            //SamplesCount=SampleRate / FreqRes;

            //chart1.Series[0].Points.Clear();
            //chart2.Series[0].Points.Clear();
            textBox1.AppendText(SimulTime.ToString() + " s\n");
            textBox1.AppendText(SampleRate.ToString() + " Hz\n");
            textBox1.AppendText(TimeSpacing.ToString() + " s\n");
            textBox1.AppendText(SamplesCount.ToString() + " cnt\n");
            textBox1.AppendText(FreqRes.ToString() + " Hz\n");
            textBox1.AppendText("\n");



            //double[] Wave = Signals.SineWave(200 * Math.Pow(10, 3), TimeSpacing);
            //double[] Wave1 = Signals.SineWave(300 * Math.Pow(10, 3), TimeSpacing);
            //double[] Wave2 = Signals.SineWave(500 * Math.Pow(10, 3), TimeSpacing);

            double[] Signal = new double[(int)SamplesCount];

            Random rnd = new Random();
            for (int i = 0; i < SamplesCount; i++)
            {
                Signal[i] = rnd.Next(2);
            }

            double[] Re = new double[(int)SamplesCount / 2];
            double[] Im = new double[(int)SamplesCount / 2];

            for (int i = 0; i < (int)SamplesCount / 2; i++)
            {
                byte b3 = (byte)Signal[i * 2];
                byte b2 = (byte)Signal[i * 2 + 1];
                byte res = b3;
                res <<= 1;
                res += b2;
                Mapper.QPSK(res, ref Re[i], ref Im[i]);
            }

            //забиваем им все отсчеты
            //for (int i = 0; i < SamplesCount;)
            //{
            //    for (int j = 0; j < Wave.Length && i < SamplesCount; j++, i++)
            //    {
            //        Signal[i] = Wave[j];
            //    }
            //}
            //for (int i = 0; i < SamplesCount;)
            //{
            //    for (int j = 0; j < Wave1.Length && i < SamplesCount; j++, i++)
            //    {
            //        Signal[i] += Wave1[j];
            //    }
            //}
            //for (int i = 0; i < SamplesCount;)
            //{
            //    for (int j = 0; j < Wave2.Length && i < SamplesCount; j++, i++)
            //    {
            //        Signal[i] += Wave2[j];
            //    }
            //}

            //double[] spec = new double[Signal.Length];
            //var r = Task.Run(() => Fourier.FFT.FFTAnalysis(ref Signal, ref spec));
            //вывод
            double[] TimePoints = new double[(int)SamplesCount / 2];
            //for (int i = 0; i < Signal.Length; i++)
            //    TimePoints[i] = (i * TimeSpacing * Math.Pow(10, 6));
            ////chart1.Series[0].Points.DataBindXY(TimePoints, Signal);
            //int ViewTime = (int)(10.0 / TimeSpacing);
            //for (int i = 0; i < Signal.Count && i < 20; i++)
            //{
            //    chart1.Series[0].Points.AddXY(i * TimeSpacing, Signal[i]);
            //}

            //CDF fir = new CDF("coefs.flt");
            //double[] Filtered = new double[Signal.Length];
            //for (int i = 0; i < Signal.Length; i++)
            //    Filtered[i] = fir.Out(Signal[i]);
            for (int i = 0; i < TimePoints.Length; i++)
            {
                TimePoints[i] = TimeSpacing * 2 * i * 1000;
            }
            chart1.Series[0].Points.DataBindXY(TimePoints, Re);
            chart1.Series[1].Points.DataBindXY(TimePoints, Im);

            CDF fir = new CDF("coefs.flt");
            double[] FilteredRe = new double[Re.Length];
            for (int i = 0; i < Re.Length; i++)
                FilteredRe[i] = fir.Out(Re[i]);
            double[] FilteredIm = new double[Im.Length];
            for (int i = 0; i < Im.Length; i++)
                FilteredIm[i] = fir.Out(Im[i]);

            chart3.Series[0].Points.DataBindXY(TimePoints, FilteredRe);
            chart3.Series[1].Points.DataBindXY(TimePoints, FilteredIm);
            //double[] specf = new double[Filtered.Length];
            //var tr = Task.Run(() => Fourier.FFT.FFTAnalysis(ref Filtered, ref specf));
            ////спектр
            //r.Wait();
            //tr.Wait();
            //double[] FreqPoints = new double[spec.Length];
            //for (int i = 0; i < spec.Length; i++)
            //    FreqPoints[i] = (i * FreqRes * Math.Pow(10, -3));
            //chart2.Series[0].Points.DataBindXY(FreqPoints, spec);
            //chart2.Series[1].Points.DataBindXY(FreqPoints, specf);
            ////for (int i = 0; i < spec.Count; i++)
            //{
            //   chart2.Series[0].Points.AddXY(, spec[i]);
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            //UInt64 data = 0x1234567812345678;
            //const UInt64 sel = 0xC000000000000000;

            //Массивы отсчетов I и Q
            //List<double> ReI = new List<double>();
            //List<double> ImQ = new List<double>();

            //for (int i = 0; i < 32; i++)
            //{
            //    UInt64 num = data & sel;
            //    num >>= 62;
            //    Mapper.QPSK((byte)num,ref ReI,ref ImQ);
            //    data <<= 2;
            //}

            //Передадим 8 байт
            Random rnd = new Random();
            for (int i = 0; i < 8; i++)
            {
                //Берем байт из источника
                byte data = (byte)rnd.Next(256);
                //Разделяем по два бита, для QPSK
                for (int j = 0; j < 4; j++)
                {
                    byte bits = (byte)(data & 0xC0);
                    bits >>= 6;
                    data <<= 2;
                    //Преобразуем
                    //Mapper.QPSK(bits, ref ReI, ref ImQ);
                }
            }

            uint Fd = (uint)Fourier.FFT.GetCompatFdiscr(1000);
            //List<double> NewRe = ReDiscr(ref ReI, Fd);
            //List<double> NewIm = ReDiscr(ref ImQ, Fd);

            //double[] output = new double[SampleRate];
            //double[] input = new double[SampleRate];
            //for (int i = 0; i < SampleRate; i++) input[i] = discr1[i];
            //Fourier.FFT.FFTAnalysis(input, output);
            //for (int i = 0; i < SampleRate; i++) discr1[i] = output[i];
            //Fourier.FFT.FFTAnalysis(ref NewRe, out List<double> FFTRe);

            CDF fir = new CDF("coefs.flt");
            //List<double> FilteredRe = new List<double>();
            //for (int i = 0; i < NewRe.Count; i++)
            //FilteredRe.Add(fir.Out(NewRe[i]));
            //List<double> FilteredRe = fir.(ref NewRe);
            //List<double> FilteredIm = fir.Apply(ref NewIm);

            //Fourier.FFT.FFTAnalysis(ref FilteredRe, out List<double> FFTFilt);
            //double[] output = new double[SampleRate];
            //double[] input = new double[SampleRate];
            //for (int i = 0; i < SampleRate; i++) input[i] = discr1[i];
            //FFT.FFTAnalysis(input, output, SampleRate, SampleRate);


            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart2.Series[1].Points.Clear();
            chart3.Series[0].Points.Clear();
            chart3.Series[1].Points.Clear();
            //for (int i = 0; i < FFTRe.Count && i < 200; i++)
            //{
            //    chart1.Series[0].Points.Add(FFTRe[i]);
            //    chart1.Series[1].Points.Add(FFTFilt[i]);
            //}
            //for (int i = 0; i < NewRe.Count; i++)
            //{
            //    chart2.Series[0].Points.Add(NewRe[i]);
            //    chart2.Series[1].Points.Add(NewIm[i]);
            //}
            //for (int i = 0; i < FilteredRe.Count; i++)
            //{
            //    chart3.Series[0].Points.Add(FilteredRe[i]);
            //    //chart3.Series[1].Points.Add(FilteredIm[i]);
            //}
            //for (int i = 0; i < SampleRate && i < 300; i++)
            //{
            //chart3.Series[0].Points.Add(output[i]);
            //chart3.Series[1].Points.Add(Filtered2[i]);
            //}
            stopwatch.Stop();
            Text = stopwatch.ElapsedMilliseconds.ToString() + " ms";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Exp3();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            button2.Text = trackBar1.Value.ToString();
            button2_Click(sender, e);
        }
    }
}
