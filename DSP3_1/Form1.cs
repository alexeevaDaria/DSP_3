using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Numerics;

namespace DSP3_1
{
    public partial class Form1 : Form
    {
        enum SignalType { Harmonic, Polyharmonic }     
        Series DataSer_1, DataSer_2, DataSer_3, DataSer_4, DataSer_5, DataSer_6;
        SignalType currentSignal;
        FiltrationType currentFiltrationType;
        bool Redraw = false;
        Signal instSignal;

        Chart[] targetCharts;
        public Form1()
        {
            InitializeComponent();

            targetCharts = new Chart[4];
            targetCharts[0] = chart1;
            targetCharts[1] = chart2;
            targetCharts[2] = chart3;
            targetCharts[3] = chart4;

            groupBox2.Enabled = false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 3;
            label2.Text = "НЧ: " + Convert.ToString(trackBar2.Value);
            label3.Text = "ВЧ: " + Convert.ToString(trackBar3.Value);
            currentSignal = SignalType.Harmonic;
            currentFiltrationType = FiltrationType.none;
            Calculate(trackBar1.Value, 0, 0);          
        }
        private void ClearCharts()
        {
            for (int i = 0; i <= 2; i++)
            {
                foreach (var j in targetCharts[i].Series)
                {
                    j.Points.Clear();
                }
            }
        }

        //количество точек пофиксить
        private void Calculate(int freq, int min, int max)
        {
            Signal hs;
            int N = 1024;//360
            if (Redraw == false || instSignal == null) { 
                if (currentSignal == SignalType.Harmonic)
                {                                                 //360
                    hs = new HarmonicSignal(10, freq, Math.PI / 4, N, min, max, currentFiltrationType);
                }
                else
                {
                    double[] A = new double[7] { 1, 3, 5, 8, 10, 12, 16 };
                    double[] ph = new double[6] { Math.PI / 6, Math.PI / 4, Math.PI / 3, Math.PI / 2, Math.PI, 3 * Math.PI / 4 };
                    hs = new PolyharmonicSignal(A, freq, ph, N, min, max, currentFiltrationType);
                }
                instSignal = hs;
            }
            else if (instSignal != null & Redraw == true)
                instSignal.reDrawSignal(min, max, currentFiltrationType);

            ClearCharts();
            
            Complex[] Summa = new Complex[N];
            for (int i = 0; i <= N - 1; i++)
            {
                targetCharts[0].Series[0].Points.AddXY(2 * Math.PI * i / N, instSignal.signVal[i]);
                Summa[i] = instSignal.signVal[i];
            }

            for (int i = 0; i <= instSignal.resotorePoints - 1; i++)
            {
                targetCharts[0].Series[1].Points.AddXY(2 * Math.PI * i / instSignal.resotorePoints, instSignal.restoredSignal[i]);
                targetCharts[0].Series[2].Points.AddXY(2 * Math.PI * i / instSignal.resotorePoints, instSignal.restorednonphasedSignal[i]);
            }


            if(currentSignal == SignalType.Polyharmonic)
            {
                Complex[] Summa2 = Butterfly.DecimationInTime(Summa, true);
                for (int i=0; i < Summa2.Length; i++)
                {
                    Summa2[i] /= Summa2.Length;
                    targetCharts[3].Series[0].Points.AddXY(2 * Math.PI * i / N, Math.Sqrt(Math.Pow(Summa2[i].Real, 2) + Math.Pow(Summa2[i].Imaginary, 2)));
                }               
            }

           
            for (int i = 0; i <= instSignal.numHarm - 1; i++)
            {
                targetCharts[1].Series[0].Points.AddXY(i, instSignal.phaseSp[i]);
                targetCharts[2].Series[0].Points.AddXY(i, instSignal.amplSp[i]);
            }
            
        }


        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = "Частота: " + Convert.ToString(trackBar1.Value);
            Redraw = false;
            Calculate(trackBar1.Value, trackBar2.Value, trackBar3.Value);
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
          //  trackBar3.Minimum = trackBar2.Value + 1;
            label2.Text = "НЧ: " + Convert.ToString(trackBar2.Value);
            Redraw = true;
            Calculate(trackBar1.Value, trackBar2.Value, trackBar3.Value);
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
           // trackBar2.Maximum = trackBar3.Value - 1;
            label3.Text = "ВЧ: " + Convert.ToString(trackBar3.Value);
            Redraw = true;
            Calculate(trackBar1.Value, trackBar2.Value, trackBar3.Value);
        }

        //фильтрация для полигармоники
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                currentFiltrationType = FiltrationType.Min;
            }
            else if (comboBox2.SelectedIndex == 1)
            {
                currentFiltrationType = FiltrationType.Max;
            }
            else if(comboBox2.SelectedIndex == 3)
            {
                currentFiltrationType = FiltrationType.none;
            }
            else
            {
                currentFiltrationType = FiltrationType.MinMax;
            }
            //
            Redraw = true;
            Calculate(trackBar1.Value, trackBar2.Value, trackBar3.Value);
        }
        // тип сигнала: -гармонический
        //              -полигармонический (можно не перерисовывать после фильтрации)
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                currentSignal = SignalType.Harmonic;
                currentFiltrationType = FiltrationType.none;
                groupBox2.Enabled = false;
            }
            else
            {
                currentSignal = SignalType.Polyharmonic;
                groupBox2.Enabled = true;
            }
            Redraw = false;
            Calculate(trackBar1.Value, trackBar2.Value, trackBar3.Value);
        }

        //private void chart1_MouseEnter(object sender, EventArgs e)
        //{

        //}

        //private void chart1_Leave(object sender, EventArgs e)
        //{

        //}
        //private void chart1_MouseWheel(object sender, MouseEventArgs e)
        //{
        //    var chart = (Chart)sender;
        //    var xAxis = chart.ChartAreas[0].AxisX;
        //    var yAxis = chart.ChartAreas[0].AxisY;

        //    try
        //    {
        //        if (e.Delta < 0) // Scrolled down.
        //        {
        //            xAxis.ScaleView.ZoomReset();
        //            yAxis.ScaleView.ZoomReset();
        //        }
        //        else if (e.Delta > 0) // Scrolled up.
        //        {
        //            var xMin = xAxis.ScaleView.ViewMinimum;
        //            var xMax = xAxis.ScaleView.ViewMaximum;
        //            var yMin = yAxis.ScaleView.ViewMinimum;
        //            var yMax = yAxis.ScaleView.ViewMaximum;

        //            var posXStart = xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 4;
        //            var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 4;
        //            var posYStart = yAxis.PixelPositionToValue(e.Location.Y) - (yMax - yMin) / 4;
        //            var posYFinish = yAxis.PixelPositionToValue(e.Location.Y) + (yMax - yMin) / 4;

        //            xAxis.ScaleView.Zoom(posXStart, posXFinish);
        //            yAxis.ScaleView.Zoom(posYStart, posYFinish);
        //        }
        //    }
        //    catch { }
        //}
    }
}
