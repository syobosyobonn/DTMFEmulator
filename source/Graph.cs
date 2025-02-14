namespace DTMFEmulator
{
    public partial class Graph : Form
    {
        private int tone1Freq, tone2Freq;

        private static int DataWidth = 900;

        private byte[] CombinedData = new byte[DataWidth * 2];
        private byte[] Tone1Data = new byte[DataWidth * 2];
        private byte[] Tone2Data = new byte[DataWidth * 2];

        private int ImgWidth;
        private int ImgHeight;

        private Bitmap bitmap = new(710, 150);
        private Graphics? g;

        private Point[] Tone1Points = new Point[DataWidth];
        private Point[] Tone2Points = new Point[DataWidth];
        private Point[] CombinedPoints = new Point[DataWidth];

        private static readonly Pen RedPen = new(Color.Red, 1);
        private static readonly Pen BluePen = new(Color.Blue, 1);
        private static readonly Pen GreenPen = new(Color.Green, 2);

        public Graph()
        {
            InitializeComponent();
        }

        private void Graph_Shown(object sender, EventArgs e)
        {
            Invoke(delegate
            {
                g = Graphics.FromImage(bitmap);
                DrawingWave();
            });
        }

        private async void Graph_Resize(object? sender, EventArgs e)
        {
            await Task.Run(delegate
            {
                Invoke(DrawingWave);
            });
        }

        public async Task Draw(int inp1, int inp2)
        {
            if (inp1 != tone1Freq || inp2 != tone2Freq)
            {
                tone1Freq = inp1;
                tone2Freq = inp2;

                CalcWave();
                await DrawingWave();
            }
        }

        private void CalcWave()
        {
            for (int i = 0; i < DataWidth; i++)
            {
                short sample1 = (short)(Math.Sin(2 * Math.PI * tone1Freq * i / 44100) * short.MaxValue);
                short sample2 = (short)(Math.Sin(2 * Math.PI * tone2Freq * i / 44100) * short.MaxValue);

                Tone1Data[i * 2] = (byte)(sample1 & 0xFF);
                Tone1Data[i * 2 + 1] = (byte)((sample1 >> 8) & 0xFF);

                Tone2Data[i * 2] = (byte)(sample2 & 0xFF);
                Tone2Data[i * 2 + 1] = (byte)((sample2 >> 8) & 0xFF);

                short CombinedSample = (short)((sample1 + sample2) / 2);

                CombinedData[i * 2] = (byte)(CombinedSample & 0xFF);
                CombinedData[i * 2 + 1] = (byte)((CombinedSample >> 8) & 0xFF);
            }

            GC.Collect();
        }

        private Task DrawingWave()
        {
            ImgWidth = GraphPictureBox.Width;
            ImgHeight = GraphPictureBox.Height;

            bitmap = new(ImgWidth, ImgHeight);
            g = Graphics.FromImage(bitmap);

            g.Clear(BackColor);

            int amplitude = ImgHeight / 2;

            for (int i = 0; i < DataWidth; i++)
            {
                short sample1 = (short)(Tone1Data[i * 2] | (Tone1Data[i * 2 + 1] << 8));
                short sample2 = (short)(Tone2Data[i * 2] | (Tone2Data[i * 2 + 1] << 8));
                short CombinedSample = (short)(CombinedData[i * 2] | (CombinedData[i * 2 + 1] << 8));

                int x = (int)((i / (float)(DataWidth)) * ImgWidth);

                int y1 = (int)((sample1 / (float)short.MaxValue) * amplitude);
                int y2 = (int)((sample2 / (float)short.MaxValue) * amplitude);
                int yCombined = (int)((CombinedSample / (float)short.MaxValue) * amplitude);

                Tone1Points[i] = new Point(x, amplitude - y1);
                Tone2Points[i] = new Point(x, amplitude - y2);
                CombinedPoints[i] = new Point(x, amplitude - yCombined);
            }

            g.DrawLines(RedPen, Tone1Points);
            g.DrawLines(BluePen, Tone2Points);
            g.DrawLines(GreenPen, CombinedPoints);

            GraphPictureBox.Image = bitmap;

            GC.Collect();

            return Task.CompletedTask;
        }

        public Task UpdateDataWidth()
        {
            DataWidth = (int)Program.setting.DrDtWthNumericUpDown.Value;

            CombinedData = new byte[DataWidth * 2];
            Tone1Data = new byte[DataWidth * 2];
            Tone2Data = new byte[DataWidth * 2];

            Tone1Points = new Point[DataWidth];
            Tone2Points = new Point[DataWidth];
            CombinedPoints = new Point[DataWidth];

            CalcWave();
            DrawingWave();

            return Task.CompletedTask;
        }
    }
}
