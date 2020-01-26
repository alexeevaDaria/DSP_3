using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP3_1
{
    abstract class Signal
    {
        internal int n;
        internal double[] signal, restSignal, nfSignal;
        internal double[] sineSp, cosineSp;
        internal double[] amplSp, phaseSp;
        internal int numHarm = 100;
        internal int min, max;
        internal FiltrationType filtrationType;
        internal int resotorePoints;
        public Signal(int min, int max, FiltrationType filtrationType)
        {
            this.min = min;
            this.max = max;
            this.filtrationType = filtrationType;

        }
        public void reDrawSignal(int min, int max, FiltrationType filtrationType)
        {
            this.min = min;
            this.max = max;
            this.filtrationType = filtrationType;
            int resotorePoints = n % 2 == 0 ? n / 2 : (n /2 - 1);

            //signal = GenerateSignal();
            sineSp = GetSineSpectrum();
            cosineSp = GetCosineSpectrum();
            amplSp = GetAmplSpectrum();
            phaseSp = GetPhaseSpectrum();
            restSignal = RestoreSignal();
            nfSignal = RestoreNFSignal();
        }
        public double[] signVal { get { return signal; } }
        public double[] amplSpectrum { get { return amplSp; } }
        public double[] phaseSpectrum { get { return phaseSp; } }
        public double[] restoredSignal { get { return restSignal; } }
        public double[] restorednonphasedSignal { get { return nfSignal; } }

        internal virtual double[] GenerateSignal()
        {
            return null;
        }

        internal double[] GetSineSpectrum()
        {
            double[] values = new double[numHarm];
            for (int j = 0; j <= numHarm - 1; j++)
            {
                double val = 0;
                for (int i = 0; i <= n - 1; i++)
                {
                    val += signal[i] * Math.Sin(2 * Math.PI * i * j / n);
                }
                values[j] = 2 * val / n;
            }
            return values;
        }

        internal double[] GetCosineSpectrum()
        {
            double[] values = new double[numHarm];
            for (int j = 0; j <= numHarm - 1; j++)
            {
                double val = 0;
                for (int i = 0; i <= n - 1; i++)
                {
                    val += signal[i] * Math.Cos(2 * Math.PI * i * j / n);
                }
                values[j] = 2 * val / n;
            }
            return values;
        }

        internal double[] GetAmplSpectrum()
        {
            double[] values = new double[numHarm];
            double[] temper = new double[numHarm];
            double tempValue;

            for (int j = 0; j <= numHarm - 1; j++)
            {
                tempValue = Math.Sqrt(Math.Pow(sineSp[j], 2) + Math.Pow(cosineSp[j], 2));
                temper[j] = tempValue;
                switch (filtrationType)
                {
                    case FiltrationType.MinMax:
                        values[j] = (j > max && j < min) ? tempValue : 0;
                        break;
                    case FiltrationType.Max:
                        values[j] = j < max ? 0 : tempValue;
                        break;
                    case FiltrationType.Min:
                        values[j] = j > min ? 0 : tempValue;
                        break;
                    case FiltrationType.none:
                        values[j] = tempValue;
                        break;
                }
            }
            return values;
        }

        internal double[] GetPhaseSpectrum()
        {
            double[] values = new double[numHarm];
            for (int j = 0; j <= numHarm - 1; j++)
            {
                values[j] = Math.Atan(sineSp[j] / cosineSp[j]);
            }
            return values;
        }

        internal double[] RestoreSignal()
        {
            //каждый четное значение            
            double[] values = new double[resotorePoints];
            int temp = 0;
            for (int i = 0; i <= n - 1; i++)
            {
                double val = 0;
                for (int j = 0; j <= numHarm - 1; j++)
                {
                    val += amplSp[j] * Math.Cos(2 * Math.PI * i * j / n - phaseSp[j]);
                }
                //четное значение
                if (i % 2 == 0)
                {
                    values[temp] = val;
                    temp++;
                }               
            }
            return values;
        }
        
        internal double[] RestoreNFSignal()
        {
            double[] values = new double[resotorePoints];
            int temp = 0;
            for (int i = 0; i <= n - 1; i++)
            {
                double val = 0;
                for (int j = 0; j <= numHarm - 1; j++)
                {                             
                    val += amplSp[j] * Math.Cos(2 * Math.PI * i * j / n);
                }
                //четное значение
                if (i % 2 == 0)
                {
                    values[temp] = val;
                    temp++;
                }
            }
            return values;
        }
    }
}
