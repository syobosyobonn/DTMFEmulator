namespace DTMFEmulator
{
    public partial class Setting : Form
    {
        private readonly Dictionary<char, (string Text, decimal[] freqs)> DefaultMap = new()
        {
            { '1', ( "1", [697, 1209] ) },
            { '2', ( "2", [697, 1336] ) },
            { '3', ( "3", [697, 1477] ) },
            { '4', ( "4", [770, 1209] ) },
            { '5', ( "5", [770, 1336] ) },
            { '6', ( "6", [770, 1477] ) },
            { '7', ( "7", [852, 1209] ) },
            { '8', ( "8", [852, 1336] ) },
            { '9', ( "9", [852, 1477] ) },
            { 'S', ( "*", [941, 1209] ) },
            { '0', ( "0", [941, 1336] ) },
            { 'H', ( "#", [941, 1477] ) },
            { 'A', ( "A", [697, 1633] ) },
            { 'B', ( "B", [770, 1633] )},
            { 'C', ( "C", [852, 1633] ) },
            { 'D', ( "D", [941, 1633] ) },
            { 'n', ( "Ding", [380, 400] ) },
            { 's', ( "Disconnected", [400, 400] ) }
        };

        public Setting()
        {
            InitializeComponent();

            TPListBox.SelectedIndex = 0;
        }

        private async void TextBox_TextChanged(object sender, EventArgs e)
        {
            await Program.main.UpdateState(true, false);
            NonSelectioningTP();
        }
        private async void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            await Program.main.UpdateState(false, true);
            NonSelectioningTP();
        }

        private async void ResetSounds(object sender, EventArgs e) => await Program.main.SetVolume();
        private async void DrDtWthNumericUpDown_ValueChanged(object sender, EventArgs e) => await Program.graph.UpdateDataWidth();

        private async void TPListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TPListBox.SelectedIndex == 0)
            {
                foreach (KeyValuePair<char, (string Text, decimal[] freqs)> dicItem in DefaultMap)
                {
                    await ChangeValues(dicItem.Key, true, dicItem.Value.Text, dicItem.Value.freqs);
                }

                TPRichTextBox.Text =
                    "通常の電話機。\n" +
                    "\n" +
                    "■参考\n" +
                    "Wikipedia\n" +
                    "https://ja.wikipedia.org/wiki/DTMF";
            }
            else if (TPListBox.SelectedIndex == 1)
            {
                foreach (KeyValuePair<char, (string Text, decimal[] _freqs)> dicItem in DefaultMap)
                {
                    await ChangeValues(dicItem.Key, true, dicItem.Value.Text, [500, 500]);
                }

                TPRichTextBox.Text =
                    "全て500Hz。";
            }
            else if (TPListBox.SelectedIndex == 2)
            {
                foreach (KeyValuePair<char, (string Text, decimal[] _freqs)> dicItem in DefaultMap)
                {
                    await ChangeValues(dicItem.Key, true, dicItem.Value.Text, [1000, 1000]);
                }

                TPRichTextBox.Text =
                    "全て1000Hz。";
            }
            else if (TPListBox.SelectedIndex == 3)
            {
                foreach (KeyValuePair<char, (string Text, decimal[] _freqs)> dicItem in DefaultMap)
                {
                    await ChangeValues(dicItem.Key, true, dicItem.Value.Text, [2000, 2000]);
                }

                TPRichTextBox.Text =
                    "全て2000Hz。";
            }
            else if (TPListBox.SelectedIndex == 4)
            {
                foreach (KeyValuePair<char, (string Text, decimal[] _freqs)> dicItem in DefaultMap)
                {
                    await ChangeValues(dicItem.Key, false, dicItem.Value.Text, [1000, 1000]);
                }

                TPRichTextBox.Text =
                    "全て非表示。(1000Hz)";
            }
            else if (TPListBox.SelectedIndex == 5)
            {
                if (!Program.main.AcceptedAqdnd)
                {
                    if (MessageBox.Show("アクアリウムは踊らない、及び同作品の黒電話について理解してますか?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                        return;
                    if (MessageBox.Show("本当に?", "", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;

                    Program.main.AcceptedAqdnd = true;
                }

                Dictionary<char, (bool visible, string text, decimal[] freqs)> AqodMap = new()
                {
                    { '1', ( true, "1", [620, 890] ) },
                    { '2', ( true, "2", [1050, 1450] ) },
                    { '3', ( true, "3", [700, 1000] ) },
                    { '4', ( true, "4", [850, 1200] ) },
                    { '5', ( true, "5", [800, 1100] ) },
                    { '6', ( true, "6", [850, 1200] ) },
                    { '7', ( true, "7", [1110, 1600] ) },
                    { '8', ( true, "8", [950, 1350] ) },
                    { '9', ( true, "9", [800, 1200] ) },
                    { 'S', ( true, "*", [1100, 1500] ) },
                    { '0', ( true, "0", [1000, 1500] ) },
                    { 'H', ( true, "#", [750, 1050] ) },
                    { 'A', ( false, "A", [1000, 1000] ) },
                    { 'B', ( false, "B", [1000, 1000] ) },
                    { 'C', ( false, "C", [1000, 1000] ) },
                    { 'D', ( false, "D", [1000, 1000] ) },
                    { 'n', ( false, "Ding", [1000, 1000] ) },
                    { 's', ( false, "Disconnected", [1000, 1000] ) }
                };

                foreach (KeyValuePair<char, (bool visible, string text, decimal[] freqs)> dicItem in AqodMap)
                {
                    await ChangeValues(dicItem.Key, dicItem.Value.visible, dicItem.Value.text, dicItem.Value.freqs);
                }

                TPRichTextBox.Text =
                    "アクアリウムは踊らないより\n" +
                    "\"黒電話の問題\"に登場する\n" +
                    "電話の音を再現。\n" +
                    "耳コピのため正確性に欠けます。\n" +
                    "\n" +
                    "🐟️原作\n" +
                    "アクアリウムは踊らない\n" +
                    "The Aquarium does not dance\n" +
                    "https://daidai7742.wixsite.com/aqua-dance";

                AqodCheckBox.Checked = true;
                AqodCheckBox.Visible = true;
            }
        }

        private void NonSelectioningTP()
        {
            TPListBox.ClearSelected();
            TPRichTextBox.Text = string.Empty;
        }

        private Task ChangeValues(char Mode, bool Checked, string Letter, decimal[] Freqs)
        {
            Dictionary<char, (CheckBox CheckBox, TextBox TextBox, NumericUpDown Tone1, NumericUpDown Tone2)> Map = new()
            {
                { '1', (CheckBox1, TextBox1, ToneNumericUpDown1_1, ToneNumericUpDown1_2) },
                { '2', (CheckBox2, TextBox2, ToneNumericUpDown2_1, ToneNumericUpDown2_2) },
                { '3', (CheckBox3, TextBox3, ToneNumericUpDown3_1, ToneNumericUpDown3_2) },
                { '4', (CheckBox4, TextBox4, ToneNumericUpDown4_1, ToneNumericUpDown4_2) },
                { '5', (CheckBox5, TextBox5, ToneNumericUpDown5_1, ToneNumericUpDown5_2) },
                { '6', (CheckBox6, TextBox6, ToneNumericUpDown6_1, ToneNumericUpDown6_2) },
                { '7', (CheckBox7, TextBox7, ToneNumericUpDown7_1, ToneNumericUpDown7_2) },
                { '8', (CheckBox8, TextBox8, ToneNumericUpDown8_1, ToneNumericUpDown8_2) },
                { '9', (CheckBox9, TextBox9, ToneNumericUpDown9_1, ToneNumericUpDown9_2) },
                { 'S', (CheckBoxStar, TextBoxStar, ToneNumericUpDownStar_1, ToneNumericUpDownStar_2) },
                { '0', (CheckBox0, TextBox0, ToneNumericUpDown0_1, ToneNumericUpDown0_2) },
                { 'H', (CheckBoxHash, TextBoxHash, ToneNumericUpDownHash_1, ToneNumericUpDownHash_2) },
                { 'A', (CheckBoxA, TextBoxA, ToneNumericUpDownA_1, ToneNumericUpDownA_2) },
                { 'B', (CheckBoxB, TextBoxB, ToneNumericUpDownB_1, ToneNumericUpDownB_2) },
                { 'C', (CheckBoxC, TextBoxC, ToneNumericUpDownC_1, ToneNumericUpDownC_2) },
                { 'D', (CheckBoxD, TextBoxD, ToneNumericUpDownD_1, ToneNumericUpDownD_2) },
                { 'n', (CheckBoxDin, null!, ToneNumericUpDownDin_1, ToneNumericUpDownDin_2) },
                { 's', (CheckBoxDis, null!, ToneNumericUpDownDis_1, ToneNumericUpDownDis_2) }
            };

            if (Map.TryGetValue(Mode, out var controls))
            {
                if (controls.CheckBox != null)
                    controls.CheckBox.Checked = Checked;

                if (controls.TextBox != null)
                    controls.TextBox.Text = Letter;

                if (controls.Tone1 != null && controls.Tone2 != null)
                {
                    controls.Tone1.Value = Freqs[0];
                    controls.Tone2.Value = Freqs[1];
                }
            }

            return Task.CompletedTask;
        }

        private void AqodCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (AqodCheckBox.Checked == true)
            {
                bool Dance = false;
                Program.main.TheAquariumDoesNot(Dance); //アクアリウムは踊らない
            }
            else
            {
                Program.main.TheAquariumDoesNot(true);

                AqodCheckBox.Visible = false;
            }
        }
    }
}
