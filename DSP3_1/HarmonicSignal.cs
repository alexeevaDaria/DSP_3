using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP3_1
{
    class HarmonicSignal : Signal
    {
        double A, f, phi;
        public HarmonicSignal(double amplitude, double freq, double phase, int discrPoints,
            int min, int max, FiltrationType filtrationType) : base(min, max, filtrationType)
        {
            A = amplitude;
            n = discrPoints;
            f = freq;
            phi = phase;

            numHarm = n % 2 == 0 ? n / 2 : (n - 1)/2;
            resotorePoints = n % 2 == 0 ? n / 2 : (n / 2 - 1);
            //resotorePoints = n;

            //numHarm = 30;
            signal = GenerateSignal();
            sineSp = GetSineSpectrum();
            cosineSp = GetCosineSpectrum();
            amplSp = GetAmplSpectrum();
            phaseSp = GetPhaseSpectrum();
            restSignal = RestoreSignal();
            nfSignal = RestoreNFSignal();
        }

        internal override double[] GenerateSignal()
        {
            double[] sign = new double[n];
            for (int i = 0; i <= n - 1; i++)
            {
                sign[i] = A * Math.Cos(2 * Math.PI * f * i / n + phi);
            }
            return sign;
        }

    }
}
