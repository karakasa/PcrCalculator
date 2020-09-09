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
            AirportDataset.LoadData();

            PopulateAirportNames();
            PopulateEntryAndDestinations();
            PopulateAirlineList();
            comboBox10.SelectedIndex = 0;

            UpdateTransfer();
        }

        private void PopulateAirportNames()
        {
            comboBox2.Items.Add("其他");
            comboBox3.Items.Add("其他");
            comboBox4.Items.Add("其他");

            comboBox2.Items.AddRange(AirportDataset.AirportNames);
            comboBox3.Items.AddRange(AirportDataset.AirportNames);
            comboBox4.Items.AddRange(AirportDataset.AirportNames);

            comboBox2.SelectedIndex = comboBox3.SelectedIndex = comboBox4.SelectedIndex = 0;
        }
        private void UpdateTransfer()
        {
            checkBox2.Enabled = dateTimePicker2.Enabled = dateTimePicker4.Enabled = comboBox8.Enabled = comboBox3.Enabled = checkBox1.Checked;
            dateTimePicker3.Enabled = comboBox4.Enabled = dateTimePicker5.Enabled = comboBox9.Enabled = checkBox2.Checked && checkBox2.Enabled;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTransfer();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTransfer();
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
            var list = Enumerable.Repeat("其他", 1).Concat(AirlineDataset.InterlineData.Select(it => $"{it.Code} {it.FriendlyName}")).ToArray();
            comboBox7.Items.AddRange(list);
            comboBox8.Items.AddRange(list);
            comboBox9.Items.AddRange(list);
            comboBox7.SelectedIndex = comboBox8.SelectedIndex = comboBox9.SelectedIndex = 0;

        }

        private void 显示结果_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            var trip = new TripCheck();
            LoadIntoTripCheck(trip);
            trip.Check();
            SetState(trip.Status);
            textBox1.Text = string.Join("\r\n", trip.Messages);
        }

        private void LoadIntoTripCheck(TripCheck trip)
        {
            trip.BaggageCount = comboBox10.SelectedIndex;
            trip.EntryPoint = comboBox5.SelectedItem.ToString();
            trip.FinalDestination = comboBox6.SelectedItem.ToString();

            AddSegment(trip, comboBox2, comboBox7, dateTimePicker1, dateTimePicker4);
            if (checkBox1.Checked)
            {
                AddSegment(trip, comboBox3, comboBox8, dateTimePicker2, dateTimePicker5);
                if (checkBox2.Checked)
                {
                    AddSegment(trip, comboBox4, comboBox9, dateTimePicker3, dateTimePicker6);
                }
            }
        }

        private void AddSegment(TripCheck trip, ComboBox departAirport, ComboBox airline, DateTimePicker depart, DateTimePicker arrival)
        {
            trip.Segments.Add(new Segment()
            {
                DepartureAirport = departAirport.SelectedIndex == 0 ? "?" : AirportDataset.Airports[departAirport.SelectedIndex - 1].Code,
                AirlineCode = airline.SelectedIndex == 0 ? "??" : AirlineDataset.InterlineData[airline.SelectedIndex - 1].Code,
                LocalDepartureTime = depart.Value,
                LocalArrivalTime = arrival.Value
            });
        }

        private void SetState(RouteStatus status)
        {
            switch (status)
            {
                case RouteStatus.Pass:
                    label10.BackColor = Color.LawnGreen;
                    label10.ForeColor = Color.Black;
                    label10.Text = "可以成行";
                    break;
                case RouteStatus.AdditionalDocument:
                    label10.BackColor = Color.SkyBlue;
                    label10.ForeColor = Color.Black;
                    label10.Text = "需要额外旅行文件";
                    break;
                case RouteStatus.Suspicious:
                    label10.BackColor = Color.Yellow;
                    label10.ForeColor = Color.Black;
                    label10.Text = "可疑";
                    break;
                case RouteStatus.Invalid:
                    label10.BackColor = Color.Red;
                    label10.ForeColor = Color.White;
                    label10.Text = "无法成行";
                    break;
            }
        }
    }
}
