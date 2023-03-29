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
    public partial class saveFile : Form
    {
        bool fileExists = false;
        public Form1 mainForm;
        string config = "";

        public saveFile()
        {
            InitializeComponent();
        }

        private void submitFile_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please enter a file name");
            }
            else if (textBox1.Text == "Board 1" || textBox1.Text == "Board 2" || textBox1.Text == "Board 3" || textBox1.Text == "Board 4" || textBox1.Text == "Board 5" || textBox1.Text == "Board 6" || textBox1.Text == "Board 7" || textBox1.Text == "Board 8")
            {
                MessageBox.Show("This file name is used to keep track of the latest configuration for each board, please use another file name");
            }
            else
            {
                mainForm.generateConfig();
                Console.WriteLine(mainForm.c1_intensity.Text);
                Console.WriteLine(mainForm.sendToHardware);
                config = mainForm.sendToHardware;
                checkFileListAndSave();
            }
        }

        private void checkFileListAndSave()
        {
            string path = @"C:\Users\WZS19\Documents\GitHub\Illumination-controller\IlluminationController2\savedConfigs";
            string[] fileList = Directory.GetFiles(path);

            foreach (string file in fileList)
            {
                string pathWithFilename = path + "\\" + textBox1.Text + ".txt";

                

                if (pathWithFilename == file)
                {
                    fileExists = true;
                    break;
                }
                else
                {
                    continue;
                }
                
                Console.WriteLine(file);
            }

            if (fileExists)
            {
                Console.WriteLine("test");
                DialogResult userInput = MessageBox.Show("A file with this name already exists, overwrite this file?", "", MessageBoxButtons.YesNo);
                if (userInput == DialogResult.Yes)
                {
                    path += "\\" + textBox1.Text + ".txt";
                    Console.WriteLine(path);
                    File.WriteAllText(path, config);
                    MessageBox.Show("File has been overwritten");
                }
                else
                {
                    
                }
            }
            else
            {
                path += "\\" + textBox1.Text + ".txt";
                Console.WriteLine(path);
                File.WriteAllText(path, config);
                MessageBox.Show("File has been saved");
            }


            // generate a list of configuration files for user to switch to
            // Files that no longer exist should not be seen as an option to the user
            // Values that are stored within the file are displayed on the LED & Channel Settings
        }
    }
}
