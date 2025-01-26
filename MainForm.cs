using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace DTMFEmulator
{
    public partial class MainForm : Form
    {
        private WaveOutEvent outputDevice = null!;
        private SignalGenerator tone1Generator = null!;
        private SignalGenerator tone2Generator = null!;
        private bool isMouseDown = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeAudio();
        }

        private void InitializeAudio()
        {
            outputDevice = new();
            var waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 1);

            tone1Generator = new(44100, 1) { Frequency = 697, Type = SignalGeneratorType.Sin };
            tone2Generator = new(44100, 1) { Frequency = 1209, Type = SignalGeneratorType.Sin };

            MixingSampleProvider mixer = new(waveFormat);
            mixer.AddMixerInput(tone1Generator);
            mixer.AddMixerInput(tone2Generator);

            outputDevice.Init(mixer);
        }

        private void PlayTone(double tone1Freq, double tone2Freq)
        {
            tone1Generator.Frequency = tone1Freq;
            tone2Generator.Frequency = tone2Freq;
            outputDevice.Play();
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
            switch (btn.Name)
            {
                case nameof(Button1):
                    PlayTone(697, 1209); break;
                case nameof(Button2):
                    PlayTone(697, 1336); break;
                case nameof(Button3):
                    PlayTone(697, 1477); break;
                case nameof(Button4):
                    PlayTone(770, 1209); break;
                case nameof(Button5):
                    PlayTone(770, 1336); break;
                case nameof(Button6):
                    PlayTone(770, 1477); break;
                case nameof(Button7):
                    PlayTone(852, 1209); break;
                case nameof(Button8):
                    PlayTone(852, 1336); break;
                case nameof(Button9):
                    PlayTone(852, 1477); break;
                case nameof(Button0):
                    PlayTone(941, 1336); break;
                case nameof(ButtonStar):
                    PlayTone(941, 1209); break;
                case nameof(ButtonHash):
                    PlayTone(941, 1477); break;
                case nameof(CustomToneButton):
                    PlayTone((double)ToneNumericUpDown1.Value, (double)ToneNumericUpDown2.Value); break;
            }
        }
    }
}
