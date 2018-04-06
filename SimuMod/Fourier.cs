using System;
using System.Numerics;
using System.Collections.Generic;

namespace Fourier
{
    class ComplexFFT
    {
        /// <summary>
        /// Вычисление поворачивающего модуля e^(-i*2*PI*k/N)
        /// </summary>
        /// <param name="k"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        private static Complex w(int k, int N)
        {
            if (k % N == 0) return 1;
            double arg = -2 * Math.PI * k / N;
            return new Complex(Math.Cos(arg), Math.Sin(arg));
        }
        /// <summary>
        /// Возвращает спектр сигнала
        /// </summary>
        /// <param name="x">Массив значений сигнала. Количество значений должно быть степенью 2</param>
        /// <returns>Массив со значениями спектра сигнала</returns>
        public static Complex[] fft(Complex[] x)
        {
            Complex[] X;
            int N = x.Length;
            if (N == 2)
            {
                X = new Complex[2];
                X[0] = x[0] + x[1];
                X[1] = x[0] - x[1];
            }
            else
            {
                Complex[] x_even = new Complex[N / 2];
                Complex[] x_odd = new Complex[N / 2];
                for (int i = 0; i < N / 2; i++)
                {
                    x_even[i] = x[2 * i];
                    x_odd[i] = x[2 * i + 1];
                }
                Complex[] X_even = fft(x_even);
                Complex[] X_odd = fft(x_odd);
                X = new Complex[N];
                for (int i = 0; i < N / 2; i++)
                {
                    X[i] = X_even[i] + w(i, N) * X_odd[i];
                    X[i + N / 2] = X_even[i] - w(i, N) * X_odd[i];
                }
            }
            return X;
        }
        /// <summary>
        /// Центровка массива значений полученных в fft (спектральная составляющая при нулевой частоте будет в центре массива)
        /// </summary>
        /// <param name="X">Массив значений полученный в fft</param>
        /// <returns></returns>
        public static Complex[] nfft(Complex[] X)
        {
            int N = X.Length;
            Complex[] X_n = new Complex[N];
            for (int i = 0; i < N / 2; i++)
            {
                X_n[i] = X[N / 2 + i];
                X_n[N / 2 + i] = X[i];
            }
            return X_n;
        }
    }

    class FFT
    {
        const double TwoPi = 6.283185307179586;

        private static int CorrectBuffer(ref List<double> data)
        {
            int i = 0;
            int cnt = data.Count;
            do
            {
                int pow = (int)Math.Pow(2, i);
                if (cnt == pow)
                {
                    return data.Count;
                }
                else if (cnt > pow)
                {
                    i++;
                    continue;
                }
                else if (cnt < pow)
                {
                    for (int j = 0; j < pow - cnt; j++)
                    {
                        data.Add(0);
                    }
                    return data.Count;
                }
                else
                    return -1;
            } while (true);
        }

        private static bool CheckCompat(ref double[] data)
        {
            int i = 0;
            int cnt = data.Length;
            do
            {
                int pow = (int)Math.Pow(2, i);
                if (cnt == pow)
                {
                    return true;
                }
                else if (cnt > pow)
                {
                    i++;
                    continue;
                }
                else if (cnt < pow)
                {
                    return false;
                }
                else
                    return false;
            } while (true);
        }

        public static UInt64 GetCompatFdiscr(UInt64 cnt)
        {
            UInt64 i = 0;
            do
            {
                UInt64 pow = (UInt64)Math.Pow(2, i);
                if (cnt == pow)
                {
                    return pow;
                }
                else if (cnt > pow)
                {
                    i++;
                    continue;
                }
                else if (cnt < pow)
                {
                    return pow;
                }
                else
                    return 0;
            } while (true);
        }

        public static int GetCompatFdiscr(int cnt)
        {
            int i = 0;
            do
            {
                int pow = (int)Math.Pow(2, i);
                if (cnt == pow)
                {
                    return pow;
                }
                else if (cnt > pow)
                {
                    i++;
                    continue;
                }
                else if (cnt < pow)
                {
                    return pow;
                }
                else
                    return 0;
            } while (true);
        }

        // AVal - массив анализируемых данных, Nvl - длина массива должна быть кратна степени 2.
        // FTvl - массив полученных значений, Nft - длина массива должна быть равна Nvl.
        public static void FFTAnalysis(ref double[] AVal, ref double[] FTvl)
        {
            //CorrectBuffer(ref AVal);
            if (!CheckCompat(ref AVal)) return;

            int i, j, n, m, Mmax, Istp;
            double Tmpr, Tmpi, Wtmp, Theta;
            double Wpr, Wpi, Wr, Wi;
            double[] Tmvl;

            n = AVal.Length * 2;
            Tmvl = new double[n];

            for (i = 0; i < n; i += 2)
            {
                Tmvl[i] = 0;
                Tmvl[i + 1] = AVal[i / 2];
            }

            i = 1; j = 1;
            while (i < n)
            {
                if (j > i)
                {
                    Tmpr = Tmvl[i]; Tmvl[i] = Tmvl[j]; Tmvl[j] = Tmpr;
                    Tmpr = Tmvl[i + 1]; Tmvl[i + 1] = Tmvl[j + 1]; Tmvl[j + 1] = Tmpr;
                }
                i = i + 2; m = AVal.Length;
                while ((m >= 2) && (j > m))
                {
                    j = j - m; m = m >> 1;
                }
                j = j + m;
            }

            Mmax = 2;
            while (n > Mmax)
            {
                Theta = -TwoPi / Mmax; Wpi = Math.Sin(Theta);
                Wtmp = Math.Sin(Theta / 2); Wpr = Wtmp * Wtmp * 2;
                Istp = Mmax * 2; Wr = 1; Wi = 0; m = 1;

                while (m < Mmax)
                {
                    i = m; m = m + 2; Tmpr = Wr; Tmpi = Wi;
                    Wr = Wr - Tmpr * Wpr - Tmpi * Wpi;
                    Wi = Wi + Tmpr * Wpi - Tmpi * Wpr;

                    while (i < n)
                    {
                        j = i + Mmax;
                        Tmpr = Wr * Tmvl[j] - Wi * Tmvl[j - 1];
                        Tmpi = Wi * Tmvl[j] + Wr * Tmvl[j - 1];

                        Tmvl[j] = Tmvl[i] - Tmpr; Tmvl[j - 1] = Tmvl[i - 1] - Tmpi;
                        Tmvl[i] = Tmvl[i] + Tmpr; Tmvl[i - 1] = Tmvl[i - 1] + Tmpi;
                        i = i + Istp;
                    }
                }

                Mmax = Istp;
            }

            for (i = 0; i < AVal.Length; i++)
            {
                j = i * 2;
                FTvl[i] = 2 * Math.Sqrt(Math.Pow(Tmvl[j], 2) + Math.Pow(Tmvl[j + 1], 2)) / AVal.Length;
            }

            //delete[] Tmvl;
        }
    }
}
