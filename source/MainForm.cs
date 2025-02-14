using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DTMFEmulator
{
    public partial class MainForm : Form
    {
        public WaveOutEvent outputDevice = null!;
        public SignalGenerator tone1Generator = null!;
        public SignalGenerator tone2Generator = null!;
        public bool isMouseDown = false;

        private static Dictionary<Button, (CheckBox checkBox, TextBox textBox, NumericUpDown Tone1, NumericUpDown Tone2)>? Map;

        public bool AcceptedAqdnd = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeAudio(1);

            outputDevice.Play();
            Thread.Sleep(10);
            outputDevice.Stop();

            InitializeAudio(50);
        }

        public Task InitializeAudio(int volume)
        {
            outputDevice = new();
            var waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 1);

            tone1Generator = new(44100, 1) { Frequency = 697, Type = SignalGeneratorType.Sin };
            tone2Generator = new(44100, 1) { Frequency = 1209, Type = SignalGeneratorType.Sin };

            MixingSampleProvider mixer = new(waveFormat);
            mixer.AddMixerInput(tone1Generator);
            mixer.AddMixerInput(tone2Generator);

            VolumeSampleProvider volumeProvider = new(mixer)
            {
                Volume = volume / 100.0f
            };

            outputDevice.Init(volumeProvider);

            return Task.CompletedTask;
        }

        public async Task SetVolume()
        {
            await InitializeAudio((int)Program.setting.VlNumericUpDown.Value);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Map = new()
            {
                { Button1, (Program.setting.CheckBox1, Program.setting.TextBox1, Program.setting.ToneNumericUpDown1_1, Program.setting.ToneNumericUpDown1_2) },
                { Button2, (Program.setting.CheckBox2, Program.setting.TextBox2, Program.setting.ToneNumericUpDown2_1, Program.setting.ToneNumericUpDown2_2) },
                { Button3, (Program.setting.CheckBox3, Program.setting.TextBox3, Program.setting.ToneNumericUpDown3_1, Program.setting.ToneNumericUpDown3_2) },
                { Button4, (Program.setting.CheckBox4, Program.setting.TextBox4, Program.setting.ToneNumericUpDown4_1, Program.setting.ToneNumericUpDown4_2) },
                { Button5, (Program.setting.CheckBox5, Program.setting.TextBox5, Program.setting.ToneNumericUpDown5_1, Program.setting.ToneNumericUpDown5_2) },
                { Button6, (Program.setting.CheckBox6, Program.setting.TextBox6, Program.setting.ToneNumericUpDown6_1, Program.setting.ToneNumericUpDown6_2) },
                { Button7, (Program.setting.CheckBox7, Program.setting.TextBox7, Program.setting.ToneNumericUpDown7_1, Program.setting.ToneNumericUpDown7_2) },
                { Button8, (Program.setting.CheckBox8, Program.setting.TextBox8, Program.setting.ToneNumericUpDown8_1, Program.setting.ToneNumericUpDown8_2) },
                { Button9, (Program.setting.CheckBox9, Program.setting.TextBox9, Program.setting.ToneNumericUpDown9_1, Program.setting.ToneNumericUpDown9_2) },
                { ButtonStar, (Program.setting.CheckBoxStar, Program.setting.TextBoxStar, Program.setting.ToneNumericUpDownStar_1, Program.setting.ToneNumericUpDownStar_2) },
                { Button0, (Program.setting.CheckBox0, Program.setting.TextBox0, Program.setting.ToneNumericUpDown0_1, Program.setting.ToneNumericUpDown0_2) },
                { ButtonHash, (Program.setting.CheckBoxHash, Program.setting.TextBoxHash, Program.setting.ToneNumericUpDownHash_1, Program.setting.ToneNumericUpDownHash_2) },
                { ButtonA, (Program.setting.CheckBoxA, Program.setting.TextBoxA, Program.setting.ToneNumericUpDownA_1, Program.setting.ToneNumericUpDownA_2) },
                { ButtonB, (Program.setting.CheckBoxB, Program.setting.TextBoxB, Program.setting.ToneNumericUpDownB_1, Program.setting.ToneNumericUpDownB_2) },
                { ButtonC, (Program.setting.CheckBoxC, Program.setting.TextBoxC, Program.setting.ToneNumericUpDownC_1, Program.setting.ToneNumericUpDownC_2) },
                { ButtonD, (Program.setting.CheckBoxD, Program.setting.TextBoxD, Program.setting.ToneNumericUpDownD_1, Program.setting.ToneNumericUpDownD_2) },
                { DingButton, (Program.setting.CheckBoxDin, null!, Program.setting.ToneNumericUpDownDin_1, Program.setting.ToneNumericUpDownDin_2) },
                { DisconnectedButton, (Program.setting.CheckBoxDis, null!, Program.setting.ToneNumericUpDownDis_1, Program.setting.ToneNumericUpDownDis_2) }
            };


            Program.graph.Show(this);
            Program.setting.Show(this);
            BringToFront();
        }

        private async void PlayTone(double tone1Freq, double tone2Freq)
        {
            tone1Generator.Frequency = tone1Freq;
            tone2Generator.Frequency = tone2Freq;

            outputDevice.Play();

            if (!Program.graph.IsDisposed) { await Task.Run(() => Program.graph.Draw((int)tone1Freq, (int)tone2Freq)); }
        }

        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            if (sender is Button btn)
            {
                PlayButtonTone(btn);
            }
        }

        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            outputDevice.Stop();
        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown && sender is Button btn)
            {
                PlayButtonTone(btn);
            }
        }

        private void PlayButtonTone(Button btn)
        {
            if (Map.TryGetValue(btn, out var tones))
            {
                PlayTone((double)tones.Tone1.Value, (double)tones.Tone2.Value);
            }
        }


        public Task UpdateState(bool IsBtnText, bool IsBtnVisible)
        {
            if (IsBtnText)
            {
                foreach (Control control in this.Controls)
                {
                    if (control is Button btn)
                    {
                        if (Map.TryGetValue(btn, out var map))
                        {
                            btn.Text = map.textBox.Text;
                        }
                    }
                }
            }

            if (IsBtnVisible)
            {
                foreach (Control control in Controls)
                {
                    if (control is Button btn)
                    {
                        if (Map.TryGetValue(btn, out var map))
                        {
                            btn.Visible = map.checkBox.Checked;
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }

        public void TheAquariumDoesNot(bool Dance)  //ƒAƒNƒAƒŠƒEƒ€‚Í—x‚ç‚È‚¢
        {
            if (Dance == false)                     //—x‚ç‚È‚¢
            {
                int value = 1;
                _ = DwmSetWindowAttribute(Handle, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref value, (uint)Marshal.SizeOf<int>());

                BackColor = Color.Black;
                ForeColor = Color.White;

                Size = new(241, 338);
                MaximumSize = new(241, 338);

                foreach (Control control in Controls)
                {
                    if (control is Button btn)
                    {
                        btn.BackColor = Color.Black;
                        btn.ForeColor = Color.FromArgb(218, 218, 252);

                        btn.Font = new(new FontFamily(GenericFontFamilies.Serif), 25, FontStyle.Italic);

                        btn.FlatStyle = FlatStyle.Flat;
                        btn.FlatAppearance.BorderSize = 0;
                        btn.FlatAppearance.BorderColor = btn.BackColor;
                        btn.FlatAppearance.CheckedBackColor = btn.BackColor;
                        btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
                        btn.FlatAppearance.MouseOverBackColor = btn.BackColor;

                        btn.Cursor = Cursors.Hand;
                    }
                }
            }
            else
            {
                int value = 0;
                _ = DwmSetWindowAttribute(Handle, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref value, (uint)Marshal.SizeOf<int>());

                BackColor = SystemColors.Window;
                ForeColor = SystemColors.WindowText;

                MaximumSize = new Size(316, 363);

                foreach (Control control in Controls)
                {
                    if (control is Button btn)
                    {
                        btn.BackColor = Color.Transparent;
                        btn.ForeColor = SystemColors.WindowText;

                        btn.Font = new(new FontFamily("Yu Gothic UI"), 9);

                        btn.FlatStyle = FlatStyle.Standard;

                        btn.Cursor = Cursors.Default;
                    }
                }
            }
        }

        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, ref int pvAttribute, uint cbAttribute);
        private enum DWMWINDOWATTRIBUTE
        {
            DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
            DWMWA_WINDOW_CORNER_PREFERENCE = 33,
            DWMWA_CAPTION_COLOR = 35,
            DWMWA_MICA_EFFECT = 1029,
            DWMWA_LAST
        }
    }
}
