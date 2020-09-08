using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PcrCalculator
{
    public partial class FormMain : Form
    {
        private TimeZoneInfo[] _timezones = TimeZoneInfo.GetSystemTimeZones().ToArray();
        public FormMain()
        {
            InitializeComponent();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateTimezones();
            PopulateEntryAndDestinations();
            PopulateAirlineList();
            comboBox10.SelectedIndex = 0;

            UpdateTransfer();
        }


        private void UpdateTransfer()
        {
            if (checkBox1.Checked)
            {
                checkBox2.Enabled = dateTimePicker2.Enabled = dateTimePicker5.Enabled = comboBox8.Enabled = comboBox3.Enabled = true;
            }
            else
            {
                checkBox2.Enabled = dateTimePicker2.Enabled = dateTimePicker5.Enabled = comboBox8.Enabled = comboBox3.Enabled = false;
            }

            dateTimePicker3.Enabled = comboBox4.Enabled = dateTimePicker6.Enabled = comboBox9.Enabled = checkBox2.Checked && checkBox2.Enabled;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTransfer();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTransfer();
        }

        private void PopulateTimezones()
        {
            foreach (var it in _timezones)
            {
                var index = comboBox1.Items.Add(it.DisplayName);
                if (it.StandardName == TimeZoneInfo.Local.StandardName)
                {
                    comboBox1.SelectedIndex = index;
                }
            }
        }

        private void PopulateEntryAndDestinations()
        {
            comboBox5.Items.AddRange(DomesticDataset.EntryPoints);

            comboBox5.SelectedIndex = 0;

                comboBox6.Items.AddRange(DomesticDataset.FinalDestinations);

            comboBox6.SelectedIndex = 0;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PopulateAirlineList()
        {
            var list = Enumerable.Repeat("其他", 1).Concat(AirlineDataset.InterlineData.Select(it => $"{it.FriendlyName} ({it.Code})")).ToArray();
            comboBox7.Items.AddRange(list);
            comboBox8.Items.AddRange(list);
            comboBox9.Items.AddRange(list);
            comboBox7.SelectedIndex = comboBox8.SelectedIndex = comboBox9.SelectedIndex = 0;

        }
    }
}
