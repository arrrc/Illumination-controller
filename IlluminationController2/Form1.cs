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
using System.Diagnostics;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
//using System.Text.Json;
//using System.Text.Json.Serialization;

namespace IlluminationController2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // Global Variables
        int c1_rgb_value;
        int c1_pulse_value;
        int c1_delay_value;
        int c1_testStop;

        int c2_rgb_value;
        int c2_pulse_value;
        int c2_delay_value;
        int c2_testStop;

        int c3_rgb_value;
        int c3_pulse_value;
        int c3_delay_value;
        int c3_testStop;

        int c4_rgb_value;
        int c4_pulse_value;
        int c4_delay_value;
        int c4_testStop;

        int c5_rgb_value;
        int c5_pulse_value;
        int c5_delay_value;
        int c5_testStop;

        int c6_rgb_value;
        int c6_pulse_value;
        int c6_delay_value;
        int c6_testStop;

        int c7_rgb_value;
        int c7_pulse_value;
        int c7_delay_value;
        int c7_testStop;

        int c8_rgb_value;
        int c8_pulse_value;
        int c8_delay_value;
        int c8_testStop;

        int c9_rgb_value;
        int c9_pulse_value;
        int c9_delay_value;
        int c9_testStop;

        int c10_rgb_value;
        int c10_pulse_value;
        int c10_delay_value;
        int c10_testStop;

        int c11_rgb_value;
        int c11_pulse_value;
        int c11_delay_value;
        int c11_testStop;

        int c12_rgb_value;
        int c12_pulse_value;
        int c12_delay_value;
        int c12_testStop;

        int c13_rgb_value;
        int c13_pulse_value;
        int c13_delay_value;
        int c13_testStop;

        int c14_rgb_value;
        int c14_pulse_value;
        int c14_delay_value;
        int c14_testStop;

        int c15_rgb_value;
        int c15_pulse_value;
        int c15_delay_value;
        int c15_testStop;

        int c16_rgb_value;
        int c16_pulse_value;
        int c16_delay_value;
        int c16_testStop;

        static SerialPort portConn;
        public string sendToHardware = "";
        string dataReceived = "";
        bool didDataReceiveThreadExit = false;
        int numTimesDataSent = 0;
        List<string> config = new List<string>();
        int noTimesRemoveEventFired = 0;

        List<string> splitData = new List<string>();

        // Global Functions
        public bool checkBit(string text, int type)
        {
            int bitCount = 0;
            if (text.Length == type)
            {
                for (int i = 0; i < type; i++)
                {
                    if (text[i] == '1' || text[i] == '0') { bitCount++; }
                }
                if (bitCount == type) { return true; }
            }
            return false;
        }

        public string convertHex(string bit)
        {
            int start = 0;
            int grab = 4;

            string hex_str = "#";
            string[] Bin_to_Hex = {"0000","0001","0010","0011",
                                   "0100","0101","0110","0111",
                                   "1000","1001","1010","1011",
                                   "1100","1101","1110","1111" };

            string[] Hex = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };

            for (int i=0; i<3; i++)
            {
                string fourBits = bit.Substring(start, grab);

                for (int j=0; j<Bin_to_Hex.Length; j++)
                {
                    if (fourBits == Bin_to_Hex[j])
                    {
                        hex_str += Hex[j];
                        hex_str += Hex[j];
                    }
                }
                start = start + 4;
            }
            return hex_str;
        }

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

        public bool checkPulse(string pulse)
        {
            if (pulse.Length >= 0 && pulse.Length < 6)
            {
                try
                {
                    int pulse_value = Convert.ToInt32(pulse);
                    if (pulse_value >= 0 && pulse_value < 65537) { return true; }
                }
                catch { return false; }
            }
            return false;
        }

        public bool checkDelay(string delay)
        {
            if (delay.Length >= 0 && delay.Length < 6)
            {
                try
                {
                    int delay_value = Convert.ToInt32(delay);
                    if (delay_value >= 0 && delay_value < 65537) { return true; }
                }
                catch { return false; }
            }
            return false;
        }


        //groupNo can be ungrouped or the respective group number each channel is assigned to
        private string displayValues(string channel, string intensity, string edge, string mode, string strobe, string pulse, string delay)
        {
            if (intensity == "") { intensity = "None"; }
            if (edge == "") { edge = "None"; }
            if (mode == "") { mode = "None"; }
            if (strobe == "") { strobe = "None"; }
            if (pulse == "") { pulse = "None"; }
            if (delay == "") { delay = "None"; }

            string consoleDisplay = $"\n[{channel} Settings]: " +
                $"Intensity: {intensity}, Edge: {edge}, Mode: {mode}, Strobe: {strobe}, Pulse: {pulse}, Delay: {delay} ";

            return consoleDisplay;
        }

        private string displaySettings(string setting, string group, string first, string second, string third)
        {
            if (setting == "Grouped")
            {
                return $"\n{group} Settings, [GROUPED Red:{first}, Green:{second}, Blue:{third}]";
            }
            else if (setting == "Ungrouped")
            {
                return $"\n{group} Settings, [UNGROUPED:Red:{first}, Green:{second}, Blue:{third}]";
            }
            else
            {
                return "displaySettings function went wrong";
            }
        }

        private void writeFile(string filename)
        {
            string path = @"C:\Users\WZS20\Documents\GitHub\Illumination-controller\IlluminationController2\bin\Debug";
            var combinedPath = Path.Combine(path, filename);

            if (File.Exists(combinedPath))
            {
                Console.WriteLine("File does exists");
                using (StreamWriter sw = File.CreateText(combinedPath))
                {
                    sw.WriteLine("Hi");
                    sw.WriteLine("Dab");
                }
            }
            else
            {
                Console.WriteLine("File not found");
            }
        }

        private void readFile(string filename)
        {
            string path = @"C:\Users\WZS20\Documents\GitHub\Illumination-controller\IlluminationController2\bin\Debug";
            var combinedPath = Path.Combine(path, filename);

            if (File.Exists(combinedPath))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null) { Console.WriteLine(line); }
                }
            }
            else { Console.WriteLine("File not found"); }
        }

        //// For Switch Config
        //private void generateFileList()
        //{
        //    string path = @"C:\Users\WZS20\Documents\GitHub\Illumination-controller\IlluminationController2\bin\Debug";
        //    string[] fileList = Directory.GetFiles(path);

        //    foreach(string file in fileList)
        //    {
        //        Console.WriteLine(file);
        //    }


        //    // generate a list of configuration files for user to switch to
        //    // Files that no longer exist should not be seen as an option to the user
        //    // Values that are stored within the file are displayed on the LED & Channel Settings
        //}

        // Individual Channel Functions
        private void c1_light_loop()
        {
            while (c1_testStop == 1)
            {
                c1_status.BackColor = Color.FromArgb(c1_rgb_value, 0, 0);
                Thread.Sleep(c1_pulse_value);
                c1_status.BackColor = Color.Transparent;
                Thread.Sleep(c1_delay_value);
            }
            c1_status.BackColor = Color.FromArgb(c1_rgb_value, 0, 0);
        }

        private void c2_light_loop()
        {
            while (c2_testStop == 1)
            {
                c2_status.BackColor = Color.FromArgb(0, c2_rgb_value, 0);
                Thread.Sleep(c2_pulse_value);
                c2_status.BackColor = Color.Transparent;
                Thread.Sleep(c2_delay_value);
            }
            c2_status.BackColor = Color.FromArgb(0, c2_rgb_value, 0);
        }

        private void c3_light_loop()
        {
            while (c3_testStop == 1)
            {
                c3_status.BackColor = Color.FromArgb(0, 0, c3_rgb_value);
                Thread.Sleep(c3_pulse_value);
                c3_status.BackColor = Color.Transparent;
                Thread.Sleep(c3_delay_value);
            }
            c3_status.BackColor = Color.FromArgb(0, 0, c3_rgb_value);
        }

        private void c4_light_loop()
        {
            while (c4_testStop == 1)
            {
                c4_status.BackColor = Color.FromArgb(c4_rgb_value, 0, 0);
                Thread.Sleep(c4_pulse_value);
                c4_status.BackColor = Color.Transparent;
                Thread.Sleep(c4_delay_value);
            }
            c4_status.BackColor = Color.FromArgb(c4_rgb_value, 0, 0);
        }

        private void c5_light_loop()
        {
            while (c5_testStop == 1)
            {
                c5_status.BackColor = Color.FromArgb(0, c5_rgb_value, 0);
                Thread.Sleep(c5_pulse_value);
                c5_status.BackColor = Color.Transparent;
                Thread.Sleep(c5_delay_value);
            }
            c5_status.BackColor = Color.FromArgb(0, c5_rgb_value, 0);
        }

        private void c6_light_loop()
        {
            while (c6_testStop == 1)
            {
                c6_status.BackColor = Color.FromArgb(0, 0, c6_rgb_value);
                Thread.Sleep(c6_pulse_value);
                c6_status.BackColor = Color.Transparent;
                Thread.Sleep(c6_delay_value);
            }
            c6_status.BackColor = Color.FromArgb(0, 0, c6_rgb_value);
        }

        private void c7_light_loop()
        {
            while (c7_testStop == 1)
            {
                c7_status.BackColor = Color.FromArgb(c7_rgb_value, 0, 0);
                Thread.Sleep(c7_pulse_value);
                c7_status.BackColor = Color.Transparent;
                Thread.Sleep(c7_delay_value);
            }
            c7_status.BackColor = Color.FromArgb(c7_rgb_value, 0, 0);
        }

        private void c8_light_loop()
        {
            while (c8_testStop == 1)
            {
                c8_status.BackColor = Color.FromArgb(0, c8_rgb_value, 0);
                Thread.Sleep(c8_pulse_value);
                c8_status.BackColor = Color.Transparent;
                Thread.Sleep(c8_delay_value);
            }
            c8_status.BackColor = Color.FromArgb(0, c8_rgb_value, 0);
        }

        private void c9_light_loop()
        {
            while (c9_testStop == 1)
            {
                c9_status.BackColor = Color.FromArgb(0, 0, c9_rgb_value);
                Thread.Sleep(c9_pulse_value);
                c9_status.BackColor = Color.Transparent;
                Thread.Sleep(c9_delay_value);
            }
            c9_status.BackColor = Color.FromArgb(0, 0, c9_rgb_value);
        }

        private void c10_light_loop()
        {
            while (c10_testStop == 1)
            {
                c10_status.BackColor = Color.FromArgb(c10_rgb_value, 0, 0);
                Thread.Sleep(c10_pulse_value);
                c10_status.BackColor = Color.Transparent;
                Thread.Sleep(c10_delay_value);
            }
            c10_status.BackColor = Color.FromArgb(c10_rgb_value, 0, 0);
        }

        private void c11_light_loop()
        {
            while (c11_testStop == 1)
            {
                c11_status.BackColor = Color.FromArgb(0, c11_rgb_value, 0);
                Thread.Sleep(c11_pulse_value);
                c11_status.BackColor = Color.Transparent;
                Thread.Sleep(c11_delay_value);
            }
            c11_status.BackColor = Color.FromArgb(0, c11_rgb_value, 0);
        }

        private void c12_light_loop()
        {
            while (c12_testStop == 1)
            {
                c12_status.BackColor = Color.FromArgb(0, 0, c12_rgb_value);
                Thread.Sleep(c12_pulse_value);
                c12_status.BackColor = Color.Transparent;
                Thread.Sleep(c12_delay_value);
            }
            c12_status.BackColor = Color.FromArgb(0, 0, c12_rgb_value);
        }

        private void c13_light_loop()
        {
            while (c13_testStop == 1)
            {
                c13_status.BackColor = Color.FromArgb(c13_rgb_value, 0, 0);
                Thread.Sleep(c13_pulse_value);
                c13_status.BackColor = Color.Transparent;
                Thread.Sleep(c13_delay_value);
            }
            c13_status.BackColor = Color.FromArgb(c13_rgb_value, 0, 0);
        }

        private void c14_light_loop()
        {
            while (c14_testStop == 1)
            {
                c14_status.BackColor = Color.FromArgb(0, c14_rgb_value, 0);
                Thread.Sleep(c14_pulse_value);
                c14_status.BackColor = Color.Transparent;
                Thread.Sleep(c14_delay_value);
            }
            c14_status.BackColor = Color.FromArgb(0, c14_rgb_value, 0);
        }

        private void c15_light_loop()
        {
            while (c15_testStop == 1)
            {
                c15_status.BackColor = Color.FromArgb(0, 0, c15_rgb_value);
                Thread.Sleep(c15_pulse_value);
                c15_status.BackColor = Color.Transparent;
                Thread.Sleep(c15_delay_value);
            }
            c15_status.BackColor = Color.FromArgb(0, 0, c15_rgb_value);
        }

        private void c16_light_loop()
        {
            while (c16_testStop == 1)
            {
                c16_status.BackColor = Color.FromArgb(0, 0, c16_rgb_value);
                Thread.Sleep(c16_pulse_value);
                c16_status.BackColor = Color.Transparent;
                Thread.Sleep(c16_delay_value);
            }
            c16_status.BackColor = Color.FromArgb(0, 0, c16_rgb_value);
        }

        // Channel 1
        private void c1_title_Click(object sender, EventArgs e)
        {

        }

        private void c1_intensity_TextChanged(object sender, EventArgs e)
        {
            if (checkIntensity(c1_intensity.Text))
            {
                int rgb_value = getRGB(c1_intensity.Text);
                c1_status.BackColor = Color.FromArgb(rgb_value, 0, 0);
                c1_error.ForeColor = Color.White;
                c1_error.Text = "Error: ";

                c1_rgb_value = rgb_value;
            }
            else
            {
                c1_test.Enabled = false;
                c1_status.BackColor = Color.Transparent;
                c1_error.ForeColor = Color.Red;
                c1_error.Text = "Error: Invalid value inputted for intensity. Please use integers from 0 to 4096.";

            }

            if (checkIntensity(c1_intensity.Text) && checkIntensity(c2_intensity.Text) && checkIntensity(c3_intensity.Text) && checkIntensity(c4_intensity.Text) && checkIntensity(c5_intensity.Text) && checkIntensity(c6_intensity.Text) && checkIntensity(c7_intensity.Text) && checkIntensity(c8_intensity.Text) && checkIntensity(c9_intensity.Text) && checkIntensity(c10_intensity.Text) && checkIntensity(c11_intensity.Text) && checkIntensity(c12_intensity.Text) && checkIntensity(c13_intensity.Text) && checkIntensity(c14_intensity.Text) && checkIntensity(c15_intensity.Text) && checkIntensity(c16_intensity.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c1_edge_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void c1_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            c1_testStop = 0;
            c1_test.Text = "Test";
            if (g1_setting.SelectedIndex == 1)
            {
                c2_mode.SelectedIndex = c1_mode.SelectedIndex;
                c3_mode.SelectedIndex = c1_mode.SelectedIndex;
            }

                if (c1_mode.SelectedIndex == 1)
            {
                // Strobe is selected
                c1_strobe.Enabled = true;
                c1_edge.Enabled = true;
                c1_pulse.Enabled = true;
                c1_delay.Enabled = true;
            }
            else
            {
                //Static is selected
                c1_strobe.Enabled = false;
                c1_edge.Enabled = false;
                c1_pulse.Enabled = false;
                c1_delay.Enabled = false;
            }
        }

        private void c1_strobe_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        

        private void c1_pulse_TextChanged(object sender, EventArgs e)
        {
            // Pulse
            c1_testStop = 0;
            c1_test.Text = "Test";
            //if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text))
            if(checkPulse(c1_pulse.Text))
            {
                int pulse_value = Convert.ToInt32(c1_pulse.Text);
                //int delay_value = Convert.ToInt32(c1_delay.Text);

                c1_test.Enabled = true;
                c1_error.ForeColor = Color.White;
                c1_error.Text = "Error: ";
                c1_test.Enabled = true;

                c1_pulse_value = pulse_value;
                //c1_delay_value = delay_value;

                if (g1_setting.SelectedIndex == 1)
                {
                    c2_pulse_value = pulse_value;
                    c3_pulse_value = pulse_value;
                    c2_pulse.Text = c1_pulse.Text;
                    c3_pulse.Text = c1_pulse.Text;

                    
                }
                else
                {

                }

            }
            else
            {
                c1_test.Enabled = false;
                c1_error.ForeColor = Color.Red;
                c1_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c1_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c1_delay_TextChanged(object sender, EventArgs e)
        {
            // Delay
            c1_testStop = 0;
            c1_test.Text = "Test";
            //if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text))
            if(checkDelay(c1_delay.Text))
            {
                //int pulse_value = Convert.ToInt32(c1_pulse.Text);
                int delay_value = Convert.ToInt32(c1_delay.Text);

                c1_test.Enabled = true;
                c1_error.ForeColor = Color.White;
                c1_error.Text = "Error: ";
                c1_test.Enabled = true;

                //c1_pulse_value = pulse_value;
                c1_delay_value = delay_value;
                if (g1_setting.SelectedIndex == 1)
                {
                    

                    c2_delay_value = delay_value;
                    c3_delay_value = delay_value;
                    c2_delay.Text = c1_delay.Text;
                    c3_delay.Text = c1_delay.Text;
                }
                else
                {

                }
            }
            else
            {
                c1_test.Enabled = false;
                c1_error.ForeColor = Color.Red;
                c1_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c1_test.Enabled = false;
                
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c1_status_Click(object sender, EventArgs e)
        {

        }

        private void c1_test_Click(object sender, EventArgs e)
        {
            Thread c1_testing = new Thread(c1_light_loop);
            if (c1_testStop == 0)
            {
                c1_testing.Start();
                c1_test.Text = "Stop";
            }
            else
            {
                c1_testing.Abort();
                c1_test.Text = "Test";
            }
            c1_testStop++;
            if (c1_testStop == 2) { c1_testStop = 0; }
        }

        private void c1_error_Click(object sender, EventArgs e)
        {

        }

        // Channel 2
        private void c2_title_Click(object sender, EventArgs e)
        {

        }

        private void c2_intensity_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void c2_edge_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void c2_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            c2_testStop = 0;
            c2_test.Text = "Test";
            if (c2_mode.SelectedIndex == 1)
            {
                // Strobe is selected
                c2_strobe.Enabled = true;
                c2_edge.Enabled = true;
                c2_pulse.Enabled = true;
                c2_delay.Enabled = true;
            }
            else
            {
                //Static is selected
                c2_strobe.Enabled = false;
                c2_edge.Enabled = false;
                c2_pulse.Enabled = false;
                c2_delay.Enabled = false;
            }
        }

        private void c2_strobe_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void c2_pulse_TextChanged(object sender, EventArgs e)
        {
            // Pulse
            c2_testStop = 0;
            c2_test.Text = "Test";
            if (checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c2_pulse.Text);
                int delay_value = Convert.ToInt32(c2_delay.Text);

                c2_test.Enabled = true;
                c2_error.ForeColor = Color.White;
                c2_error.Text = "Error: ";
                c2_test.Enabled = true;

                c2_pulse_value = pulse_value;
                c2_delay_value = delay_value;
            }
            else
            {
                c2_test.Enabled = false;
                c2_error.ForeColor = Color.Red;
                c2_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c2_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c2_delay_TextChanged(object sender, EventArgs e)
        {
            // Delay
            c2_testStop = 0;
            c2_test.Text = "Test";
            if (checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c2_pulse.Text);
                int delay_value = Convert.ToInt32(c2_delay.Text);

                c2_test.Enabled = true;
                c2_error.ForeColor = Color.White;
                c2_error.Text = "Error: ";
                c2_test.Enabled = true;

                c2_pulse_value = pulse_value;
                c2_delay_value = delay_value;
            }
            else
            {
                c2_test.Enabled = false;
                c2_error.ForeColor = Color.Red;
                c2_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c2_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c2_status_Click(object sender, EventArgs e)
        {

        }

        private void c2_test_Click(object sender, EventArgs e)
        {
            Thread c2_testing = new Thread(c2_light_loop);
            if (c2_testStop == 0)
            {
                c2_testing.Start();
                c2_test.Text = "Stop";
            }
            else
            {
                c2_testing.Abort();
                c2_test.Text = "Test";
            }
            c2_testStop++;
            if (c2_testStop == 2) { c2_testStop = 0; }
        }

        private void c2_error_Click(object sender, EventArgs e)
        {

        }

        // Channel 3
        private void c3_title_Click(object sender, EventArgs e)
        {

        }

        private void c3_intensity_TextChanged(object sender, EventArgs e)
        {
            if (checkIntensity(c3_intensity.Text))
            {
                int rgb_value = getRGB(c3_intensity.Text);
                c3_status.BackColor = Color.FromArgb(0, 0, rgb_value);
                c3_error.ForeColor = Color.White;
                c3_error.Text = "Error: ";

                c3_rgb_value = rgb_value;

            }
            else
            {
                //Console.WriteLine("Invalid Value");
                c3_test.Enabled = false;
                c3_status.BackColor = Color.Transparent;
                c3_error.ForeColor = Color.Red;
                c3_error.Text = "Error: Invalid value inputted for intensity. Please use integers from 0 to 4096.";

            }

            if (checkIntensity(c1_intensity.Text) && checkIntensity(c2_intensity.Text) && checkIntensity(c3_intensity.Text) && checkIntensity(c4_intensity.Text) && checkIntensity(c5_intensity.Text) && checkIntensity(c6_intensity.Text) && checkIntensity(c7_intensity.Text) && checkIntensity(c8_intensity.Text) && checkIntensity(c9_intensity.Text) && checkIntensity(c10_intensity.Text) && checkIntensity(c11_intensity.Text) && checkIntensity(c12_intensity.Text) && checkIntensity(c13_intensity.Text) && checkIntensity(c14_intensity.Text) && checkIntensity(c15_intensity.Text) && checkIntensity(c16_intensity.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c3_edge_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void c3_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            c3_testStop = 0;
            c3_test.Text = "Test";
            if (c3_mode.SelectedIndex == 1)
            {
                // Strobe is selected
                c3_strobe.Enabled = true;
                c3_edge.Enabled = true;
                c3_pulse.Enabled = true;
                c3_delay.Enabled = true;
            }
            else
            {
                //Static is selected
                c3_strobe.Enabled = false;
                c3_edge.Enabled = false;
                c3_pulse.Enabled = false;
                c3_delay.Enabled = false;
            }
        }

        private void c3_strobe_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void c3_pulse_TextChanged(object sender, EventArgs e)
        {
            // Pulse
            c3_testStop = 0;
            c3_test.Text = "Test";
            if (checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c3_pulse.Text);
                int delay_value = Convert.ToInt32(c3_delay.Text);

                c3_test.Enabled = true;
                c3_error.ForeColor = Color.White;
                c3_error.Text = "Error: ";
                c3_test.Enabled = true;

                c3_pulse_value = pulse_value;
                c3_delay_value = delay_value;
            }
            else
            {
                c3_test.Enabled = false;
                c3_error.ForeColor = Color.Red;
                c3_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c3_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c3_delay_TextChanged(object sender, EventArgs e)
        {
            // Delay
            c3_testStop = 0;
            c3_test.Text = "Test";
            if (checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c3_pulse.Text);
                int delay_value = Convert.ToInt32(c3_delay.Text);

                c3_test.Enabled = true;
                c3_error.ForeColor = Color.White;
                c3_error.Text = "Error: ";
                c3_test.Enabled = true;

                c3_pulse_value = pulse_value;
                c3_delay_value = delay_value;
            }
            else
            {
                c3_test.Enabled = false;
                c3_error.ForeColor = Color.Red;
                c3_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c3_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c3_status_Click(object sender, EventArgs e)
        {

        }

        private void c3_test_Click(object sender, EventArgs e)
        {
            Thread c3_testing = new Thread(c3_light_loop);
            if (c3_testStop == 0)
            {
                c3_testing.Start();
                c3_test.Text = "Stop";
            }
            else
            {
                c3_testing.Abort();
                c3_test.Text = "Test";
            }
            c3_testStop++;
            if (c3_testStop == 2) { c3_testStop = 0; }
        }

        private void c3_error_Click(object sender, EventArgs e)
        {

        }

        private void c2_intensity_TextChanged_1(object sender, EventArgs e)
        {
            if (checkIntensity(c2_intensity.Text))
            {
                int rgb_value = getRGB(c2_intensity.Text);
                c2_status.BackColor = Color.FromArgb(0, rgb_value, 0);
                c2_error.ForeColor = Color.White;
                c2_error.Text = "Error: ";

                c2_rgb_value = rgb_value;

            }
            else
            {
                //Console.WriteLine("Invalid Value");
                c2_test.Enabled = false;
                c2_status.BackColor = Color.Transparent;
                c2_error.ForeColor = Color.Red;
                c2_error.Text = "Error: Invalid value inputted for intensity. Please use integers from 0 to 4096.";

            }

            if (checkIntensity(c1_intensity.Text) && checkIntensity(c2_intensity.Text) && checkIntensity(c3_intensity.Text) && checkIntensity(c4_intensity.Text) && checkIntensity(c5_intensity.Text) && checkIntensity(c6_intensity.Text) && checkIntensity(c7_intensity.Text) && checkIntensity(c8_intensity.Text) && checkIntensity(c9_intensity.Text) && checkIntensity(c10_intensity.Text) && checkIntensity(c11_intensity.Text) && checkIntensity(c12_intensity.Text) && checkIntensity(c13_intensity.Text) && checkIntensity(c14_intensity.Text) && checkIntensity(c15_intensity.Text) && checkIntensity(c16_intensity.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void g1_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            c1_testStop = 0;
            c1_test.Text = "Test";
            c2_testStop = 0;
            c2_test.Text = "Test";
            c3_testStop = 0;
            c3_test.Text = "Test";
            if (g1_setting.SelectedIndex == 0)
            {
                c2_pulse.ReadOnly = false;
                c2_delay.ReadOnly = false;
                c3_pulse.ReadOnly = false;
                c3_delay.ReadOnly = false;
                c2_mode.Enabled = true;
                c3_mode.Enabled = true;


                // Abort threads

            }
            else if (g1_setting.SelectedIndex == 1)
            {
                c2_pulse.ReadOnly = true;
                c2_delay.ReadOnly = true;
                c3_pulse.ReadOnly = true;
                c3_delay.ReadOnly = true;
                c2_mode.Enabled = false;
                c3_mode.Enabled = false;


                c2_pulse.Text = c1_pulse.Text;
                c3_pulse.Text = c1_pulse.Text;
                c2_delay.Text = c1_delay.Text;
                c3_delay.Text = c1_delay.Text;

                c2_mode.SelectedIndex = c1_mode.SelectedIndex;
                c3_mode.SelectedIndex = c1_mode.SelectedIndex;

            }
        }

        private void c1_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            c1_testStop = 0;
            c1_test.Text = "Test";
            
            
        }

        private void c2_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            c2_testStop = 0;
            c2_test.Text = "Test";
            
        }

        private void c3_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            c3_testStop = 0;
            c3_test.Text = "Test";
            
        }


        // COM Port Settings
        private void comPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            Console.WriteLine(portConn.PortName);
            if(comPort.Text == "")
            {
                uploadConfig.Enabled = false;
            }
            else
            {
                uploadConfig.Enabled = true;
            }
        }

        private void closeConn_Click(object sender, EventArgs e)
        {
            if (portConn.IsOpen)
            {
                portConn.Close();
                closeConn.Enabled = false;
                openConn.Enabled = true;
                uploadConfig.Enabled = false;
            }
            else
            {
                MessageBox.Show("Port is already closed");
            }
        }

        private void openConn_Click(object sender, EventArgs e)
        {
            if (comPort.Text == "")
            {
                MessageBox.Show("Please select a COM port");
            }          
            else
            {
                portConn.Open();

                if (lightSelect.Text == "")
                {
                    closeConn.Enabled = true;
                    openConn.Enabled = false;
                }
                else
                {
                    closeConn.Enabled = true;
                    openConn.Enabled = false;
                    uploadConfig.Enabled = true;
                }
                
            }
            
        }

        delegate void SetTextCallback(string text);

        void updateConsole(string consoleData)
        {
            if (consoleDisplay.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(updateConsole);
                this.Invoke(d, new object[] { consoleData });
            }
            else
            {
                splitData.Clear();
                

                splitData = consoleData.Split('.').ToList();
                //Console.WriteLine(consoleData);

                foreach(string s in splitData)
                {
                    //Console.WriteLine(s);
                    consoleDisplay.Items.Add(s);
                }
            }
        }

        void receiveDataHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(3000);
                dataReceived = portConn.ReadExisting();
                Console.WriteLine(dataReceived);
                didDataReceiveThreadExit = true;
                updateConsole(dataReceived);
            }
            catch
            {

            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                portConn = new SerialPort();
                portConn.BaudRate = 115200;
                int counter = 0;

                while (comPort.Text == "" && counter < 3)
                {
                    foreach (string portName in SerialPort.GetPortNames())
                    {
                        Console.WriteLine(portName);
                        portConn.PortName = portName;
                        portConn.Open();

                        Thread.Sleep(1000);
                        string test = portConn.ReadExisting();
                        Console.WriteLine(test);

                        portConn.Write("QB\r\n");
                        Console.WriteLine("sent data");


                        Thread.Sleep(1200);
                        string reply = portConn.ReadExisting();
                        Console.WriteLine(reply);

                        if (reply.Contains("PICS"))
                        {
                            Console.WriteLine("port is open");
                            comPort.Text = portName;
                            portConn.DataReceived += new SerialDataReceivedEventHandler(receiveDataHandler);
                            enableConfig("filler");
                            break;
                        }
                        else
                        {
                            portConn.Close();
                            continue;
                        }
                    }
                    counter++;
                }

                if (counter >= 3)
                {
                    label101.Visible = true;
                    label101.Text = "USB not plugged in, plug it in";
                    label101.ForeColor = System.Drawing.Color.Red;
                    disableConfig("filler");
                }

                // Declare a ManagementEventWatcher object and set up the event handler
                ManagementEventWatcher deviceRemoveWatcher = new ManagementEventWatcher();
                ManagementEventWatcher deviceInsertionWatcher = new ManagementEventWatcher();

                deviceInsertionWatcher.EventArrived += new EventArrivedEventHandler(DeviceInsertedEvent);
                deviceRemoveWatcher.EventArrived += new EventArrivedEventHandler(DeviceRemovedEvent);

                // Set up the query for USB device removal
                WqlEventQuery removalQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 3");
                WqlEventQuery insertionQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");

                // Start listening for the USB device removal event
                deviceRemoveWatcher.Query = removalQuery;
                deviceInsertionWatcher.Query = insertionQuery;

                deviceInsertionWatcher.Start();
                deviceRemoveWatcher.Start();
            }
            catch
            {
                MessageBox.Show("App is already running");
                Application.Exit();
            }
        }

        private void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        {
            //changePortErrMsg("retry");
            Console.WriteLine("EVENT FIRING");
            bool checkUSB = checkIfUsbAlive();
            if (checkUSB == true)
            {
                changePortErrMsg("true");
                enableConfig("filler");
            }
            else
            {
                changePortErrMsg("retry");
            }

            return;
        }

        // Define the event handler method
        private void DeviceRemovedEvent(object sender, EventArrivedEventArgs e)
        {
            portConn.Close();
            bool checkUSB = checkIfUsbAlive();
            Console.WriteLine(checkUSB);
            if (checkUSB == false)
            {
                changePortErrMsg("false");
                disableConfig("filler");
            }
            
            return;
        }

        private void disableConfig(string filler)
        {
            if (mainTab.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(disableConfig);
                this.Invoke(d, new object[] { filler });
            }
            else
            {
                mainTab.Enabled = false;
            }

        }

        private void enableConfig(string filler)
        {
            if (mainTab.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(enableConfig);
                this.Invoke(d, new object[] { filler });
            }
            else
            {
                mainTab.Enabled = true;
            }

        }

        void changePortErrMsg(string data)
        {
            if (label101.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(changePortErrMsg);
                this.Invoke(d, new object[] { data });
            }
            else
            {
                if (data == "false")
                {
                    label101.Visible = true;
                    label101.ForeColor = Color.Red;
                    label101.Text = "USB has been unplugged, plug\nthe USB back in to edit config";
                    comPort.Text = "";
                    closeConn.Enabled = false;
                }
                else if (data == "true")
                {
                    
                    portError.Text = "";
                    label101.Text = "";
                    closeConn.Enabled = true;
                }
                else
                {
                    label101.Visible = true;
                    Console.WriteLine("RETRYING");
                    label101.Text = "Retrying...";
                }
            }
        }

        bool checkIfUsbAlive()
        {
            string reply = "";
            foreach (string portName in SerialPort.GetPortNames())
            {
                portConn.Close();

                Console.WriteLine(portName);
                portConn.PortName = portName;
                try
                {
                    portConn.Open();
                }
                catch
                {
                    continue;
                }
                portConn.Write("Q");
                Console.WriteLine("sent data");
                Thread.Sleep(50);

                try
                {
                    reply = portConn.ReadExisting();

                }
                catch
                {

                }

                Console.WriteLine(reply);
                if (reply == "Q")
                {
                    updateComPortTextbox(portName);
                    return true;
                }
            }
            return false;
        }

        void updateComPortTextbox(string port)
        {
            if (comPort.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(updateComPortTextbox);
                this.Invoke(d, new object[] { port });
            }
            else
            {
                comPort.Text = port;
                label101.Text = "";
            }
        }

        void detectNewComPort(object sender, EventArgs e)
        {
            Console.WriteLine("abcd");

            foreach (string portName in SerialPort.GetPortNames())
            {
                Console.WriteLine(portName);
                portConn.PortName = portName;
                portConn.Open();

                portConn.Write("Q");
                Console.WriteLine("sent data");
                Thread.Sleep(50);

                string reply = portConn.ReadExisting();



                Console.WriteLine(reply);
                if (reply == "Q")
                {
                    Console.WriteLine("port is open");
                    comPort.Text = portName;
                    portConn.DataReceived += new SerialDataReceivedEventHandler(receiveDataHandler);

                    break;
                }
                else
                {
                    portConn.Close();
                    continue;
                }
            }
        }

        // Main Controls 
        private void switchConfig_Click(object sender, EventArgs e)
        {
            openFile fileSelect = new openFile();
            fileSelect.mainForm = this;
            fileSelect.ShowDialog();
        }

        private void updateConfig_Click(object sender, EventArgs e)
        {
            //generateFileList();
            generateConfig();
            saveFile saveForm = new saveFile();
            saveForm.mainForm = this;
            saveForm.ShowDialog();
        }

        public void generateConfig()
        {
            string c1_addText = displayValues("CH1", c1_intensity.Text, c1_edge.Text, c1_mode.Text, c1_strobe.Text, c1_pulse.Text, c1_delay.Text);
            string c2_addText = displayValues("CH2", c2_intensity.Text, c2_edge.Text, c2_mode.Text, c2_strobe.Text, c2_pulse.Text, c2_delay.Text);
            string c3_addText = displayValues("CH3", c3_intensity.Text, c3_edge.Text, c3_mode.Text, c3_strobe.Text, c3_pulse.Text, c3_delay.Text);
            string c4_addText = displayValues("CH4", c4_intensity.Text, c4_edge.Text, c4_mode.Text, c4_strobe.Text, c4_pulse.Text, c4_delay.Text);
            string c5_addText = displayValues("CH5", c5_intensity.Text, c5_edge.Text, c5_mode.Text, c5_strobe.Text, c5_pulse.Text, c5_delay.Text);
            string c6_addText = displayValues("CH6", c6_intensity.Text, c6_edge.Text, c6_mode.Text, c6_strobe.Text, c6_pulse.Text, c6_delay.Text);
            string c7_addText = displayValues("CH7", c7_intensity.Text, c7_edge.Text, c7_mode.Text, c7_strobe.Text, c7_pulse.Text, c7_delay.Text);
            string c8_addText = displayValues("CH8", c8_intensity.Text, c8_edge.Text, c8_mode.Text, c8_strobe.Text, c8_pulse.Text, c8_delay.Text);
            string c9_addText = displayValues("CH9", c9_intensity.Text, c9_edge.Text, c9_mode.Text, c9_strobe.Text, c9_pulse.Text, c9_delay.Text);
            string c10_addText = displayValues("CH10", c10_intensity.Text, c10_edge.Text, c10_mode.Text, c10_strobe.Text, c10_pulse.Text, c10_delay.Text);
            string c11_addText = displayValues("CH11", c11_intensity.Text, c11_edge.Text, c11_mode.Text, c11_strobe.Text, c11_pulse.Text, c11_delay.Text);
            string c12_addText = displayValues("CH12", c12_intensity.Text, c12_edge.Text, c12_mode.Text, c12_strobe.Text, c12_pulse.Text, c12_delay.Text);
            string c13_addText = displayValues("CH13", c13_intensity.Text, c13_edge.Text, c13_mode.Text, c13_strobe.Text, c13_pulse.Text, c13_delay.Text);
            string c14_addText = displayValues("CH14", c14_intensity.Text, c14_edge.Text, c14_mode.Text, c14_strobe.Text, c14_pulse.Text, c14_delay.Text);
            string c15_addText = displayValues("CH15", c15_intensity.Text, c15_edge.Text, c15_mode.Text, c15_strobe.Text, c15_pulse.Text, c15_delay.Text);
            string c16_addText = displayValues("CH16", c16_intensity.Text, c16_edge.Text, c16_mode.Text, c16_strobe.Text, c16_pulse.Text, c16_delay.Text);

            string g1_addText = displaySettings(g1_setting.Text, "Group 1", "CH1", "CH2", "CH3");
            string g2_addText = displaySettings(g2_setting.Text, "Group 2", "CH4", "CH5", "CH6");
            string g3_addText = displaySettings(g3_setting.Text, "Group 3", "CH7", "CH8", "CH9");
            string g4_addText = displaySettings(g4_setting.Text, "Group 4", "CH10", "CH11", "CH12");
            string g5_addText = displaySettings(g5_setting.Text, "Group 5", "CH13", "CH14", "CH15");
            string g6_addText = $"\nGroup 6 Settings, [UNGROUPED Red:CH16]";

            //uses global list to add data and generate the string to send to the hardware
            config.Clear();
            sendToHardware = "";

            config.Add("MAIN BOARD = " + lightSelect.Text);

            config.Add(g1_addText);
            config.Add(c1_addText);
            config.Add(c2_addText);
            config.Add(c3_addText);

            config.Add(g2_addText);
            config.Add(c4_addText);
            config.Add(c5_addText);
            config.Add(c6_addText);

            config.Add(g3_addText);
            config.Add(c7_addText);
            config.Add(c8_addText);
            config.Add(c9_addText);

            config.Add(g4_addText);
            config.Add(c10_addText);
            config.Add(c11_addText);
            config.Add(c12_addText);

            config.Add(g5_addText);
            config.Add(c13_addText);
            config.Add(c14_addText);
            config.Add(c15_addText);

            config.Add(g6_addText);
            config.Add(c16_addText);

            for (int i = 0; i < config.Count(); i++)
            {
                sendToHardware += config[i].ToString();
            }
            sendToHardware += "\n\\r\\n";

            Console.WriteLine(sendToHardware);
        }

        private void uploadConfig_Click(object sender, EventArgs e)
        {
            try
            {
                //Grab Values
                generateConfig();
                string board = sendToHardware.Substring(13, 7);

                string path = @"..\..\savedConfigs";

                path += "\\" + board + ".txt";

                File.WriteAllText(path, sendToHardware);

                //clear console
                consoleDisplay.Items.Clear();
                
                Console.WriteLine(sendToHardware);

                uploadConfig.Enabled = false;
                Thread sendData = new Thread(sendDataToHardware);
                sendData.Start();
            }
            catch
            {
                Console.WriteLine("Device disconnected");
                consoleDisplay.Items.Add("Device is not connected");
            }


        }

        

        void sendDataToHardware()
        {
            Console.WriteLine("test");

            try
            {
                portConn.Write(sendToHardware);

                Thread.Sleep(1000);
                enableUploadBtn("filler");
                   
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please select a COM Port");
            }
            

            Thread.CurrentThread.Abort();
        }

        void enableUploadBtn(string filler)
        {
            if (uploadConfig.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(enableUploadBtn);
                this.Invoke(d, new object[] { filler });
            }
            else
            {
                uploadConfig.Enabled = true;
            }
        }

        void intensity_set()
        {
            portConn.Write("IS " + lightSelect.Text + ", " + "1, " + c1_intensity + "\r\n");
            portConn.Write("IS " + lightSelect.Text + ", " + "2, " + c2_intensity + "\r\n");
            portConn.Write("IS " + lightSelect.Text + ", " + "3, " + c3_intensity + "\r\n");
            portConn.Write("IS " + lightSelect.Text + ", " + "4, " + c4_intensity + "\r\n");
            portConn.Write("IS " + lightSelect.Text + ", " + "5, " + c5_intensity + "\r\n");
            portConn.Write("IS " + lightSelect.Text + ", " + "6, " + c6_intensity + "\r\n");
            portConn.Write("IS " + lightSelect.Text + ", " + "7, " + c7_intensity + "\r\n");
            portConn.Write("IS " + lightSelect.Text + ", " + "8, " + c8_intensity + "\r\n");
            Console.WriteLine(portConn.ReadExisting());

        }

        void mode_set()
        {
            portConn.Write("MS " + lightSelect.Text + ", 1, " + c1_mode.SelectedIndex.ToString() + "\r\n");
            portConn.Write("MS " + lightSelect.Text + ", 2, " + c2_mode.SelectedIndex.ToString() + "\r\n");
            portConn.Write("MS " + lightSelect.Text + ", 3, " + c3_mode.SelectedIndex.ToString() + "\r\n");
            portConn.Write("MS " + lightSelect.Text + ", 4, " + c4_mode.SelectedIndex.ToString() + "\r\n");
            portConn.Write("MS " + lightSelect.Text + ", 5, " + c5_mode.SelectedIndex.ToString() + "\r\n");
            portConn.Write("MS " + lightSelect.Text + ", 6, " + c6_mode.SelectedIndex.ToString() + "\r\n");
            portConn.Write("MS " + lightSelect.Text + ", 7, " + c7_mode.SelectedIndex.ToString() + "\r\n");
            portConn.Write("MS " + lightSelect.Text + ", 8, " + c8_mode.SelectedIndex.ToString() + "\r\n");
            Console.WriteLine(portConn.ReadExisting());

        }

        void edge_set()
        {
            portConn.Write("ES " + lightSelect.Text + ", 1, " + c1_edge + "\r\n");
            portConn.Write("ES " + lightSelect.Text + ", 2, " + c2_edge + "\r\n");
            portConn.Write("ES " + lightSelect.Text + ", 3, " + c3_edge + "\r\n");
            portConn.Write("ES " + lightSelect.Text + ", 4, " + c4_edge + "\r\n");
            portConn.Write("ES " + lightSelect.Text + ", 5, " + c5_edge + "\r\n");
            portConn.Write("ES " + lightSelect.Text + ", 6, " + c6_edge + "\r\n");
            portConn.Write("ES " + lightSelect.Text + ", 7, " + c7_edge + "\r\n");
            portConn.Write("ES " + lightSelect.Text + ", 8, " + c8_edge + "\r\n");
            Console.WriteLine(portConn.ReadExisting());

        }

        void strobe_set()
        {
            portConn.Write("SS " + lightSelect.Text + ", 1, " + c1_strobe + "\r\n");
            portConn.Write("SS " + lightSelect.Text + ", 2, " + c2_strobe + "\r\n");
            portConn.Write("SS " + lightSelect.Text + ", 3, " + c3_strobe + "\r\n");
            portConn.Write("SS " + lightSelect.Text + ", 4, " + c4_strobe + "\r\n");
            portConn.Write("SS " + lightSelect.Text + ", 5, " + c5_strobe + "\r\n");
            portConn.Write("SS " + lightSelect.Text + ", 6, " + c6_strobe + "\r\n");
            portConn.Write("SS " + lightSelect.Text + ", 7, " + c7_strobe + "\r\n");
            portConn.Write("SS " + lightSelect.Text + ", 8, " + c8_strobe + "\r\n");
            Console.WriteLine(portConn.ReadExisting());

        }

        void pulse_set()
        {
            portConn.Write("PS " + lightSelect.Text + ", 1, " + c1_pulse + "\r\n");
            portConn.Write("PS " + lightSelect.Text + ", 2, " + c2_pulse + "\r\n");
            portConn.Write("PS " + lightSelect.Text + ", 3, " + c3_pulse + "\r\n");
            portConn.Write("PS " + lightSelect.Text + ", 4, " + c4_pulse + "\r\n");
            portConn.Write("PS " + lightSelect.Text + ", 5, " + c5_pulse + "\r\n");
            portConn.Write("PS " + lightSelect.Text + ", 6, " + c6_pulse + "\r\n");
            portConn.Write("PS " + lightSelect.Text + ", 7, " + c7_pulse + "\r\n");
            portConn.Write("PS " + lightSelect.Text + ", 8, " + c8_pulse + "\r\n");
            Console.WriteLine(portConn.ReadExisting());

        }

        void delay_set()
        {
            portConn.Write("DS " + lightSelect.Text + ", 1, " + c1_delay + "\r\n");
            portConn.Write("DS " + lightSelect.Text + ", 2, " + c2_delay + "\r\n");
            portConn.Write("DS " + lightSelect.Text + ", 3, " + c3_delay + "\r\n");
            portConn.Write("DS " + lightSelect.Text + ", 4, " + c4_delay + "\r\n");
            portConn.Write("DS " + lightSelect.Text + ", 5, " + c5_delay + "\r\n");
            portConn.Write("DS " + lightSelect.Text + ", 6, " + c6_delay + "\r\n");
            portConn.Write("DS " + lightSelect.Text + ", 7, " + c7_delay + "\r\n");
            portConn.Write("DS " + lightSelect.Text + ", 8, " + c8_delay + "\r\n");
            Console.WriteLine(portConn.ReadExisting());

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        // Panel to Tab Mapping
        private void g1_panel_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void g2_panel_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void g3_panel_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void g4_panel_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void g5_panel_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void g2_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            c4_testStop = 0;
            c4_test.Text = "Test";
            c5_testStop = 0;
            c5_test.Text = "Test";
            c6_testStop = 0;
            c6_test.Text = "Test";
            if (g2_setting.SelectedIndex == 0)
            {
                c5_pulse.ReadOnly = false;
                c5_delay.ReadOnly = false;
                c6_pulse.ReadOnly = false;
                c6_delay.ReadOnly = false;
                c5_mode.Enabled = true;
                c6_mode.Enabled = true;
                // Abort threads

            }
            else if (g2_setting.SelectedIndex == 1)
            {
                c5_pulse.ReadOnly = true;
                c5_delay.ReadOnly = true;
                c6_pulse.ReadOnly = true;
                c6_delay.ReadOnly = true;
                c5_mode.Enabled = false;
                c6_mode.Enabled = false;


                c5_pulse.Text = c4_pulse.Text;
                c6_pulse.Text = c4_pulse.Text;
                c5_delay.Text = c4_delay.Text;
                c6_delay.Text = c4_delay.Text;

                c5_mode.SelectedIndex = c4_mode.SelectedIndex;
                c6_mode.SelectedIndex = c4_mode.SelectedIndex;

            }
        }

        private void c4_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            c4_testStop = 0;
            c4_test.Text = "Test";
            
        }

        private void c5_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            c5_testStop = 0;
            c5_test.Text = "Test";
            
        }

        private void c6_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            c6_testStop = 0;
            c6_test.Text = "Test";
            
        }

        private void g3_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            c7_testStop = 0;
            c7_test.Text = "Test";
            c8_testStop = 0;
            c8_test.Text = "Test";
            c9_testStop = 0;
            c9_test.Text = "Test";
            if (g3_setting.SelectedIndex == 0)
            {
                c8_pulse.ReadOnly = false;
                c8_delay.ReadOnly = false;
                c9_pulse.ReadOnly = false;
                c9_delay.ReadOnly = false;
                c8_mode.Enabled = true;
                c9_mode.Enabled = true;

                // Abort threads

            }
            else if (g3_setting.SelectedIndex == 1)
            {
                c8_pulse.ReadOnly = true;
                c8_delay.ReadOnly = true;
                c9_pulse.ReadOnly = true;
                c9_delay.ReadOnly = true;
                c8_mode.Enabled = false;
                c9_mode.Enabled = false;

                c8_pulse.Text = c7_pulse.Text;
                c9_pulse.Text = c7_pulse.Text;
                c8_delay.Text = c7_delay.Text;
                c9_delay.Text = c7_delay.Text;

                c8_mode.SelectedIndex = c7_mode.SelectedIndex;
                c9_mode.SelectedIndex = c7_mode.SelectedIndex;

            }
        }

        private void c7_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            c7_testStop = 0;
            c7_test.Text = "Test";
            
        }

        private void c8_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            c8_testStop = 0;
            c8_test.Text = "Test";
            
        }

        private void c9_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            c9_testStop = 0;
            c9_test.Text = "Test";
            
        }

        private void g4_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            c10_testStop = 0;
            c10_test.Text = "Test";
            c11_testStop = 0;
            c11_test.Text = "Test";
            c12_testStop = 0;
            c12_test.Text = "Test";
            if (g4_setting.SelectedIndex == 0)
            {
                c11_pulse.ReadOnly = false;
                c11_delay.ReadOnly = false;
                c12_pulse.ReadOnly = false;
                c12_delay.ReadOnly = false;
                c11_mode.Enabled = true;
                c12_mode.Enabled = true;

                // Abort threads

            }
            else if (g4_setting.SelectedIndex == 1)
            {
                c11_pulse.ReadOnly = true;
                c11_delay.ReadOnly = true;
                c12_pulse.ReadOnly = true;
                c12_delay.ReadOnly = true;
                c11_mode.Enabled = false;
                c12_mode.Enabled = false;

                c11_pulse.Text = c10_pulse.Text;
                c12_pulse.Text = c10_pulse.Text;
                c11_delay.Text = c10_delay.Text;
                c12_delay.Text = c10_delay.Text;

                
                c11_mode.SelectedIndex = c10_mode.SelectedIndex;
                c12_mode.SelectedIndex = c10_mode.SelectedIndex;
            }
        }

        private void c10_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            c10_testStop = 0;
            c10_test.Text = "Test";
            
        }

        private void c11_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            c11_testStop = 0;
            c11_test.Text = "Test";
            
        }

        private void c12_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            c12_testStop = 0;
            c12_test.Text = "Test";
            
        }

        private void g5_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            c13_testStop = 0;
            c13_test.Text = "Test";
            c14_testStop = 0;
            c14_test.Text = "Test";
            c15_testStop = 0;
            c15_test.Text = "Test";
            if (g5_setting.SelectedIndex == 0)
            {
                c14_pulse.ReadOnly = false;
                c14_delay.ReadOnly = false;
                c15_pulse.ReadOnly = false;
                c15_delay.ReadOnly = false;
                c14_mode.Enabled = true;
                c15_mode.Enabled = true;

                // Abort threads

            }
            else if (g5_setting.SelectedIndex == 1)
            {
                c14_pulse.ReadOnly = true;
                c14_delay.ReadOnly = true;
                c15_pulse.ReadOnly = true;
                c15_delay.ReadOnly = true;
                c14_mode.Enabled = false;
                c15_mode.Enabled = false;

                c14_pulse.Text = c13_pulse.Text;
                c15_pulse.Text = c13_pulse.Text;
                c14_delay.Text = c13_delay.Text;
                c15_delay.Text = c13_delay.Text;

                c14_mode.SelectedIndex = c13_mode.SelectedIndex;
                c15_mode.SelectedIndex = c13_mode.SelectedIndex;

            }
        }

        private void c13_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            c13_testStop = 0;
            c13_test.Text = "Test";
            
        }

        private void c14_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            c14_testStop = 0;
            c14_test.Text = "Test";
            
        }

        private void c15_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            c15_testStop = 0;
            c15_test.Text = "Test";
            
        }

        private void c4_intensity_TextChanged(object sender, EventArgs e)
        {
            if (checkIntensity(c4_intensity.Text))
            {
                int rgb_value = getRGB(c4_intensity.Text);
                c4_status.BackColor = Color.FromArgb(rgb_value, 0, 0);
                c4_error.ForeColor = Color.White;
                c4_error.Text = "Error: ";

                c4_rgb_value = rgb_value;

            }
            else
            {
                c4_test.Enabled = false;
                c4_status.BackColor = Color.Transparent;
                c4_error.ForeColor = Color.Red;
                c4_error.Text = "Error: Invalid value inputted for intensity. Please use integers from 0 to 4096.";

            }

            if (checkIntensity(c1_intensity.Text) && checkIntensity(c2_intensity.Text) && checkIntensity(c3_intensity.Text) && checkIntensity(c4_intensity.Text) && checkIntensity(c5_intensity.Text) && checkIntensity(c6_intensity.Text) && checkIntensity(c7_intensity.Text) && checkIntensity(c8_intensity.Text) && checkIntensity(c9_intensity.Text) && checkIntensity(c10_intensity.Text) && checkIntensity(c11_intensity.Text) && checkIntensity(c12_intensity.Text) && checkIntensity(c13_intensity.Text) && checkIntensity(c14_intensity.Text) && checkIntensity(c15_intensity.Text) && checkIntensity(c16_intensity.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c4_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            c4_testStop = 0;
            c4_test.Text = "Test";
            if (g2_setting.SelectedIndex == 1)
            {
                c5_mode.SelectedIndex = c4_mode.SelectedIndex;
                c6_mode.SelectedIndex = c4_mode.SelectedIndex;
            }

            if (c4_mode.SelectedIndex == 1)
            {
                // Strobe is selected
                c4_strobe.Enabled = true;
                c4_edge.Enabled = true;
                c4_pulse.Enabled = true;
                c4_delay.Enabled = true;
            }
            else
            {
                //Static is selected
                c4_strobe.Enabled = false;
                c4_edge.Enabled = false;
                c4_pulse.Enabled = false;
                c4_delay.Enabled = false;
            }
        }

        private void c4_pulse_TextChanged(object sender, EventArgs e)
        {
            // Pulse
            c4_testStop = 0;
            c4_test.Text = "Test";
            if (checkPulse(c4_pulse.Text))
            {
                int pulse_value = Convert.ToInt32(c4_pulse.Text);
                //int delay_value = Convert.ToInt32(c1_delay.Text);

                c4_test.Enabled = true;
                c4_error.ForeColor = Color.White;
                c4_error.Text = "Error: ";
                c4_test.Enabled = true;

                c4_pulse_value = pulse_value;
                //c1_delay_value = delay_value;

                if (g2_setting.SelectedIndex == 1)
                {
                    c5_pulse_value = pulse_value;
                    c6_pulse_value = pulse_value;
                    c5_pulse.Text = c4_pulse.Text;
                    c6_pulse.Text = c4_pulse.Text;


                }
                else
                {

                }

            }

            else
            {
                c4_test.Enabled = false;
                c4_error.ForeColor = Color.Red;
                c4_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c4_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c4_delay_TextChanged(object sender, EventArgs e)
        {
            // Delay
            c4_testStop = 0;
            c4_test.Text = "Test";
            if (checkDelay(c4_delay.Text))
            {
                //int pulse_value = Convert.ToInt32(c1_pulse.Text);
                int delay_value = Convert.ToInt32(c4_delay.Text);

                c4_test.Enabled = true;
                c4_error.ForeColor = Color.White;
                c4_error.Text = "Error: ";
                c4_test.Enabled = true;

                //c1_pulse_value = pulse_value;
                c4_delay_value = delay_value;
                if (g2_setting.SelectedIndex == 1)
                {
                    c5_delay_value = delay_value;
                    c6_delay_value = delay_value;
                    c5_delay.Text = c4_delay.Text;
                    c6_delay.Text = c4_delay.Text;
                }
                else
                {

                }
            }
            else
            {
                c4_test.Enabled = false;
                c4_error.ForeColor = Color.Red;
                c4_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c4_test.Enabled = false;

            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c4_test_Click(object sender, EventArgs e)
        {
            Thread c4_testing = new Thread(c4_light_loop);
            if (c4_testStop == 0)
            {
                c4_testing.Start();
                c4_test.Text = "Stop";
            }
            else
            {
                c4_testing.Abort();
                c4_test.Text = "Test";
            }
            c4_testStop++;
            if (c4_testStop == 2) { c4_testStop = 0; }
        }

        private void c5_intensity_TextChanged(object sender, EventArgs e)
        {
            if (checkIntensity(c5_intensity.Text))
            {
                int rgb_value = getRGB(c5_intensity.Text);
                c5_status.BackColor = Color.FromArgb(0, rgb_value, 0);
                c5_error.ForeColor = Color.White;
                c5_error.Text = "Error: ";

                c5_rgb_value = rgb_value;

            }
            else
            {
                //Console.WriteLine("Invalid Value");
                c5_test.Enabled = false;
                c5_status.BackColor = Color.Transparent;
                c5_error.ForeColor = Color.Red;
                c5_error.Text = "Error: Invalid value inputted for intensity. Please use integers from 0 to 4096.";

            }

            if (checkIntensity(c1_intensity.Text) && checkIntensity(c2_intensity.Text) && checkIntensity(c3_intensity.Text) && checkIntensity(c4_intensity.Text) && checkIntensity(c5_intensity.Text) && checkIntensity(c6_intensity.Text) && checkIntensity(c7_intensity.Text) && checkIntensity(c8_intensity.Text) && checkIntensity(c9_intensity.Text) && checkIntensity(c10_intensity.Text) && checkIntensity(c11_intensity.Text) && checkIntensity(c12_intensity.Text) && checkIntensity(c13_intensity.Text) && checkIntensity(c14_intensity.Text) && checkIntensity(c15_intensity.Text) && checkIntensity(c16_intensity.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c5_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            c5_testStop = 0;
            c5_test.Text = "Test";
            if (c5_mode.SelectedIndex == 1)
            {
                // Strobe is selected
                c5_strobe.Enabled = true;
                c5_edge.Enabled = true;
                c5_pulse.Enabled = true;
                c5_delay.Enabled = true;
            }
            else
            {
                //Static is selected
                c5_strobe.Enabled = false;
                c5_edge.Enabled = false;
                c5_pulse.Enabled = false;
                c5_delay.Enabled = false;
            }
        }

        private void c5_pulse_TextChanged(object sender, EventArgs e)
        {
            // Pulse
            c5_testStop = 0;
            c5_test.Text = "Test";
            if (checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c5_pulse.Text);
                int delay_value = Convert.ToInt32(c5_delay.Text);

                c5_test.Enabled = true;
                c5_error.ForeColor = Color.White;
                c5_error.Text = "Error: ";
                c5_test.Enabled = true;

                c5_pulse_value = pulse_value;
                c5_delay_value = delay_value;

            }
            else
            {
                c5_test.Enabled = false;
                c5_error.ForeColor = Color.Red;
                c5_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c5_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c5_delay_TextChanged(object sender, EventArgs e)
        {
            // Delay
            c5_testStop = 0;
            c5_test.Text = "Test";
            if (checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c5_pulse.Text);
                int delay_value = Convert.ToInt32(c5_delay.Text);

                c5_test.Enabled = true;
                c5_error.ForeColor = Color.White;
                c5_error.Text = "Error: ";
                c5_test.Enabled = true;

                c5_pulse_value = pulse_value;
                c5_delay_value = delay_value;
            }
            else
            {
                c5_test.Enabled = false;
                c5_error.ForeColor = Color.Red;
                c5_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c5_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c5_test_Click(object sender, EventArgs e)
        {
            Thread c5_testing = new Thread(c5_light_loop);
            if (c5_testStop == 0)
            {
                c5_testing.Start();
                c5_test.Text = "Stop";
            }
            else
            {
                c5_testing.Abort();
                c5_test.Text = "Test";
            }
            c5_testStop++;
            if (c5_testStop == 2) { c5_testStop = 0; }
        }

        private void c6_intensity_TextChanged(object sender, EventArgs e)
        {
            if (checkIntensity(c6_intensity.Text))
            {
                int rgb_value = getRGB(c6_intensity.Text);
                c6_status.BackColor = Color.FromArgb(0, 0, rgb_value);
                c6_error.ForeColor = Color.White;
                c6_error.Text = "Error: ";

                c6_rgb_value = rgb_value;

            }
            else
            {
                //Console.WriteLine("Invalid Value");
                c6_test.Enabled = false;
                c6_status.BackColor = Color.Transparent;
                c6_error.ForeColor = Color.Red;
                c6_error.Text = "Error: Invalid value inputted for intensity. Please use integers from 0 to 4096.";

            }

            if (checkIntensity(c1_intensity.Text) && checkIntensity(c2_intensity.Text) && checkIntensity(c3_intensity.Text) && checkIntensity(c4_intensity.Text) && checkIntensity(c5_intensity.Text) && checkIntensity(c6_intensity.Text) && checkIntensity(c7_intensity.Text) && checkIntensity(c8_intensity.Text) && checkIntensity(c9_intensity.Text) && checkIntensity(c10_intensity.Text) && checkIntensity(c11_intensity.Text) && checkIntensity(c12_intensity.Text) && checkIntensity(c13_intensity.Text) && checkIntensity(c14_intensity.Text) && checkIntensity(c15_intensity.Text) && checkIntensity(c16_intensity.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c6_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            c6_testStop = 0;
            c6_test.Text = "Test";
            if (c6_mode.SelectedIndex == 1)
            {
                // Strobe is selected
                c6_strobe.Enabled = true;
                c6_edge.Enabled = true;
                c6_pulse.Enabled = true;
                c6_delay.Enabled = true;
            }
            else
            {
                //Static is selected
                c6_strobe.Enabled = false;
                c6_edge.Enabled = false;
                c6_pulse.Enabled = false;
                c6_delay.Enabled = false;
            }
        }

        private void c6_pulse_TextChanged(object sender, EventArgs e)
        {
            // Pulse
            c6_testStop = 0;
            c6_test.Text = "Test";
            if (checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c6_pulse.Text);
                int delay_value = Convert.ToInt32(c6_delay.Text);

                c6_test.Enabled = true;
                c6_error.ForeColor = Color.White;
                c6_error.Text = "Error: ";
                c6_test.Enabled = true;

                c6_pulse_value = pulse_value;
                c6_delay_value = delay_value;

            }
            else
            {
                c6_test.Enabled = false;
                c6_error.ForeColor = Color.Red;
                c6_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c6_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c6_delay_TextChanged(object sender, EventArgs e)
        {
            // Delay
            c6_testStop = 0;
            c6_test.Text = "Test";
            if (checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c6_pulse.Text);
                int delay_value = Convert.ToInt32(c6_delay.Text);

                c6_test.Enabled = true;
                c6_error.ForeColor = Color.White;
                c6_error.Text = "Error: ";
                c6_test.Enabled = true;

                c6_pulse_value = pulse_value;
                c6_delay_value = delay_value;
            }
            else
            {
                c6_test.Enabled = false;
                c6_error.ForeColor = Color.Red;
                c6_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c6_test.Enabled = false;

            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c6_test_Click(object sender, EventArgs e)
        {
            Thread c6_testing = new Thread(c6_light_loop);
            if (c6_testStop == 0)
            {
                c6_testing.Start();
                c6_test.Text = "Stop";
            }
            else
            {
                c6_testing.Abort();
                c6_test.Text = "Test";
            }
            c6_testStop++;
            if (c6_testStop == 2) { c6_testStop = 0; }
        }

        private void c7_intensity_TextChanged(object sender, EventArgs e)
        {
            if (checkIntensity(c7_intensity.Text))
            {
                int rgb_value = getRGB(c7_intensity.Text);
                c7_status.BackColor = Color.FromArgb(rgb_value, 0, 0);
                c7_error.ForeColor = Color.White;
                c7_error.Text = "Error: ";

                c7_rgb_value = rgb_value;

            }
            else
            {
                c7_test.Enabled = false;
                c7_status.BackColor = Color.Transparent;
                c7_error.ForeColor = Color.Red;
                c7_error.Text = "Error: Invalid value inputted for intensity. Please use integers from 0 to 4096.";

            }
            if (checkIntensity(c1_intensity.Text) && checkIntensity(c2_intensity.Text) && checkIntensity(c3_intensity.Text) && checkIntensity(c4_intensity.Text) && checkIntensity(c5_intensity.Text) && checkIntensity(c6_intensity.Text) && checkIntensity(c7_intensity.Text) && checkIntensity(c8_intensity.Text) && checkIntensity(c9_intensity.Text) && checkIntensity(c10_intensity.Text) && checkIntensity(c11_intensity.Text) && checkIntensity(c12_intensity.Text) && checkIntensity(c13_intensity.Text) && checkIntensity(c14_intensity.Text) && checkIntensity(c15_intensity.Text) && checkIntensity(c16_intensity.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c7_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            c7_testStop = 0;
            c7_test.Text = "Test";
            if (g3_setting.SelectedIndex == 1)
            {
                c8_mode.SelectedIndex = c7_mode.SelectedIndex;
                c9_mode.SelectedIndex = c7_mode.SelectedIndex;
            }

            if (c7_mode.SelectedIndex == 1)
            {
                // Strobe is selected
                c7_strobe.Enabled = true;
                c7_edge.Enabled = true;
                c7_pulse.Enabled = true;
                c7_delay.Enabled = true;
            }
            else
            {
                //Static is selected
                c7_strobe.Enabled = false;
                c7_edge.Enabled = false;
                c7_pulse.Enabled = false;
                c7_delay.Enabled = false;
            }
        }

        private void c7_pulse_TextChanged(object sender, EventArgs e)
        {
            // Pulse
            c7_testStop = 0;
            c7_test.Text = "Test";
            if (checkPulse(c7_pulse.Text))
            {
                int pulse_value = Convert.ToInt32(c7_pulse.Text);
                //int delay_value = Convert.ToInt32(c1_delay.Text);

                c7_test.Enabled = true;
                c7_error.ForeColor = Color.White;
                c7_error.Text = "Error: ";
                c7_test.Enabled = true;

                c7_pulse_value = pulse_value;
                //c1_delay_value = delay_value;

                if (g3_setting.SelectedIndex == 1)
                {
                    c8_pulse_value = pulse_value;
                    c9_pulse_value = pulse_value;
                    c8_pulse.Text = c7_pulse.Text;
                    c9_pulse.Text = c7_pulse.Text;


                }
                else
                {

                }

            }
            else
            {
                c7_test.Enabled = false;
                c7_error.ForeColor = Color.Red;
                c7_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c7_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c7_delay_TextChanged(object sender, EventArgs e)
        {
            // Delay
            c7_testStop = 0;
            c7_test.Text = "Test";
            if (checkDelay(c7_delay.Text))
            {
                //int pulse_value = Convert.ToInt32(c1_pulse.Text);
                int delay_value = Convert.ToInt32(c7_delay.Text);

                c7_test.Enabled = true;
                c7_error.ForeColor = Color.White;
                c7_error.Text = "Error: ";
                c7_test.Enabled = true;

                //c1_pulse_value = pulse_value;
                c7_delay_value = delay_value;
                if (g3_setting.SelectedIndex == 1)
                {


                    c8_delay_value = delay_value;
                    c9_delay_value = delay_value;
                    c8_delay.Text = c7_delay.Text;
                    c9_delay.Text = c7_delay.Text;
                }
                else
                {

                }
            }
            else
            {
                c7_test.Enabled = false;
                c7_error.ForeColor = Color.Red;
                c7_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c7_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c7_test_Click(object sender, EventArgs e)
        {
            Thread c7_testing = new Thread(c7_light_loop);
            if (c7_testStop == 0)
            {
                c7_testing.Start();
                c7_test.Text = "Stop";
            }
            else
            {
                c7_testing.Abort();
                c7_test.Text = "Test";
            }
            c7_testStop++;
            if (c7_testStop == 2) { c7_testStop = 0; }
        }

        private void c8_intensity_TextChanged(object sender, EventArgs e)
        {
            if (checkIntensity(c8_intensity.Text))
            {
                int rgb_value = getRGB(c8_intensity.Text);
                c8_status.BackColor = Color.FromArgb(0, rgb_value, 0);
                c8_error.ForeColor = Color.White;
                c8_error.Text = "Error: ";

                c8_rgb_value = rgb_value;

            }
            else
            {
                //Console.WriteLine("Invalid Value");
                c8_test.Enabled = false;
                c8_status.BackColor = Color.Transparent;
                c8_error.ForeColor = Color.Red;
                c8_error.Text = "Error: Invalid value inputted for intensity. Please use integers from 0 to 4096.";

            }

            if (checkIntensity(c1_intensity.Text) && checkIntensity(c2_intensity.Text) && checkIntensity(c3_intensity.Text) && checkIntensity(c4_intensity.Text) && checkIntensity(c5_intensity.Text) && checkIntensity(c6_intensity.Text) && checkIntensity(c7_intensity.Text) && checkIntensity(c8_intensity.Text) && checkIntensity(c9_intensity.Text) && checkIntensity(c10_intensity.Text) && checkIntensity(c11_intensity.Text) && checkIntensity(c12_intensity.Text) && checkIntensity(c13_intensity.Text) && checkIntensity(c14_intensity.Text) && checkIntensity(c15_intensity.Text) && checkIntensity(c16_intensity.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c8_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            c8_testStop = 0;
            c8_test.Text = "Test";
            if (c8_mode.SelectedIndex == 1)
            {
                // Strobe is selected
                c8_strobe.Enabled = true;
                c8_edge.Enabled = true;
                c8_pulse.Enabled = true;
                c8_delay.Enabled = true;
            }
            else
            {
                //Static is selected
                c8_strobe.Enabled = false;
                c8_edge.Enabled = false;
                c8_pulse.Enabled = false;
                c8_delay.Enabled = false;
            }
        }

        private void c8_pulse_TextChanged(object sender, EventArgs e)
        {
            // Pulse
            c8_testStop = 0;
            c8_test.Text = "Test";
            if (checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c8_pulse.Text);
                int delay_value = Convert.ToInt32(c8_delay.Text);

                c8_test.Enabled = true;
                c8_error.ForeColor = Color.White;
                c8_error.Text = "Error: ";
                c8_test.Enabled = true;

                c8_pulse_value = pulse_value;
                c8_delay_value = delay_value;

            }
            else
            {
                c8_test.Enabled = false;
                c8_error.ForeColor = Color.Red;
                c8_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c8_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c8_delay_TextChanged(object sender, EventArgs e)
        {
            // Delay
            c8_testStop = 0;
            c8_test.Text = "Test";
            if (checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c8_pulse.Text);
                int delay_value = Convert.ToInt32(c8_delay.Text);

                c8_test.Enabled = true;
                c8_error.ForeColor = Color.White;
                c8_error.Text = "Error: ";
                c8_test.Enabled = true;

                c8_pulse_value = pulse_value;
                c8_delay_value = delay_value;
            }
            else
            {
                c8_test.Enabled = false;
                c8_error.ForeColor = Color.Red;
                c8_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c8_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c8_test_Click(object sender, EventArgs e)
        {
            Thread c8_testing = new Thread(c8_light_loop);
            if (c8_testStop == 0)
            {
                c8_testing.Start();
                c8_test.Text = "Stop";
            }
            else
            {
                c8_testing.Abort();
                c8_test.Text = "Test";
            }
            c8_testStop++;
            if (c8_testStop == 2) { c8_testStop = 0; }
        }

        private void c9_intensity_TextChanged(object sender, EventArgs e)
        {
            if (checkIntensity(c9_intensity.Text))
            {
                int rgb_value = getRGB(c9_intensity.Text);
                c9_status.BackColor = Color.FromArgb(0, 0, rgb_value);
                c9_error.ForeColor = Color.White;
                c9_error.Text = "Error: ";

                c9_rgb_value = rgb_value;

            }
            else
            {
                //Console.WriteLine("Invalid Value");
                c9_test.Enabled = false;
                c9_status.BackColor = Color.Transparent;
                c9_error.ForeColor = Color.Red;
                c9_error.Text = "Error: Invalid value inputted for intensity. Please use integers from 0 to 4096.";

            }

            if (checkIntensity(c1_intensity.Text) && checkIntensity(c2_intensity.Text) && checkIntensity(c3_intensity.Text) && checkIntensity(c4_intensity.Text) && checkIntensity(c5_intensity.Text) && checkIntensity(c6_intensity.Text) && checkIntensity(c7_intensity.Text) && checkIntensity(c8_intensity.Text) && checkIntensity(c9_intensity.Text) && checkIntensity(c10_intensity.Text) && checkIntensity(c11_intensity.Text) && checkIntensity(c12_intensity.Text) && checkIntensity(c13_intensity.Text) && checkIntensity(c14_intensity.Text) && checkIntensity(c15_intensity.Text) && checkIntensity(c16_intensity.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c9_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            c9_testStop = 0;
            c9_test.Text = "Test";
            if (c9_mode.SelectedIndex == 1)
            {
                // Strobe is selected
                c9_strobe.Enabled = true;
                c9_edge.Enabled = true;
                c9_pulse.Enabled = true;
                c9_delay.Enabled = true;
            }
            else
            {
                //Static is selected
                c9_strobe.Enabled = false;
                c9_edge.Enabled = false;
                c9_pulse.Enabled = false;
                c9_delay.Enabled = false;
            }
        }

        private void c9_pulse_TextChanged(object sender, EventArgs e)
        {
            // Pulse
            c9_testStop = 0;
            c9_test.Text = "Test";
            if (checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c9_pulse.Text);
                int delay_value = Convert.ToInt32(c9_delay.Text);

                c9_test.Enabled = true;
                c9_error.ForeColor = Color.White;
                c9_error.Text = "Error: ";
                c9_test.Enabled = true;

                c9_pulse_value = pulse_value;
                c9_delay_value = delay_value;

            }
            else
            {
                c9_test.Enabled = false;
                c9_error.ForeColor = Color.Red;
                c9_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c9_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c9_delay_TextChanged(object sender, EventArgs e)
        {
            // Delay
            c9_testStop = 0;
            c9_test.Text = "Test";
            if (checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c9_pulse.Text);
                int delay_value = Convert.ToInt32(c9_delay.Text);

                c9_test.Enabled = true;
                c9_error.ForeColor = Color.White;
                c9_error.Text = "Error: ";
                c9_test.Enabled = true;

                c9_pulse_value = pulse_value;
                c9_delay_value = delay_value;

            }
            else
            {
                c9_test.Enabled = false;
                c9_error.ForeColor = Color.Red;
                c9_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c9_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c9_test_Click(object sender, EventArgs e)
        {
            Thread c9_testing = new Thread(c9_light_loop);
            if (c9_testStop == 0)
            {
                c9_testing.Start();
                c9_test.Text = "Stop";
            }
            else
            {
                c9_testing.Abort();
                c9_test.Text = "Test";
            }
            c9_testStop++;
            if (c9_testStop == 2) { c9_testStop = 0; }
        }

        private void c10_intensity_TextChanged(object sender, EventArgs e)
        {
            if (checkIntensity(c10_intensity.Text))
            {
                int rgb_value = getRGB(c10_intensity.Text);
                c10_status.BackColor = Color.FromArgb(rgb_value, 0, 0);
                c10_error.ForeColor = Color.White;
                c10_error.Text = "Error: ";

                c10_rgb_value = rgb_value;

            }
            else
            {
                c10_test.Enabled = false;
                c10_status.BackColor = Color.Transparent;
                c10_error.ForeColor = Color.Red;
                c10_error.Text = "Error: Invalid value inputted for intensity. Please use integers from 0 to 4096.";

            }
            if (checkIntensity(c1_intensity.Text) && checkIntensity(c2_intensity.Text) && checkIntensity(c3_intensity.Text) && checkIntensity(c4_intensity.Text) && checkIntensity(c5_intensity.Text) && checkIntensity(c6_intensity.Text) && checkIntensity(c7_intensity.Text) && checkIntensity(c8_intensity.Text) && checkIntensity(c9_intensity.Text) && checkIntensity(c10_intensity.Text) && checkIntensity(c11_intensity.Text) && checkIntensity(c12_intensity.Text) && checkIntensity(c13_intensity.Text) && checkIntensity(c14_intensity.Text) && checkIntensity(c15_intensity.Text) && checkIntensity(c16_intensity.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c10_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            c10_testStop = 0;
            c10_test.Text = "Test";
            if (g4_setting.SelectedIndex == 1)
            {
                c11_mode.SelectedIndex = c10_mode.SelectedIndex;
                c12_mode.SelectedIndex = c10_mode.SelectedIndex;
            }

            if (c10_mode.SelectedIndex == 1)
            {
                // Strobe is selected
                c10_strobe.Enabled = true;
                c10_edge.Enabled = true;
                c10_pulse.Enabled = true;
                c10_delay.Enabled = true;
            }
            else
            {
                //Static is selected
                c10_strobe.Enabled = false;
                c10_edge.Enabled = false;
                c10_pulse.Enabled = false;
                c10_delay.Enabled = false;
            }
        }

        private void c10_pulse_TextChanged(object sender, EventArgs e)
        {
            // Pulse
            c10_testStop = 0;
            c10_test.Text = "Test";
            if (checkPulse(c10_pulse.Text))
            {
                int pulse_value = Convert.ToInt32(c10_pulse.Text);
                //int delay_value = Convert.ToInt32(c1_delay.Text);

                c10_test.Enabled = true;
                c10_error.ForeColor = Color.White;
                c10_error.Text = "Error: ";
                c10_test.Enabled = true;

                c10_pulse_value = pulse_value;
                //c1_delay_value = delay_value;

                if (g4_setting.SelectedIndex == 1)
                {
                    c11_pulse_value = pulse_value;
                    c12_pulse_value = pulse_value;
                    c11_pulse.Text = c10_pulse.Text;
                    c12_pulse.Text = c10_pulse.Text;


                }
                else
                {

                }

            }
            else
            {
                c10_test.Enabled = false;
                c10_error.ForeColor = Color.Red;
                c10_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c10_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c10_delay_TextChanged(object sender, EventArgs e)
        {
            // Delay
            c10_testStop = 0;
            c10_test.Text = "Test";
            if (checkDelay(c10_delay.Text))
            {
                //int pulse_value = Convert.ToInt32(c1_pulse.Text);
                int delay_value = Convert.ToInt32(c10_delay.Text);

                c10_test.Enabled = true;
                c10_error.ForeColor = Color.White;
                c10_error.Text = "Error: ";
                c10_test.Enabled = true;

                //c1_pulse_value = pulse_value;
                c10_delay_value = delay_value;
                if (g4_setting.SelectedIndex == 1)
                {


                    c11_delay_value = delay_value;
                    c12_delay_value = delay_value;
                    c11_delay.Text = c10_delay.Text;
                    c12_delay.Text = c10_delay.Text;
                }
                else
                {

                }
            }
            else
            {
                c10_test.Enabled = false;
                c10_error.ForeColor = Color.Red;
                c10_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c10_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c10_test_Click(object sender, EventArgs e)
        {
            Thread c10_testing = new Thread(c10_light_loop);
            if (c10_testStop == 0)
            {
                c10_testing.Start();
                c10_test.Text = "Stop";
            }
            else
            {
                c10_testing.Abort();
                c10_test.Text = "Test";
            }
            c10_testStop++;
            if (c10_testStop == 2) { c10_testStop = 0; }
        }

        private void c11_intensity_TextChanged(object sender, EventArgs e)
        {
            if (checkIntensity(c11_intensity.Text))
            {
                int rgb_value = getRGB(c11_intensity.Text);
                c11_status.BackColor = Color.FromArgb(0, rgb_value, 0);
                c11_error.ForeColor = Color.White;
                c11_error.Text = "Error: ";

                c11_rgb_value = rgb_value;

            }
            else
            {
                //Console.WriteLine("Invalid Value");
                c11_test.Enabled = false;
                c11_status.BackColor = Color.Transparent;
                c11_error.ForeColor = Color.Red;
                c11_error.Text = "Error: Invalid value inputted for intensity. Please use integers from 0 to 4096.";

            }
            if (checkIntensity(c1_intensity.Text) && checkIntensity(c2_intensity.Text) && checkIntensity(c3_intensity.Text) && checkIntensity(c4_intensity.Text) && checkIntensity(c5_intensity.Text) && checkIntensity(c6_intensity.Text) && checkIntensity(c7_intensity.Text) && checkIntensity(c8_intensity.Text) && checkIntensity(c9_intensity.Text) && checkIntensity(c10_intensity.Text) && checkIntensity(c11_intensity.Text) && checkIntensity(c12_intensity.Text) && checkIntensity(c13_intensity.Text) && checkIntensity(c14_intensity.Text) && checkIntensity(c15_intensity.Text) && checkIntensity(c16_intensity.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c11_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            c11_testStop = 0;
            c11_test.Text = "Test";
            if (c11_mode.SelectedIndex == 1)
            {
                // Strobe is selected
                c11_strobe.Enabled = true;
                c11_edge.Enabled = true;
                c11_pulse.Enabled = true;
                c11_delay.Enabled = true;
            }
            else
            {
                //Static is selected
                c11_strobe.Enabled = false;
                c11_edge.Enabled = false;
                c11_pulse.Enabled = false;
                c11_delay.Enabled = false;
            }
        }

        private void c11_pulse_TextChanged(object sender, EventArgs e)
        {
            // Pulse
            c11_testStop = 0;
            c11_test.Text = "Test";
            if (checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c11_pulse.Text);
                int delay_value = Convert.ToInt32(c11_delay.Text);

                c11_test.Enabled = true;
                c11_error.ForeColor = Color.White;
                c11_error.Text = "Error: ";
                c11_test.Enabled = true;

                c11_pulse_value = pulse_value;
                c11_delay_value = delay_value;

            }
            else
            {
                c11_test.Enabled = false;
                c11_error.ForeColor = Color.Red;
                c11_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c11_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c11_delay_TextChanged(object sender, EventArgs e)
        {
            // Delay
            c11_testStop = 0;
            c11_test.Text = "Test";
            if (checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c11_pulse.Text);
                int delay_value = Convert.ToInt32(c11_delay.Text);

                c11_test.Enabled = true;
                c11_error.ForeColor = Color.White;
                c11_error.Text = "Error: ";
                c11_test.Enabled = true;

                c11_pulse_value = pulse_value;
                c11_delay_value = delay_value;

            }
            else
            {
                c11_test.Enabled = false;
                c11_error.ForeColor = Color.Red;
                c11_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c11_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c11_test_Click(object sender, EventArgs e)
        {
            Thread c11_testing = new Thread(c11_light_loop);
            if (c11_testStop == 0)
            {
                c11_testing.Start();
                c11_test.Text = "Stop";
            }
            else
            {
                c11_testing.Abort();
                c11_test.Text = "Test";
            }
            c11_testStop++;
            if (c11_testStop == 2) { c11_testStop = 0; }
        }

        private void c12_intensity_TextChanged(object sender, EventArgs e)
        {
            if (checkIntensity(c12_intensity.Text))
            {
                int rgb_value = getRGB(c12_intensity.Text);
                c12_status.BackColor = Color.FromArgb(0, 0, rgb_value);
                c12_error.ForeColor = Color.White;
                c12_error.Text = "Error: ";

                c12_rgb_value = rgb_value;

            }
            else
            {
                //Console.WriteLine("Invalid Value");
                c12_test.Enabled = false;
                c12_status.BackColor = Color.Transparent;
                c12_error.ForeColor = Color.Red;
                c12_error.Text = "Error: Invalid value inputted for intensity. Please use integers from 0 to 4096.";

            }
            if (checkIntensity(c1_intensity.Text) && checkIntensity(c2_intensity.Text) && checkIntensity(c3_intensity.Text) && checkIntensity(c4_intensity.Text) && checkIntensity(c5_intensity.Text) && checkIntensity(c6_intensity.Text) && checkIntensity(c7_intensity.Text) && checkIntensity(c8_intensity.Text) && checkIntensity(c9_intensity.Text) && checkIntensity(c10_intensity.Text) && checkIntensity(c11_intensity.Text) && checkIntensity(c12_intensity.Text) && checkIntensity(c13_intensity.Text) && checkIntensity(c14_intensity.Text) && checkIntensity(c15_intensity.Text) && checkIntensity(c16_intensity.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c12_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            c12_testStop = 0;
            c12_test.Text = "Test";
            if (c12_mode.SelectedIndex == 1)
            {
                // Strobe is selected
                c12_strobe.Enabled = true;
                c12_edge.Enabled = true;
                c12_pulse.Enabled = true;
                c12_delay.Enabled = true;
            }
            else
            {
                //Static is selected
                c12_strobe.Enabled = false;
                c12_edge.Enabled = false;
                c12_pulse.Enabled = false;
                c12_delay.Enabled = false;
            }
        }

        private void c12_pulse_TextChanged(object sender, EventArgs e)
        {
            // Pulse
            c12_testStop = 0;
            c12_test.Text = "Test";
            if (checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c12_pulse.Text);
                int delay_value = Convert.ToInt32(c12_delay.Text);

                c12_test.Enabled = true;
                c12_error.ForeColor = Color.White;
                c12_error.Text = "Error: ";
                c12_test.Enabled = true;

                c12_pulse_value = pulse_value;
                c12_delay_value = delay_value;

            }
            else
            {
                c12_test.Enabled = false;
                c12_error.ForeColor = Color.Red;
                c12_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c12_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c12_delay_TextChanged(object sender, EventArgs e)
        {
            // Delay
            c12_testStop = 0;
            c12_test.Text = "Test";
            if (checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c12_pulse.Text);
                int delay_value = Convert.ToInt32(c12_delay.Text);

                c12_test.Enabled = true;
                c12_error.ForeColor = Color.White;
                c12_error.Text = "Error: ";
                c12_test.Enabled = true;

                c12_pulse_value = pulse_value;
                c12_delay_value = delay_value;

            }
            else
            {
                c12_test.Enabled = false;
                c12_error.ForeColor = Color.Red;
                c12_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c12_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c12_test_Click(object sender, EventArgs e)
        {
            Thread c12_testing = new Thread(c12_light_loop);
            if (c12_testStop == 0)
            {
                c12_testing.Start();
                c12_test.Text = "Stop";
            }
            else
            {
                c12_testing.Abort();
                c12_test.Text = "Test";
            }
            c12_testStop++;
            if (c12_testStop == 2) { c12_testStop = 0; }
        }

        private void c13_intensity_TextChanged(object sender, EventArgs e)
        {
            if (checkIntensity(c13_intensity.Text))
            {
                int rgb_value = getRGB(c13_intensity.Text);
                c13_status.BackColor = Color.FromArgb(rgb_value, 0, 0);
                c13_error.ForeColor = Color.White;
                c13_error.Text = "Error: ";

                c13_rgb_value = rgb_value;

            }
            else
            {
                c13_test.Enabled = false;
                c13_status.BackColor = Color.Transparent;
                c13_error.ForeColor = Color.Red;
                c13_error.Text = "Error: Invalid value inputted for intensity. Please use integers from 0 to 4096.";

            }
            if (checkIntensity(c1_intensity.Text) && checkIntensity(c2_intensity.Text) && checkIntensity(c3_intensity.Text) && checkIntensity(c4_intensity.Text) && checkIntensity(c5_intensity.Text) && checkIntensity(c6_intensity.Text) && checkIntensity(c7_intensity.Text) && checkIntensity(c8_intensity.Text) && checkIntensity(c9_intensity.Text) && checkIntensity(c10_intensity.Text) && checkIntensity(c11_intensity.Text) && checkIntensity(c12_intensity.Text) && checkIntensity(c13_intensity.Text) && checkIntensity(c14_intensity.Text) && checkIntensity(c15_intensity.Text) && checkIntensity(c16_intensity.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c13_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            c13_testStop = 0;
            c13_test.Text = "Test";
            if (g5_setting.SelectedIndex == 1)
            {
                c14_mode.SelectedIndex = c13_mode.SelectedIndex;
                c15_mode.SelectedIndex = c13_mode.SelectedIndex;
            }

            if (c13_mode.SelectedIndex == 1)
            {
                // Strobe is selected
                c13_strobe.Enabled = true;
                c13_edge.Enabled = true;
                c13_pulse.Enabled = true;
                c13_delay.Enabled = true;
            }
            else
            {
                //Static is selected
                c13_strobe.Enabled = false;
                c13_edge.Enabled = false;
                c13_pulse.Enabled = false;
                c13_delay.Enabled = false;
            }
        }

        private void c13_pulse_TextChanged(object sender, EventArgs e)
        {
            // Pulse
            c13_testStop = 0;
            c13_test.Text = "Test";
            if (checkPulse(c13_pulse.Text))
            {
                int pulse_value = Convert.ToInt32(c13_pulse.Text);
                //int delay_value = Convert.ToInt32(c1_delay.Text);

                c13_test.Enabled = true;
                c13_error.ForeColor = Color.White;
                c13_error.Text = "Error: ";
                c13_test.Enabled = true;

                c13_pulse_value = pulse_value;
                //c1_delay_value = delay_value;

                if (g5_setting.SelectedIndex == 1)
                {
                    c14_pulse_value = pulse_value;
                    c15_pulse_value = pulse_value;
                    c14_pulse.Text = c13_pulse.Text;
                    c15_pulse.Text = c13_pulse.Text;


                }
                else
                {

                }

            }
            else
            {
                c13_test.Enabled = false;
                c13_error.ForeColor = Color.Red;
                c13_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c13_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c13_delay_TextChanged(object sender, EventArgs e)
        {
            // Delay
            c13_testStop = 0;
            c13_test.Text = "Test";
            if (checkDelay(c13_delay.Text))
            {
                //int pulse_value = Convert.ToInt32(c1_pulse.Text);
                int delay_value = Convert.ToInt32(c13_delay.Text);

                c13_test.Enabled = true;
                c13_error.ForeColor = Color.White;
                c13_error.Text = "Error: ";
                c13_test.Enabled = true;

                //c1_pulse_value = pulse_value;
                c13_delay_value = delay_value;
                if (g5_setting.SelectedIndex == 1)
                {


                    c14_delay_value = delay_value;
                    c15_delay_value = delay_value;
                    c14_delay.Text = c13_delay.Text;
                    c15_delay.Text = c13_delay.Text;
                }
                else
                {

                }
            }
            else
            {
                c13_test.Enabled = false;
                c13_error.ForeColor = Color.Red;
                c13_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c13_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c13_test_Click(object sender, EventArgs e)
        {
            Thread c13_testing = new Thread(c13_light_loop);
            if (c13_testStop == 0)
            {
                c13_testing.Start();
                c13_test.Text = "Stop";
            }
            else
            {
                c13_testing.Abort();
                c13_test.Text = "Test";
            }
            c13_testStop++;
            if (c13_testStop == 2) { c13_testStop = 0; }
        }

        private void c14_intensity_TextChanged(object sender, EventArgs e)
        {
            if (checkIntensity(c14_intensity.Text))
            {
                int rgb_value = getRGB(c14_intensity.Text);
                c14_status.BackColor = Color.FromArgb(0, rgb_value, 0);
                c14_error.ForeColor = Color.White;
                c14_error.Text = "Error: ";

                c14_rgb_value = rgb_value;

            }
            else
            {
                //Console.WriteLine("Invalid Value");
                c14_test.Enabled = false;
                c14_status.BackColor = Color.Transparent;
                c14_error.ForeColor = Color.Red;
                c14_error.Text = "Error: Invalid value inputted for intensity. Please use integers from 0 to 4096.";

            }
            if (checkIntensity(c1_intensity.Text) && checkIntensity(c2_intensity.Text) && checkIntensity(c3_intensity.Text) && checkIntensity(c4_intensity.Text) && checkIntensity(c5_intensity.Text) && checkIntensity(c6_intensity.Text) && checkIntensity(c7_intensity.Text) && checkIntensity(c8_intensity.Text) && checkIntensity(c9_intensity.Text) && checkIntensity(c10_intensity.Text) && checkIntensity(c11_intensity.Text) && checkIntensity(c12_intensity.Text) && checkIntensity(c13_intensity.Text) && checkIntensity(c14_intensity.Text) && checkIntensity(c15_intensity.Text) && checkIntensity(c16_intensity.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c14_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            c14_testStop = 0;
            c14_test.Text = "Test";
            if (c14_mode.SelectedIndex == 1)
            {
                // Strobe is selected
                c14_strobe.Enabled = true;
                c14_edge.Enabled = true;
                c14_pulse.Enabled = true;
                c14_delay.Enabled = true;
            }
            else
            {
                //Static is selected
                c14_strobe.Enabled = false;
                c14_edge.Enabled = false;
                c14_pulse.Enabled = false;
                c14_delay.Enabled = false;
            }
        }

        private void c14_pulse_TextChanged(object sender, EventArgs e)
        {
            // Pulse
            c14_testStop = 0;
            c14_test.Text = "Test";
            if (checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c14_pulse.Text);
                int delay_value = Convert.ToInt32(c14_delay.Text);

                c14_test.Enabled = true;
                c14_error.ForeColor = Color.White;
                c14_error.Text = "Error: ";
                c14_test.Enabled = true;

                c14_pulse_value = pulse_value;
                c14_delay_value = delay_value;

            }
            else
            {
                c14_test.Enabled = false;
                c14_error.ForeColor = Color.Red;
                c14_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c14_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c14_delay_TextChanged(object sender, EventArgs e)
        {
            // Pulse
            c14_testStop = 0;
            c14_test.Text = "Test";
            if (checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c14_pulse.Text);
                int delay_value = Convert.ToInt32(c14_delay.Text);

                c14_test.Enabled = true;
                c14_error.ForeColor = Color.White;
                c14_error.Text = "Error: ";
                c14_test.Enabled = true;

                c14_pulse_value = pulse_value;
                c14_delay_value = delay_value;

            }
            else
            {
                c14_test.Enabled = false;
                c14_error.ForeColor = Color.Red;
                c14_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c14_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c14_test_Click(object sender, EventArgs e)
        {
            Thread c14_testing = new Thread(c14_light_loop);
            if (c14_testStop == 0)
            {
                c14_testing.Start();
                c14_test.Text = "Stop";
            }
            else
            {
                c14_testing.Abort();
                c14_test.Text = "Test";
            }
            c14_testStop++;
            if (c14_testStop == 2) { c14_testStop = 0; }
        }

        private void c15_intensity_TextChanged(object sender, EventArgs e)
        {
            if (checkIntensity(c15_intensity.Text))
            {
                int rgb_value = getRGB(c15_intensity.Text);
                c15_status.BackColor = Color.FromArgb(0, 0, rgb_value);
                c15_error.ForeColor = Color.White;
                c15_error.Text = "Error: ";

                c15_rgb_value = rgb_value;

            }
            else
            {
                //Console.WriteLine("Invalid Value");
                c15_test.Enabled = false;
                c15_status.BackColor = Color.Transparent;
                c15_error.ForeColor = Color.Red;
                c15_error.Text = "Error: Invalid value inputted for intensity. Please use integers from 0 to 4096.";

            }
            if (checkIntensity(c1_intensity.Text) && checkIntensity(c2_intensity.Text) && checkIntensity(c3_intensity.Text) && checkIntensity(c4_intensity.Text) && checkIntensity(c5_intensity.Text) && checkIntensity(c6_intensity.Text) && checkIntensity(c7_intensity.Text) && checkIntensity(c8_intensity.Text) && checkIntensity(c9_intensity.Text) && checkIntensity(c10_intensity.Text) && checkIntensity(c11_intensity.Text) && checkIntensity(c12_intensity.Text) && checkIntensity(c13_intensity.Text) && checkIntensity(c14_intensity.Text) && checkIntensity(c15_intensity.Text) && checkIntensity(c16_intensity.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c15_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            c15_testStop = 0;
            c15_test.Text = "Test";
            if (c15_mode.SelectedIndex == 1)
            {
                // Strobe is selected
                c15_strobe.Enabled = true;
                c15_edge.Enabled = true;
                c15_pulse.Enabled = true;
                c15_delay.Enabled = true;
            }
            else
            {
                //Static is selected
                c15_strobe.Enabled = false;
                c15_edge.Enabled = false;
                c15_pulse.Enabled = false;
                c15_delay.Enabled = false;
            }
        }

        private void c15_pulse_TextChanged(object sender, EventArgs e)
        {
            // Pulse
            c15_testStop = 0;
            c15_test.Text = "Test";
            if (checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c15_pulse.Text);
                int delay_value = Convert.ToInt32(c15_delay.Text);

                c15_test.Enabled = true;
                c15_error.ForeColor = Color.White;
                c15_error.Text = "Error: ";
                c15_test.Enabled = true;

                c15_pulse_value = pulse_value;
                c15_delay_value = delay_value;

            }
            else
            {
                c15_test.Enabled = false;
                c15_error.ForeColor = Color.Red;
                c15_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c15_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c15_delay_TextChanged(object sender, EventArgs e)
        {
            // Delay
            c15_testStop = 0;
            c15_test.Text = "Test";
            if (checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c15_pulse.Text);
                int delay_value = Convert.ToInt32(c15_delay.Text);

                c15_test.Enabled = true;
                c15_error.ForeColor = Color.White;
                c15_error.Text = "Error: ";
                c15_test.Enabled = true;

                c15_pulse_value = pulse_value;
                c15_delay_value = delay_value;

            }
            else
            {
                c15_test.Enabled = false;
                c15_error.ForeColor = Color.Red;
                c15_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c15_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c15_test_Click(object sender, EventArgs e)
        {
            Thread c15_testing = new Thread(c15_light_loop);
            if (c15_testStop == 0)
            {
                c15_testing.Start();
                c15_test.Text = "Stop";
            }
            else
            {
                c15_testing.Abort();
                c15_test.Text = "Test";
            }
            c15_testStop++;
            if (c15_testStop == 2) { c15_testStop = 0; }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label133_Click(object sender, EventArgs e)
        {

        }

        private void lightSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lightSelect.Text == "")
            {
                uploadConfig.Enabled = false;
            }
            else
            {
                loadLatestBoardConfig();
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Thread c16_testing = new Thread(c16_light_loop);
            if (c16_testStop == 0)
            {
                c16_testing.Start();
                c16_test.Text = "Stop";
            }
            else
            {
                c16_testing.Abort();
                c16_test.Text = "Test";
            }
            c16_testStop++;
            if (c16_testStop == 2) { c16_testStop = 0; }
        }

        private void loadLatestBoardConfig()
        {
            bool grouped = false;
            string intensity = "";
            string edge = "";
            string mode = "";
            string strobe = "";
            string pulse = "";
            string delay = "";

            string pathWithFileName = @"..\..\savedConfigs\" + lightSelect.Text + ".txt";

            string loadedConfig = File.ReadAllText(pathWithFileName);


            string[] splitString = loadedConfig.Split('.');


            for (int i = 0; i < splitString.Length; i++)
            {
                if (splitString[i][1] == 'G')
                {
                    string groupedOrUngroup = splitString[i].Substring(20, 7);
                    Console.WriteLine(groupedOrUngroup);
                    if (groupedOrUngroup == "GROUPED")
                    {
                        grouped = true;
                    }
                    else if (groupedOrUngroup == "UNGROUP")
                    {

                        grouped = false;
                    }
                }

                else if (splitString[i][1] == '[')
                {
                    int intensityIndexPos = splitString[i].IndexOf("Intensity") + 11;
                    int intensityNextCommaIndexPos = splitString[i].IndexOf(",", intensityIndexPos);
                    int intensityLengthBetweenBothIndex = intensityNextCommaIndexPos - intensityIndexPos;

                    int edgeIndexPos = splitString[i].IndexOf("Edge") + 6;
                    int edgeNextCommaIndexPos = splitString[i].IndexOf(",", edgeIndexPos);
                    int edgeLengthBetweenBothIndex = edgeNextCommaIndexPos - edgeIndexPos;

                    int modeIndexPos = splitString[i].IndexOf("Mode") + 6;
                    int modeNextCommaIndexPos = splitString[i].IndexOf(",", modeIndexPos);
                    int modeLengthBetweenBothIndex = modeNextCommaIndexPos - modeIndexPos;

                    int strobeIndexPos = splitString[i].IndexOf("Strobe:") + 8;
                    int strobeNextCommaIndexPos = splitString[i].IndexOf(",", strobeIndexPos);
                    int strobeLengthBetweenBothIndex = strobeNextCommaIndexPos - strobeIndexPos;

                    int pulseIndexPos = splitString[i].IndexOf("Pulse") + 7;
                    int pulseNextCommaIndexPos = splitString[i].IndexOf(",", pulseIndexPos);
                    int pulseLengthBetweenBothIndex = pulseNextCommaIndexPos - pulseIndexPos;

                    int delayIndexPos = splitString[i].IndexOf("Delay") + 7;
                    //there is a space at the end of the line
                    int delayEndOfLineIndex = splitString[i].IndexOf(" ", delayIndexPos);
                    int delayLengthBetweenBothIndex = delayEndOfLineIndex - delayIndexPos;

                    intensity = splitString[i].Substring(intensityIndexPos, intensityLengthBetweenBothIndex);
                    edge = splitString[i].Substring(edgeIndexPos, edgeLengthBetweenBothIndex);
                    mode = splitString[i].Substring(modeIndexPos, modeLengthBetweenBothIndex);
                    strobe = splitString[i].Substring(strobeIndexPos, strobeLengthBetweenBothIndex);
                    pulse = splitString[i].Substring(pulseIndexPos, pulseLengthBetweenBothIndex);
                    delay = splitString[i].Substring(delayIndexPos, delayLengthBetweenBothIndex);

                    if (mode == "None")
                    {
                        mode = "Static";
                    }

                    Console.WriteLine(intensity);
                    Console.WriteLine(edge);
                    Console.WriteLine(mode);
                    Console.WriteLine(strobe);
                    Console.WriteLine(pulse);
                    Console.WriteLine(delay);
                }
                switch (i)
                {
                    //group cases
                    case 1:
                        Console.WriteLine("THIS PART IS RUNNING HADBGFSHIAHFSHAJGFUJG");

                        if (grouped == true)
                        {
                            g1_setting.SelectedIndex = 1;
                        }
                        else
                        {
                            Console.WriteLine("THIS PART IS RUNNING LOL");
                            g1_setting.SelectedIndex = 0;
                        }
                        break;

                    case 5:
                        if (grouped == true)
                        {
                            g2_setting.SelectedIndex = 1;
                        }
                        else
                        {
                            g2_setting.SelectedIndex = 0;
                        }
                        break;

                    case 9:
                        if (grouped == true)
                        {
                            g3_setting.SelectedIndex = 1;
                        }
                        else
                        {
                            g3_setting.SelectedIndex = 0;
                        }
                        break;

                    case 13:
                        if (grouped == true)
                        {
                            g4_setting.SelectedIndex = 1;
                        }
                        else
                        {
                            g4_setting.SelectedIndex = 0;
                        }
                        break;

                    case 17:
                        if (grouped == true)
                        {
                            g5_setting.SelectedIndex = 1;
                        }
                        else
                        {
                            g5_setting.SelectedIndex = 0;
                        }
                        break;

                    //light config cases
                    case 2:
                        if (intensity == "None")
                        {
                            c1_intensity.Text = "0";
                        }
                        else
                        {
                            c1_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            c1_pulse.Text = "0";
                        }
                        else
                        {
                            c1_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            c1_delay.Text = "0";
                        }
                        else
                        {
                            c1_delay.Text = delay;
                        }

                        c1_edge.Text = edge;
                        c1_mode.Text = mode;
                        c1_strobe.Text = strobe;

                        break;
                    case 3:
                        if (intensity == "None")
                        {
                            c2_intensity.Text = "0";
                        }
                        else
                        {
                            c2_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            c2_pulse.Text = "0";
                        }
                        else
                        {
                            c2_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            c2_delay.Text = "0";
                        }
                        else
                        {
                            c2_delay.Text = delay;
                        }

                        c2_edge.Text = edge;
                        c2_mode.Text = mode;
                        c2_strobe.Text = strobe;

                        break;
                    case 4:
                        if (intensity == "None")
                        {
                            c3_intensity.Text = "0";
                        }
                        else
                        {
                            c3_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            c3_pulse.Text = "0";
                        }
                        else
                        {
                            c3_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            c3_delay.Text = "0";
                        }
                        else
                        {
                            c3_delay.Text = delay;
                        }

                        c3_edge.Text = edge;
                        c3_mode.Text = mode;
                        c3_strobe.Text = strobe;

                        break;

                    case 6:
                        if (intensity == "None")
                        {
                            c4_intensity.Text = "0";
                        }
                        else
                        {
                            c4_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            c4_pulse.Text = "0";
                        }
                        else
                        {
                            c4_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            c4_delay.Text = "0";
                        }
                        else
                        {
                            c4_delay.Text = delay;
                        }

                        c4_edge.Text = edge;
                        c4_mode.Text = mode;
                        c4_strobe.Text = strobe;

                        break;

                    case 7:
                        if (intensity == "None")
                        {
                            c5_intensity.Text = "0";
                        }
                        else
                        {
                            c5_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            c5_pulse.Text = "0";
                        }
                        else
                        {
                            c5_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            c5_delay.Text = "0";
                        }
                        else
                        {
                            c5_delay.Text = delay;
                        }

                        c5_edge.Text = edge;
                        c5_mode.Text = mode;
                        c5_strobe.Text = strobe;

                        break;

                    case 8:
                        if (intensity == "None")
                        {
                            c6_intensity.Text = "0";
                        }
                        else
                        {
                            c6_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            c6_pulse.Text = "0";
                        }
                        else
                        {
                            c6_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            c6_delay.Text = "0";
                        }
                        else
                        {
                            c6_delay.Text = delay;
                        }

                        c6_edge.Text = edge;
                        c6_mode.Text = mode;
                        c6_strobe.Text = strobe;

                        break;

                    case 10:
                        if (intensity == "None")
                        {
                            c7_intensity.Text = "0";
                        }
                        else
                        {
                            c7_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            c7_pulse.Text = "0";
                        }
                        else
                        {
                            c7_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            c7_delay.Text = "0";
                        }
                        else
                        {
                            c7_delay.Text = delay;
                        }

                        c7_edge.Text = edge;
                        c7_mode.Text = mode;
                        c7_strobe.Text = strobe;

                        break;

                    case 11:
                        if (intensity == "None")
                        {
                            c8_intensity.Text = "0";
                        }
                        else
                        {
                            c8_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            c8_pulse.Text = "0";
                        }
                        else
                        {
                            c8_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            c8_delay.Text = "0";
                        }
                        else
                        {
                            c8_delay.Text = delay;
                        }

                        c8_edge.Text = edge;
                        c8_mode.Text = mode;
                        c8_strobe.Text = strobe;

                        break;

                    case 12:
                        if (intensity == "None")
                        {
                            c9_intensity.Text = "0";
                        }
                        else
                        {
                            c9_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            c9_pulse.Text = "0";
                        }
                        else
                        {
                            c9_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            c9_delay.Text = "0";
                        }
                        else
                        {
                            c9_delay.Text = delay;
                        }

                        c9_edge.Text = edge;
                        c9_mode.Text = mode;
                        c9_strobe.Text = strobe;

                        break;

                    case 14:
                        if (intensity == "None")
                        {
                            c10_intensity.Text = "0";
                        }
                        else
                        {
                            c10_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            c10_pulse.Text = "0";
                        }
                        else
                        {
                            c10_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            c10_delay.Text = "0";
                        }
                        else
                        {
                            c10_delay.Text = delay;
                        }

                        c10_edge.Text = edge;
                        c10_mode.Text = mode;
                        c10_strobe.Text = strobe;

                        break;

                    case 15:
                        if (intensity == "None")
                        {
                            c11_intensity.Text = "0";
                        }
                        else
                        {
                            c11_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            c11_pulse.Text = "0";
                        }
                        else
                        {
                            c11_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            c11_delay.Text = "0";
                        }
                        else
                        {
                            c11_delay.Text = delay;
                        }

                        c11_edge.Text = edge;
                        c11_mode.Text = mode;
                        c11_strobe.Text = strobe;

                        break;

                    case 16:
                        if (intensity == "None")
                        {
                            c12_intensity.Text = "0";
                        }
                        else
                        {
                            c12_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            c12_pulse.Text = "0";
                        }
                        else
                        {
                            c12_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            c12_delay.Text = "0";
                        }
                        else
                        {
                            c12_delay.Text = delay;
                        }

                        c12_edge.Text = edge;
                        c12_mode.Text = mode;
                        c12_strobe.Text = strobe;

                        break;

                    case 18:
                        if (intensity == "None")
                        {
                            c13_intensity.Text = "0";
                        }
                        else
                        {
                            c13_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            c13_pulse.Text = "0";
                        }
                        else
                        {
                            c13_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            c13_delay.Text = "0";
                        }
                        else
                        {
                            c13_delay.Text = delay;
                        }

                        c13_edge.Text = edge;
                        c13_mode.Text = mode;
                        c13_strobe.Text = strobe;

                        break;

                    case 19:
                        if (intensity == "None")
                        {
                            c14_intensity.Text = "0";
                        }
                        else
                        {
                            c14_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            c14_pulse.Text = "0";
                        }
                        else
                        {
                            c14_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            c14_delay.Text = "0";
                        }
                        else
                        {
                            c14_delay.Text = delay;
                        }

                        c14_edge.Text = edge;
                        c14_mode.Text = mode;
                        c14_strobe.Text = strobe;

                        break;

                    case 20:
                        if (intensity == "None")
                        {
                            c15_intensity.Text = "0";
                        }
                        else
                        {
                            c15_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            c15_pulse.Text = "0";
                        }
                        else
                        {
                            c15_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            c15_delay.Text = "0";
                        }
                        else
                        {
                            c15_delay.Text = delay;
                        }

                        c15_edge.Text = edge;
                        c15_mode.Text = mode;
                        c15_strobe.Text = strobe;

                        break;

                    case 22:
                        if (intensity == "None")
                        {
                            c16_intensity.Text = "0";
                        }
                        else
                        {
                            c16_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            c16_pulse.Text = "0";
                        }
                        else
                        {
                            c16_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            c16_delay.Text = "0";
                        }
                        else
                        {
                            c16_delay.Text = delay;
                        }

                        c16_edge.Text = edge;
                        c16_mode.Text = mode;
                        c16_strobe.Text = strobe;

                        break;
                }


            }
        }
        
         void restartConn()
         {
            bool status = false;
            while (status == false)
            {
                status = checkIfUsbAlive();
            }

         }


        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void c16_intensity_TextChanged(object sender, EventArgs e)
        {
            if (checkIntensity(c16_intensity.Text))
            {
                int rgb_value = getRGB(c16_intensity.Text);
                c16_status.BackColor = Color.FromArgb(rgb_value, 0, 0);
                c16_error.ForeColor = Color.White;
                c16_error.Text = "Error: ";

                c16_rgb_value = rgb_value;

            }
            else
            {
                c16_test.Enabled = false;
                c16_status.BackColor = Color.Transparent;
                c16_error.ForeColor = Color.Red;
                c16_error.Text = "Error: Invalid value inputted for intensity. Please use integers from 0 to 4096.";

            }
            
            if (checkIntensity(c1_intensity.Text) && checkIntensity(c2_intensity.Text) && checkIntensity(c3_intensity.Text) && checkIntensity(c4_intensity.Text) && checkIntensity(c5_intensity.Text) && checkIntensity(c6_intensity.Text) && checkIntensity(c7_intensity.Text) && checkIntensity(c8_intensity.Text) && checkIntensity(c9_intensity.Text) && checkIntensity(c10_intensity.Text) && checkIntensity(c11_intensity.Text) && checkIntensity(c12_intensity.Text) && checkIntensity(c13_intensity.Text) && checkIntensity(c14_intensity.Text) && checkIntensity(c15_intensity.Text) && checkIntensity(c16_intensity.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c16_edge_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void c16_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            c16_testStop = 0;
            c16_test.Text = "Test";
            if (c16_mode.SelectedIndex == 1)
            {
                // Strobe is selected
                c16_strobe.Enabled = true;
                c16_edge.Enabled = true;
                c16_pulse.Enabled = true;
                c16_delay.Enabled = true;
            }
            else
            {
                //Static is selected
                c16_strobe.Enabled = false;
                c16_edge.Enabled = false;
                c16_pulse.Enabled = false;
                c16_delay.Enabled = false;
            }
        }

        private void c16_strobe_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void c16_pulse_TextChanged(object sender, EventArgs e)
        {
            c16_testStop = 0;
            c16_test.Text = "Test";
            if (checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c16_pulse.Text);
                int delay_value = Convert.ToInt32(c16_delay.Text);

                c16_test.Enabled = true;
                c16_error.ForeColor = Color.White;
                c16_error.Text = "Error: ";
                c16_test.Enabled = true;

                c16_pulse_value = pulse_value;
                c16_delay_value = delay_value;

            }
            else
            {
                c16_test.Enabled = false;
                c16_error.ForeColor = Color.Red;
                c16_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c16_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c16_delay_TextChanged(object sender, EventArgs e)
        {
            c16_testStop = 0;
            c16_test.Text = "Test";
            if (checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c16_pulse.Text);
                int delay_value = Convert.ToInt32(c16_delay.Text);

                c16_test.Enabled = true;
                c16_error.ForeColor = Color.White;
                c16_error.Text = "Error: ";
                c16_test.Enabled = true;

                c16_pulse_value = pulse_value;
                c16_delay_value = delay_value;

            }
            else
            {
                c16_test.Enabled = false;
                c16_error.ForeColor = Color.Red;
                c16_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c16_test.Enabled = false;
            }

            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text) && checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text) && checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text) && checkPulse(c4_pulse.Text) && checkDelay(c4_delay.Text) && checkPulse(c5_pulse.Text) && checkDelay(c5_delay.Text) && checkPulse(c6_pulse.Text) && checkDelay(c6_delay.Text) && checkPulse(c7_pulse.Text) && checkDelay(c7_delay.Text) && checkPulse(c8_pulse.Text) && checkDelay(c8_delay.Text) && checkPulse(c9_pulse.Text) && checkDelay(c9_delay.Text) && checkPulse(c10_pulse.Text) && checkDelay(c10_delay.Text) && checkPulse(c11_pulse.Text) && checkDelay(c11_delay.Text) && checkPulse(c12_pulse.Text) && checkDelay(c12_delay.Text) && checkPulse(c13_pulse.Text) && checkDelay(c13_delay.Text) && checkPulse(c14_pulse.Text) && checkDelay(c14_delay.Text) && checkPulse(c15_pulse.Text) && checkDelay(c15_delay.Text) && checkPulse(c16_pulse.Text) && checkDelay(c16_delay.Text))
            {
                if (lightSelect.Text != "")
                {
                    uploadConfig.Enabled = true;
                }
            }
            else
            {
                uploadConfig.Enabled = false;
            }
        }

        private void c13_edge_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void c13_strobe_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            c1_intensity.Text = "0";
            c2_intensity.Text = "0";
            c3_intensity.Text = "0";
            c4_intensity.Text = "0";
            c5_intensity.Text = "0";
            c6_intensity.Text = "0";
            c7_intensity.Text = "0";
            c8_intensity.Text = "0";
            c9_intensity.Text = "0";
            c10_intensity.Text = "0";
            c11_intensity.Text = "0";
            c12_intensity.Text = "0";
            c13_intensity.Text = "0";
            c14_intensity.Text = "0";
            c15_intensity.Text = "0";
            c16_intensity.Text = "0";

        }

        private void testUpdatedComms_Click(object sender, EventArgs e)
        {
            intensity_set();
            mode_set();
            pulse_set();
            delay_set();
            edge_set();
            strobe_set();
            Console.WriteLine("COMPLETED");
        }
    }
}
