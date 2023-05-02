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

namespace IlluminationController2
{
    public partial class openFile : Form
    {
        string loadedConfig = "";
        string path = @"..\..\savedConfigs";
        public Form1 mainForm;
        bool grouped;


        public openFile()
        {
            InitializeComponent();
        }

        private void openFile_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string intensity = "";
            string edge = "";
            string mode = "";
            string strobe = "";
            string pulse = "";
            string delay = "";


            if (fileSelect.Text == "")
            {
                MessageBox.Show("Pick a file");
                return;
            }

            string pathWithFilename = @"..\..\savedConfigs\" + fileSelect.Text;
            loadedConfig = File.ReadAllText(pathWithFilename);

            //there is a space at the beginning of the array
            string[] splitString = loadedConfig.Split('\n');

            Console.WriteLine(splitString.Length);
            for (int i = 0; i < splitString.Length; i++)
            {
                if (splitString[i][0] == 'G')
                {
                    string groupedOrUngroup = splitString[i].Substring(19, 7);
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
                            mainForm.g1_setting.SelectedIndex = 1;
                        }
                        else
                        {
                            Console.WriteLine("THIS PART IS RUNNING LOL");
                            mainForm.g1_setting.SelectedIndex = 0;
                        }
                        break;

                    case 5:
                        if (grouped == true)
                        {
                            mainForm.g2_setting.SelectedIndex = 1;
                        }
                        else
                        {
                            mainForm.g2_setting.SelectedIndex = 0;
                        }
                        break;

                    case 9:
                        if (grouped == true)
                        {
                            mainForm.g3_setting.SelectedIndex = 1;
                        }
                        else
                        {
                            mainForm.g3_setting.SelectedIndex = 0;
                        }
                        break;

                    case 13:
                        if (grouped == true)
                        {
                            mainForm.g4_setting.SelectedIndex = 1;
                        }
                        else
                        {
                            mainForm.g4_setting.SelectedIndex = 0;
                        }
                        break;

                    case 17:
                        if (grouped == true)
                        {
                            mainForm.g5_setting.SelectedIndex = 1;
                        }
                        else
                        {
                            mainForm.g5_setting.SelectedIndex = 0;
                        }
                        break;

                    //light config cases
                    case 2:
                        if(intensity == "None")
                        {
                            mainForm.c1_intensity.Text = "0";
                        }
                        else
                        {
                            mainForm.c1_intensity.Text = intensity;
                        }
                            
                        if (pulse == "None")
                        {
                            mainForm.c1_pulse.Text = "0";
                        }
                        else
                        {
                            mainForm.c1_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            mainForm.c1_delay.Text = "0";
                        }
                        else
                        {
                            mainForm.c1_delay.Text = delay;
                        }

                        mainForm.c1_edge.Text = edge;
                        mainForm.c1_mode.Text = mode;
                        mainForm.c1_strobe.Text = strobe;

                        break;
                    case 3:
                        if (intensity == "None")
                        {
                            mainForm.c2_intensity.Text = "0";
                        }
                        else
                        {
                            mainForm.c2_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            mainForm.c2_pulse.Text = "0";
                        }
                        else
                        {
                            mainForm.c2_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            mainForm.c2_delay.Text = "0";
                        }
                        else
                        {
                            mainForm.c2_delay.Text = delay;
                        }

                        mainForm.c2_edge.Text = edge;
                        mainForm.c2_mode.Text = mode;
                        mainForm.c2_strobe.Text = strobe;

                        break;
                    case 4:
                        if (intensity == "None")
                        {
                            mainForm.c3_intensity.Text = "0";
                        }
                        else
                        {
                            mainForm.c3_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            mainForm.c3_pulse.Text = "0";
                        }
                        else
                        {
                            mainForm.c3_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            mainForm.c3_delay.Text = "0";
                        }
                        else
                        {
                            mainForm.c3_delay.Text = delay;
                        }

                        mainForm.c3_edge.Text = edge;
                        mainForm.c3_mode.Text = mode;
                        mainForm.c3_strobe.Text = strobe;

                        break;

                    case 6:
                        if (intensity == "None")
                        {
                            mainForm.c4_intensity.Text = "0";
                        }
                        else
                        {
                            mainForm.c4_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            mainForm.c4_pulse.Text = "0";
                        }
                        else
                        {
                            mainForm.c4_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            mainForm.c4_delay.Text = "0";
                        }
                        else
                        {
                            mainForm.c4_delay.Text = delay;
                        }

                        mainForm.c4_edge.Text = edge;
                        mainForm.c4_mode.Text = mode;
                        mainForm.c4_strobe.Text = strobe;

                        break;

                    case 7:
                        if (intensity == "None")
                        {
                            mainForm.c5_intensity.Text = "0";
                        }
                        else
                        {
                            mainForm.c5_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            mainForm.c5_pulse.Text = "0";
                        }
                        else
                        {
                            mainForm.c5_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            mainForm.c5_delay.Text = "0";
                        }
                        else
                        {
                            mainForm.c5_delay.Text = delay;
                        }

                        mainForm.c5_edge.Text = edge;
                        mainForm.c5_mode.Text = mode;
                        mainForm.c5_strobe.Text = strobe;

                        break;

                    case 8:
                        if (intensity == "None")
                        {
                            mainForm.c6_intensity.Text = "0";
                        }
                        else
                        {
                            mainForm.c6_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            mainForm.c6_pulse.Text = "0";
                        }
                        else
                        {
                            mainForm.c6_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            mainForm.c6_delay.Text = "0";
                        }
                        else
                        {
                            mainForm.c6_delay.Text = delay;
                        }

                        mainForm.c6_edge.Text = edge;
                        mainForm.c6_mode.Text = mode;
                        mainForm.c6_strobe.Text = strobe;

                        break;

                    case 10:
                        if (intensity == "None")
                        {
                            mainForm.c7_intensity.Text = "0";
                        }
                        else
                        {
                            mainForm.c7_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            mainForm.c7_pulse.Text = "0";
                        }
                        else
                        {
                            mainForm.c7_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            mainForm.c7_delay.Text = "0";
                        }
                        else
                        {
                            mainForm.c7_delay.Text = delay;
                        }

                        mainForm.c7_edge.Text = edge;
                        mainForm.c7_mode.Text = mode;
                        mainForm.c7_strobe.Text = strobe;

                        break;

                    case 11:
                        if (intensity == "None")
                        {
                            mainForm.c8_intensity.Text = "0";
                        }
                        else
                        {
                            mainForm.c8_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            mainForm.c8_pulse.Text = "0";
                        }
                        else
                        {
                            mainForm.c8_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            mainForm.c8_delay.Text = "0";
                        }
                        else
                        {
                            mainForm.c8_delay.Text = delay;
                        }

                        mainForm.c8_edge.Text = edge;
                        mainForm.c8_mode.Text = mode;
                        mainForm.c8_strobe.Text = strobe;

                        break;

                    case 12:
                        if (intensity == "None")
                        {
                            mainForm.c9_intensity.Text = "0";
                        }
                        else
                        {
                            mainForm.c9_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            mainForm.c9_pulse.Text = "0";
                        }
                        else
                        {
                            mainForm.c9_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            mainForm.c9_delay.Text = "0";
                        }
                        else
                        {
                            mainForm.c9_delay.Text = delay;
                        }

                        mainForm.c9_edge.Text = edge;
                        mainForm.c9_mode.Text = mode;
                        mainForm.c9_strobe.Text = strobe;

                        break;

                    case 14:
                        if (intensity == "None")
                        {
                            mainForm.c10_intensity.Text = "0";
                        }
                        else
                        {
                            mainForm.c10_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            mainForm.c10_pulse.Text = "0";
                        }
                        else
                        {
                            mainForm.c10_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            mainForm.c10_delay.Text = "0";
                        }
                        else
                        {
                            mainForm.c10_delay.Text = delay;
                        }

                        mainForm.c10_edge.Text = edge;
                        mainForm.c10_mode.Text = mode;
                        mainForm.c10_strobe.Text = strobe;

                        break;

                    case 15:
                        if (intensity == "None")
                        {
                            mainForm.c11_intensity.Text = "0";
                        }
                        else
                        {
                            mainForm.c11_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            mainForm.c11_pulse.Text = "0";
                        }
                        else
                        {
                            mainForm.c11_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            mainForm.c11_delay.Text = "0";
                        }
                        else
                        {
                            mainForm.c11_delay.Text = delay;
                        }

                        mainForm.c11_edge.Text = edge;
                        mainForm.c11_mode.Text = mode;
                        mainForm.c11_strobe.Text = strobe;

                        break;

                    case 16:
                        if (intensity == "None")
                        {
                            mainForm.c12_intensity.Text = "0";
                        }
                        else
                        {
                            mainForm.c12_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            mainForm.c12_pulse.Text = "0";
                        }
                        else
                        {
                            mainForm.c12_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            mainForm.c12_delay.Text = "0";
                        }
                        else
                        {
                            mainForm.c12_delay.Text = delay;
                        }

                        mainForm.c12_edge.Text = edge;
                        mainForm.c12_mode.Text = mode;
                        mainForm.c12_strobe.Text = strobe;

                        break;

                    case 18:
                        if (intensity == "None")
                        {
                            mainForm.c13_intensity.Text = "0";
                        }
                        else
                        {
                            mainForm.c13_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            mainForm.c13_pulse.Text = "0";
                        }
                        else
                        {
                            mainForm.c13_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            mainForm.c13_delay.Text = "0";
                        }
                        else
                        {
                            mainForm.c13_delay.Text = delay;
                        }

                        mainForm.c13_edge.Text = edge;
                        mainForm.c13_mode.Text = mode;
                        mainForm.c13_strobe.Text = strobe;

                        break;

                    case 19:
                        if (intensity == "None")
                        {
                            mainForm.c14_intensity.Text = "0";
                        }
                        else
                        {
                            mainForm.c14_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            mainForm.c14_pulse.Text = "0";
                        }
                        else
                        {
                            mainForm.c14_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            mainForm.c14_delay.Text = "0";
                        }
                        else
                        {
                            mainForm.c14_delay.Text = delay;
                        }

                        mainForm.c14_edge.Text = edge;
                        mainForm.c14_mode.Text = mode;
                        mainForm.c14_strobe.Text = strobe;

                        break;

                    case 20:
                        if (intensity == "None")
                        {
                            mainForm.c15_intensity.Text = "0";
                        }
                        else
                        {
                            mainForm.c15_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            mainForm.c15_pulse.Text = "0";
                        }
                        else
                        {
                            mainForm.c15_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            mainForm.c15_delay.Text = "0";
                        }
                        else
                        {
                            mainForm.c15_delay.Text = delay;
                        }

                        mainForm.c15_edge.Text = edge;
                        mainForm.c15_mode.Text = mode;
                        mainForm.c15_strobe.Text = strobe;

                        break;

                    case 22:
                        if (intensity == "None")
                        {
                            mainForm.c16_intensity.Text = "0";
                        }
                        else
                        {
                            mainForm.c16_intensity.Text = intensity;
                        }

                        if (pulse == "None")
                        {
                            mainForm.c16_pulse.Text = "0";
                        }
                        else
                        {
                            mainForm.c16_pulse.Text = pulse;
                        }

                        if (delay == "None")
                        {
                            mainForm.c16_delay.Text = "0";
                        }
                        else
                        {
                            mainForm.c16_delay.Text = delay;
                        }

                        mainForm.c16_edge.Text = edge;
                        mainForm.c16_mode.Text = mode;
                        mainForm.c16_strobe.Text = strobe;

                        break;
                }

                
            }

            MessageBox.Show("Config has been loaded");
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void fileSelect_Click(object sender, EventArgs e)
        {
            fileSelect.Items.Clear();

            string[] fileList = Directory.GetFiles(path);

            foreach (string file in fileList)
            {
                Console.WriteLine(file);
                string fileName = file.Substring(19);
                if (fileName == "Board 1.txt" || fileName == "Board 2.txt" || fileName == "Board 3.txt" || fileName == "Board 4.txt" || fileName == "Board 5.txt" || fileName == "Board 6.txt" || fileName == "Board 7.txt" || fileName == "Board 8.txt")
                {
                    continue;
                }
                else
                {
                    fileSelect.Items.Add(fileName);
                }
            }

            foreach(string item in fileSelect.Items)
            {
                
            }
        }
    }
}
