using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Controller_Design_2
{
    public partial class SaveFile : Form
    {
        public SaveFile()
        {
            InitializeComponent();
        }

        private bool _dragging = false;
        private Point _offset;
        private Point _start_point = new Point(0, 0);

        bool fileExists = false;
        public MainApp mainForm;
        string config = "";

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text == "")
            {
                MessageBox.Show("Please enter a file name");
            }
            else if (guna2TextBox1.Text == "Board 1" || guna2TextBox1.Text == "Board 2" || guna2TextBox1.Text == "Board 3" || guna2TextBox1.Text == "Board 4" || guna2TextBox1.Text == "Board 5" || guna2TextBox1.Text == "Board 6" || guna2TextBox1.Text == "Board 7" || guna2TextBox1.Text == "Board 8")
            {
                MessageBox.Show("This file name is used to keep track of the latest configuration for each board, please use another file name");
            }
            else
            {
                mainForm.generateConfig();
                Console.WriteLine(mainForm.c1_rgb_value);
                Console.WriteLine(mainForm.sendToHardware);
                config = mainForm.sendToHardware;
                checkFileListAndSave();
            }
        }

        private void checkFileListAndSave()
        {
            string path = @"..\..\savedConfigs";
            string[] fileList = Directory.GetFiles(path);

            foreach (string file in fileList)
            {
                string pathWithFilename = path + "\\" + guna2TextBox1.Text + ".txt";



                if (pathWithFilename == file)
                {
                    fileExists = true;
                    break;
                }
                else
                {
                    continue;
                }

            }

            if (fileExists)
            {
                Console.WriteLine("test");
                DialogResult userInput = MessageBox.Show("A file with this name already exists, overwrite this file?", "", MessageBoxButtons.YesNo);
                if (userInput == DialogResult.Yes)
                {
                    path += "\\" + guna2TextBox1.Text + ".txt";
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
                path += "\\" + guna2TextBox1.Text + ".txt";
                Console.WriteLine(path);
                File.WriteAllText(path, config);
                MessageBox.Show("File has been saved");
            }


            // generate a list of configuration files for user to switch to
            // Files that no longer exist should not be seen as an option to the user
            // Values that are stored within the file are displayed on the LED & Channel Settings
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {

        }

        private void SaveFile_Load(object sender, EventArgs e)
        {
            Application.Exit();
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
    }
}
