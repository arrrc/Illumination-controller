using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;
using System.IO;
using System.Windows.Forms.VisualStyles;
using System.Management;

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
        int c1_rgb_value = 0;
        int c2_rgb_value = 0;
        int c3_rgb_value = 0;
        int c4_rgb_value = 0;
        int c5_rgb_value = 0;
        int c6_rgb_value = 0;
        int c7_rgb_value = 0;
        int c8_rgb_value = 0;
        int c9_rgb_value = 0;
        int c10_rgb_value = 0;
        int c11_rgb_value = 0;
        int c12_rgb_value = 0;
        int c13_rgb_value = 0;
        int c14_rgb_value = 0;
        int c15_rgb_value = 0;
        int c16_rgb_value = 0;

        int c1_edge_value = 0;
        int c2_edge_value = 0;
        int c3_edge_value = 0;
        int c4_edge_value = 0;
        int c5_edge_value = 0;
        int c6_edge_value = 0;
        int c7_edge_value = 0;
        int c8_edge_value = 0;
        int c9_edge_value = 0;
        int c10_edge_value = 0;
        int c11_edge_value = 0;
        int c12_edge_value = 0;
        int c13_edge_value = 0;
        int c14_edge_value = 0;
        int c15_edge_value = 0;
        int c16_edge_value = 0;

        int c1_pulse_value = 0;
        int c2_pulse_value = 0;
        int c3_pulse_value = 0;
        int c4_pulse_value = 0;
        int c5_pulse_value = 0;
        int c6_pulse_value = 0;
        int c7_pulse_value = 0;
        int c8_pulse_value = 0;
        int c9_pulse_value = 0;
        int c10_pulse_value = 0;
        int c11_pulse_value = 0;
        int c12_pulse_value = 0;
        int c13_pulse_value = 0;
        int c14_pulse_value = 0;
        int c15_pulse_value = 0;
        int c16_pulse_value = 0;

        int c1_delay_value = 0;
        int c2_delay_value = 0;
        int c3_delay_value = 0;
        int c4_delay_value = 0;
        int c5_delay_value = 0;
        int c6_delay_value = 0;
        int c7_delay_value = 0;
        int c8_delay_value = 0;
        int c9_delay_value = 0;
        int c10_delay_value = 0;
        int c11_delay_value = 0;
        int c12_delay_value = 0;
        int c13_delay_value = 0;
        int c14_delay_value = 0;
        int c15_delay_value = 0;
        int c16_delay_value = 0;

        string c1_mode_value = "Static";
        string c2_mode_value = "Static";
        string c3_mode_value = "Static";
        string c4_mode_value = "Static";
        string c5_mode_value = "Static";
        string c6_mode_value = "Static";
        string c7_mode_value = "Static";
        string c8_mode_value = "Static";
        string c9_mode_value = "Static";
        string c10_mode_value = "Static";
        string c11_mode_value = "Static";
        string c12_mode_value = "Static";
        string c13_mode_value = "Static";
        string c14_mode_value = "Static";
        string c15_mode_value = "Static";
        string c16_mode_value = "Static";

        string c1_strobe_value = "None";
        string c2_strobe_value = "None";
        string c3_strobe_value = "None";
        string c4_strobe_value = "None";
        string c5_strobe_value = "None";
        string c6_strobe_value = "None";
        string c7_strobe_value = "None";
        string c8_strobe_value = "None";
        string c9_strobe_value = "None";
        string c10_strobe_value = "None";
        string c11_strobe_value = "None";
        string c12_strobe_value = "None";
        string c13_strobe_value = "None";
        string c14_strobe_value = "None";
        string c15_strobe_value = "None";
        string c16_strobe_value = "None";

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

        int currentStrobe = 1;
        
        int led1_testStop;
        int led2_testStop;
        int led3_testStop;

        int led1_rgb;
        int led2_rgb;
        int led3_rgb;
        int led1_pulse;
        int led2_pulse;
        int led3_pulse;
        int led1_delay;
        int led2_delay;
        int led3_delay;

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

        public string getEdge(int value)
        {
            if (value == 1) { return "Rising"; }
            else if (value == 1) { return "Falling"; }
            else { return "None"; }
        }

        public bool checkStrobeValue(string value)
        {
            if (value.Length >= 0 && value.Length < 6)
            {
                try
                {
                    int strobe_value = Convert.ToInt32(value);
                    if (strobe_value >= 0 && strobe_value < 65537) { return true; }
                }
                catch { return false; }
            }
            return false;
        }

        public void setBlinkValues()
        {
            if (c010203_isCurrent) 
            {
                led1_rgb = getRGB(Convert.ToString(c1_rgb_value));
                led2_rgb = getRGB(Convert.ToString(c2_rgb_value));
                led3_rgb = getRGB(Convert.ToString(c3_rgb_value));

                led1_pulse = c1_pulse_value;
                led2_pulse = c2_pulse_value;
                led3_pulse = c3_pulse_value;

                led1_delay = c1_delay_value;
                led2_delay = c2_delay_value;
                led3_delay = c3_delay_value;
            }
            else if (c040506_isCurrent)
            {
                led1_rgb = getRGB(Convert.ToString(c4_rgb_value));
                led2_rgb = getRGB(Convert.ToString(c5_rgb_value));
                led3_rgb = getRGB(Convert.ToString(c6_rgb_value));

                led1_pulse = c4_pulse_value;
                led2_pulse = c5_pulse_value;
                led3_pulse = c6_pulse_value;

                led1_delay = c4_delay_value;
                led2_delay = c5_delay_value;
                led3_delay = c6_delay_value;
            }
            else if (c070809_isCurrent)
            {
                led1_rgb = getRGB(Convert.ToString(c7_rgb_value));
                led2_rgb = getRGB(Convert.ToString(c8_rgb_value));
                led3_rgb = getRGB(Convert.ToString(c9_rgb_value));

                led1_pulse = c7_pulse_value;
                led2_pulse = c8_pulse_value;
                led3_pulse = c9_pulse_value;

                led1_delay = c7_delay_value;
                led2_delay = c8_delay_value;
                led3_delay = c9_delay_value;
            }
            else if (c101112_isCurrent)
            {
                led1_rgb = getRGB(Convert.ToString(c10_rgb_value));
                led2_rgb = getRGB(Convert.ToString(c11_rgb_value));
                led3_rgb = getRGB(Convert.ToString(c12_rgb_value));

                led1_pulse = c10_pulse_value;
                led2_pulse = c11_pulse_value;
                led3_pulse = c12_pulse_value;

                led1_delay = c10_delay_value;
                led2_delay = c11_delay_value;
                led3_delay = c12_delay_value;
            }
            else if (c131415_isCurrent)
            {
                led1_rgb = getRGB(Convert.ToString(c13_rgb_value));
                led2_rgb = getRGB(Convert.ToString(c14_rgb_value));
                led3_rgb = getRGB(Convert.ToString(c15_rgb_value));

                led1_pulse = c13_pulse_value;
                led2_pulse = c14_pulse_value;
                led3_pulse = c15_pulse_value;

                led1_delay = c13_delay_value;
                led2_delay = c14_delay_value;
                led3_delay = c15_delay_value;
            }
            else
            {
                led1_rgb = getRGB(Convert.ToString(c16_rgb_value));

                led1_pulse = c16_pulse_value;

                led1_delay = c16_delay_value;
            }
        }

        private void led1_lightLoop()
        {
            while (led1_testStop == 1)
            {
                led1_light.FillColor = Color.FromArgb(led1_rgb, 0, 0);
                Thread.Sleep(led1_pulse);
                led1_light.FillColor = Color.Transparent;
                Thread.Sleep(led1_delay);
            }
            led1_light.FillColor = Color.FromArgb(led1_rgb, 0, 0);
        }

        private void led2_lightLoop()
        {
            while (led2_testStop == 1)
            {
                led2_light.FillColor = Color.FromArgb(0, led2_rgb, 0);
                Thread.Sleep(led2_pulse);
                led2_light.FillColor = Color.Transparent;
                Thread.Sleep(led2_delay);
            }
            led2_light.FillColor = Color.FromArgb(0, led2_rgb, 0);
        }

        private void led3_lightLoop()
        {
            while (led3_testStop == 1)
            {
                led3_light.FillColor = Color.FromArgb(0, 0, led3_rgb);
                Thread.Sleep(led3_pulse);
                led3_light.FillColor = Color.Transparent;
                Thread.Sleep(led3_delay);
            }
            led3_light.FillColor = Color.FromArgb(0, 0, led3_rgb);
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
            Console.WriteLine("Action Changed");
            // Channels 1 2 3
            if (c010203_isCurrent)
            {
                if (chooseGrp.SelectedIndex == 0) { c010203_isGrouped = true; }
                else { c010203_isGrouped = false; }
            }
            // Channels 4 5 6
            else if (c040506_isCurrent)
            {
                if (chooseGrp.SelectedIndex == 0) { c040506_isGrouped = true; }
                else { c040506_isGrouped = false; }
            }
            // Channels 7 8 9
            else if (c070809_isCurrent)
            {
                if (chooseGrp.SelectedIndex == 0) { c070809_isGrouped = true; }
                else { c070809_isGrouped = false; }
            }
            // Channels 10 11 12
            else if (c101112_isCurrent)
            {
                if (chooseGrp.SelectedIndex == 0) { c101112_isGrouped = true; }
                else { c101112_isGrouped = false; }
            }
            // Channels 13 14 15
            else if (c131415_isCurrent)
            {
                if (chooseGrp.SelectedIndex == 0) { c131415_isGrouped = true; }
                else { c131415_isGrouped = false; }
            }

            //Console.WriteLine("c000123 " + c010203_isGrouped);
            //Console.WriteLine("c000456 " + c040506_isGrouped);
            //Console.WriteLine("c000789 " + c070809_isGrouped);
            //Console.WriteLine("c101112 " + c101112_isGrouped);
            //Console.WriteLine("c131415 " + c131415_isGrouped);
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
            led1_testStop = 0;
            if (checkIntensity(led1_intensity.Text))
            {
                int rgb_value = getRGB(led1_intensity.Text);
                
                errorText.Text = getError(1, false);
                led1_light.FillColor = Color.FromArgb(rgb_value, 0, 0);

                if (c010203_isCurrent) { c1_rgb_value = Convert.ToInt32(led1_intensity.Text); Console.WriteLine("C1 rgb is updated"); }
                else if (c040506_isCurrent) { c4_rgb_value = Convert.ToInt32(led1_intensity.Text); Console.WriteLine("C4 rgb is updated"); }
                else if (c070809_isCurrent) { c7_rgb_value = Convert.ToInt32(led1_intensity.Text); Console.WriteLine("C7 rgb is updated"); }
                else if (c101112_isCurrent) { c10_rgb_value = Convert.ToInt32(led1_intensity.Text); Console.WriteLine("C10 rgb is updated"); }
                else if (c131415_isCurrent) { c13_rgb_value = Convert.ToInt32(led1_intensity.Text); Console.WriteLine("C13 rgb is updated"); }
                else { c16_rgb_value = Convert.ToInt32(led1_intensity.Text); Console.WriteLine("C16 is updated"); }

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
            led2_testStop = 0;
            if (checkIntensity(led2_intensity.Text))
            {
                int rgb_value = getRGB(led2_intensity.Text);
                errorText.Text = getError(2, false);

                led2_light.FillColor = Color.FromArgb(0, rgb_value, 0);

                if (c010203_isCurrent) { c2_rgb_value = Convert.ToInt32(led2_intensity.Text); Console.WriteLine("C2 rgb is updated"); }
                else if (c040506_isCurrent) { c5_rgb_value = Convert.ToInt32(led2_intensity.Text); Console.WriteLine("C5 rgb is updated"); }
                else if (c070809_isCurrent) { c8_rgb_value = Convert.ToInt32(led2_intensity.Text); Console.WriteLine("C8 rgb is updated"); }
                else if (c101112_isCurrent) { c11_rgb_value = Convert.ToInt32(led2_intensity.Text); Console.WriteLine("C11 rgb is updated"); }
                else if (c131415_isCurrent) { c14_rgb_value = Convert.ToInt32(led2_intensity.Text); Console.WriteLine("C14 rgb is updated"); }
                else { Console.WriteLine("nothing is updated"); }
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
            led3_testStop = 0;
            if (checkIntensity(led3_intensity.Text))
            {
                int rgb_value = getRGB(led3_intensity.Text);
                errorText.Text = getError(3, false);

                led3_light.FillColor = Color.FromArgb(0, 0, rgb_value);

                if (c010203_isCurrent) { c3_rgb_value = Convert.ToInt32(led3_intensity.Text); Console.WriteLine("C3 is updated"); }
                else if (c040506_isCurrent) { c6_rgb_value = Convert.ToInt32(led3_intensity.Text); Console.WriteLine("C6 is updated"); }
                else if (c070809_isCurrent) { c9_rgb_value = Convert.ToInt32(led3_intensity.Text); Console.WriteLine("C9 is updated"); }
                else if (c101112_isCurrent) { c12_rgb_value = Convert.ToInt32(led3_intensity.Text); Console.WriteLine("C12 is updated"); }
                else if (c131415_isCurrent) { c15_rgb_value = Convert.ToInt32(led3_intensity.Text); Console.WriteLine("C15 is updated"); }
                else { Console.WriteLine("nothing is updated"); }
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
            led1_testStop = 0;
            led2_testStop = 0;
            led3_testStop = 0;
            if (selectGrp.SelectedIndex == 0)
            {
                c010203_isCurrent = true;
                c040506_isCurrent = false;
                c070809_isCurrent = false;
                c101112_isCurrent = false;
                c131415_isCurrent = false;
                c160000_isCurrent = false;

                // Change Visuals
                chooseGrp.Enabled = true;
                led1_channel.Text = "Channel 1";
                led2_channel.Text = "Channel 2";
                led3_channel.Text = "Channel 3";
                led2_intensity.Enabled = true;
                led3_intensity.Enabled = true;
                led2_mode.Enabled = true;
                led3_mode.Enabled = true;
                led2_strobe.Enabled = true;
                led3_strobe.Enabled = true;
                led2_test.Enabled = true;
                led3_test.Enabled = true;
                led2_intensity.ForeColor = Color.Black;
                led3_intensity.ForeColor = Color.Black;

                led1_intensity.Text = Convert.ToString(c1_rgb_value);
                led2_intensity.Text = Convert.ToString(c2_rgb_value);   
                led3_intensity.Text = Convert.ToString(c3_rgb_value);

                if (c010203_isGrouped) { chooseGrp.SelectedIndex = 0; }
                else { chooseGrp.SelectedIndex = 1; }
            }
            else if (selectGrp.SelectedIndex == 1)
            {
                c010203_isCurrent = false;
                c040506_isCurrent = true;
                c070809_isCurrent = false;
                c101112_isCurrent = false;
                c131415_isCurrent = false;
                c160000_isCurrent = false;

                // Change Visuals
                chooseGrp.Enabled = true;
                led1_channel.Text = "Channel 4";
                led2_channel.Text = "Channel 5";
                led3_channel.Text = "Channel 6";
                led2_intensity.Enabled = true;
                led3_intensity.Enabled = true;
                led2_mode.Enabled = true;
                led3_mode.Enabled = true;
                led2_strobe.Enabled = true;
                led3_strobe.Enabled = true;
                led2_test.Enabled = true;
                led3_test.Enabled = true;
                led2_intensity.ForeColor = Color.Black;
                led3_intensity.ForeColor = Color.Black;

                led1_intensity.Text = Convert.ToString(c4_rgb_value);
                led2_intensity.Text = Convert.ToString(c5_rgb_value);
                led3_intensity.Text = Convert.ToString(c6_rgb_value);

                if (c040506_isGrouped) { chooseGrp.SelectedIndex = 0; }
                else { chooseGrp.SelectedIndex = 1; }
            }
            else if (selectGrp.SelectedIndex == 2)
            {
                c010203_isCurrent = false;
                c040506_isCurrent = false;
                c070809_isCurrent = true;
                c101112_isCurrent = false;
                c131415_isCurrent = false;
                c160000_isCurrent = false;

                // Change Visuals
                chooseGrp.Enabled = true;
                led1_channel.Text = "Channel 7";
                led2_channel.Text = "Channel 8";
                led3_channel.Text = "Channel 9";
                led2_intensity.Enabled = true;
                led3_intensity.Enabled = true;
                led2_mode.Enabled = true;
                led3_mode.Enabled = true;
                led2_strobe.Enabled = true;
                led3_strobe.Enabled = true;
                led2_test.Enabled = true;
                led3_test.Enabled = true;
                led2_intensity.ForeColor = Color.Black;
                led3_intensity.ForeColor = Color.Black;

                led1_intensity.Text = Convert.ToString(c7_rgb_value);
                led2_intensity.Text = Convert.ToString(c8_rgb_value);
                led3_intensity.Text = Convert.ToString(c9_rgb_value);

                if (c070809_isGrouped) { chooseGrp.SelectedIndex = 0; }
                else { chooseGrp.SelectedIndex = 1; }
            }
            else if (selectGrp.SelectedIndex == 3)
            {
                c010203_isCurrent = false;
                c040506_isCurrent = false;
                c070809_isCurrent = false;
                c101112_isCurrent = true;
                c131415_isCurrent = false;
                c160000_isCurrent = false;

                // Change Visuals
                chooseGrp.Enabled = true;
                led1_channel.Text = "Channel 10";
                led2_channel.Text = "Channel 11";
                led3_channel.Text = "Channel 12";
                led2_intensity.Enabled = true;
                led3_intensity.Enabled = true;
                led2_mode.Enabled = true;
                led3_mode.Enabled = true;
                led2_strobe.Enabled = true;
                led3_strobe.Enabled = true;
                led2_test.Enabled = true;
                led3_test.Enabled = true;
                led2_intensity.ForeColor = Color.Black;
                led3_intensity.ForeColor = Color.Black;

                led1_intensity.Text = Convert.ToString(c10_rgb_value);
                led2_intensity.Text = Convert.ToString(c11_rgb_value);
                led3_intensity.Text = Convert.ToString(c12_rgb_value);

                if (c101112_isGrouped) { chooseGrp.SelectedIndex = 0; }
                else { chooseGrp.SelectedIndex = 1; }

            }
            else if (selectGrp.SelectedIndex == 4)
            {
                c010203_isCurrent = false;
                c040506_isCurrent = false;
                c070809_isCurrent = false;
                c101112_isCurrent = false;
                c131415_isCurrent = true;
                c160000_isCurrent = false;

                // Change Visuals
                chooseGrp.Enabled = true;
                led1_channel.Text = "Channel 13";
                led2_channel.Text = "Channel 14";
                led3_channel.Text = "Channel 15";
                led2_intensity.Enabled = true;
                led3_intensity.Enabled = true;
                led2_mode.Enabled = true;
                led3_mode.Enabled = true;
                led2_strobe.Enabled = true;
                led3_strobe.Enabled = true;
                led2_test.Enabled = true;
                led3_test.Enabled = true;
                led2_intensity.ForeColor = Color.Black;
                led3_intensity.ForeColor = Color.Black;

                led1_intensity.Text = Convert.ToString(c13_rgb_value);
                led2_intensity.Text = Convert.ToString(c14_rgb_value);
                led3_intensity.Text = Convert.ToString(c15_rgb_value);

                if (c131415_isGrouped) { chooseGrp.SelectedIndex = 0; }
                else { chooseGrp.SelectedIndex = 1; }
            }
            else
            {
                c010203_isCurrent = false;
                c040506_isCurrent = false;
                c070809_isCurrent = false;
                c101112_isCurrent = false;
                c131415_isCurrent = false;
                c160000_isCurrent = true;

                // Change Visuals
                led1_channel.Text = "Channel 16";
                led2_channel.Text = "Channel NA";
                led3_channel.Text = "Channel NA";
                led2_intensity.Enabled = false;
                led3_intensity.Enabled = false;
                led2_mode.Enabled = false;
                led3_mode.Enabled = false;
                led2_strobe.Enabled = false;
                led3_strobe.Enabled = false;
                led2_test.Enabled = false;
                led3_test.Enabled = false;
                led2_intensity.ForeColor = Color.White;
                led3_intensity.ForeColor = Color.White;

                led1_intensity.Text = Convert.ToString(c16_rgb_value);
                led2_intensity.Text = "0";
                led3_intensity.Text = "0";

                chooseGrp.Enabled = false;
            }
        }

        private void strobeChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strobeChannel.SelectedIndex == 0) { 
                currentStrobe = 1;
                setEdge.SelectedIndex = c1_edge_value;
                setPulse.Text = Convert.ToString(c1_pulse_value);
                setDelay.Text = Convert.ToString(c1_delay_value);
            }
            else if (strobeChannel.SelectedIndex == 1) { 
                currentStrobe = 2;
                setEdge.SelectedIndex = c2_edge_value;
                setPulse.Text = Convert.ToString(c2_pulse_value);
                setDelay.Text = Convert.ToString(c2_delay_value);
            }
            else if (strobeChannel.SelectedIndex == 2) { 
                currentStrobe = 3;
                setEdge.SelectedIndex = c3_edge_value;
                setPulse.Text = Convert.ToString(c3_pulse_value);
                setDelay.Text = Convert.ToString(c3_delay_value);
            }
            else if (strobeChannel.SelectedIndex == 3) { 
                currentStrobe = 4;
                setEdge.SelectedIndex = c4_edge_value;
                setPulse.Text = Convert.ToString(c4_pulse_value);
                setDelay.Text = Convert.ToString(c4_delay_value);
            }
            else if (strobeChannel.SelectedIndex == 4) { 
                currentStrobe = 5;
                setEdge.SelectedIndex = c5_edge_value;
                setPulse.Text = Convert.ToString(c5_pulse_value);
                setDelay.Text = Convert.ToString(c5_delay_value);
            }
            else if (strobeChannel.SelectedIndex == 5) { 
                currentStrobe = 6;
                setEdge.SelectedIndex = c6_edge_value;
                setPulse.Text = Convert.ToString(c6_pulse_value);
                setDelay.Text = Convert.ToString(c6_delay_value);
            }
            else if (strobeChannel.SelectedIndex == 6) { 
                currentStrobe = 7;
                setEdge.SelectedIndex = c7_edge_value;
                setPulse.Text = Convert.ToString(c7_pulse_value);
                setDelay.Text = Convert.ToString(c7_delay_value);
            }
            else if (strobeChannel.SelectedIndex == 7) { 
                currentStrobe = 8;
                setEdge.SelectedIndex = c8_edge_value;
                setPulse.Text = Convert.ToString(c8_pulse_value);
                setDelay.Text = Convert.ToString(c8_delay_value);
            }
            else if (strobeChannel.SelectedIndex == 8) { 
                currentStrobe = 9;
                setEdge.SelectedIndex = c9_edge_value;
                setPulse.Text = Convert.ToString(c9_pulse_value);
                setDelay.Text = Convert.ToString(c9_delay_value);
            }
            else if (strobeChannel.SelectedIndex == 9) { 
                currentStrobe = 10;
                setEdge.SelectedIndex = c10_edge_value;
                setPulse.Text = Convert.ToString(c10_pulse_value);
                setDelay.Text = Convert.ToString(c10_delay_value);
            }
            else if (strobeChannel.SelectedIndex == 10) { 
                currentStrobe = 11;
                setEdge.SelectedIndex = c11_edge_value;
                setPulse.Text = Convert.ToString(c11_pulse_value);
                setDelay.Text = Convert.ToString(c11_delay_value);
            }
            else if (strobeChannel.SelectedIndex == 11) { 
                currentStrobe = 12;
                setEdge.SelectedIndex = c12_edge_value;
                setPulse.Text = Convert.ToString(c12_pulse_value);
                setDelay.Text = Convert.ToString(c12_delay_value);
            }
            else if (strobeChannel.SelectedIndex == 12) { 
                currentStrobe = 13;
                setEdge.SelectedIndex = c13_edge_value;
                setPulse.Text = Convert.ToString(c13_pulse_value);
                setDelay.Text = Convert.ToString(c13_delay_value);
            }
            else if (strobeChannel.SelectedIndex == 13) { 
                currentStrobe = 14;
                setEdge.SelectedIndex = c14_edge_value;
                setPulse.Text = Convert.ToString(c14_pulse_value);
                setDelay.Text = Convert.ToString(c14_delay_value);
            }
            else if (strobeChannel.SelectedIndex == 14) { 
                currentStrobe = 15;
                setEdge.SelectedIndex = c15_edge_value;
                setPulse.Text = Convert.ToString(c15_pulse_value);
                setDelay.Text = Convert.ToString(c15_delay_value);
            }
            else if (strobeChannel.SelectedIndex == 15) { 
                currentStrobe = 16;
                setEdge.SelectedIndex = c16_edge_value;
                setPulse.Text = Convert.ToString(c16_pulse_value);
                setDelay.Text = Convert.ToString(c16_delay_value);
            }

            //Console.WriteLine(currentStrobe); 
        }

        private void setEdge_SelectedIndexChanged(object sender, EventArgs e)
        {
            int edge_value = setEdge.SelectedIndex;
            //Console.WriteLine(edge_value);

            switch (currentStrobe)
            {
                case 1:
                    c1_edge_value = edge_value;
                    break;
                case 2:
                    c2_edge_value = edge_value;
                    break;
                case 3:
                    c3_edge_value = edge_value;
                    break;
                case 4:
                    c4_edge_value = edge_value;
                    break;
                case 5:
                    c5_edge_value = edge_value;
                    break;
                case 6:
                    c6_edge_value = edge_value;
                    break;
                case 7:
                    c7_edge_value = edge_value;
                    break;
                case 8:
                    c8_edge_value = edge_value;
                    break;
                case 9:
                    c9_edge_value = edge_value;
                    break;
                case 10:
                    c10_edge_value = edge_value;
                    break;
                case 11:
                    c11_edge_value = edge_value;
                    break;
                case 12:
                    c12_edge_value = edge_value;
                    break;
                case 13:
                    c13_edge_value = edge_value;
                    break;
                case 14:
                    c14_edge_value = edge_value;
                    break;
                case 15:
                    c15_edge_value = edge_value;
                    break;
                case 16:
                    c16_edge_value = edge_value;
                    break;
            } // End of Switch Loop

            // To test Edge Values
            //Console.WriteLine("Channel 1 " + c1_edge_value);
            //Console.WriteLine("Channel 2 " + c2_edge_value);
            //Console.WriteLine("Channel 3 " + c3_edge_value);
            //Console.WriteLine("Channel 4 " + c4_edge_value);
            //Console.WriteLine("Channel 5 " + c5_edge_value);
            //Console.WriteLine("Channel 6 " + c6_edge_value);
            //Console.WriteLine("Channel 7 " + c7_edge_value);
            //Console.WriteLine("Channel 8 " + c8_edge_value);
            //Console.WriteLine("Channel 9 " + c9_edge_value);
            //Console.WriteLine("Channel 10 " + c10_edge_value);
            //Console.WriteLine("Channel 11 " + c11_edge_value);
            //Console.WriteLine("Channel 12 " + c12_edge_value);
            //Console.WriteLine("Channel 13 " + c13_edge_value);
            //Console.WriteLine("Channel 14 " + c14_edge_value);
            //Console.WriteLine("Channel 15 " + c15_edge_value);
            //Console.WriteLine("Channel 16 " + c16_edge_value);
        }

        private void setPulse_TextChanged(object sender, EventArgs e)
        {
            if (checkStrobeValue(setPulse.Text))
            {
                int pulse_value = Convert.ToInt32(setPulse.Text);
                switch (currentStrobe)
                {
                    case 1:
                        c1_pulse_value = pulse_value;
                        break;
                    case 2:
                        c2_pulse_value = pulse_value;
                        break;
                    case 3:
                        c3_pulse_value = pulse_value;
                        break;
                    case 4:
                        c4_pulse_value = pulse_value;
                        break;
                    case 5:
                        c5_pulse_value = pulse_value;
                        break;
                    case 6:
                        c6_pulse_value = pulse_value;
                        break;
                    case 7:
                        c7_pulse_value = pulse_value;
                        break;
                    case 8:
                        c8_pulse_value = pulse_value;
                        break;
                    case 9:
                        c9_pulse_value = pulse_value;
                        break;
                    case 10:
                        c10_pulse_value = pulse_value;
                        break;
                    case 11:
                        c11_pulse_value = pulse_value;
                        break;
                    case 12:
                        c12_pulse_value = pulse_value;
                        break;
                    case 13:
                        c13_pulse_value = pulse_value;
                        break;
                    case 14:
                        c14_pulse_value = pulse_value;
                        break;
                    case 15:
                        c15_pulse_value = pulse_value;
                        break;
                    case 16:
                        c16_pulse_value = pulse_value;
                        break;
                } // End of Switch Loop

                // To test Pulse Values
                //Console.WriteLine("Channel 1 " + c1_pulse_value);
                //Console.WriteLine("Channel 2 " + c2_pulse_value);
                //Console.WriteLine("Channel 3 " + c3_pulse_value);
                //Console.WriteLine("Channel 4 " + c4_pulse_value);
                //Console.WriteLine("Channel 5 " + c5_pulse_value);
                //Console.WriteLine("Channel 6 " + c6_pulse_value);
                //Console.WriteLine("Channel 7 " + c7_pulse_value);
                //Console.WriteLine("Channel 8 " + c8_pulse_value);
                //Console.WriteLine("Channel 9 " + c9_pulse_value);
                //Console.WriteLine("Channel 10 " + c10_pulse_value);
                //Console.WriteLine("Channel 11 " + c11_pulse_value);
                //Console.WriteLine("Channel 12 " + c12_pulse_value);
                //Console.WriteLine("Channel 13 " + c13_pulse_value);
                //Console.WriteLine("Channel 14 " + c14_pulse_value);
                //Console.WriteLine("Channel 15 " + c15_pulse_value);
                //Console.WriteLine("Channel 16 " + c16_pulse_value);
            }
            else
            {
                Console.WriteLine("Invalid Pulse Value");
            }
        }

        private void setDelay_TextChanged(object sender, EventArgs e)
        {
            if (checkStrobeValue(setDelay.Text))
            {
                int delay_value = Convert.ToInt32(setDelay.Text);
                switch (currentStrobe)
                {
                    case 1:
                        c1_delay_value = delay_value;
                        break;
                    case 2:
                        c2_delay_value = delay_value;
                        break;
                    case 3:
                        c3_delay_value = delay_value;
                        break;
                    case 4:
                        c4_delay_value = delay_value;
                        break;
                    case 5:
                        c5_delay_value = delay_value;
                        break;
                    case 6:
                        c6_delay_value = delay_value;
                        break;
                    case 7:
                        c7_delay_value = delay_value;
                        break;
                    case 8:
                        c8_delay_value = delay_value;
                        break;
                    case 9:
                        c9_delay_value = delay_value;
                        break;
                    case 10:
                        c10_delay_value = delay_value;
                        break;
                    case 11:
                        c11_delay_value = delay_value;
                        break;
                    case 12:
                        c12_delay_value = delay_value;
                        break;
                    case 13:
                        c13_delay_value = delay_value;
                        break;
                    case 14:
                        c14_delay_value = delay_value;
                        break;
                    case 15:
                        c15_delay_value = delay_value;
                        break;
                    case 16:
                        c16_delay_value = delay_value;
                        break;
                } // End of Switch Loop

                // To test Pulse Values
                //Console.WriteLine("Channel 1 " + c1_delay_value);
                //Console.WriteLine("Channel 2 " + c2_delay_value);
                //Console.WriteLine("Channel 3 " + c3_delay_value);
                //Console.WriteLine("Channel 4 " + c4_delay_value);
                //Console.WriteLine("Channel 5 " + c5_delay_value);
                //Console.WriteLine("Channel 6 " + c6_delay_value);
                //Console.WriteLine("Channel 7 " + c7_delay_value);
                //Console.WriteLine("Channel 8 " + c8_delay_value);
                //Console.WriteLine("Channel 9 " + c9_delay_value);
                //Console.WriteLine("Channel 10 " + c10_delay_value);
                //Console.WriteLine("Channel 11 " + c11_delay_value);
                //Console.WriteLine("Channel 12 " + c12_delay_value);
                //Console.WriteLine("Channel 13 " + c13_delay_value);
                //Console.WriteLine("Channel 14 " + c14_delay_value);
                //Console.WriteLine("Channel 15 " + c15_delay_value);
                //Console.WriteLine("Channel 16 " + c16_delay_value);
            }
            else
            {
                Console.WriteLine("Invalid Delay Value");
            }
        }

        private void led1_test_Click(object sender, EventArgs e)
        {
            setBlinkValues();
            Thread led1_blink = new Thread(led1_lightLoop);
            if (led1_testStop == 0)
            {
                led1_blink.Start();
                led1_test.Text = "Stop";
            }
            else
            {
                led1_blink.Abort();
                led1_test.Text = "Start";
            }
            led1_testStop++;
            if (led1_testStop == 2) { led1_testStop = 0; }
        }

        private void led2_test_Click(object sender, EventArgs e)
        {
            setBlinkValues();
            Thread led2_blink = new Thread(led2_lightLoop);
            if (led2_testStop == 0)
            {
                led2_blink.Start();
                led2_test.Text = "Stop";
            }
            else
            {
                led2_blink.Abort();
                led2_test.Text = "Start";
            }
            led2_testStop++;
            if (led2_testStop == 2) { led2_testStop = 0; }
        }

        private void led3_test_Click(object sender, EventArgs e)
        {
            setBlinkValues();
            Thread led3_blink = new Thread(led3_lightLoop);
            if (led3_testStop == 0)
            {
                led3_blink.Start();
                led3_test.Text = "Stop";
            }
            else
            {
                led3_blink.Abort();
                led3_test.Text = "Start";
            }
            led3_testStop++;
            if (led3_testStop == 2) { led3_testStop = 0; }
        }
    }
}
