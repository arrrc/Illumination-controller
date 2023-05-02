using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Controller_Design_2
{
    public partial class OpenFile : Form
    {
        public OpenFile()
        {
            InitializeComponent();
        }

        private bool _dragging = false;
        private Point _offset;
        private Point _start_point = new Point(0, 0);
        public MainApp mainForm;
        string loadedConfig = "";
        string path = @"..\..\savedConfigs";
        bool grouped;

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string intensity = "";
            string edge = "";
            string mode = "";
            string strobe = "";
            string pulse = "";
            string delay = "";


            if (guna2ComboBox1.Text == "")
            {
                MessageBox.Show("Pick a file");
                return;
            }

            string pathWithFilename = @"..\..\savedConfigs\" + guna2ComboBox1.Text;
            loadedConfig = File.ReadAllText(pathWithFilename);

            //there is a space at the beginning of the array
            string[] splitString = loadedConfig.Split('\n');


            for (int i = 0; i < splitString.Length; i++)
            {
                if (splitString[i][0] == 'G')
                {
                    string groupedOrUngroup = splitString[i].Substring(20, 7);
                    //Console.WriteLine(groupedOrUngroup);
                    if (groupedOrUngroup == "GROUPED")
                    {
                        grouped = true;
                    }
                    else if (groupedOrUngroup == "UNGROUP")
                    {

                        grouped = false;
                    }
                }

                else if (splitString[i][0] == '[')
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


                    //Console.WriteLine(intensity);
                    //Console.WriteLine(edge);
                    //Console.WriteLine(mode);
                    //Console.WriteLine(strobe);
                    //Console.WriteLine(pulse);
                    //Console.WriteLine(delay);

                }


                //Console.WriteLine(intensity);
                //Console.WriteLine(edge);
                //Console.WriteLine(mode);
                //Console.WriteLine(strobe);
                //Console.WriteLine(pulse);
                //Console.WriteLine(delay);

                switch (i)
                {
                    //group cases
                    case 1:
                        //Console.WriteLine("THIS PART IS RUNNING HADBGFSHIAHFSHAJGFUJG");

                        if (grouped == true)
                        {
                            mainForm.c010203_isGrouped = true;
                        }
                        else
                        {
                            //Console.WriteLine("THIS PART IS RUNNING LOL");
                            mainForm.c010203_isGrouped = false;
                        }
                        break;

                    case 5:
                        if (grouped == true)
                        {
                            mainForm.c040506_isGrouped = true;
                        }
                        else
                        {
                            mainForm.c040506_isGrouped = false;
                        }
                        break;

                    case 9:
                        if (grouped == true)
                        {
                            mainForm.c070809_isGrouped = true;
                        }
                        else
                        {
                            mainForm.c070809_isGrouped = false;
                        }
                        break;

                    case 13:
                        if (grouped == true)
                        {
                            mainForm.c101112_isGrouped = true;
                        }
                        else
                        {
                            mainForm.c101112_isGrouped = false;
                        }
                        break;

                    case 17:
                        if (grouped == true)
                        {
                            mainForm.c131415_isGrouped = true;
                        }
                        else
                        {
                            mainForm.c131415_isGrouped = false;
                        }
                        break;

                    //light config cases
                    case 2:
                        if (intensity == "0")
                        {
                            mainForm.c1_rgb_value = 0;
                        }
                        else
                        {
                            mainForm.c1_rgb_value = Convert.ToInt32(intensity);
                            //Console.WriteLine("THE VALUE IS MF " +  mainForm.c1_rgb_value);
                        }

                        if (pulse == "0")
                        {
                            mainForm.c1_pulse_value = 0;
                        }
                        else
                        {
                            mainForm.c1_pulse_value = Convert.ToInt32(pulse);
                        }

                        if (delay == "0")
                        {
                            mainForm.c1_delay_value = 0;
                        }
                        else
                        {
                            mainForm.c1_delay_value = Convert.ToInt32(delay);
                        }

                        mainForm.c1_edge_value = edge;
                        mainForm.c1_mode_value = mode;
                        mainForm.c1_strobe_value = strobe;

                        break;
                    case 3:
                        if (intensity == "0")
                        {
                            mainForm.c2_rgb_value = 0;
                        }
                        else
                        {
                            mainForm.c2_rgb_value = Convert.ToInt32(intensity);
                        }

                        if (pulse == "0")
                        {
                            mainForm.c2_pulse_value = 0;
                        }
                        else
                        {
                            mainForm.c2_pulse_value = Convert.ToInt32(pulse);
                        }

                        if (delay == "0")
                        {
                            mainForm.c2_delay_value = 0;
                        }
                        else
                        {
                            mainForm.c2_delay_value = Convert.ToInt32(delay);
                        }

                        mainForm.c2_edge_value = edge;
                        mainForm.c2_mode_value = mode;
                        mainForm.c2_strobe_value = strobe;

                        break;
                    case 4:
                        if (intensity == "0")
                        {
                            mainForm.c3_rgb_value = 0;
                        }
                        else
                        {
                            mainForm.c3_rgb_value = Convert.ToInt32(intensity);
                        }

                        if (pulse == "0")
                        {
                            mainForm.c3_pulse_value = 0;
                        }
                        else
                        {
                            mainForm.c3_pulse_value = Convert.ToInt32(pulse);
                        }

                        if (delay == "0")
                        {
                            mainForm.c3_delay_value = 0;
                        }
                        else
                        {
                            mainForm.c3_delay_value = Convert.ToInt32(delay);
                        }

                        mainForm.c3_edge_value = edge;
                        mainForm.c3_mode_value = mode;
                        mainForm.c3_strobe_value = strobe;

                        break;

                    case 6:
                        if (intensity == "0")
                        {
                            mainForm.c4_rgb_value = 0;
                        }
                        else
                        {
                            mainForm.c4_rgb_value = Convert.ToInt32(intensity);
                        }

                        if (pulse == "0")
                        {
                            mainForm.c4_pulse_value = 0;
                        }
                        else
                        {
                            mainForm.c4_pulse_value = Convert.ToInt32(pulse);
                        }

                        if (delay == "0")
                        {
                            mainForm.c4_delay_value = 0;
                        }
                        else
                        {
                            mainForm.c4_delay_value = Convert.ToInt32(delay);
                        }

                        mainForm.c4_edge_value = edge;
                        mainForm.c4_mode_value = mode;
                        mainForm.c4_strobe_value = strobe;

                        break;

                    case 7:
                        if (intensity == "0")
                        {
                            mainForm.c5_rgb_value = 0;
                        }
                        else
                        {
                            mainForm.c5_delay_value = Convert.ToInt32(intensity);
                        }

                        if (pulse == "0")
                        {
                            mainForm.c5_rgb_value = 0;
                        }
                        else
                        {
                            mainForm.c5_delay_value = Convert.ToInt32(pulse);
                        }

                        if (delay == "0")
                        {
                            mainForm.c5_delay_value = 0;
                        }
                        else
                        {
                            mainForm.c5_delay_value = Convert.ToInt32(delay);
                        }

                        mainForm.c5_edge_value = edge;
                        mainForm.c5_mode_value = mode;
                        mainForm.c5_strobe_value = strobe;

                        break;

                    case 8:
                        if (intensity == "0")
                        {
                            mainForm.c6_rgb_value = 0;
                        }
                        else
                        {
                            mainForm.c6_delay_value = Convert.ToInt32(intensity);
                        }

                        if (pulse == "0")
                        {
                            mainForm.c6_rgb_value = 0;
                        }
                        else
                        {
                            mainForm.c6_delay_value = Convert.ToInt32(pulse);
                        }

                        if (delay == "0")
                        {
                            mainForm.c6_delay_value = 0;
                        }
                        else
                        {
                            mainForm.c6_delay_value = Convert.ToInt32(delay);
                        }

                        mainForm.c6_edge_value = edge;
                        mainForm.c6_mode_value = mode;
                        mainForm.c6_strobe_value = strobe;

                        break;

                    case 10:
                        if (intensity == "0")
                        {
                            mainForm.c7_rgb_value = 0;
                        }
                        else
                        {
                            mainForm.c7_rgb_value = Convert.ToInt32(intensity);
                        }

                        if (pulse == "0")
                        {
                            mainForm.c7_pulse_value = 0;
                        }
                        else
                        {
                            mainForm.c7_pulse_value = Convert.ToInt32(pulse);
                        }

                        if (delay == "0")
                        {
                            mainForm.c7_delay_value = 0;
                        }
                        else
                        {
                            mainForm.c7_delay_value = Convert.ToInt32(delay);
                        }

                        mainForm.c7_edge_value = edge;
                        mainForm.c7_mode_value = mode;
                        mainForm.c7_strobe_value = strobe;

                        break;

                    case 11:
                        if (intensity == "0")
                        {
                            mainForm.c8_rgb_value = 0;
                        }
                        else
                        {
                            mainForm.c8_rgb_value = Convert.ToInt32(intensity);
                        }

                        if (pulse == "0")
                        {
                            mainForm.c8_pulse_value = 0;
                        }
                        else
                        {
                            mainForm.c8_pulse_value = Convert.ToInt32(pulse);
                        }

                        if (delay == "0")
                        {
                            mainForm.c8_delay_value = 0;
                        }
                        else
                        {
                            mainForm.c8_delay_value = Convert.ToInt32(delay);
                        }

                        mainForm.c8_edge_value = edge;
                        mainForm.c8_mode_value = mode;
                        mainForm.c8_strobe_value = strobe;

                        break;

                    case 12:
                        if (intensity == "0")
                        {
                            mainForm.c9_rgb_value = 0;
                        }
                        else
                        {
                            mainForm.c9_rgb_value = Convert.ToInt32(intensity);
                        }

                        if (pulse == "0")
                        {
                            mainForm.c9_pulse_value = 0;
                        }
                        else
                        {
                            mainForm.c9_pulse_value = Convert.ToInt32(pulse);
                        }

                        if (delay == "0")
                        {
                            mainForm.c9_delay_value = 0;
                        }
                        else
                        {
                            mainForm.c9_delay_value = Convert.ToInt32(delay);
                        }

                        mainForm.c9_edge_value = edge;
                        mainForm.c9_mode_value = mode;
                        mainForm.c9_strobe_value = strobe;

                        break;

                    case 14:
                        if (intensity == "0")
                        {
                            mainForm.c10_rgb_value = 0;
                        }
                        else
                        {
                            mainForm.c10_rgb_value = Convert.ToInt32(intensity);
                        }

                        if (pulse == "0")
                        {
                            mainForm.c10_pulse_value = 0;
                        }
                        else
                        {
                            mainForm.c10_pulse_value = Convert.ToInt32(pulse);
                        }

                        if (delay == "0")
                        {
                            mainForm.c10_delay_value = 0;
                        }
                        else
                        {
                            mainForm.c10_delay_value = Convert.ToInt32(delay);
                        }

                        mainForm.c10_edge_value = edge;
                        mainForm.c10_mode_value = mode;
                        mainForm.c10_strobe_value = strobe;

                        break;

                    case 15:
                        if (intensity == "0")
                        {
                            mainForm.c11_rgb_value = 0;
                        }
                        else
                        {
                            mainForm.c11_rgb_value = Convert.ToInt32(intensity);
                        }

                        if (pulse == "0")
                        {
                            mainForm.c11_pulse_value = 0;
                        }
                        else
                        {
                            mainForm.c11_pulse_value = Convert.ToInt32(pulse);
                        }

                        if (delay == "0")
                        {
                            mainForm.c11_delay_value = 0;
                        }
                        else
                        {
                            mainForm.c11_delay_value = Convert.ToInt32(delay);
                        }

                        mainForm.c11_edge_value = edge;
                        mainForm.c11_mode_value = mode;
                        mainForm.c11_strobe_value = strobe;

                        break;

                    case 16:
                        if (intensity == "0")
                        {
                            mainForm.c12_rgb_value = 0;
                        }
                        else
                        {
                            mainForm.c12_rgb_value = Convert.ToInt32(intensity);
                        }

                        if (pulse == "0")
                        {
                            mainForm.c12_pulse_value = 0;
                        }
                        else
                        {
                            mainForm.c12_pulse_value = Convert.ToInt32(pulse);
                        }

                        if (delay == "0")
                        {
                            mainForm.c12_delay_value = 0;
                        }
                        else
                        {
                            mainForm.c12_delay_value =  Convert.ToInt32(delay);
                        }

                        mainForm.c12_edge_value = edge;
                        mainForm.c12_mode_value = mode;
                        mainForm.c12_strobe_value = strobe;

                        break;

                    case 18:
                        if (intensity == "0")
                        {
                            mainForm.c13_rgb_value = 0;
                        }
                        else
                        {
                            mainForm.c13_rgb_value = Convert.ToInt32(intensity);
                        }

                        if (pulse == "0")
                        {
                            mainForm.c13_pulse_value = 0;
                        }
                        else
                        {
                            mainForm.c13_pulse_value = Convert.ToInt32(pulse);
                        }

                        if (delay == "0")
                        {
                            mainForm.c13_delay_value = 0;
                        }
                        else
                        {
                            mainForm.c13_delay_value = Convert.ToInt32(delay);
                        }

                        mainForm.c13_edge_value = edge;
                        mainForm.c13_mode_value = mode;
                        mainForm.c13_strobe_value = strobe;

                        break;

                    case 19:
                        if (intensity == "0")
                        {
                            mainForm.c14_rgb_value = 0;
                        }
                        else
                        {
                            mainForm.c14_rgb_value = Convert.ToInt32(intensity);
                        }

                        if (pulse == "0")
                        {
                            mainForm.c14_pulse_value = 0;
                        }
                        else
                        {
                            mainForm.c14_pulse_value = Convert.ToInt32(pulse);
                        }

                        if (delay == "0")
                        {
                            mainForm.c14_delay_value = 0;
                        }
                        else
                        {
                            mainForm.c14_delay_value = Convert.ToInt32(delay);
                        }

                        mainForm.c14_edge_value = edge;
                        mainForm.c14_mode_value = mode;
                        mainForm.c14_strobe_value = strobe;

                        break;

                    case 20:
                        if (intensity == "0")
                        {
                            mainForm.c15_rgb_value = 0;
                        }
                        else
                        {
                            mainForm.c15_rgb_value = Convert.ToInt32(intensity);
                        }

                        if (pulse == "0")
                        {
                            mainForm.c15_pulse_value = 0;
                        }
                        else
                        {
                            mainForm.c15_pulse_value = Convert.ToInt32(pulse);
                        }

                        if (delay == "0")
                        {
                            mainForm.c15_delay_value = 0;
                        }
                        else
                        {
                            mainForm.c15_delay_value = Convert.ToInt32(delay);
                        }

                        mainForm.c15_edge_value = edge;
                        mainForm.c15_mode_value = mode;
                        mainForm.c15_strobe_value = strobe;

                        break;

                    case 22:
                        if (intensity == "0")
                        {
                            mainForm.c16_rgb_value = 0;
                        }
                        else
                        {
                            mainForm.c16_rgb_value = Convert.ToInt32(intensity);
                        }

                        if (pulse == "0")
                        {
                            mainForm.c16_pulse_value = 0;
                        }
                        else
                        {
                            mainForm.c16_pulse_value = Convert.ToInt32(pulse);
                        }

                        if (delay == "0")
                        {
                            mainForm.c16_delay_value = 0;
                        }
                        else
                        {
                            mainForm.c16_delay_value = Convert.ToInt32(delay);
                        }

                        mainForm.c16_edge_value = edge;
                        mainForm.c16_mode_value = mode;
                        mainForm.c16_strobe_value = strobe;

                        
                        break;
                }


            }

            mainForm.setModes();
            mainForm.setStrobe();
            mainForm.setStrobeSettings();
            MessageBox.Show("Config has been loaded");
            this.Close();
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2ComboBox1_Click(object sender, EventArgs e)
        {
            guna2ComboBox1.Items.Clear();

            string[] fileList = Directory.GetFiles(path);

            foreach (string file in fileList)
            {
                //Console.WriteLine(file);
                string fileName = file.Substring(19);
                if (fileName == "Board 1.txt" || fileName == "Board 2.txt" || fileName == "Board 3.txt" || fileName == "Board 4.txt" || fileName == "Board 5.txt" || fileName == "Board 6.txt" || fileName == "Board 7.txt" || fileName == "Board 8.txt")
                {
                    continue;
                }
                else
                {
                    guna2ComboBox1.Items.Add(fileName);
                }
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
