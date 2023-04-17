using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Controller_Design_2
{
    public partial class MainApp : Form
    {
        private bool _dragging = false;
        private Point _offset;
        private Point _start_point = new Point(0, 0);


        public MainApp()
        {
            InitializeComponent();
        }

        // Global Variables
        int c1_rgb_value;
        int c1_pulse_value;
        int c1_delay_value;

        int c2_rgb_value;
        int c2_pulse_value;
        int c2_delay_value;

        int c3_rgb_value;
        int c3_pulse_value;
        int c3_delay_value;

        int c4_rgb_value;
        int c4_pulse_value;
        int c4_delay_value;

        int c5_rgb_value;
        int c5_pulse_value;
        int c5_delay_value;

        int c6_rgb_value;
        int c6_pulse_value;
        int c6_delay_value;

        int c7_rgb_value;
        int c7_pulse_value;
        int c7_delay_value;

        int c8_rgb_value;
        int c8_pulse_value;
        int c8_delay_value;

        int c9_rgb_value;
        int c9_pulse_value;
        int c9_delay_value;

        int c10_rgb_value;
        int c10_pulse_value;
        int c10_delay_value;

        int c11_rgb_value;
        int c11_pulse_value;
        int c11_delay_value;

        int c12_rgb_value;
        int c12_pulse_value;
        int c12_delay_value;

        int c13_rgb_value;
        int c13_pulse_value;
        int c13_delay_value;

        int c14_rgb_value;
        int c14_pulse_value;
        int c14_delay_value;

        int c15_rgb_value;
        int c15_pulse_value;
        int c15_delay_value;

        int c16_rgb_value;
        int c16_pulse_value;
        int c16_delay_value;

        bool c010203_isGrouped = true;
        bool c040506_isGrouped = true;
        bool c070809_isGrouped = true;
        bool c101112_isGrouped = true;
        bool c131415_isGrouped = true;

        bool c010203_isCurrent = true;
        bool c040506_isCurrent = false;
        bool c070809_isCurrent = false;
        bool c101112_isCurrent = false;
        bool c131415_isCurrent = false;
        bool c160000_isCurrent = false;


        // Global Functions
        public bool checkIntensity(string intensity)
        {
            if (intensity.Length >= 0 && intensity.Length < 5)
            {
                try
                {
                    int value = Convert.ToInt32(intensity);
                    if (value >= 0 && value < 4097) { return true; }
                }
                catch { return false; }
            }
            return false;
        }

        public int getRGB(string intensity)
        {
            double max_val = 4096;
            double current_val = Convert.ToDouble(intensity);

            double ratio = current_val / max_val;
            double rgb_value = Math.Floor(ratio * 255);

            int final_rgb = Convert.ToInt32(rgb_value);

            return final_rgb;
        }


        // {Channel, Position} ; -1 for Not in use
        int[,] position =
        {
                { 1, -1 },
                { 2, -1 },
                { 3, -1 },
                { 4, -1 },
                { 5, -1 },
                { 6, -1 },
                { 7, -1 },
                { 8, -1 },
                { 9, -1 },
                { 10, -1 },
                { 11, -1 },
                { 12, -1 },
                { 13, -1 },
                { 14, -1 },
                { 15, -1 },
                { 16, -1 },
        };
        

        public string getError(int channel, bool error)
        {
            string[] intensityErrors =
            {
                " C1 Invalid Intensity Value",
                ", C2 Invalid Intensity Value",
                ", C3 Invalid Intensity Value",
                ", C4 Invalid Intensity Value",
                ", C5 Invalid Intensity Value",
                ", C6 Invalid Intensity Value",
                ", C7 Invalid Intensity Value",
                ", C8 Invalid Intensity Value",
                ", C9 Invalid Intensity Value",
                ", C10 Invalid Intensity Value",
                ", C11 Invalid Intensity Value",
                ", C12 Invalid Intensity Value",
                ", C13 Invalid Intensity Value",
                ", C14 Invalid Intensity Value",
                ", C15 Invalid Intensity Value",
                ", C16 Invalid Intensity Value",
            };

            if (error)
            {
                position[channel - 1, 1] = 1;
            }
            else
            {
                position[channel - 1, 1] = -1;
            }

            string errorTextStr = "";
            for (int i = 0; i < position.GetLength(0); i++)
            {
                if (position[i, 1] != -1)
                {
                    errorTextStr += intensityErrors[i];
                }
            }

            if (errorTextStr == "")
            {
                errorText.ForeColor = Color.White;
            }

            //if (errorTextStr[0] == ',')
            //{
            //    errorTextStr.Remove(0, 2);
            //}

            return errorTextStr;
        }

        public void groupedLED()
        {
            led1_colour.FillColor = Color.Red;
            led2_colour.FillColor = Color.Lime;
            led3_colour.FillColor = Color.Blue;
        }

        public void ungroupedLED()
        {
            led1_colour.FillColor = Color.White;
            led2_colour.FillColor = Color.White;
            led3_colour.FillColor = Color.White;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainApp_Load(object sender, EventArgs e)
        {
            //ControlExtension.Draggable(MainApp, true);
        }

        private void MainApp_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;
            _start_point = new Point(e.X, e.Y);
        }

        private void MainApp_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this._start_point.X, p.Y - this._start_point.Y);
            }
        }

        private void MainApp_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
        }

        private void chooseGrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chooseGrp.SelectedIndex == 0)
            {
                led1_colour.FillColor = Color.Red;
                led2_colour.FillColor = Color.Lime;
                led3_colour.FillColor = Color.Blue;
            }
            else
            {
                led1_colour.FillColor = Color.White;
                led2_colour.FillColor = Color.White;
                led3_colour.FillColor = Color.White;
            }
        }

        private void led1_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (led1_mode.SelectedIndex == 0) { led1_strobe.Enabled = false; }
            else { led1_strobe.Enabled = true; }
        }

        private void led2_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (led2_mode.SelectedIndex == 0) { led2_strobe.Enabled = false; }
            else { led2_strobe.Enabled = true; }
        }

        private void led3_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (led3_mode.SelectedIndex == 0) { led3_strobe.Enabled = false; }
            else { led3_strobe.Enabled = true; }
        }

        private void clearStrobe_Click(object sender, EventArgs e)
        {
            led1_strobe.SelectedIndex = 0;
            led2_strobe.SelectedIndex = 0; 
            led3_strobe.SelectedIndex = 0;
        }

        private void led1_intensity_TextChanged(object sender, EventArgs e)
        {
            if (checkIntensity(led1_intensity.Text))
            {
                int rgb_value = getRGB(led1_intensity.Text);
                c1_rgb_value = rgb_value;
                //errorText.ForeColor = Color.White;
                errorText.Text = getError(1, false);
            }
            else
            {
                // Set error text
                errorText.ForeColor = Color.Red;
                errorText.Text = getError(1, true);
            };
        }

        private void led2_intensity_TextChanged(object sender, EventArgs e)
        {
            if (checkIntensity(led2_intensity.Text))
            {
                int rgb_value = getRGB(led2_intensity.Text);
                c2_rgb_value = rgb_value;
                //errorText.ForeColor = Color.White;
                errorText.Text = getError(2, false);
            }
            else
            {
                // Set error text
                errorText.ForeColor = Color.Red;
                errorText.Text = getError(2, true);
            };
        }

        private void led3_intensity_TextChanged(object sender, EventArgs e)
        {
            if (checkIntensity(led3_intensity.Text))
            {
                int rgb_value = getRGB(led3_intensity.Text);
                c3_rgb_value = rgb_value;
                //errorText.ForeColor = Color.White;
                errorText.Text = getError(3, false);
            }
            else
            {
                // Set error text
                errorText.ForeColor = Color.Red;
                errorText.Text = getError(3, true);
            };
        }

        private void selectGrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectGrp.SelectedIndex == 0)
            {
                c010203_isCurrent = true;

                // Change Visuals
                led1_channel.Text = "Channel 1";
                led2_channel.Text = "Channel 2";
                led3_channel.Text = "Channel 3";
                if (c010203_isGrouped) { groupedLED(); }
                else { ungroupedLED(); }
            }
            else if (selectGrp.SelectedIndex == 1)
            {
                c040506_isCurrent = true;

                // Change Visuals
                led1_channel.Text = "Channel 4";
                led2_channel.Text = "Channel 5";
                led3_channel.Text = "Channel 6";
                if (c040506_isGrouped) { groupedLED(); }
                else { ungroupedLED(); }
            }
            else if (selectGrp.SelectedIndex == 2)
            {
                c070809_isCurrent = true;

                // Change Visuals
                led1_channel.Text = "Channel 7";
                led2_channel.Text = "Channel 8";
                led3_channel.Text = "Channel 9";
                if (c070809_isGrouped) { groupedLED(); }
                else { ungroupedLED(); }
            }
            else if (selectGrp.SelectedIndex == 3)
            {
                c101112_isCurrent = true;

                // Change Visuals
                led1_channel.Text = "Channel 10";
                led2_channel.Text = "Channel 11";
                led3_channel.Text = "Channel 12";
                if (c101112_isGrouped) { groupedLED(); }
                else { ungroupedLED(); }

            }
            else if (selectGrp.SelectedIndex == 4)
            {
                c131415_isCurrent = true;

                // Change Visuals
                led1_channel.Text = "Channel 13";
                led2_channel.Text = "Channel 14";
                led3_channel.Text = "Channel 15";
                if (c131415_isGrouped) { groupedLED(); }
                else { ungroupedLED(); }
            }
            else
            {
                c160000_isCurrent = true;

                // Change Visuals
                led1_channel.Text = "Channel 16";
                
            }
        }
    }
}
