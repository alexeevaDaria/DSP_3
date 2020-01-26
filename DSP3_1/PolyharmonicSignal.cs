using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP3_1
{

    class PolyharmonicSignal : Signal
    {
        double[] A, phi;
        double[] resA, resPhi;
        double f;
        public PolyharmonicSignal(double[] amplitudes, double freq, double[] phases, int discrPoints, 
            int min, int max, FiltrationType filtrationType): base (min, max, filtrationType)
        {
            A = amplitudes;
            n = discrPoints;
            f = freq;
            phi = phases;
            //количество гармоник оценки
            //numHarm = n % 2 == 0 ? n / 2 : (n - 1)/2;

            //количество точек восстановления, если решишь оставить их на N не забудт
            //в классе сигнала убрать ограничение на впихивание в массив только четных чисел,
            //но это опять же выкидон демона, он хотел увидеть как сигнал по половине значение восстанавливается 

            resotorePoints = n % 2 == 0 ? n / 2 : (n / 2 - 1);
            //resotorePoints = n;

            numHarm = 30;

            resA = new double[numHarm];
            resPhi = new double[numHarm];

            Random rnd = new Random();
            for (int i = 0; i < numHarm - 1; i++)
            {
                resA[i] = A[rnd.Next(7)];
                resPhi[i] = phi[rnd.Next(6)];
            }
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
            Random rnd = new Random();
            for (int i = 0; i <= n - 1; i++)
            {
                double tm = 0;
                for (int j = 0; j <= numHarm - 1; j++)
                {
                    //вариант 1 - изначальный
                     tm+= A[rnd.Next(7)]* Math.Cos(2 * Math.PI * f * i / n - phi[rnd.Next(6)]);
                    //вообщем суть в том что закоменченое снизу работает как нужно, 
                    //но тогда сигналы не совпадают (красная и синии линии)

                    //вариант 2 - нужный демону но с проблемами описанными выше
                    //tm += resA[j] * Math.Cos(2 * Math.PI * j  * i / n - resPhi[j]);
                }
                sign[i] = tm;
            }
            return sign;
        }
    }
}
