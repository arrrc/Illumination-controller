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
        string path = @"C:\Users\WZS19\Documents\GitHub\Illumination-controller\IlluminationController2\savedConfigs";
        public Form1 mainForm;

        public openFile()
        {
            InitializeComponent();
        }

        private void openFile_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(fileSelect.Text == "")
            {
                MessageBox.Show("Pick a file");
                return;
            }

            string pathWithFilename = @"C:\Users\WZS19\Documents\GitHub\Illumination-controller\IlluminationController2\savedConfigs\" + fileSelect.Text;
            loadedConfig = File.ReadAllText(pathWithFilename);

            string[] splitString = loadedConfig.Split('.');


            for (int i = 0; i < splitString.Length; i++)
            {
                if (splitString[i][1] != '[')
                {
                    continue;
                }
                else
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

                    string intensity = splitString[i].Substring(intensityIndexPos, intensityLengthBetweenBothIndex);
                    string edge = splitString[i].Substring(edgeIndexPos, edgeLengthBetweenBothIndex);
                    string mode = splitString[i].Substring(modeIndexPos, modeLengthBetweenBothIndex);
                    string strobe = splitString[i].Substring(strobeIndexPos, strobeLengthBetweenBothIndex);
                    string pulse = splitString[i].Substring(pulseIndexPos, pulseLengthBetweenBothIndex);
                    string delay = splitString[i].Substring(delayIndexPos, delayLengthBetweenBothIndex);


                    Console.WriteLine(intensity);
                    Console.WriteLine(edge);
                    Console.WriteLine(mode);
                    Console.WriteLine(strobe);
                    Console.WriteLine(pulse);
                    Console.WriteLine(delay);

                    switch (i)
                    {
                        case 2:
                            mainForm.c1_intensity.Text = intensity;
                            mainForm.c1_edge.Text = edge;
                            mainForm.c1_mode.Text = mode;
                            mainForm.c1_strobe.Text = strobe;
                            mainForm.c1_pulse.Text = pulse;
                            mainForm.c1_delay.Text = delay;
                            break;
                        case 3:
                            mainForm.c2_intensity.Text = intensity;
                            mainForm.c2_edge.Text = edge;
                            mainForm.c2_mode.Text = mode;
                            mainForm.c2_strobe.Text = strobe;
                            mainForm.c2_pulse.Text = pulse;
                            mainForm.c2_delay.Text = delay;

                            break;
                        case 4:
                            mainForm.c3_intensity.Text = intensity;
                            mainForm.c3_edge.Text = edge;
                            mainForm.c3_mode.Text = mode;
                            mainForm.c3_strobe.Text = strobe;
                            mainForm.c3_pulse.Text = pulse;
                            mainForm.c3_delay.Text = delay;
                            break;

                        case 6:
                            mainForm.c4_intensity.Text = intensity;
                            mainForm.c4_edge.Text = edge;
                            mainForm.c4_mode.Text = mode;
                            mainForm.c4_strobe.Text = strobe;
                            mainForm.c4_pulse.Text = pulse;
                            mainForm.c4_delay.Text = delay;
                            break;

                        case 7:
                            mainForm.c5_intensity.Text = intensity;
                            mainForm.c5_edge.Text = edge;
                            mainForm.c5_mode.Text = mode;
                            mainForm.c5_strobe.Text = strobe;
                            mainForm.c5_pulse.Text = pulse;
                            mainForm.c5_delay.Text = delay;
                            break;

                        case 8:
                            mainForm.c6_intensity.Text = intensity;
                            mainForm.c6_edge.Text = edge;
                            mainForm.c6_mode.Text = mode;
                            mainForm.c6_strobe.Text = strobe;
                            mainForm.c6_pulse.Text = pulse;
                            mainForm.c6_delay.Text = delay;
                            break;

                        case 10:
                            mainForm.c7_intensity.Text = intensity;
                            mainForm.c7_edge.Text = edge;
                            mainForm.c7_mode.Text = mode;
                            mainForm.c7_strobe.Text = strobe;
                            mainForm.c7_pulse.Text = pulse;
                            mainForm.c7_delay.Text = delay;
                            break;

                        case 11:
                            mainForm.c8_intensity.Text = intensity;
                            mainForm.c8_edge.Text = edge;
                            mainForm.c8_mode.Text = mode;
                            mainForm.c8_strobe.Text = strobe;
                            mainForm.c8_pulse.Text = pulse;
                            mainForm.c8_delay.Text = delay;
                            break;

                        case 12:
                            mainForm.c9_intensity.Text = intensity;
                            mainForm.c9_edge.Text = edge;
                            mainForm.c9_mode.Text = mode;
                            mainForm.c9_strobe.Text = strobe;
                            mainForm.c9_pulse.Text = pulse;
                            mainForm.c9_delay.Text = delay;
                            break;

                        case 14:
                            mainForm.c10_intensity.Text = intensity;
                            mainForm.c10_edge.Text = edge;
                            mainForm.c10_mode.Text = mode;
                            mainForm.c10_strobe.Text = strobe;
                            mainForm.c10_pulse.Text = pulse;
                            mainForm.c10_delay.Text = delay;
                            break;

                        case 15:
                            mainForm.c11_intensity.Text = intensity;
                            mainForm.c11_edge.Text = edge;
                            mainForm.c11_mode.Text = mode;
                            mainForm.c11_strobe.Text = strobe;
                            mainForm.c11_pulse.Text = pulse;
                            mainForm.c11_delay.Text = delay;
                            break;

                        case 16:
                            mainForm.c12_intensity.Text = intensity;
                            mainForm.c12_edge.Text = edge;
                            mainForm.c12_mode.Text = mode;
                            mainForm.c12_strobe.Text = strobe;
                            mainForm.c12_pulse.Text = pulse;
                            mainForm.c12_delay.Text = delay;
                            break;

                        case 18:
                            mainForm.c13_intensity.Text = intensity;
                            mainForm.c13_edge.Text = edge;
                            mainForm.c13_mode.Text = mode;
                            mainForm.c13_strobe.Text = strobe;
                            mainForm.c13_pulse.Text = pulse;
                            mainForm.c13_delay.Text = delay;
                            break;

                        case 19:
                            mainForm.c14_intensity.Text = intensity;
                            mainForm.c14_edge.Text = edge;
                            mainForm.c14_mode.Text = mode;
                            mainForm.c14_strobe.Text = strobe;
                            mainForm.c14_pulse.Text = pulse;
                            mainForm.c14_delay.Text = delay;
                            break;

                        case 20:
                            mainForm.c15_intensity.Text = intensity;
                            mainForm.c15_edge.Text = edge;
                            mainForm.c15_mode.Text = mode;
                            mainForm.c15_strobe.Text = strobe;
                            mainForm.c15_pulse.Text = pulse;
                            mainForm.c15_delay.Text = delay;
                            break;

                        case 22:
                            mainForm.c16_intensity.Text = intensity;
                            mainForm.c16_edge.Text = edge;
                            mainForm.c16_mode.Text = mode;
                            mainForm.c16_strobe.Text = strobe;
                            mainForm.c16_pulse.Text = pulse;
                            mainForm.c16_delay.Text = delay;
                            break;
                    }

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
                string fileName = file.Substring(93);
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
