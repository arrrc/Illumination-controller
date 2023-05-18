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
        public int c1_rgb_value = 0;
        public int c2_rgb_value = 0;
        public int c3_rgb_value = 0;
        public int c4_rgb_value = 0;
        public int c5_rgb_value = 0;
        public int c6_rgb_value = 0;
        public int c7_rgb_value = 0;
        public int c8_rgb_value = 0;
        public int c9_rgb_value = 0;
        public int c10_rgb_value = 0;
        public int c11_rgb_value = 0;
        public int c12_rgb_value = 0;
        public int c13_rgb_value = 0;
        public int c14_rgb_value = 0;
        public int c15_rgb_value = 0;
        public int c16_rgb_value = 0;

        public string c1_edge_value = "None";
        public string c2_edge_value = "None";
        public string c3_edge_value = "None";
        public string c4_edge_value = "None";
        public string c5_edge_value = "None";
        public string c6_edge_value = "None";
        public string c7_edge_value = "None";
        public string c8_edge_value = "None";
        public string c9_edge_value = "None";
        public string c10_edge_value = "None";
        public string c11_edge_value = "None";
        public string c12_edge_value = "None";
        public string c13_edge_value = "None";
        public string c14_edge_value = "None";
        public string c15_edge_value = "None";
        public string c16_edge_value = "None";

        public int c1_pulse_value = 0;
        public int c2_pulse_value = 0;
        public int c3_pulse_value = 0;
        public int c4_pulse_value = 0;
        public int c5_pulse_value = 0;
        public int c6_pulse_value = 0;
        public int c7_pulse_value = 0;
        public int c8_pulse_value = 0;
        public int c9_pulse_value = 0;
        public int c10_pulse_value = 0;
        public int c11_pulse_value = 0;
        public int c12_pulse_value = 0;
        public int c13_pulse_value = 0;
        public int c14_pulse_value = 0;
        public int c15_pulse_value = 0;
        public int c16_pulse_value = 0;

        public int c1_delay_value = 0;
        public int c2_delay_value = 0;
        public int c3_delay_value = 0;
        public int c4_delay_value = 0;
        public int c5_delay_value = 0;
        public int c6_delay_value = 0;
        public int c7_delay_value = 0;
        public int c8_delay_value = 0;
        public int c9_delay_value = 0;
        public int c10_delay_value = 0;
        public int c11_delay_value = 0;
        public int c12_delay_value = 0;
        public int c13_delay_value = 0;
        public int c14_delay_value = 0;
        public int c15_delay_value = 0;
        public int c16_delay_value = 0;

        public string c1_mode_value = "Static";
        public string c2_mode_value = "Static";
        public string c3_mode_value = "Static";
        public string c4_mode_value = "Static";
        public string c5_mode_value = "Static";
        public string c6_mode_value = "Static";
        public string c7_mode_value = "Static";
        public string c8_mode_value = "Static";
        public string c9_mode_value = "Static";
        public string c10_mode_value = "Static";
        public string c11_mode_value = "Static";
        public string c12_mode_value = "Static";
        public string c13_mode_value = "Static";
        public string c14_mode_value = "Static";
        public string c15_mode_value = "Static";
        public string c16_mode_value = "Static";

        public string c1_strobe_value = "None";
        public string c2_strobe_value = "None";
        public string c3_strobe_value = "None";
        public string c4_strobe_value = "None";
        public string c5_strobe_value = "None";
        public string c6_strobe_value = "None";
        public string c7_strobe_value = "None";
        public string c8_strobe_value = "None";
        public string c9_strobe_value = "None";
        public string c10_strobe_value = "None";
        public string c11_strobe_value = "None";
        public string c12_strobe_value = "None";
        public string c13_strobe_value = "None";
        public string c14_strobe_value = "None";
        public string c15_strobe_value = "None";
        public string c16_strobe_value = "None";

        public bool c010203_isGrouped = true;
        public bool c040506_isGrouped = true;
        public bool c070809_isGrouped = true;
        public bool c101112_isGrouped = true;
        public bool c131415_isGrouped = true;

        bool c010203_isCurrent = true;
        bool c040506_isCurrent = false;
        bool c070809_isCurrent = false;
        bool c101112_isCurrent = false;
        bool c131415_isCurrent = false;
        bool c160000_isCurrent = false;

        string currentStrobe = "Group 1";
        
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

        static SerialPort portConn;
        public string sendToHardware = "";
        string dataReceived = "";
        bool didDataReceiveThreadExit = false;
        int numTimesDataSent = 0;
        List<string> config = new List<string>();
        int noTimesRemoveEventFired = 0;
        List<string> splitData = new List<string>();



        delegate void SetTextCallback(string text);

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

        public int returnEdgeIndex(string value)
        {
            if (value == "Rising")
            {
                return 1;
            }
            else if (value == "Falling")
            {
                return 2;
            }
            else
            {
                return 0;
            }
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

        public void displayModes()
        {
            // To test Mode Values
            Console.WriteLine("Display Modes");
            Console.WriteLine("Channel 1 " + c1_mode_value);
            Console.WriteLine("Channel 2 " + c2_mode_value);
            Console.WriteLine("Channel 3 " + c3_mode_value);
            Console.WriteLine("Channel 4 " + c4_mode_value);
            Console.WriteLine("Channel 5 " + c5_mode_value);
            Console.WriteLine("Channel 6 " + c6_mode_value);
            Console.WriteLine("Channel 7 " + c7_mode_value);
            Console.WriteLine("Channel 8 " + c8_mode_value);
            Console.WriteLine("Channel 9 " + c9_mode_value);
            Console.WriteLine("Channel 10 " + c10_mode_value);
            Console.WriteLine("Channel 11 " + c11_mode_value);
            Console.WriteLine("Channel 12 " + c12_mode_value);
            Console.WriteLine("Channel 13 " + c13_mode_value);
            Console.WriteLine("Channel 14 " + c14_mode_value);
            Console.WriteLine("Channel 15 " + c15_mode_value);
            Console.WriteLine("Channel 16 " + c16_mode_value);

        }

        public void displayStrobes()
        {
            // To test Mode Values
            Console.WriteLine("Display Strobe Values");
            Console.WriteLine("Channel 1 " + c1_strobe_value);
            Console.WriteLine("Channel 2 " + c2_strobe_value);
            Console.WriteLine("Channel 3 " + c3_strobe_value);
            Console.WriteLine("Channel 4 " + c4_strobe_value);
            Console.WriteLine("Channel 5 " + c5_strobe_value);
            Console.WriteLine("Channel 6 " + c6_strobe_value);
            Console.WriteLine("Channel 7 " + c7_strobe_value);
            Console.WriteLine("Channel 8 " + c8_strobe_value);
            Console.WriteLine("Channel 9 " + c9_strobe_value);
            Console.WriteLine("Channel 10 " + c10_strobe_value);
            Console.WriteLine("Channel 11 " + c11_strobe_value);
            Console.WriteLine("Channel 12 " + c12_strobe_value);
            Console.WriteLine("Channel 13 " + c13_strobe_value);
            Console.WriteLine("Channel 14 " + c14_strobe_value);
            Console.WriteLine("Channel 15 " + c15_strobe_value);
            Console.WriteLine("Channel 16 " + c16_strobe_value);

        }

        public int returnStrobe(string value)
        {
            if (value == "None") { return 0; }
            else if (value == "1") { return 1; }
            else if (value == "2") { return 2; }
            else if (value == "3") { return 3; }
            else if (value == "4") { return 4; }
            else if (value == "5") { return 5; }
            else if (value == "6") { return 6; }
            else if (value == "7") { return 7; }
            else { return 8; }
        }

        public void setIntensity()
        {
            // Channels 1 2 3
            if (c010203_isCurrent)
            {
                led1_intensity.Text = c1_rgb_value.ToString();
                led2_intensity.Text = c2_rgb_value.ToString();
                led3_intensity.Text = c3_rgb_value.ToString();
            }
            // Channels 4 5 6
            else if (c040506_isCurrent)
            {
                led1_intensity.Text = c4_rgb_value.ToString();
                led2_intensity.Text = c5_rgb_value.ToString();
                led3_intensity.Text = c6_rgb_value.ToString();
            }
            // Channels 7 8 9
            else if (c070809_isCurrent)
            {
                led1_intensity.Text = c7_rgb_value.ToString();
                led2_intensity.Text = c8_rgb_value.ToString();
                led3_intensity.Text = c9_rgb_value.ToString();
            }
            // Channels 10 11 12
            else if (c101112_isCurrent)
            {
                led1_intensity.Text = c10_rgb_value.ToString();
                led2_intensity.Text = c11_rgb_value.ToString();
                led3_intensity.Text = c12_rgb_value.ToString();
            }
            // Channels 13 14 15
            else if (c131415_isCurrent)
            {
                led1_intensity.Text = c13_rgb_value.ToString();
                led2_intensity.Text = c14_rgb_value.ToString();
                led3_intensity.Text = c15_rgb_value.ToString();
            }
            else
            {
                led1_intensity.Text = c16_rgb_value.ToString();
            }
        }

        public void setStrobe()
        {
            // Channels 1 2 3
            if (c010203_isCurrent)
            {
                led1_strobe.SelectedIndex = returnStrobe(c1_strobe_value);
                led2_strobe.SelectedIndex = returnStrobe(c2_strobe_value);
                led3_strobe.SelectedIndex = returnStrobe(c3_strobe_value);
            }
            // Channels 4 5 6
            else if (c040506_isCurrent)
            {
                led1_strobe.SelectedIndex = returnStrobe(c4_strobe_value);
                led2_strobe.SelectedIndex = returnStrobe(c5_strobe_value);
                led3_strobe.SelectedIndex = returnStrobe(c6_strobe_value);
            }
            // Channels 7 8 9
            else if (c070809_isCurrent)
            {
                led1_strobe.SelectedIndex = returnStrobe(c7_strobe_value);
                led2_strobe.SelectedIndex = returnStrobe(c8_strobe_value);
                led3_strobe.SelectedIndex = returnStrobe(c9_strobe_value);
            }
            // Channels 10 11 12
            else if (c101112_isCurrent)
            {
                led1_strobe.SelectedIndex = returnStrobe(c10_strobe_value);
                led2_strobe.SelectedIndex = returnStrobe(c11_strobe_value);
                led3_strobe.SelectedIndex = returnStrobe(c12_strobe_value);
            }
            // Channels 13 14 15
            else if (c131415_isCurrent)
            {
                led1_strobe.SelectedIndex = returnStrobe(c13_strobe_value);
                led2_strobe.SelectedIndex = returnStrobe(c14_strobe_value);
                led3_strobe.SelectedIndex = returnStrobe(c15_strobe_value);
            }
            else
            {
                led1_strobe.SelectedIndex = returnStrobe(c16_strobe_value);
                led2_strobe.SelectedIndex = 0;
                led3_strobe.SelectedIndex = 0;
            }
        }

        public void resetModes()
        {
            led1_mode.SelectedIndex = 0; led1_strobe.Enabled = false;
            led2_mode.SelectedIndex = 0; led2_strobe.Enabled = false;
            led3_mode.SelectedIndex = 0; led3_strobe.Enabled = false;
        }

        public void setModes()
        {
            // Channels 1 2 3
            if (c010203_isCurrent)
            {
                if (c1_mode_value == "Static") { led1_mode.SelectedIndex = 0;  led1_strobe.Enabled = false; }
                else if (c1_mode_value == "Strobe") { led1_mode.SelectedIndex = 1; led1_strobe.Enabled = true; }

                if (c2_mode_value == "Static")  { led2_mode.SelectedIndex = 0; led2_strobe.Enabled = false; }
                else if (c2_mode_value == "Strobe") { led2_mode.SelectedIndex = 1; led2_strobe.Enabled = true; }

                if (c3_mode_value == "Static") { led3_mode.SelectedIndex = 0; led3_strobe.Enabled = false; }
                else if (c3_mode_value == "Strobe")  { led3_mode.SelectedIndex = 1; led3_strobe.Enabled = true; }
            }
            // Channels 4 5 6
            else if (c040506_isCurrent)
            {
                if (c4_mode_value == "Static") { led1_mode.SelectedIndex = 0; led1_strobe.Enabled = false; }
                else if (c4_mode_value == "Strobe") { led1_mode.SelectedIndex = 1; led1_strobe.Enabled = true; }

                if (c5_mode_value == "Static") { led2_mode.SelectedIndex = 0; led2_strobe.Enabled = false; }
                else if (c5_mode_value == "Strobe") { led2_mode.SelectedIndex = 1; led2_strobe.Enabled = true; }

                if (c6_mode_value == "Static") { led3_mode.SelectedIndex = 0; led3_strobe.Enabled = false; }
                else if (c6_mode_value == "Strobe") { led3_mode.SelectedIndex = 1; led3_strobe.Enabled = true; }
            }
            //Channels 7 8 9
            else if (c070809_isCurrent)
            {
                if (c7_mode_value == "Static") { led1_mode.SelectedIndex = 0; led1_strobe.Enabled = false; }
                else if (c7_mode_value == "Strobe") { led1_mode.SelectedIndex = 1; led1_strobe.Enabled = true; }

                if (c8_mode_value == "Static") { led2_mode.SelectedIndex = 0; led2_strobe.Enabled = false; }
                else if (c8_mode_value == "Strobe") { led2_mode.SelectedIndex = 1; led2_strobe.Enabled = true; }

                if (c9_mode_value == "Static") { led3_mode.SelectedIndex = 0; led3_strobe.Enabled = false; }
                else if (c9_mode_value == "Strobe") { led3_mode.SelectedIndex = 1; led3_strobe.Enabled = true; }
            }
            //Channels 10 11 12
            else if (c101112_isCurrent)
            {
                if (c10_mode_value == "Static") { led1_mode.SelectedIndex = 0; led1_strobe.Enabled = false; }
                else if (c10_mode_value == "Strobe") { led1_mode.SelectedIndex = 1; led1_strobe.Enabled = true; }

                if (c11_mode_value == "Static") { led2_mode.SelectedIndex = 0; led2_strobe.Enabled = false; }
                else if (c11_mode_value == "Strobe") { led2_mode.SelectedIndex = 1; led2_strobe.Enabled = true; }

                if (c12_mode_value == "Static") { led3_mode.SelectedIndex = 0; led3_strobe.Enabled = false; }
                else if (c12_mode_value == "Strobe") { led3_mode.SelectedIndex = 1; led3_strobe.Enabled = true; }
            }
            //Channels 13 14 15
            else if (c131415_isCurrent)
            {
                if (c13_mode_value == "Static") { led1_mode.SelectedIndex = 0; led1_strobe.Enabled = false; }
                else if (c13_mode_value == "Strobe") { led1_mode.SelectedIndex = 1; led1_strobe.Enabled = true; }

                if (c14_mode_value == "Static") { led2_mode.SelectedIndex = 0; led2_strobe.Enabled = false; }
                else if (c14_mode_value == "Strobe") { led2_mode.SelectedIndex = 1; led2_strobe.Enabled = true; }

                if (c15_mode_value == "Static") { led3_mode.SelectedIndex = 0; led3_strobe.Enabled = false; }
                else if (c15_mode_value == "Strobe") { led3_mode.SelectedIndex = 1; led3_strobe.Enabled = true; }
            }
            else
            {
                if (c16_mode_value == "Static") { led1_mode.SelectedIndex = 0; led1_strobe.Enabled = false; }
                else if (c16_mode_value == "Strobe") { led1_mode.SelectedIndex = 1; led1_strobe.Enabled = true; }
            }

        }

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
            try
            {
                portConn = new SerialPort();
                portConn.BaudRate = 115200;
                int counter = 0;

                while (COMport.Text == "" && counter < 5)
                {
                    foreach (string portName in SerialPort.GetPortNames())
                    {
                        Console.WriteLine(portName);
                        portConn.PortName = portName;
                        portConn.Open();

                        portConn.Write("QB\r\n");
                        Console.WriteLine("sent data");


                        Thread.Sleep(1200);
                        string reply = portConn.ReadExisting();
                        Console.WriteLine(reply);

                        if (reply.Contains("PICS"))
                        {
                            Console.WriteLine("port is open");
                            COMport.Text = portName;
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

                if (counter >= 5)
                {
                    portError.Visible = true;
                    portError.Text = "USB not plugged in, plug it in";
                    portError.ForeColor = System.Drawing.Color.Red;
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
            Console.WriteLine("EVENT FIRING");
            bool checkUSB = checkIfUsbAlive();
            Console.WriteLine(checkUSB);
            while (checkUSB == false)
            {
                Console.WriteLine(checkUSB);

                checkUSB = checkIfUsbAlive();
                changePortErrMsg("retry");
            }

            if (checkUSB == true)
            {
                changePortErrMsg("true");
                enableConfig("filler");
            }

            return;
        }

        private void DeviceRemovedEvent(object sender, EventArrivedEventArgs e)
        {
            bool checkUSB = checkIfUsbAlive();
            if (checkUSB == false)
            {
                changePortErrMsg("false");
                disableConfig("filler");
            }

            return;
        }

        private void disableConfig(string filler)
        {
            if (panel5.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(disableConfig);
                this.Invoke(d, new object[] { filler });
            }
            else
            {
                led1_intensity.Enabled = false;
                led1_mode.Enabled = false; 
                led1_strobe.Enabled = false;
                led1_test.Enabled = false;
                led2_intensity.Enabled = false;
                led2_mode.Enabled = false;
                led2_strobe.Enabled = false;
                led2_test.Enabled = false;
                led3_intensity.Enabled = false;
                led3_mode.Enabled = false;
                led3_strobe.Enabled = false;
                led3_test.Enabled = false;
                strobeChannel.Enabled = false;
                setEdge.Enabled = false;
                setDelay.Enabled = false;
                setPulse.Enabled = false;
                selectGrp.Enabled = false;
                chooseGrp.Enabled = false;
            }

        }

        private void enableConfig(string filler)
        {
            if (panel5.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(enableConfig);
                this.Invoke(d, new object[] { filler });
            }
            else
            {
                led1_intensity.Enabled = true;
                led1_mode.Enabled = true;
                led1_strobe.Enabled = true;
                led1_test.Enabled = true;
                led2_intensity.Enabled = true;
                led2_mode.Enabled = true;
                led2_strobe.Enabled = true;
                led2_test.Enabled = true;
                led3_intensity.Enabled = true;
                led3_mode.Enabled = true;
                led3_strobe.Enabled = true;
                led3_test.Enabled = true;
                strobeChannel.Enabled = true;
                setEdge.Enabled = true;
                setDelay.Enabled = true;
                setPulse.Enabled = true;
                selectGrp.Enabled = true;
                chooseGrp.Enabled = true;
            }

        }

        void changePortErrMsg(string data)
        {
            if (portError.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(changePortErrMsg);
                this.Invoke(d, new object[] { data });
            }
            else
            {
                if (data == "false")
                {
                    portError.Visible = true;
                    portError.ForeColor = Color.Red;
                    portError.Text = "USB has been unplugged, plug the USB back in";
                    COMport.Text = "";
                    closePort.Enabled = false;
                }
                else if (data == "true")
                {

                    portError.Text = "";
                    portError.Text = "";
                    closePort.Enabled = true;
                }
                else
                {
                    portError.Visible = true;
                    Console.WriteLine("RETRYING");
                    portError.Text = "Retrying...";
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

                portConn.Open();
                

                portConn.Write("QB\r\n");
                Console.WriteLine("sent data");
                Thread.Sleep(2000);

                reply = portConn.ReadExisting();



                Console.WriteLine(reply);
                if (reply.Contains("PICS"))
                {
                    updateComPortTextbox(portName);
                    return true;
                }
            }
            return false;
        }

        void updateComPortTextbox(string port)
        {
            if (COMport.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(updateComPortTextbox);
                this.Invoke(d, new object[] { port });
            }
            else
            {
                COMport.Text = port;
                portError.Text = "";
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

                foreach (string s in splitData)
                {
                    //Console.WriteLine(s);
                    consoleDisplay.Items.Add(s);
                }
            }
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
            // Channels 1 2 3
            if (c010203_isCurrent)
            {
                if (chooseGrp.SelectedIndex == 0) 
                { 
                    c010203_isGrouped = true; 
                }
                else 
                { 
                    c010203_isGrouped = false; 
                }
                c1_edge_value = "None";
                c2_edge_value = "None";
                c3_edge_value = "None";
                c1_pulse_value = 0;
                c2_pulse_value = 0;
                c3_pulse_value = 0;
                c1_delay_value = 0;
                c2_delay_value = 0;
                c3_delay_value = 0;
                resetModes();

            }
            // Channels 4 5 6
            else if (c040506_isCurrent)
            {
                if (chooseGrp.SelectedIndex == 0) 
                { 
                    c040506_isGrouped = true; 
                }
                else 
                { 
                    c040506_isGrouped = false; 
                }
                c4_edge_value = "None";
                c5_edge_value = "None";
                c6_edge_value = "None";
                c4_pulse_value = 0;
                c5_pulse_value = 0;
                c6_pulse_value = 0;
                c4_delay_value = 0;
                c5_delay_value = 0;
                c6_delay_value = 0;
                resetModes();

            }
            // Channels 7 8 9
            else if (c070809_isCurrent)
            {
                if (chooseGrp.SelectedIndex == 0) 
                {
                    c070809_isGrouped = true; 
                }
                else 
                { 
                    c070809_isGrouped = false; 
                }
                c7_edge_value = "None";
                c8_edge_value = "None";
                c9_edge_value = "None";
                c7_pulse_value = 0;
                c8_pulse_value = 0;
                c9_pulse_value = 0;
                c7_delay_value = 0;
                c8_delay_value = 0;
                c9_delay_value = 0;
                resetModes();

            }
            // Channels 10 11 12
            else if (c101112_isCurrent)
            {
                if (chooseGrp.SelectedIndex == 0) 
                { 
                    c101112_isGrouped = true; 
                }
                else 
                { 
                    c101112_isGrouped = false; 
                }
                c10_edge_value = "None";
                c11_edge_value = "None";
                c12_edge_value = "None";
                c10_pulse_value = 0;
                c11_pulse_value = 0;
                c12_pulse_value = 0;
                c10_delay_value = 0;
                c11_delay_value = 0;
                c12_delay_value = 0;
                resetModes();

            }
            // Channels 13 14 15
            else if (c131415_isCurrent)
            {
                if (chooseGrp.SelectedIndex == 0) 
                { 
                    c131415_isGrouped = true; 
                }
                else 
                { 
                    c131415_isGrouped = false; 
                }
                c13_edge_value = "None";
                c14_edge_value = "None";
                c15_edge_value = "None";
                c13_pulse_value = 0;
                c14_pulse_value = 0;
                c15_pulse_value = 0;
                c13_delay_value = 0;
                c14_delay_value = 0;
                c15_delay_value = 0;
                resetModes();

            }
            else
            {

            }
            //displayModes();

            //Console.WriteLine("c000123 " + c010203_isGrouped);
            //Console.WriteLine("c000456 " + c040506_isGrouped);
            //Console.WriteLine("c000789 " + c070809_isGrouped);
            //Console.WriteLine("c101112 " + c101112_isGrouped);
            //Console.WriteLine("c131415 " + c131415_isGrouped);

            // Change Strobe Settings Visuals
            strobeChannel.Items.Clear();
            if (c010203_isGrouped)
            {
                strobeChannel.Items.Add("Group 1");
            }
            else
            {
                strobeChannel.Items.Add("Channel 1");
                strobeChannel.Items.Add("Channel 2");
                strobeChannel.Items.Add("Channel 3");
            }

            if (c040506_isGrouped)
            {
                strobeChannel.Items.Add("Group 2");
            }
            else
            {
                strobeChannel.Items.Add("Channel 4");
                strobeChannel.Items.Add("Channel 5");
                strobeChannel.Items.Add("Channel 6");
            }

            if (c070809_isGrouped)
            {
                strobeChannel.Items.Add("Group 3");
            }
            else
            {
                strobeChannel.Items.Add("Channel 7");
                strobeChannel.Items.Add("Channel 8");
                strobeChannel.Items.Add("Channel 9");
            }
            if (c101112_isGrouped)
            {
                strobeChannel.Items.Add("Group 4");
            }
            else
            {
                strobeChannel.Items.Add("Channel 10");
                strobeChannel.Items.Add("Channel 11");
                strobeChannel.Items.Add("Channel 12");
            }
            if (c131415_isGrouped)
            {
                strobeChannel.Items.Add("Group 5");
            }
            else
            {
                strobeChannel.Items.Add("Channel 13");
                strobeChannel.Items.Add("Channel 14");
                strobeChannel.Items.Add("Channel 15");
            }
            strobeChannel.Items.Add("Channel 16");
            setModes();

        }

        private void led1_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (led1_mode.SelectedIndex == 0) { led1_strobe.Enabled = false; }
            else { led1_strobe.Enabled = true; }

            // Channels 1 2 3
            if (c010203_isCurrent)
            {
                c1_mode_value = led1_mode.Text;
                if (c010203_isGrouped)
                {
                    c2_mode_value = led1_mode.Text;
                    c3_mode_value = led1_mode.Text;
                }
            }
            // Channels 4 5 6
            else if (c040506_isCurrent)
            {
                c4_mode_value = led1_mode.Text;
                if (c040506_isGrouped)
                {
                    c5_mode_value = led1_mode.Text;
                    c6_mode_value = led1_mode.Text;
                }
            }
            // Channels 7 8 9
            else if (c070809_isCurrent)
            {
                c7_mode_value = led1_mode.Text;
                if (c070809_isGrouped)
                {
                    c8_mode_value = led1_mode.Text;
                    c9_mode_value = led1_mode.Text;
                }
            }
            // Channels 10 11 12
            else if (c101112_isCurrent)
            {
                c10_mode_value = led1_mode.Text;
                if (c101112_isGrouped)
                {
                    c11_mode_value = led1_mode.Text;
                    c12_mode_value = led1_mode.Text;
                }
            }
            // Channels 13 14 15
            else if (c131415_isCurrent)
            {
                c13_mode_value = led1_mode.Text;
                if (c131415_isGrouped)
                {
                    c14_mode_value = led1_mode.Text;
                    c15_mode_value = led1_mode.Text;
                }
            }
            else
            {
                c16_mode_value = led1_mode.Text;
            }
            setModes();

            //displayModes();
        }

        private void led2_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Channels 1 2 3
            if (c010203_isCurrent)
            {
                c2_mode_value = led2_mode.Text;
                if (c010203_isGrouped)
                {
                    c1_mode_value = led2_mode.Text;
                    c3_mode_value = led2_mode.Text;
                }
            }
            // Channels 4 5 6
            else if (c040506_isCurrent)
            {
                c5_mode_value = led2_mode.Text;
                if (c040506_isGrouped)
                {
                    c4_mode_value = led2_mode.Text;
                    c6_mode_value = led2_mode.Text;
                }
            }
            // Channels 7 8 9
            else if (c070809_isCurrent)
            {
                c8_mode_value = led2_mode.Text;
                if (c070809_isGrouped)
                {
                    c7_mode_value = led2_mode.Text;
                    c9_mode_value = led2_mode.Text;
                }
            }
            // Channels 10 11 12
            else if (c101112_isCurrent)
            {
                c11_mode_value = led2_mode.Text;
                if (c101112_isGrouped)
                {
                    c10_mode_value = led2_mode.Text;
                    c12_mode_value = led2_mode.Text;
                }
            }
            // Channels 13 14 15
            else if (c131415_isCurrent)
            {
                c14_mode_value = led2_mode.Text;
                if (c131415_isGrouped)
                {
                    c13_mode_value = led2_mode.Text;
                    c15_mode_value = led2_mode.Text;
                }
            }
            else
            {
                // Do nothing
            }
            setModes();

            //displayModes();
        }

        private void led3_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (led3_mode.SelectedIndex == 0) { led3_strobe.Enabled = false; }
            else { led3_strobe.Enabled = true; }

            // Channels 1 2 3
            if (c010203_isCurrent)
            {
                c3_mode_value = led3_mode.Text;
                if (c010203_isGrouped)
                {
                    c1_mode_value = led3_mode.Text;
                    c2_mode_value = led3_mode.Text;
                }
            }
            // Channels 4 5 6
            else if (c040506_isCurrent)
            {
                c6_mode_value = led3_mode.Text;
                if (c040506_isGrouped)
                {
                    c4_mode_value = led3_mode.Text;
                    c5_mode_value = led3_mode.Text;
                }
            }
            // Channels 7 8 9
            else if (c070809_isCurrent)
            {
                c9_mode_value = led3_mode.Text;
                if (c070809_isGrouped)
                {
                    c7_mode_value = led3_mode.Text;
                    c8_mode_value = led3_mode.Text;
                }
            }
            // Channels 10 11 12
            else if (c101112_isCurrent)
            {
                c12_mode_value = led3_mode.Text;
                if (c101112_isGrouped)
                {
                    c10_mode_value = led3_mode.Text;
                    c11_mode_value = led3_mode.Text;
                }
            }
            // Channels 13 14 15
            else if (c131415_isCurrent)
            {
                c15_mode_value = led3_mode.Text;
                if (c131415_isGrouped)
                {
                    c13_mode_value = led3_mode.Text;
                    c14_mode_value = led3_mode.Text;
                }
            }
            else
            {
                // Do nothing
            }
            setModes();

            //displayModes();
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
                
                //errorText.Text = getError(1, false);
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
                //errorText.ForeColor = Color.Red;
                //errorText.Text = getError(1, true);
            };
        }

        private void led2_intensity_TextChanged(object sender, EventArgs e)
        {
            led2_testStop = 0;
            if (checkIntensity(led2_intensity.Text))
            {
                int rgb_value = getRGB(led2_intensity.Text);
                //errorText.Text = getError(2, false);

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
                //errorText.ForeColor = Color.Red;
                //errorText.Text = getError(2, true);
            };
        }

        private void led3_intensity_TextChanged(object sender, EventArgs e)
        {
            led3_testStop = 0;
            if (checkIntensity(led3_intensity.Text))
            {
                int rgb_value = getRGB(led3_intensity.Text);
                //errorText.Text = getError(3, false);

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
                //errorText.ForeColor = Color.Red;
                //errorText.Text = getError(3, true);
            };
        }

        private void selectGrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            led1_testStop = 0;
            led2_testStop = 0;
            led3_testStop = 0;

            Console.WriteLine("Group Changed");

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
            setModes();
            setStrobe();
            //displayModes();
            //displayStrobes();
        }

        private void strobeChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string StrobeIndex = strobeChannel.Text;
            Console.WriteLine(StrobeIndex);

            switch (StrobeIndex)
            {
                case "Group 1":
                    currentStrobe = "Group 1";
                    setEdge.SelectedIndex = returnEdgeIndex(c1_edge_value);
                    setPulse.Text = Convert.ToString(c1_pulse_value);
                    setDelay.Text = Convert.ToString(c1_delay_value);
                    break;
                case "Group 2":
                    currentStrobe = "Group 2";
                    setEdge.SelectedIndex = returnEdgeIndex(c4_edge_value);
                    setPulse.Text = Convert.ToString(c4_pulse_value);
                    setDelay.Text = Convert.ToString(c4_delay_value);
                    break;
                case "Group 3":
                    currentStrobe = "Group 3";
                    setEdge.SelectedIndex = returnEdgeIndex(c7_edge_value);
                    setPulse.Text = Convert.ToString(c7_pulse_value);
                    setDelay.Text = Convert.ToString(c7_delay_value);
                    break;
                case "Group 4":
                    currentStrobe = "Group 4";
                    setEdge.SelectedIndex = returnEdgeIndex(c10_edge_value);
                    setPulse.Text = Convert.ToString(c10_pulse_value);
                    setDelay.Text = Convert.ToString(c10_delay_value);
                    break;
                case "Group 5":
                    currentStrobe = "Group 5";
                    setEdge.SelectedIndex = returnEdgeIndex(c13_edge_value);
                    setPulse.Text = Convert.ToString(c13_pulse_value);
                    setDelay.Text = Convert.ToString(c13_delay_value);
                    break;
                case "Channel 1":
                    currentStrobe = "Channel 1";
                    setEdge.SelectedIndex = returnEdgeIndex(c1_edge_value);
                    setPulse.Text = Convert.ToString(c1_pulse_value);
                    setDelay.Text = Convert.ToString(c1_delay_value);
                    break;
                case "Channel 2":
                    currentStrobe = "Channel 2";
                    setEdge.SelectedIndex = returnEdgeIndex(c2_edge_value);
                    setPulse.Text = Convert.ToString(c2_pulse_value);
                    setDelay.Text = Convert.ToString(c2_delay_value);
                    break;
                case "Channel 3":
                    currentStrobe = "Channel 3";
                    setEdge.SelectedIndex = returnEdgeIndex(c3_edge_value);
                    setPulse.Text = Convert.ToString(c3_pulse_value);
                    setDelay.Text = Convert.ToString(c3_delay_value);
                    break;
                case "Channel 4":
                    currentStrobe = "Channel 4";
                    setEdge.SelectedIndex = returnEdgeIndex(c4_edge_value);
                    setPulse.Text = Convert.ToString(c4_pulse_value);
                    setDelay.Text = Convert.ToString(c4_delay_value);
                    break;
                case "Channel 5":
                    currentStrobe = "Channel 5";
                    setEdge.SelectedIndex = returnEdgeIndex(c5_edge_value);
                    setPulse.Text = Convert.ToString(c5_pulse_value);
                    setDelay.Text = Convert.ToString(c5_delay_value);
                    break;
                case "Channel 6":
                    currentStrobe = "Channel 6";
                    setEdge.SelectedIndex = returnEdgeIndex(c6_edge_value);
                    setPulse.Text = Convert.ToString(c6_pulse_value);
                    setDelay.Text = Convert.ToString(c6_delay_value);
                    break;
                case "Channel 7":
                    currentStrobe = "Channel 7";
                    setEdge.SelectedIndex = returnEdgeIndex(c7_edge_value);
                    setPulse.Text = Convert.ToString(c7_pulse_value);
                    setDelay.Text = Convert.ToString(c7_delay_value);
                    break;
                case "Channel 8":
                    currentStrobe = "Channel 8";
                    setEdge.SelectedIndex = returnEdgeIndex(c9_edge_value);
                    setPulse.Text = Convert.ToString(c9_pulse_value);
                    setDelay.Text = Convert.ToString(c9_delay_value);
                    break;
                case "Channel 9":
                    currentStrobe = "Channel 9";
                    setEdge.SelectedIndex = returnEdgeIndex(c9_edge_value);
                    setPulse.Text = Convert.ToString(c9_pulse_value);
                    setDelay.Text = Convert.ToString(c9_delay_value);
                    break;
                case "Channel 10":
                    currentStrobe = "Channel 10";
                    setEdge.SelectedIndex = returnEdgeIndex(c10_edge_value);
                    setPulse.Text = Convert.ToString(c10_pulse_value);
                    setDelay.Text = Convert.ToString(c10_delay_value);
                    break;
                case "Channel 11":
                    currentStrobe = "Channel 11";
                    setEdge.SelectedIndex = returnEdgeIndex(c11_edge_value);
                    setPulse.Text = Convert.ToString(c11_pulse_value);
                    setDelay.Text = Convert.ToString(c11_delay_value);
                    break;
                case "Channel 12":
                    currentStrobe = "Channel 12";
                    setEdge.SelectedIndex = returnEdgeIndex(c12_edge_value);
                    setPulse.Text = Convert.ToString(c12_pulse_value);
                    setDelay.Text = Convert.ToString(c12_delay_value);
                    break;
                case "Channel 13":
                    currentStrobe = "Channel 13";
                    setEdge.SelectedIndex = returnEdgeIndex(c13_edge_value);
                    setPulse.Text = Convert.ToString(c13_pulse_value);
                    setDelay.Text = Convert.ToString(c13_delay_value);
                    break;
                case "Channel 14":
                    currentStrobe = "Channel 14";
                    setEdge.SelectedIndex = returnEdgeIndex(c14_edge_value);
                    setPulse.Text = Convert.ToString(c14_pulse_value);
                    setDelay.Text = Convert.ToString(c14_delay_value);
                    break;
                case "Channel 15":
                    currentStrobe = "Channel 15";
                    setEdge.SelectedIndex = returnEdgeIndex(c15_edge_value);
                    setPulse.Text = Convert.ToString(c15_pulse_value);
                    setDelay.Text = Convert.ToString(c15_delay_value);
                    break;
                case "Channel 16":
                    currentStrobe = "Channel 16";
                    setEdge.SelectedIndex = returnEdgeIndex(c16_edge_value);
                    setPulse.Text = Convert.ToString(c16_pulse_value);
                    setDelay.Text = Convert.ToString(c16_delay_value);
                    break;
            }
        }

        public void setStrobeSettings()
        {
            string StrobeIndex = strobeChannel.Text;
            Console.WriteLine(StrobeIndex);

            switch (StrobeIndex)
            {
                case "Group 1":
                    currentStrobe = "Group 1";
                    setEdge.SelectedIndex = returnEdgeIndex(c1_edge_value);
                    setPulse.Text = Convert.ToString(c1_pulse_value);
                    setDelay.Text = Convert.ToString(c1_delay_value);
                    break;
                case "Group 2":
                    currentStrobe = "Group 2";
                    setEdge.SelectedIndex = returnEdgeIndex(c4_edge_value);
                    setPulse.Text = Convert.ToString(c4_pulse_value);
                    setDelay.Text = Convert.ToString(c4_delay_value);
                    break;
                case "Group 3":
                    currentStrobe = "Group 3";
                    setEdge.SelectedIndex = returnEdgeIndex(c7_edge_value);
                    setPulse.Text = Convert.ToString(c7_pulse_value);
                    setDelay.Text = Convert.ToString(c7_delay_value);
                    break;
                case "Group 4":
                    currentStrobe = "Group 4";
                    setEdge.SelectedIndex = returnEdgeIndex(c10_edge_value);
                    setPulse.Text = Convert.ToString(c10_pulse_value);
                    setDelay.Text = Convert.ToString(c10_delay_value);
                    break;
                case "Group 5":
                    currentStrobe = "Group 5";
                    setEdge.SelectedIndex = returnEdgeIndex(c13_edge_value);
                    setPulse.Text = Convert.ToString(c13_pulse_value);
                    setDelay.Text = Convert.ToString(c13_delay_value);
                    break;
                case "Channel 1":
                    currentStrobe = "Channel 1";
                    setEdge.SelectedIndex = returnEdgeIndex(c1_edge_value);
                    setPulse.Text = Convert.ToString(c1_pulse_value);
                    setDelay.Text = Convert.ToString(c1_delay_value);
                    break;
                case "Channel 2":
                    currentStrobe = "Channel 2";
                    setEdge.SelectedIndex = returnEdgeIndex(c2_edge_value);
                    setPulse.Text = Convert.ToString(c2_pulse_value);
                    setDelay.Text = Convert.ToString(c2_delay_value);
                    break;
                case "Channel 3":
                    currentStrobe = "Channel 3";
                    setEdge.SelectedIndex = returnEdgeIndex(c3_edge_value);
                    setPulse.Text = Convert.ToString(c3_pulse_value);
                    setDelay.Text = Convert.ToString(c3_delay_value);
                    break;
                case "Channel 4":
                    currentStrobe = "Channel 4";
                    setEdge.SelectedIndex = returnEdgeIndex(c4_edge_value);
                    setPulse.Text = Convert.ToString(c4_pulse_value);
                    setDelay.Text = Convert.ToString(c4_delay_value);
                    break;
                case "Channel 5":
                    currentStrobe = "Channel 5";
                    setEdge.SelectedIndex = returnEdgeIndex(c5_edge_value);
                    setPulse.Text = Convert.ToString(c5_pulse_value);
                    setDelay.Text = Convert.ToString(c5_delay_value);
                    break;
                case "Channel 6":
                    currentStrobe = "Channel 6";
                    setEdge.SelectedIndex = returnEdgeIndex(c6_edge_value);
                    setPulse.Text = Convert.ToString(c6_pulse_value);
                    setDelay.Text = Convert.ToString(c6_delay_value);
                    break;
                case "Channel 7":
                    currentStrobe = "Channel 7";
                    setEdge.SelectedIndex = returnEdgeIndex(c7_edge_value);
                    setPulse.Text = Convert.ToString(c7_pulse_value);
                    setDelay.Text = Convert.ToString(c7_delay_value);
                    break;
                case "Channel 8":
                    currentStrobe = "Channel 8";
                    setEdge.SelectedIndex = returnEdgeIndex(c9_edge_value);
                    setPulse.Text = Convert.ToString(c9_pulse_value);
                    setDelay.Text = Convert.ToString(c9_delay_value);
                    break;
                case "Channel 9":
                    currentStrobe = "Channel 9";
                    setEdge.SelectedIndex = returnEdgeIndex(c9_edge_value);
                    setPulse.Text = Convert.ToString(c9_pulse_value);
                    setDelay.Text = Convert.ToString(c9_delay_value);
                    break;
                case "Channel 10":
                    currentStrobe = "Channel 10";
                    setEdge.SelectedIndex = returnEdgeIndex(c10_edge_value);
                    setPulse.Text = Convert.ToString(c10_pulse_value);
                    setDelay.Text = Convert.ToString(c10_delay_value);
                    break;
                case "Channel 11":
                    currentStrobe = "Channel 11";
                    setEdge.SelectedIndex = returnEdgeIndex(c11_edge_value);
                    setPulse.Text = Convert.ToString(c11_pulse_value);
                    setDelay.Text = Convert.ToString(c11_delay_value);
                    break;
                case "Channel 12":
                    currentStrobe = "Channel 12";
                    setEdge.SelectedIndex = returnEdgeIndex(c12_edge_value);
                    setPulse.Text = Convert.ToString(c12_pulse_value);
                    setDelay.Text = Convert.ToString(c12_delay_value);
                    break;
                case "Channel 13":
                    currentStrobe = "Channel 13";
                    setEdge.SelectedIndex = returnEdgeIndex(c13_edge_value);
                    setPulse.Text = Convert.ToString(c13_pulse_value);
                    setDelay.Text = Convert.ToString(c13_delay_value);
                    break;
                case "Channel 14":
                    currentStrobe = "Channel 14";
                    setEdge.SelectedIndex = returnEdgeIndex(c14_edge_value);
                    setPulse.Text = Convert.ToString(c14_pulse_value);
                    setDelay.Text = Convert.ToString(c14_delay_value);
                    break;
                case "Channel 15":
                    currentStrobe = "Channel 15";
                    setEdge.SelectedIndex = returnEdgeIndex(c15_edge_value);
                    setPulse.Text = Convert.ToString(c15_pulse_value);
                    setDelay.Text = Convert.ToString(c15_delay_value);
                    break;
                case "Channel 16":
                    currentStrobe = "Channel 16";
                    setEdge.SelectedIndex = returnEdgeIndex(c16_edge_value);
                    setPulse.Text = Convert.ToString(c16_pulse_value);
                    setDelay.Text = Convert.ToString(c16_delay_value);
                    break;
            }
            //if (strobeChannel.SelectedIndex == 0)
            //{
            //    currentStrobe = 1;
            //    setEdge.SelectedIndex = c1_edge_value;
            //    setPulse.Text = Convert.ToString(c1_pulse_value);
            //    setDelay.Text = Convert.ToString(c1_delay_value);
            //}
            //else if (strobeChannel.SelectedIndex == 1)
            //{
            //    currentStrobe = 2;
            //    setEdge.SelectedIndex = c2_edge_value;
            //    setPulse.Text = Convert.ToString(c2_pulse_value);
            //    setDelay.Text = Convert.ToString(c2_delay_value);
            //}
            //else if (strobeChannel.SelectedIndex == 2)
            //{
            //    currentStrobe = 3;
            //    setEdge.SelectedIndex = c3_edge_value;
            //    setPulse.Text = Convert.ToString(c3_pulse_value);
            //    setDelay.Text = Convert.ToString(c3_delay_value);
            //}
            //else if (strobeChannel.SelectedIndex == 3)
            //{
            //    currentStrobe = 4;
            //    setEdge.SelectedIndex = c4_edge_value;
            //    setPulse.Text = Convert.ToString(c4_pulse_value);
            //    setDelay.Text = Convert.ToString(c4_delay_value);
            //}
            //else if (strobeChannel.SelectedIndex == 4)
            //{
            //    currentStrobe = 5;
            //    setEdge.SelectedIndex = c5_edge_value;
            //    setPulse.Text = Convert.ToString(c5_pulse_value);
            //    setDelay.Text = Convert.ToString(c5_delay_value);
            //}
            //else if (strobeChannel.SelectedIndex == 5)
            //{
            //    currentStrobe = 6;
            //    setEdge.SelectedIndex = c6_edge_value;
            //    setPulse.Text = Convert.ToString(c6_pulse_value);
            //    setDelay.Text = Convert.ToString(c6_delay_value);
            //}
            //else if (strobeChannel.SelectedIndex == 6)
            //{
            //    currentStrobe = 7;
            //    setEdge.SelectedIndex = c7_edge_value;
            //    setPulse.Text = Convert.ToString(c7_pulse_value);
            //    setDelay.Text = Convert.ToString(c7_delay_value);
            //}
            //else if (strobeChannel.SelectedIndex == 7)
            //{
            //    currentStrobe = 8;
            //    setEdge.SelectedIndex = c8_edge_value;
            //    setPulse.Text = Convert.ToString(c8_pulse_value);
            //    setDelay.Text = Convert.ToString(c8_delay_value);
            //}
            //else if (strobeChannel.SelectedIndex == 8)
            //{
            //    currentStrobe = 9;
            //    setEdge.SelectedIndex = c9_edge_value;
            //    setPulse.Text = Convert.ToString(c9_pulse_value);
            //    setDelay.Text = Convert.ToString(c9_delay_value);
            //}
            //else if (strobeChannel.SelectedIndex == 9)
            //{
            //    currentStrobe = 10;
            //    setEdge.SelectedIndex = c10_edge_value;
            //    setPulse.Text = Convert.ToString(c10_pulse_value);
            //    setDelay.Text = Convert.ToString(c10_delay_value);
            //}
            //else if (strobeChannel.SelectedIndex == 10)
            //{
            //    currentStrobe = 11;
            //    setEdge.SelectedIndex = c11_edge_value;
            //    setPulse.Text = Convert.ToString(c11_pulse_value);
            //    setDelay.Text = Convert.ToString(c11_delay_value);
            //}
            //else if (strobeChannel.SelectedIndex == 11)
            //{
            //    currentStrobe = 12;
            //    setEdge.SelectedIndex = c12_edge_value;
            //    setPulse.Text = Convert.ToString(c12_pulse_value);
            //    setDelay.Text = Convert.ToString(c12_delay_value);
            //}
            //else if (strobeChannel.SelectedIndex == 12)
            //{
            //    currentStrobe = 13;
            //    setEdge.SelectedIndex = c13_edge_value;
            //    setPulse.Text = Convert.ToString(c13_pulse_value);
            //    setDelay.Text = Convert.ToString(c13_delay_value);
            //}
            //else if (strobeChannel.SelectedIndex == 13)
            //{
            //    currentStrobe = 14;
            //    setEdge.SelectedIndex = c14_edge_value;
            //    setPulse.Text = Convert.ToString(c14_pulse_value);
            //    setDelay.Text = Convert.ToString(c14_delay_value);
            //}
            //else if (strobeChannel.SelectedIndex == 14)
            //{
            //    currentStrobe = 15;
            //    setEdge.SelectedIndex = c15_edge_value;
            //    setPulse.Text = Convert.ToString(c15_pulse_value);
            //    setDelay.Text = Convert.ToString(c15_delay_value);
            //}
            //else if (strobeChannel.SelectedIndex == 15)
            //{
            //    currentStrobe = 16;
            //    setEdge.SelectedIndex = c16_edge_value;
            //    setPulse.Text = Convert.ToString(c16_pulse_value);
            //    setDelay.Text = Convert.ToString(c16_delay_value);
            //}


        }

        private void setEdge_SelectedIndexChanged(object sender, EventArgs e)
        {
            string edge_value = setEdge.Text;
            //Console.WriteLine(edge_value);

            switch (currentStrobe)
            {
                case "Group 1":
                    c1_edge_value = edge_value;
                    c2_edge_value = edge_value;
                    c3_edge_value = edge_value;
                    break;
                case "Group 2":
                    c4_edge_value = edge_value;
                    c5_edge_value = edge_value;
                    c6_edge_value = edge_value;
                    break;
                case "Group 3":
                    c7_edge_value = edge_value;
                    c8_edge_value = edge_value;
                    c9_edge_value = edge_value;
                    break;
                case "Group 4":
                    c10_edge_value = edge_value;
                    c11_edge_value = edge_value;
                    c12_edge_value = edge_value;
                    break;
                case "Group 5":
                    c13_edge_value = edge_value;
                    c14_edge_value = edge_value;
                    c15_edge_value = edge_value;
                    break;
                case "Channel 1":
                    c1_edge_value = edge_value;
                    break;
                case "Channel 2":
                    c2_edge_value = edge_value;
                    break;
                case "Channel 3":
                    c3_edge_value = edge_value;
                    break;
                case "Channel 4":
                    c4_edge_value = edge_value;
                    break;
                case "Channel 5":
                    c5_edge_value = edge_value;
                    break;
                case "Channel 6":
                    c6_edge_value = edge_value;
                    break;
                case "Channel 7":
                    c7_edge_value = edge_value;
                    break;
                case "Channel 8":
                    c8_edge_value = edge_value;
                    break;
                case "Channel 9":
                    c9_edge_value = edge_value;
                    break;
                case "Channel 10":
                    c10_edge_value = edge_value;
                    break;
                case "Channel 11":
                    c11_edge_value = edge_value;
                    break;
                case "Channel 12":
                    c12_edge_value = edge_value;
                    break;
                case "Channel 13":
                    c13_edge_value = edge_value;
                    break;
                case "Channel 14":
                    c14_edge_value = edge_value;
                    break;
                case "Channel 15":
                    c15_edge_value = edge_value;
                    break;
                case "Channel 16":
                    c16_edge_value = edge_value;
                    break;
            } // End of Switch Loop

            // To test Edge Values
            Console.WriteLine("Channel 1 " + c1_edge_value);
            Console.WriteLine("Channel 2 " + c2_edge_value);
            Console.WriteLine("Channel 3 " + c3_edge_value);
            Console.WriteLine("Channel 4 " + c4_edge_value);
            Console.WriteLine("Channel 5 " + c5_edge_value);
            Console.WriteLine("Channel 6 " + c6_edge_value);
            Console.WriteLine("Channel 7 " + c7_edge_value);
            Console.WriteLine("Channel 8 " + c8_edge_value);
            Console.WriteLine("Channel 9 " + c9_edge_value);
            Console.WriteLine("Channel 10 " + c10_edge_value);
            Console.WriteLine("Channel 11 " + c11_edge_value);
            Console.WriteLine("Channel 12 " + c12_edge_value);
            Console.WriteLine("Channel 13 " + c13_edge_value);
            Console.WriteLine("Channel 14 " + c14_edge_value);
            Console.WriteLine("Channel 15 " + c15_edge_value);
            Console.WriteLine("Channel 16 " + c16_edge_value);
        }

        private void setPulse_TextChanged(object sender, EventArgs e)
        {
            if (checkStrobeValue(setPulse.Text))
            {
                int pulse_value = Convert.ToInt32(setPulse.Text);
                switch (currentStrobe)
                {
                    case "Group 1":
                        c1_pulse_value = pulse_value;
                        c2_pulse_value = pulse_value;
                        c3_pulse_value = pulse_value;
                        break;
                    case "Group 2":
                        c4_pulse_value = pulse_value;
                        c5_pulse_value = pulse_value;
                        c6_pulse_value = pulse_value;
                        break;
                    case "Group 3":
                        c7_pulse_value = pulse_value;
                        c8_pulse_value = pulse_value;
                        c9_pulse_value = pulse_value;
                        break;
                    case "Group 4":
                        c10_pulse_value = pulse_value;
                        c11_pulse_value = pulse_value;
                        c12_pulse_value = pulse_value;
                        break;
                    case "Group 5":
                        c13_pulse_value = pulse_value;
                        c14_pulse_value = pulse_value;
                        c15_pulse_value = pulse_value;
                        break;
                    case "Channel 1":
                        c1_pulse_value = pulse_value;
                        break;
                    case "Channel 2":
                        c2_pulse_value = pulse_value;
                        break;
                    case "Channel 3":
                        c3_pulse_value = pulse_value;
                        break;
                    case "Channel 4":
                        c4_pulse_value = pulse_value;
                        break;
                    case "Channel 5":
                        c5_pulse_value = pulse_value;
                        break;
                    case "Channel 6":
                        c6_pulse_value = pulse_value;
                        break;
                    case "Channel 7":
                        c7_pulse_value = pulse_value;
                        break;
                    case "Channel 8":
                        c8_pulse_value = pulse_value;
                        break;
                    case "Channel 9":
                        c9_pulse_value = pulse_value;
                        break;
                    case "Channel 10":
                        c10_pulse_value = pulse_value;
                        break;
                    case "Channel 11":
                        c11_pulse_value = pulse_value;
                        break;
                    case "Channel 12":
                        c12_pulse_value = pulse_value;
                        break;
                    case "Channel 13":
                        c13_pulse_value = pulse_value;
                        break;
                    case "Channel 14":
                        c14_pulse_value = pulse_value;
                        break;
                    case "Channel 15":
                        c15_pulse_value = pulse_value;
                        break;
                    case "Channel 16":
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
                    case "Group 1":
                        c1_delay_value = delay_value;
                        c2_delay_value = delay_value;
                        c3_delay_value = delay_value;
                        break;
                    case "Group 2":
                        c4_delay_value = delay_value;
                        c5_delay_value = delay_value;
                        c6_delay_value = delay_value;
                        break;
                    case "Group 3":
                        c7_delay_value = delay_value;
                        c8_delay_value = delay_value;
                        c9_delay_value = delay_value;
                        break;
                    case "Group 4":
                        c10_delay_value = delay_value;
                        c11_delay_value = delay_value;
                        c12_delay_value = delay_value;
                        break;
                    case "Group 5":
                        c13_delay_value = delay_value;
                        c14_delay_value = delay_value;
                        c15_delay_value = delay_value;
                        break;
                    case "Channel 1":
                        c1_delay_value = delay_value;
                        break;
                    case "Channel 2":
                        c2_delay_value = delay_value;
                        break;
                    case "Channel 3":
                        c3_delay_value = delay_value;
                        break;
                    case "Channel 4":
                        c4_delay_value = delay_value;
                        break;
                    case "Channel 5":
                        c5_delay_value = delay_value;
                        break;
                    case "Channel 6":
                        c6_delay_value = delay_value;
                        break;
                    case "Channel 7":
                        c7_delay_value = delay_value;
                        break;
                    case "Channel 8":
                        c8_delay_value = delay_value;
                        break;
                    case "Channel 9":
                        c9_delay_value = delay_value;
                        break;
                    case "Channel 10":
                        c10_delay_value = delay_value;
                        break;
                    case "Channel 11":
                        c11_delay_value = delay_value;
                        break;
                    case "Channel 12":
                        c12_delay_value = delay_value;
                        break;
                    case "Channel 13":
                        c13_delay_value = delay_value;
                        break;
                    case "Channel 14":
                        c14_delay_value = delay_value;
                        break;
                    case "Channel 15":
                        c15_delay_value = delay_value;
                        break;
                    case "Channel 16":
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

        private void updateFile_Click(object sender, EventArgs e)
        {
            generateConfig();
            SaveFile saveForm = new SaveFile();
            saveForm.mainForm = this;
            saveForm.ShowDialog();
        }

        public void generateConfig()
        {
            string c1_addText = displayValues("CH1", c1_rgb_value.ToString(), c1_edge_value.ToString().ToString(), c1_mode_value, c1_strobe_value, c1_pulse_value.ToString(), c1_delay_value.ToString());
            string c2_addText = displayValues("CH2", c2_rgb_value.ToString(), c2_edge_value.ToString().ToString(), c2_mode_value, c2_strobe_value, c2_pulse_value.ToString(), c1_delay_value.ToString());
            string c3_addText = displayValues("CH3", c3_rgb_value.ToString(), c3_edge_value.ToString().ToString(), c3_mode_value, c3_strobe_value, c3_pulse_value.ToString(), c3_delay_value.ToString());
            string c4_addText = displayValues("CH4", c4_rgb_value.ToString(), c4_edge_value.ToString().ToString(), c4_mode_value, c4_strobe_value, c4_pulse_value.ToString(), c4_delay_value.ToString());
            string c5_addText = displayValues("CH5", c5_rgb_value.ToString(), c5_edge_value.ToString().ToString(), c5_mode_value, c5_strobe_value, c5_pulse_value.ToString(), c5_delay_value.ToString());
            string c6_addText = displayValues("CH6", c6_rgb_value.ToString(), c6_edge_value.ToString().ToString(), c6_mode_value, c6_strobe_value, c6_pulse_value.ToString(), c6_delay_value.ToString());
            string c7_addText = displayValues("CH7", c7_rgb_value.ToString(), c7_edge_value.ToString().ToString(), c7_mode_value, c7_strobe_value, c7_pulse_value.ToString(), c7_delay_value.ToString());
            string c8_addText = displayValues("CH8", c8_rgb_value.ToString(), c8_edge_value.ToString().ToString(), c8_mode_value, c8_strobe_value, c8_pulse_value.ToString(), c8_delay_value.ToString());
            string c9_addText = displayValues("CH9", c9_rgb_value.ToString(), c9_edge_value.ToString().ToString(), c9_mode_value, c9_strobe_value, c9_pulse_value.ToString(), c9_delay_value.ToString());
            string c10_addText = displayValues("CH10", c10_rgb_value.ToString(), c10_edge_value.ToString().ToString(), c10_mode_value, c10_strobe_value, c10_pulse_value.ToString(), c10_delay_value.ToString());
            string c11_addText = displayValues("CH11", c11_rgb_value.ToString(), c11_edge_value.ToString().ToString(), c11_mode_value, c11_strobe_value, c11_pulse_value.ToString(), c11_delay_value.ToString());
            string c12_addText = displayValues("CH12", c12_rgb_value.ToString(), c12_edge_value.ToString().ToString(), c12_mode_value, c12_strobe_value, c12_pulse_value.ToString(), c12_delay_value.ToString());
            string c13_addText = displayValues("CH13", c13_rgb_value.ToString(), c13_edge_value.ToString().ToString(), c13_mode_value, c13_strobe_value, c13_pulse_value.ToString(), c13_delay_value.ToString());
            string c14_addText = displayValues("CH14", c14_rgb_value.ToString(), c14_edge_value.ToString().ToString(), c14_mode_value, c14_strobe_value, c14_pulse_value.ToString(), c14_delay_value.ToString());
            string c15_addText = displayValues("CH15", c15_rgb_value.ToString(), c15_edge_value.ToString().ToString(), c15_mode_value, c15_strobe_value, c15_pulse_value.ToString(), c15_delay_value.ToString());
            string c16_addText = displayValues("CH16", c16_rgb_value.ToString(), c16_edge_value.ToString(), c16_mode_value, c16_strobe_value, c16_pulse_value.ToString(), c16_delay_value.ToString());

            string g1_setting = "";
            string g2_setting = "";
            string g3_setting = "";
            string g4_setting = "";
            string g5_setting = "";

            if(c010203_isGrouped == true)
            {
                g1_setting = "GROUPED";
            }

            if (c040506_isGrouped == true)
            {
                g2_setting = "GROUPED";
            }

            if (c070809_isGrouped == true)
            {
                g3_setting = "GROUPED";
            }

            if (c101112_isGrouped == true)
            {
                g4_setting = "GROUPED";
            }

            if (c131415_isGrouped == true)
            {
                g5_setting = "GROUPED";
            }

            Console.WriteLine(g1_setting);

            string g1_addText = displaySettings(g1_setting, "Group 1", "CH1", "CH2", "CH3");
            string g2_addText = displaySettings(g2_setting, "Group 2", "CH4", "CH5", "CH6");
            string g3_addText = displaySettings(g3_setting, "Group 3", "CH7", "CH8", "CH9");
            string g4_addText = displaySettings(g4_setting, "Group 4", "CH10", "CH11", "CH12");
            string g5_addText = displaySettings(g5_setting, "Group 5", "CH13", "CH14", "CH15");
            string g6_addText = $"\nGroup 6 Settings, [UNGROUPED Red:CH16] .";

            //uses global list to add data and generate the string to send to the hardware
            config.Clear();
            sendToHardware = "";

            config.Add("MAIN BOARD = " + selectBoard.Text + ".");

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
            if (setting == "GROUPED")
            {
                return $"\n{group} Settings, [GROUPED Red:{first}, Green:{second}, Blue:{third}] ";
            }
            else if (setting == "UNGROUPED")
            {
                return $"\n{group} Settings, [UNGROUPED:Red:{first}, Green:{second}, Blue:{third}] ";
            }
            else
            {
                return "displaySettings function went wrong";
            }
        }

        private void fc_help_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Switch File\n" +
                            "Choose a configuration file that you have previously saved.\n\n" +
                            "Update File\n" +
                            "If this file is saved, you can choose to update it to save the new changes you have made to the currently saved one.\n\n" +
                            "Upload File\n" +
                            "Send the data over to the board to program the connected light controller.\n" +
                            "Data will also be displayed on the console for user to examine the configurations that they have made."
                ,"Info on File Control Settings", MessageBoxButtons.OK);
        }

        private void led_help_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Channel Selection\n" +
                            "A group normally consists of 3 channels in the order of [1,2,3] , [4,5,6] , [7,8,9] , [10,11,12] , [13,14,15].\n" +
                            "Except for Channel 16 will be on its own, due to the hardware limitations.\n" +
                            "Users can have the all the channels but channel 16 to be grouped or ungrouped.\n" +
                            "Grouped > Channels will display RGB on the device being programmed.\n" +
                            "Ungrouped > Channels will display their respective static RGB colours (RED/GREEN/BLUE)\n\n" +

                            "Strobe Settings\n" +
                            "Users should first select the channel to configure their respective strobe settings. By default, Channel 1 is first selected for the user.\n" +
                            "Pulse > How long the light stays on for.\n" +
                            "Delay > how long the light stays off for.\n" +
                            "Edge > Rising state, Falling state, None. Results are only visible on the device.\n\n" +

                            "LED Settings\n" +
                            "Intensity > How strong is the light.\n" +
                            "Mode > if the user selects static, the channel will not inherit the above strobe settings. If the user selects strobe, it will inheirt the above strobe settings.\n" +
                            "Strobe > Users can select none or the numbers 1 through 8. The numbers represent the 8 buttons that when pressed will cause their respective lights to strobe on the device.\n" +
                            "If users input the values correctly, users are able to test the strobe and LED settings via the testing area."
                , "Info on LED Settings", MessageBoxButtons.OK);
        }

        private void led1_strobe_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Channels 1 2 3
            if (c010203_isCurrent)
            {
                c1_strobe_value = led1_strobe.Text;
            }
            // Channels 4 5 6
            else if (c040506_isCurrent)
            {
                c4_strobe_value = led1_strobe.Text;
            }
            // Channels 7 8 9
            else if (c070809_isCurrent)
            {
                c7_strobe_value = led1_strobe.Text;
            }
            // Channels 10 11 12
            else if (c101112_isCurrent)
            {
                c10_strobe_value = led1_strobe.Text;
            }
            // Channels 13 14 15
            else if (c131415_isCurrent)
            {
                c13_strobe_value = led1_strobe.Text;
            }
            else
            {
                c16_strobe_value = led1_strobe.Text;
            }
            //displayStrobes();
        }

        private void led2_strobe_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Channels 1 2 3
            if (c010203_isCurrent)
            {
                c2_strobe_value = led2_strobe.Text;
            }
            // Channels 4 5 6
            else if (c040506_isCurrent)
            {
                c5_strobe_value = led2_strobe.Text;
            }
            // Channels 7 8 9
            else if (c070809_isCurrent)
            {
                c8_strobe_value = led2_strobe.Text;
            }
            // Channels 10 11 12
            else if (c101112_isCurrent)
            {
                c11_strobe_value = led2_strobe.Text;
            }
            // Channels 13 14 15
            else if (c131415_isCurrent)
            {
                c14_strobe_value = led2_strobe.Text;
            }
            else
            {
                // Do nothing
            }
            //displayStrobes();
        }

        private void led3_strobe_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Channels 1 2 3
            if (c010203_isCurrent)
            {
                c3_strobe_value = led3_strobe.Text;
            }
            // Channels 4 5 6
            else if (c040506_isCurrent)
            {
                c6_strobe_value = led3_strobe.Text;
            }
            // Channels 7 8 9
            else if (c070809_isCurrent)
            {
                c9_strobe_value = led3_strobe.Text;
            }
            // Channels 10 11 12
            else if (c101112_isCurrent)
            {
                c12_strobe_value = led3_strobe.Text;
            }
            // Channels 13 14 15
            else if (c131415_isCurrent)
            {
                c15_strobe_value = led3_strobe.Text;
            }
            else
            {
                // Do nothing
            }
            //displayStrobes();
        }

        private void clearIntensity_Click(object sender, EventArgs e)
        {
            led1_intensity.Text = "0";
            led2_intensity.Text = "0";
            led3_intensity.Text = "0";
        }

        private void closePort_Click(object sender, EventArgs e)
        {
            if (portConn.IsOpen)
            {
                portConn.Close();
                closePort.Enabled = false;
                openPort.Enabled = true;
                uploadFile.Enabled = false;
            }
            else
            {
                MessageBox.Show("Port is already closed");
            }
        }

        private void openPort_Click(object sender, EventArgs e)
        {

            portConn.Open();


            if (selectBoard.Text == "")
            {
                closePort.Enabled = true;
                openPort.Enabled = false;
                uploadFile.Enabled = false;
            }
            else
            {
                closePort.Enabled = true;
                openPort.Enabled = false;
                uploadFile.Enabled = true;
            }

            
        }

        private void uploadFile_Click(object sender, EventArgs e)
        {
            //try
            //{
                //Grab Values
                generateConfig();
                string board = sendToHardware.Substring(13, 7);

                string path = @"..\..\savedConfigs";

                path += "\\" + board + ".txt";

                File.WriteAllText(path, sendToHardware);

                //clear console
                consoleDisplay.Items.Clear();

                Console.WriteLine(sendToHardware);

                uploadFile.Enabled = false;
                Thread sendData = new Thread(sendDataToHardware);
                sendData.Start();
            //}
            //catch
            //{
            //    Console.WriteLine("Device disconnected");
            //    consoleDisplay.Items.Add("Device is not connected");
            //}
        }

        void sendDataToHardware()
        {
            Console.WriteLine("test");

            intensity_set("filler");
            Thread.Sleep(10);
            mode_set("filler");
            Thread.Sleep(10);

            pulse_set("filler");
            Thread.Sleep(10);

            delay_set("filler");
            Thread.Sleep(10);

            edge_set("filler");
            Thread.Sleep(10);

            strobe_set("filler");
            Thread.Sleep(10);

            enableUploadBtn("filler");


            Thread.CurrentThread.Abort();
        }

        void enableUploadBtn(string filler)
        {
            if (uploadFile.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(enableUploadBtn);
                this.Invoke(d, new object[] { filler });
            }
            else
            {
                uploadFile.Enabled = true;
            }
        }

        void intensity_set(string filler)
        {
            if (selectBoard.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(intensity_set);
                this.Invoke(d, new object[] { filler });
            }
            else
            {
                int boardNumIndex = selectBoard.Text.IndexOf(" ") + 1;
                string boardNum = selectBoard.Text.Substring(boardNumIndex);
                portConn.Write("IS " + boardNum + ", " + "0, " + c1_rgb_value.ToString() + "\r\n");
                portConn.Write("IS " + boardNum + ", " + "1, " + c2_rgb_value.ToString() + "\r\n");

                portConn.Write("IS " + boardNum + ", " + "2, " + c3_rgb_value.ToString() + "\r\n");

                portConn.Write("IS " + boardNum + ", " + "3, " + c4_rgb_value.ToString() + "\r\n");

                portConn.Write("IS " + boardNum + ", " + "4, " + c5_rgb_value.ToString() + "\r\n");

                portConn.Write("IS " + boardNum + ", " + "5, " + c6_rgb_value.ToString() + "\r\n");

                portConn.Write("IS " + boardNum + ", " + "6, " + c7_rgb_value.ToString() + "\r\n");

                portConn.Write("IS " + boardNum + ", " + "7, " + c8_rgb_value.ToString() + "\r\n");
                Console.WriteLine(portConn.ReadExisting());
                Console.WriteLine("IS " + boardNum + ", " + "1, " + c1_rgb_value.ToString() + "\r\n");
            }

        }

        void mode_set(string filler)
        {
            if (selectBoard.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(mode_set);
                this.Invoke(d, new object[] { filler });
            }
            else
            {
                int boardNumIndex = selectBoard.Text.IndexOf(" ") + 1;
                string boardNum = selectBoard.Text.Substring(boardNumIndex);

                string c1ModeNum = "0";
                string c2ModeNum = "0";
                string c3ModeNum = "0";
                string c4ModeNum = "0";
                string c5ModeNum = "0";
                string c6ModeNum = "0";
                string c7ModeNum = "0";
                string c8ModeNum = "0";

                if (c1_mode_value == "Static")
                {
                    c1ModeNum = "1";
                }
                if (c2_mode_value == "Static")
                {
                    c2ModeNum = "1";
                }
                if (c3_mode_value == "Static")
                {
                    c3ModeNum = "1";
                }
                if (c4_mode_value == "Static")
                {
                    c4ModeNum = "1";
                }
                if (c5_mode_value == "Static")
                {
                    c5ModeNum = "1";
                }
                if (c6_mode_value == "Static")
                {
                    c6ModeNum = "1";
                }
                if (c7_mode_value == "Static")
                {
                    c7ModeNum = "1";
                }
                if (c8_mode_value == "Static")
                {
                    c8ModeNum = "1";
                }

                portConn.Write("MS " + boardNum + ", 0, " + c1ModeNum + "\r\n");

                portConn.Write("MS " + boardNum + ", 1, " + c2ModeNum + "\r\n");


                portConn.Write("MS " + boardNum + ", 2, " + c3ModeNum + "\r\n");


                portConn.Write("MS " + boardNum + ", 3, " + c4ModeNum + "\r\n");


                portConn.Write("MS " + boardNum + ", 4, " + c5ModeNum + "\r\n");


                portConn.Write("MS " + boardNum + ", 5, " + c6ModeNum + "\r\n");


                portConn.Write("MS " + boardNum + ", 6, " + c7ModeNum + "\r\n");


                portConn.Write("MS " + boardNum + ", 7, " + c8ModeNum + "\r\n");
                Console.WriteLine(portConn.ReadExisting());
            }
        }

        void edge_set(string filler)
        {
            if (selectBoard.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(edge_set);
                this.Invoke(d, new object[] { filler });
            }
            else
            {
                int boardNumIndex = selectBoard.Text.IndexOf(" ") + 1;
                string boardNum = selectBoard.Text.Substring(boardNumIndex);
                portConn.Write("ES " + boardNum + ", 0, " + c1_edge_value + "\r\n");


                portConn.Write("ES " + boardNum + ", 1, " + c2_edge_value + "\r\n");


                portConn.Write("ES " + boardNum + ", 2, " + c3_edge_value + "\r\n");


                portConn.Write("ES " + boardNum + ", 3, " + c4_edge_value + "\r\n");


                portConn.Write("ES " + boardNum + ", 4, " + c5_edge_value + "\r\n");


                portConn.Write("ES " + boardNum + ", 5, " + c6_edge_value + "\r\n");


                portConn.Write("ES " + boardNum + ", 6, " + c7_edge_value + "\r\n");


                portConn.Write("ES " + boardNum + ", 7, " + c8_edge_value + "\r\n");
                Console.WriteLine(portConn.ReadExisting());
            }


        }

        void strobe_set(string filler)
        {
            if (selectBoard.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(strobe_set);
                this.Invoke(d, new object[] { filler });
            }
            else
            {
                int boardNumIndex = selectBoard.Text.IndexOf(" ") + 1;
                string boardNum = selectBoard.Text.Substring(boardNumIndex);
                portConn.Write("SS " + boardNum + ", 0, " + c1_strobe_value + "\r\n");


                portConn.Write("SS " + boardNum + ", 1, " + c2_strobe_value + "\r\n");


                portConn.Write("SS " + boardNum + ", 2, " + c3_strobe_value + "\r\n");


                portConn.Write("SS " + boardNum + ", 3, " + c4_strobe_value + "\r\n");


                portConn.Write("SS " + boardNum + ", 4, " + c5_strobe_value + "\r\n");


                portConn.Write("SS " + boardNum + ", 5, " + c6_strobe_value + "\r\n");


                portConn.Write("SS " + boardNum + ", 6, " + c7_strobe_value + "\r\n");


                portConn.Write("SS " + boardNum + ", 7, " + c8_strobe_value + "\r\n");
                Console.WriteLine(portConn.ReadExisting());
            }


        }

        void pulse_set(string filler)
        {
            if (selectBoard.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(pulse_set);
                this.Invoke(d, new object[] { filler });
            }
            else
            {
                int boardNumIndex = selectBoard.Text.IndexOf(" ") + 1;
                string boardNum = selectBoard.Text.Substring(boardNumIndex);
                portConn.Write("PS " + boardNum + ", 0, " + c1_pulse_value.ToString() + "\r\n");


                portConn.Write("PS " + boardNum + ", 1, " + c2_pulse_value.ToString() + "\r\n");


                portConn.Write("PS " + boardNum + ", 2, " + c3_pulse_value.ToString() + "\r\n");


                portConn.Write("PS " + boardNum + ", 3, " + c4_pulse_value.ToString() + "\r\n");


                portConn.Write("PS " + boardNum + ", 4, " + c5_pulse_value.ToString() + "\r\n");


                portConn.Write("PS " + boardNum + ", 5, " + c6_pulse_value.ToString() + "\r\n");


                portConn.Write("PS " + boardNum + ", 6, " + c7_pulse_value.ToString() + "\r\n");


                portConn.Write("PS " + boardNum + ", 7, " + c8_pulse_value.ToString() + "\r\n");
                Console.WriteLine(portConn.ReadExisting());
            }
        }

        void delay_set(string filler)
        {
            if (selectBoard.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(delay_set);
                this.Invoke(d, new object[] { filler });
            }
            else
            {
                int boardNumIndex = selectBoard.Text.IndexOf(" ") + 1;
                string boardNum = selectBoard.Text.Substring(boardNumIndex);
                portConn.Write("DS " + boardNum + ", 0, " + c1_delay_value.ToString() + "\r\n");


                portConn.Write("DS " + boardNum + ", 1, " + c2_delay_value.ToString() + "\r\n");


                portConn.Write("DS " + boardNum + ", 2, " + c3_delay_value.ToString() + "\r\n");


                portConn.Write("DS " + boardNum + ", 3, " + c4_delay_value.ToString() + "\r\n");


                portConn.Write("DS " + boardNum + ", 4, " + c5_delay_value.ToString() + "\r\n");


                portConn.Write("DS " + boardNum + ", 5, " + c6_delay_value.ToString() + "\r\n");


                portConn.Write("DS " + boardNum + ", 6, " + c7_delay_value.ToString() + "\r\n");


                portConn.Write("DS " + boardNum + ", 7, " + c8_delay_value.ToString() + "\r\n");
                Console.WriteLine(portConn.ReadExisting());
            }
        }

        private void switchFile_Click(object sender, EventArgs e)
        {
            OpenFile fileSelect = new OpenFile();
            fileSelect.mainForm = this;
            fileSelect.ShowDialog();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void selectBoard_SelectedIndexChanged(object sender, EventArgs e)
        {
        //    if (selectBoard.Text == "")
        //    {
        //        uploadConfig.Enabled = false;
        //    }
        //    else
        //    {
        //        loadLatestBoardConfig();
        //    }
        }

        private void bs_help_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The com port is automatically selected by querying it for the board number, the app checks for whether the reply contains the string 'PICS', if it does contain it, the port is selected\n\nClick the open port/close port button to open or close the port\n\nSelect the board to send the configuration settings to.");
        }

        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        //private void loadLatestBoardConfig()
        //{
        //    bool grouped = false;
        //    string intensity = "";
        //    string edge = "";
        //    string mode = "";
        //    string strobe = "";
        //    string pulse = "";
        //    string delay = "";

        //    string pathWithFileName = @"..\..\savedConfigs\" + selectBoard.Text + ".txt";

        //    string loadedConfig = File.ReadAllText(pathWithFileName);


        //    string[] splitString = loadedConfig.Split('.');


        //    for (int i = 0; i < splitString.Length; i++)
        //    {
        //        if (splitString[i][1] == 'G')
        //        {
        //            string groupedOrUngroup = splitString[i].Substring(20, 7);
        //            Console.WriteLine(groupedOrUngroup);
        //            if (groupedOrUngroup == "GROUPED")
        //            {
        //                grouped = true;
        //            }
        //            else if (groupedOrUngroup == "UNGROUP")
        //            {

        //                grouped = false;
        //            }
        //        }

        //        else if (splitString[i][1] == '[')
        //        {
        //            int intensityIndexPos = splitString[i].IndexOf("Intensity") + 11;
        //            int intensityNextCommaIndexPos = splitString[i].IndexOf(",", intensityIndexPos);
        //            int intensityLengthBetweenBothIndex = intensityNextCommaIndexPos - intensityIndexPos;

        //            int edgeIndexPos = splitString[i].IndexOf("Edge") + 6;
        //            int edgeNextCommaIndexPos = splitString[i].IndexOf(",", edgeIndexPos);
        //            int edgeLengthBetweenBothIndex = edgeNextCommaIndexPos - edgeIndexPos;

        //            int modeIndexPos = splitString[i].IndexOf("Mode") + 6;
        //            int modeNextCommaIndexPos = splitString[i].IndexOf(",", modeIndexPos);
        //            int modeLengthBetweenBothIndex = modeNextCommaIndexPos - modeIndexPos;

        //            int strobeIndexPos = splitString[i].IndexOf("Strobe:") + 8;
        //            int strobeNextCommaIndexPos = splitString[i].IndexOf(",", strobeIndexPos);
        //            int strobeLengthBetweenBothIndex = strobeNextCommaIndexPos - strobeIndexPos;

        //            int pulseIndexPos = splitString[i].IndexOf("Pulse") + 7;
        //            int pulseNextCommaIndexPos = splitString[i].IndexOf(",", pulseIndexPos);
        //            int pulseLengthBetweenBothIndex = pulseNextCommaIndexPos - pulseIndexPos;

        //            int delayIndexPos = splitString[i].IndexOf("Delay") + 7;
        //            //there is a space at the end of the line
        //            int delayEndOfLineIndex = splitString[i].IndexOf(" ", delayIndexPos);
        //            int delayLengthBetweenBothIndex = delayEndOfLineIndex - delayIndexPos;

        //            intensity = splitString[i].Substring(intensityIndexPos, intensityLengthBetweenBothIndex);
        //            edge = splitString[i].Substring(edgeIndexPos, edgeLengthBetweenBothIndex);
        //            mode = splitString[i].Substring(modeIndexPos, modeLengthBetweenBothIndex);
        //            strobe = splitString[i].Substring(strobeIndexPos, strobeLengthBetweenBothIndex);
        //            pulse = splitString[i].Substring(pulseIndexPos, pulseLengthBetweenBothIndex);
        //            delay = splitString[i].Substring(delayIndexPos, delayLengthBetweenBothIndex);

        //            if (mode == "None")
        //            {
        //                mode = "Static";
        //            }

        //            Console.WriteLine(intensity);
        //            Console.WriteLine(edge);
        //            Console.WriteLine(mode);
        //            Console.WriteLine(strobe);
        //            Console.WriteLine(pulse);
        //            Console.WriteLine(delay);
        //        }
        //        switch (i)
        //        {
        //            //group cases
        //            case 1:
        //                Console.WriteLine("THIS PART IS RUNNING HADBGFSHIAHFSHAJGFUJG");

        //                if (c010203_isGrouped == true)
        //                {
        //                    g1_setting.SelectedIndex = 1;
        //                }
        //                else
        //                {
        //                    Console.WriteLine("THIS PART IS RUNNING LOL");
        //                    g1_setting.SelectedIndex = 0;
        //                }
        //                break;

        //            case 5:
        //                if (c040506_isGrouped == true)
        //                {
        //                    g2_setting.SelectedIndex = 1;
        //                }
        //                else
        //                {
        //                    g2_setting.SelectedIndex = 0;
        //                }
        //                break;

        //            case 9:
        //                if (c070809_isGrouped == true)
        //                {
        //                    g3_setting.SelectedIndex = 1;
        //                }
        //                else
        //                {
        //                    g3_setting.SelectedIndex = 0;
        //                }
        //                break;

        //            case 13:
        //                if (c101112_isGrouped == true)
        //                {
        //                    g4_setting.SelectedIndex = 1;
        //                }
        //                else
        //                {
        //                    g4_setting.SelectedIndex = 0;
        //                }
        //                break;

        //            case 17:
        //                if (c131415_isGrouped == true)
        //                {
        //                    g5_setting.SelectedIndex = 1;
        //                }
        //                else
        //                {
        //                    g5_setting.SelectedIndex = 0;
        //                }
        //                break;

        //            //light config cases
        //            case 2:
        //                if (intensity == "0")
        //                {
        //                    c1_rgb_value = 0;
        //                }
        //                else
        //                {
        //                    c1_rgb_value = Convert.ToInt32(intensity);
        //                }

        //                if (pulse == "None")
        //                {
        //                    c1_pulse_value = 0;
        //                }
        //                else
        //                {
        //                    c1_pulse.Text = pulse;
        //                }

        //                if (delay == "None")
        //                {
        //                    c1_delay.Text = "0";
        //                }
        //                else
        //                {
        //                    c1_delay.Text = delay;
        //                }

        //                c1_edge.Text = edge;
        //                c1_mode.Text = mode;
        //                c1_strobe.Text = strobe;

        //                break;
        //            case 3:
        //                if (intensity == "None")
        //                {
        //                    c2_intensity.Text = "0";
        //                }
        //                else
        //                {
        //                    c2_intensity.Text = intensity;
        //                }

        //                if (pulse == "None")
        //                {
        //                    c2_pulse.Text = "0";
        //                }
        //                else
        //                {
        //                    c2_pulse.Text = pulse;
        //                }

        //                if (delay == "None")
        //                {
        //                    c2_delay.Text = "0";
        //                }
        //                else
        //                {
        //                    c2_delay.Text = delay;
        //                }

        //                c2_edge.Text = edge;
        //                c2_mode.Text = mode;
        //                c2_strobe.Text = strobe;

        //                break;
        //            case 4:
        //                if (intensity == "None")
        //                {
        //                    c3_intensity.Text = "0";
        //                }
        //                else
        //                {
        //                    c3_intensity.Text = intensity;
        //                }

        //                if (pulse == "None")
        //                {
        //                    c3_pulse.Text = "0";
        //                }
        //                else
        //                {
        //                    c3_pulse.Text = pulse;
        //                }

        //                if (delay == "None")
        //                {
        //                    c3_delay.Text = "0";
        //                }
        //                else
        //                {
        //                    c3_delay.Text = delay;
        //                }

        //                c3_edge.Text = edge;
        //                c3_mode.Text = mode;
        //                c3_strobe.Text = strobe;

        //                break;

        //            case 6:
        //                if (intensity == "None")
        //                {
        //                    c4_intensity.Text = "0";
        //                }
        //                else
        //                {
        //                    c4_intensity.Text = intensity;
        //                }

        //                if (pulse == "None")
        //                {
        //                    c4_pulse.Text = "0";
        //                }
        //                else
        //                {
        //                    c4_pulse.Text = pulse;
        //                }

        //                if (delay == "None")
        //                {
        //                    c4_delay.Text = "0";
        //                }
        //                else
        //                {
        //                    c4_delay.Text = delay;
        //                }

        //                c4_edge.Text = edge;
        //                c4_mode.Text = mode;
        //                c4_strobe.Text = strobe;

        //                break;

        //            case 7:
        //                if (intensity == "None")
        //                {
        //                    c5_intensity.Text = "0";
        //                }
        //                else
        //                {
        //                    c5_intensity.Text = intensity;
        //                }

        //                if (pulse == "None")
        //                {
        //                    c5_pulse.Text = "0";
        //                }
        //                else
        //                {
        //                    c5_pulse.Text = pulse;
        //                }

        //                if (delay == "None")
        //                {
        //                    c5_delay.Text = "0";
        //                }
        //                else
        //                {
        //                    c5_delay.Text = delay;
        //                }

        //                c5_edge.Text = edge;
        //                c5_mode.Text = mode;
        //                c5_strobe.Text = strobe;

        //                break;

        //            case 8:
        //                if (intensity == "None")
        //                {
        //                    c6_intensity.Text = "0";
        //                }
        //                else
        //                {
        //                    c6_intensity.Text = intensity;
        //                }

        //                if (pulse == "None")
        //                {
        //                    c6_pulse.Text = "0";
        //                }
        //                else
        //                {
        //                    c6_pulse.Text = pulse;
        //                }

        //                if (delay == "None")
        //                {
        //                    c6_delay.Text = "0";
        //                }
        //                else
        //                {
        //                    c6_delay.Text = delay;
        //                }

        //                c6_edge.Text = edge;
        //                c6_mode.Text = mode;
        //                c6_strobe.Text = strobe;

        //                break;

        //            case 10:
        //                if (intensity == "None")
        //                {
        //                    c7_intensity.Text = "0";
        //                }
        //                else
        //                {
        //                    c7_intensity.Text = intensity;
        //                }

        //                if (pulse == "None")
        //                {
        //                    c7_pulse.Text = "0";
        //                }
        //                else
        //                {
        //                    c7_pulse.Text = pulse;
        //                }

        //                if (delay == "None")
        //                {
        //                    c7_delay.Text = "0";
        //                }
        //                else
        //                {
        //                    c7_delay.Text = delay;
        //                }

        //                c7_edge.Text = edge;
        //                c7_mode.Text = mode;
        //                c7_strobe.Text = strobe;

        //                break;

        //            case 11:
        //                if (intensity == "None")
        //                {
        //                    c8_intensity.Text = "0";
        //                }
        //                else
        //                {
        //                    c8_intensity.Text = intensity;
        //                }

        //                if (pulse == "None")
        //                {
        //                    c8_pulse.Text = "0";
        //                }
        //                else
        //                {
        //                    c8_pulse.Text = pulse;
        //                }

        //                if (delay == "None")
        //                {
        //                    c8_delay.Text = "0";
        //                }
        //                else
        //                {
        //                    c8_delay.Text = delay;
        //                }

        //                c8_edge.Text = edge;
        //                c8_mode.Text = mode;
        //                c8_strobe.Text = strobe;

        //                break;

        //            case 12:
        //                if (intensity == "None")
        //                {
        //                    c9_intensity.Text = "0";
        //                }
        //                else
        //                {
        //                    c9_intensity.Text = intensity;
        //                }

        //                if (pulse == "None")
        //                {
        //                    c9_pulse.Text = "0";
        //                }
        //                else
        //                {
        //                    c9_pulse.Text = pulse;
        //                }

        //                if (delay == "None")
        //                {
        //                    c9_delay.Text = "0";
        //                }
        //                else
        //                {
        //                    c9_delay.Text = delay;
        //                }

        //                c9_edge.Text = edge;
        //                c9_mode.Text = mode;
        //                c9_strobe.Text = strobe;

        //                break;

        //            case 14:
        //                if (intensity == "None")
        //                {
        //                    c10_intensity.Text = "0";
        //                }
        //                else
        //                {
        //                    c10_intensity.Text = intensity;
        //                }

        //                if (pulse == "None")
        //                {
        //                    c10_pulse.Text = "0";
        //                }
        //                else
        //                {
        //                    c10_pulse.Text = pulse;
        //                }

        //                if (delay == "None")
        //                {
        //                    c10_delay.Text = "0";
        //                }
        //                else
        //                {
        //                    c10_delay.Text = delay;
        //                }

        //                c10_edge.Text = edge;
        //                c10_mode.Text = mode;
        //                c10_strobe.Text = strobe;

        //                break;

        //            case 15:
        //                if (intensity == "None")
        //                {
        //                    c11_intensity.Text = "0";
        //                }
        //                else
        //                {
        //                    c11_intensity.Text = intensity;
        //                }

        //                if (pulse == "None")
        //                {
        //                    c11_pulse.Text = "0";
        //                }
        //                else
        //                {
        //                    c11_pulse.Text = pulse;
        //                }

        //                if (delay == "None")
        //                {
        //                    c11_delay.Text = "0";
        //                }
        //                else
        //                {
        //                    c11_delay.Text = delay;
        //                }

        //                c11_edge.Text = edge;
        //                c11_mode.Text = mode;
        //                c11_strobe.Text = strobe;

        //                break;

        //            case 16:
        //                if (intensity == "None")
        //                {
        //                    c12_intensity.Text = "0";
        //                }
        //                else
        //                {
        //                    c12_intensity.Text = intensity;
        //                }

        //                if (pulse == "None")
        //                {
        //                    c12_pulse.Text = "0";
        //                }
        //                else
        //                {
        //                    c12_pulse.Text = pulse;
        //                }

        //                if (delay == "None")
        //                {
        //                    c12_delay.Text = "0";
        //                }
        //                else
        //                {
        //                    c12_delay.Text = delay;
        //                }

        //                c12_edge.Text = edge;
        //                c12_mode.Text = mode;
        //                c12_strobe.Text = strobe;

        //                break;

        //            case 18:
        //                if (intensity == "None")
        //                {
        //                    c13_intensity.Text = "0";
        //                }
        //                else
        //                {
        //                    c13_intensity.Text = intensity;
        //                }

        //                if (pulse == "None")
        //                {
        //                    c13_pulse.Text = "0";
        //                }
        //                else
        //                {
        //                    c13_pulse.Text = pulse;
        //                }

        //                if (delay == "None")
        //                {
        //                    c13_delay.Text = "0";
        //                }
        //                else
        //                {
        //                    c13_delay.Text = delay;
        //                }

        //                c13_edge.Text = edge;
        //                c13_mode.Text = mode;
        //                c13_strobe.Text = strobe;

        //                break;

        //            case 19:
        //                if (intensity == "None")
        //                {
        //                    c14_intensity.Text = "0";
        //                }
        //                else
        //                {
        //                    c14_intensity.Text = intensity;
        //                }

        //                if (pulse == "None")
        //                {
        //                    c14_pulse.Text = "0";
        //                }
        //                else
        //                {
        //                    c14_pulse.Text = pulse;
        //                }

        //                if (delay == "None")
        //                {
        //                    c14_delay.Text = "0";
        //                }
        //                else
        //                {
        //                    c14_delay.Text = delay;
        //                }

        //                c14_edge.Text = edge;
        //                c14_mode.Text = mode;
        //                c14_strobe.Text = strobe;

        //                break;

        //            case 20:
        //                if (intensity == "None")
        //                {
        //                    c15_intensity.Text = "0";
        //                }
        //                else
        //                {
        //                    c15_intensity.Text = intensity;
        //                }

        //                if (pulse == "None")
        //                {
        //                    c15_pulse.Text = "0";
        //                }
        //                else
        //                {
        //                    c15_pulse.Text = pulse;
        //                }

        //                if (delay == "None")
        //                {
        //                    c15_delay.Text = "0";
        //                }
        //                else
        //                {
        //                    c15_delay.Text = delay;
        //                }

        //                c15_edge.Text = edge;
        //                c15_mode.Text = mode;
        //                c15_strobe.Text = strobe;

        //                break;

        //            case 22:
        //                if (intensity == "None")
        //                {
        //                    c16_intensity.Text = "0";
        //                }
        //                else
        //                {
        //                    c16_intensity.Text = intensity;
        //                }

        //                if (pulse == "None")
        //                {
        //                    c16_pulse.Text = "0";
        //                }
        //                else
        //                {
        //                    c16_pulse.Text = pulse;
        //                }

        //                if (delay == "None")
        //                {
        //                    c16_delay.Text = "0";
        //                }
        //                else
        //                {
        //                    c16_delay.Text = delay;
        //                }

        //                c16_edge.Text = edge;
        //                c16_mode.Text = mode;
        //                c16_strobe.Text = strobe;

        //                break;
        //        }


        //    }
        //}
    }
}
