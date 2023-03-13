﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace IlluminationControllerUI
{
    public partial class Form1 : Form
    {
        // Trackbar Values
        int A_currentLevel = 10;
        int B_currentLevel = 0;
        int C_currentLevel = 0;
        int D_currentLevel = 0;
        int E_currentLevel = 0;
        int F_currentLevel = 0;
        int G_currentLevel = 0;
        int H_currentLevel = 0;
        int I_currentLevel = 0;


        // Input Value Global Variables
        int A_on_interval;
        int A_off_interval;
        int A_red_value;
        int A_green_value;
        int A_blue_value;
        int A_intensity_value;

        int B_on_interval;
        int B_off_interval;
        int B_red_value;
        int B_green_value;
        int B_blue_value;
        int B_intensity_value;

        int C_on_interval;
        int C_off_interval;
        int C_red_value;
        int C_green_value;
        int C_blue_value;
        int C_intensity_value;

        int D_on_interval;
        int D_off_interval;
        int D_red_value;
        int D_green_value;
        int D_blue_value;
        int D_intensity_value;

        int E_on_interval;
        int E_off_interval;
        int E_red_value;
        int E_green_value;
        int E_blue_value;
        int E_intensity_value;

        int F_on_interval;
        int F_off_interval;
        int F_red_value;
        int F_green_value;
        int F_blue_value;
        int F_intensity_value;

        int G_on_interval;
        int G_off_interval;
        int G_red_value;
        int G_green_value;
        int G_blue_value;
        int G_intensity_value;

        int H_on_interval;
        int H_off_interval;
        int H_red_value;
        int H_green_value;
        int H_blue_value;
        int H_intensity_value;

        int I_on_interval;
        int I_off_interval;
        int I_red_value;
        int I_green_value;
        int I_blue_value;
        int I_intensity_value;

        bool startStop = false;
        string mainPath = @"C:\Users\WZS20\Documents\GitHub\Illumination-controller\.vs\IlluminationControllerUI\IlluminationControllerUI\configFiles\";



        public Form1()
        {
            InitializeComponent();
        }

        private void writeDatafile(string path, string filename, string text)
        {

            if (!File.Exists(path + filename))
            {
                using (StreamWriter sw = File.CreateText(path + filename))
                {
                    sw.WriteLine(text);
                    sw.Close();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                A_green_value = Convert.ToInt32(A_green.Text);
                errorMsg.Text = "";
                A_lightButton.Enabled = true;
            }
            catch
            {
                errorMsg.Text = "Invalid input for some values";
                errorMsg.Visible = true;
                A_lightButton.Enabled = false;

                return;
            }

            if (check_all_rgb(A_red_value, A_green_value, A_blue_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(A_red_value, A_green_value, A_blue_value);
                A_status.BackColor = newColour;
            }
            else
            {
                errorMsg.Text = "Invalid input for some values";
                errorMsg.Visible = true;
                A_lightButton.Enabled = false;

                return;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void label49_Click(object sender, EventArgs e)
        {

        }

        private void label73_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label83_Click(object sender, EventArgs e)
        {

        }

        private void label84_Click(object sender, EventArgs e)
        {

        }

        private void label75_Click(object sender, EventArgs e)
        {

        }

        private void button38_Click(object sender, EventArgs e)
        {
            writeDatafile(mainPath, "configTest.txt", "Hi");
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void A_red_TextChanged(object sender, EventArgs e)
        {
            try
            {
                A_red_value = Convert.ToInt32(A_red.Text);
                errorMsg.Text = "";
                A_lightButton.Enabled = true;
            }
            catch
            {
                errorMsg.Text = "Invalid input for some values";
                errorMsg.Visible = true;
                A_lightButton.Enabled = false;

                return;
            }
            if (check_all_rgb(A_red_value, A_green_value, A_blue_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(A_red_value, A_green_value, A_blue_value);
                A_status.BackColor = newColour;
            }
            else
            {
                errorMsg.Text = "Invalid input for some values";
                errorMsg.Visible = true;
                A_lightButton.Enabled = false;
            }
        }

        private void A_on_TextChanged(object sender, EventArgs e)
        {
            // Grab values and convert to int or double
            try
            {
                A_on_interval = Convert.ToInt32(A_on.Text);
                errorMsg.Text = "";
                A_lightButton.Enabled = true;
            }
            catch
            {
                errorMsg.Text = "Invalid input for some values";
                errorMsg.Visible = true;
                A_lightButton.Enabled = false;

                return;

            }
        }

        //private void light_loop(int red, int green, int blue, int on_interval, int off_interval)
        //{
        //    while (true)
        //    {
        //        A_status.BackColor = Color.FromArgb(red, green, blue);
        //        Thread.Sleep(on_interval/1000);
        //        A_status.BackColor = Color.Transparent;
        //        Thread.Sleep(off_interval/1000);
        //    }        
        //}

        private void light_loop()
        {
            try
            {
                while (true)
                {
                    if(startStop == false)
                    {
                        break;
                    }
                    else
                    {
                        int red = Convert.ToInt32(A_red.Text);
                        int green = Convert.ToInt32(A_green.Text);
                        int blue = Convert.ToInt32(A_blue.Text);
                        int on_interval = Convert.ToInt32(A_on.Text);
                        int off_interval = Convert.ToInt32(A_off.Text);

                        A_status.BackColor = Color.FromArgb(red, green, blue);

                        Thread.Sleep(on_interval);
                        A_status.BackColor = Color.Transparent;
                        Thread.Sleep(off_interval);
                    }
                    
                }
            }
            catch
            {
                return;
            }
        }

        // Check colour values and ascertain whether they are valid
        private bool rgb_valid(int value)
        {

            if (0 <= value && value <= 255)
            {
                //Console.WriteLine(value);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool check_all_rgb(int red, int green, int blue)
        {

            if (0 <= red && red <= 255 && 0 <= green && green <= 255 && 0 <= blue && blue <= 255)
            {
                //Console.WriteLine(value);
                return true;
            }
            else
            {
                return false;
            }
        }

        // Check interval values are valid and ndo not exceed max time
        private bool intervals_valid(int value)
        {
            if ((value > 0))
            {
                return true;
            }
            return false;
        }

        private void A_off_TextChanged(object sender, EventArgs e)
        {
            try
            {
                A_off_interval = Convert.ToInt32(A_off.Text);
                errorMsg.Text = "";
                A_lightButton.Enabled = true;
            }
            catch
            {
                errorMsg.Text = "Invalid input for some values";
                errorMsg.Visible = true;
                A_lightButton.Enabled = false;

                return;

            }

        }

        private void A_blue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                A_blue_value = Convert.ToInt32(A_blue.Text);
                errorMsg.Text = "";
                A_lightButton.Enabled = true;
            }
            catch
            {
                errorMsg.Text = "Invalid input for some values";
                errorMsg.Visible = true;
                A_lightButton.Enabled = false;

                return;
            }

            if (check_all_rgb(A_red_value, A_green_value, A_blue_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(A_red_value, A_green_value, A_blue_value);
                A_status.BackColor = newColour;
            }
            else
            {
                errorMsg.Text = "Invalid input for some values";
                errorMsg.Visible = true;
                A_lightButton.Enabled = false;

                return;
            }

        }

        private void A_intensity_Scroll(object sender, EventArgs e)
        {
            A_lightButton.Enabled = true;
            A_intensity_value = Convert.ToInt32(A_trackbar.Value);
            if (A_intensity_value != 10)
            {
                intensityA.Text = "L" + A_intensity_value;
                A_red.ReadOnly = true;
                A_blue.ReadOnly = true;
                A_green.ReadOnly = true;

                double current_red_value = 0;
                double current_green_value = 0;
                double current_blue_value = 0;
                double updated_red;
                double updated_green;
                double updated_blue;

                int A_prevValue;

                try
                {
                    current_red_value = Convert.ToDouble(A_red.Text);
                    //Console.WriteLine(current_red_value);
                    current_green_value = Convert.ToDouble(A_green.Text);
                    //Console.WriteLine(current_green_value);
                    current_blue_value = Convert.ToDouble(A_blue.Text);
                    //Console.WriteLine(current_blue_value);
                }
                catch
                {
                    Console.WriteLine("error");
                    return;
                }


                // Calculate RGB Intensity
                if (A_intensity_value < A_currentLevel)
                {
                    A_prevValue = A_currentLevel;
                    Console.WriteLine(A_currentLevel);
                    updated_red = current_red_value - ((A_currentLevel - A_intensity_value) * 25);
                    updated_green = current_green_value - ((A_currentLevel - A_intensity_value) * 25);
                    updated_blue = current_blue_value - ((A_currentLevel - A_intensity_value) * 25);

                    bool red_valid_rgb = rgb_valid(Convert.ToInt32(updated_red));
                    //Console.WriteLine(rgb_red);
                    bool green_valid_rgb = rgb_valid(Convert.ToInt32(updated_green));
                    bool blue_valid_rgb = rgb_valid(Convert.ToInt32(updated_blue));

                    if (red_valid_rgb && green_valid_rgb && blue_valid_rgb)
                    {
                        A_currentLevel = A_intensity_value;
                    }
                    else
                    {
                        A_trackbar.Value = A_prevValue;
                        intensityA.Text = "L" + A_prevValue;
                        MessageBox.Show("at least 1 colour value is too low/high if the brightness is further increased/decreased");
                        return;
                    }

                }
                else if (A_intensity_value > A_currentLevel)
                {
                    A_prevValue = A_currentLevel;
                    Console.WriteLine(A_intensity_value);
                    Console.WriteLine(A_currentLevel);
                    updated_red = current_red_value + ((A_intensity_value - A_currentLevel) * 25);
                    updated_green = current_green_value + ((A_intensity_value - A_currentLevel) * 25);
                    updated_blue = current_blue_value + ((A_intensity_value - A_currentLevel) * 25);
                    A_currentLevel = A_intensity_value;

                    bool red_valid_rgb = rgb_valid(Convert.ToInt32(updated_red));
                    //Console.WriteLine(rgb_red);
                    bool green_valid_rgb = rgb_valid(Convert.ToInt32(updated_green));
                    bool blue_valid_rgb = rgb_valid(Convert.ToInt32(updated_blue));

                    if (red_valid_rgb && green_valid_rgb && blue_valid_rgb)
                    {
                        A_currentLevel = A_intensity_value;
                    }
                    else
                    {
                        A_trackbar.Value = A_prevValue;
                        intensityA.Text = "L" + A_prevValue;
                        MessageBox.Show("at least 1 colour value is too low/high if  the brightness is further increased/decreased");
                        return;
                    }
                }
                else
                {
                    A_prevValue = A_currentLevel;
                    updated_red = current_red_value;
                    updated_green = current_green_value;
                    updated_blue = current_blue_value;

                    bool red_valid_rgb = rgb_valid(Convert.ToInt32(updated_red));
                    //Console.WriteLine(rgb_red);
                    bool green_valid_rgb = rgb_valid(Convert.ToInt32(updated_green));
                    bool blue_valid_rgb = rgb_valid(Convert.ToInt32(updated_blue));

                    if (red_valid_rgb && green_valid_rgb && blue_valid_rgb)
                    {
                        A_currentLevel = A_intensity_value;
                    }
                    else
                    {
                        A_trackbar.Value = A_prevValue;
                        intensityA.Text = "L" + A_prevValue;
                        MessageBox.Show("at least 1 colour value is too low/high if  the brightness is further increased/decreased");
                        return;
                    }
                }


                updated_red = Math.Round(updated_red, 0);
                updated_green = Math.Round(updated_green, 0);
                updated_blue = Math.Round(updated_blue, 0);

                int rgb_red = Convert.ToInt32(updated_red);
                int rgb_green = Convert.ToInt32(updated_green);
                int rgb_blue = Convert.ToInt32(updated_blue);

                A_red.Text = updated_red.ToString();
                A_green.Text = updated_green.ToString();
                A_blue.Text = updated_blue.ToString();

                A_status.BackColor = Color.FromArgb(rgb_red, rgb_green, rgb_blue);
            }
            else
            {
                A_red.ReadOnly = false;
                A_blue.ReadOnly = false;
                A_green.ReadOnly = false;

                intensityA.Text = "L" + A_intensity_value;

                double current_red_value = 0;
                double current_green_value = 0;
                double current_blue_value = 0;
                double updated_red;
                double updated_green;
                double updated_blue;

                int A_prevValue;

                try
                {
                    current_red_value = Convert.ToDouble(A_red.Text);
                    //Console.WriteLine(current_red_value);
                    current_green_value = Convert.ToDouble(A_green.Text);
                    //Console.WriteLine(current_green_value);
                    current_blue_value = Convert.ToDouble(A_blue.Text);
                    //Console.WriteLine(current_blue_value);
                }
                catch
                {
                    Console.WriteLine("error");
                    return;
                }


                // Calculate RGB Intensity
                if (A_intensity_value < A_currentLevel)
                {
                    A_prevValue = A_currentLevel;
                    Console.WriteLine(A_currentLevel);
                    updated_red = current_red_value - ((A_currentLevel - A_intensity_value) * 25);
                    updated_green = current_green_value - ((A_currentLevel - A_intensity_value) * 25);
                    updated_blue = current_blue_value - ((A_currentLevel - A_intensity_value) * 25);

                    bool red_valid_rgb = rgb_valid(Convert.ToInt32(updated_red));
                    //Console.WriteLine(rgb_red);
                    bool green_valid_rgb = rgb_valid(Convert.ToInt32(updated_green));
                    bool blue_valid_rgb = rgb_valid(Convert.ToInt32(updated_blue));

                    if (red_valid_rgb && green_valid_rgb && blue_valid_rgb)
                    {
                        A_currentLevel = A_intensity_value;
                    }
                    else
                    {
                        A_trackbar.Value = A_prevValue;
                        intensityA.Text = "L" + A_prevValue;
                        MessageBox.Show("at least 1 colour value is too low/high if the brightness is further increased/decreased");
                        return;
                    }

                }
                else if (A_intensity_value > A_currentLevel)
                {
                    A_prevValue = A_currentLevel;
                    Console.WriteLine(A_intensity_value);
                    Console.WriteLine(A_currentLevel);
                    updated_red = current_red_value + ((A_intensity_value - A_currentLevel) * 25);
                    updated_green = current_green_value + ((A_intensity_value - A_currentLevel) * 25);
                    updated_blue = current_blue_value + ((A_intensity_value - A_currentLevel) * 25);
                    A_currentLevel = A_intensity_value;

                    bool red_valid_rgb = rgb_valid(Convert.ToInt32(updated_red));
                    //Console.WriteLine(rgb_red);
                    bool green_valid_rgb = rgb_valid(Convert.ToInt32(updated_green));
                    bool blue_valid_rgb = rgb_valid(Convert.ToInt32(updated_blue));

                    if (red_valid_rgb && green_valid_rgb && blue_valid_rgb)
                    {
                        A_currentLevel = A_intensity_value;
                    }
                    else
                    {
                        A_trackbar.Value = A_prevValue;
                        intensityA.Text = "L" + A_prevValue;
                        MessageBox.Show("at least 1 colour value is too low/high if  the brightness is further increased/decreased");
                        return;
                    }
                }
                else
                {
                    A_prevValue = A_currentLevel;
                    updated_red = current_red_value;
                    updated_green = current_green_value;
                    updated_blue = current_blue_value;

                    bool red_valid_rgb = rgb_valid(Convert.ToInt32(updated_red));
                    //Console.WriteLine(rgb_red);
                    bool green_valid_rgb = rgb_valid(Convert.ToInt32(updated_green));
                    bool blue_valid_rgb = rgb_valid(Convert.ToInt32(updated_blue));

                    if (red_valid_rgb && green_valid_rgb && blue_valid_rgb)
                    {
                        A_currentLevel = A_intensity_value;
                    }
                    else
                    {
                        A_trackbar.Value = A_prevValue;
                        intensityA.Text = "L" + A_prevValue;
                        MessageBox.Show("at least 1 colour value is too low/high if  the brightness is further increased/decreased");
                        return;
                    }
                }


                updated_red = Math.Round(updated_red, 0);
                updated_green = Math.Round(updated_green, 0);
                updated_blue = Math.Round(updated_blue, 0);

                int rgb_red = Convert.ToInt32(updated_red);
                int rgb_green = Convert.ToInt32(updated_green);
                int rgb_blue = Convert.ToInt32(updated_blue);

                A_red.Text = updated_red.ToString();
                A_green.Text = updated_green.ToString();
                A_blue.Text = updated_blue.ToString();

                A_status.BackColor = Color.FromArgb(rgb_red, rgb_green, rgb_blue);
            }

        }

        private int rgb_value_checker(int value)
        {
            if (value > 255)
            {
                return value = 255;
            }
            else if (value < 0)
            {
                return value = 0;
            }
            else
            {
                return value;
            }
        }

        private void B_on_TextChanged(object sender, EventArgs e)
        {
            try
            {
                B_on_interval = Convert.ToInt32(B_on.Text);
            }
            catch
            {
                Console.WriteLine("invalid interval vals");
                return;

            }

            // Ascertain whether values are valid for their purpose
            if (intervals_valid(B_on_interval))
            {
                Console.WriteLine("Interval Values are valid");
            }
        }

        private void B_off_TextChanged(object sender, EventArgs e)
        {
            try
            {
                B_on_interval = Convert.ToInt32(B_off.Text);
            }
            catch
            {
                Console.WriteLine("invalid interval vals");
                return;

            }

            // Ascertain whether values are valid for their purpose
            if (intervals_valid(B_off_interval))
            {
                Console.WriteLine("Interval Values are valid");
            }


        }

        private void B_red_TextChanged(object sender, EventArgs e)
        {
            try
            {
                B_red_value = Convert.ToInt32(B_red.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(B_red_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(B_red_value, B_green_value, B_blue_value);
                B_status.BackColor = newColour;
            }
        }

        private void B_green_TextChanged(object sender, EventArgs e)
        {
            try
            {
                B_green_value = Convert.ToInt32(B_green.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(B_green_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(B_red_value, B_green_value, B_blue_value);
                B_status.BackColor = newColour;
            }
        }

        private void B_blue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                B_blue_value = Convert.ToInt32(B_blue.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(B_blue_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(B_red_value, B_green_value, B_blue_value);
                B_status.BackColor = newColour;
            }
        }


        private void B_trackbar_Scroll(object sender, EventArgs e)
        {
            B_intensity_value = Convert.ToInt32(B_trackbar.Value);
            int calc_intensity = B_intensity_value * 10;

            intensityB.Text = Convert.ToString(calc_intensity) + "%";
        }

        private void C_on_TextChanged(object sender, EventArgs e)
        {
            try
            {
                C_on_interval = Convert.ToInt32(C_on.Text);
            }
            catch
            {
                Console.WriteLine("invalid interval vals");
                return;

            }

            // Ascertain whether values are valid for their purpose
            if (intervals_valid(C_on_interval))
            {
                Console.WriteLine("Interval Values are valid");
            }
        }

        private void C_off_TextChanged(object sender, EventArgs e)
        {
            try
            {
                C_off_interval = Convert.ToInt32(C_off.Text);
            }
            catch
            {
                Console.WriteLine("invalid interval vals");
                return;

            }

            // Ascertain whether values are valid for their purpose
            if (intervals_valid(C_on_interval))
            {
                Console.WriteLine("Interval Values are valid");
            }
        }

        private void C_red_TextChanged(object sender, EventArgs e)
        {
            try
            {
                C_red_value = Convert.ToInt32(C_red.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(C_red_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(C_red_value, C_green_value, C_blue_value);
                C_status.BackColor = newColour;
            }
        }

        private void C_green_TextChanged(object sender, EventArgs e)
        {
            try
            {
                C_green_value = Convert.ToInt32(C_green.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(C_green_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(C_red_value, C_green_value, C_blue_value);
                C_status.BackColor = newColour;
            }
        }

        private void C_blue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                C_blue_value = Convert.ToInt32(C_blue.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(C_blue_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(C_red_value, C_green_value, C_blue_value);
                C_status.BackColor = newColour;
            }
        }

        private void C_trackbar_Scroll(object sender, EventArgs e)
        {
            C_intensity_value = Convert.ToInt32(C_trackbar.Value);
            int calc_intensity = C_intensity_value * 10;

            intensityC.Text = Convert.ToString(calc_intensity) + "%";
        }

        private void D_on_TextChanged(object sender, EventArgs e)
        {
            try
            {
                D_on_interval = Convert.ToInt32(D_on.Text);
            }
            catch
            {
                Console.WriteLine("invalid interval vals");
                return;

            }

            // Ascertain whether values are valid for their purpose
            if (intervals_valid(D_on_interval))
            {
                Console.WriteLine("Interval Values are valid");
            }
        }

        private void D_off_TextChanged(object sender, EventArgs e)
        {
            try
            {
                D_off_interval = Convert.ToInt32(D_off.Text);
            }
            catch
            {
                Console.WriteLine("invalid interval vals");
                return;

            }

            // Ascertain whether values are valid for their purpose
            if (intervals_valid(D_off_interval))
            {
                Console.WriteLine("Interval Values are valid");
            }
        }

        private void D_red_TextChanged(object sender, EventArgs e)
        {
            try
            {
                D_red_value = Convert.ToInt32(D_red.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(D_red_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(D_red_value, D_green_value, D_blue_value);
                D_status.BackColor = newColour;
            }
        }

        private void D_green_TextChanged(object sender, EventArgs e)
        {
            try
            {
                D_green_value = Convert.ToInt32(D_green.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(D_green_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(D_red_value, D_green_value, D_blue_value);
                D_status.BackColor = newColour;
            }
        }

        private void D_blue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                D_blue_value = Convert.ToInt32(D_blue.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(D_blue_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(D_red_value, D_green_value, D_blue_value);
                D_status.BackColor = newColour;
            }
        }

        private void D_trackbar_Scroll(object sender, EventArgs e)
        {
            D_intensity_value = Convert.ToInt32(D_trackbar.Value);
            int calc_intensity = D_intensity_value * 10;

            intensityD.Text = Convert.ToString(calc_intensity) + "%";
        }

        private void E_on_TextChanged(object sender, EventArgs e)
        {
            try
            {
                E_on_interval = Convert.ToInt32(E_on.Text);
            }
            catch
            {
                Console.WriteLine("invalid interval vals");
                return;

            }

            // Ascertain whether values are valid for their purpose
            if (intervals_valid(E_on_interval))
            {
                Console.WriteLine("Interval Values are valid");
            }
        }

        private void E_off_TextChanged(object sender, EventArgs e)
        {
            try
            {
                E_off_interval = Convert.ToInt32(E_off.Text);
            }
            catch
            {
                Console.WriteLine("invalid interval vals");
                return;

            }

            // Ascertain whether values are valid for their purpose
            if (intervals_valid(E_off_interval))
            {
                Console.WriteLine("Interval Values are valid");
            }
        }

        private void E_red_TextChanged(object sender, EventArgs e)
        {
            try
            {
                E_red_value = Convert.ToInt32(E_red.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(E_red_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(E_red_value, E_green_value, E_blue_value);
                E_status.BackColor = newColour;
            }
        }

        private void E_green_TextChanged(object sender, EventArgs e)
        {
            try
            {
                E_green_value = Convert.ToInt32(E_green.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(E_green_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(E_red_value, E_green_value, E_blue_value);
                E_status.BackColor = newColour;
            }
        }

        private void E_blue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                E_blue_value = Convert.ToInt32(E_blue.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(E_blue_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(E_red_value, E_green_value, E_blue_value);
                E_status.BackColor = newColour;
            }
        }

        private void E_trackbar_Scroll(object sender, EventArgs e)
        {
            E_intensity_value = Convert.ToInt32(E_trackbar.Value);
            int calc_intensity = E_intensity_value * 10;

            intensityE.Text = Convert.ToString(calc_intensity) + "%";
        }

        private void F_on_TextChanged(object sender, EventArgs e)
        {
            try
            {
                F_on_interval = Convert.ToInt32(F_on.Text);
            }
            catch
            {
                Console.WriteLine("invalid interval vals");
                return;

            }

            // Ascertain whether values are valid for their purpose
            if (intervals_valid(F_on_interval))
            {
                Console.WriteLine("Interval Values are valid");
            }
        }

        private void F_off_TextChanged(object sender, EventArgs e)
        {
            try
            {
                F_off_interval = Convert.ToInt32(F_off.Text);
            }
            catch
            {
                Console.WriteLine("invalid interval vals");
                return;

            }

            // Ascertain whether values are valid for their purpose
            if (intervals_valid(F_off_interval))
            {
                Console.WriteLine("Interval Values are valid");
            }
        }

        private void F_red_TextChanged(object sender, EventArgs e)
        {
            try
            {
                F_red_value = Convert.ToInt32(F_red.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(F_red_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(F_red_value, F_green_value, F_blue_value);
                F_status.BackColor = newColour;
            }
        }

        private void F_green_TextChanged(object sender, EventArgs e)
        {
            try
            {
                F_green_value = Convert.ToInt32(F_green.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(F_green_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(F_red_value, F_green_value, F_blue_value);
                F_status.BackColor = newColour;
            }
        }

        private void F_blue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                F_blue_value = Convert.ToInt32(F_blue.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(F_blue_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(F_red_value, F_green_value, F_blue_value);
                F_status.BackColor = newColour;
            }
        }

        private void F_trackbar_Scroll(object sender, EventArgs e)
        {
            F_intensity_value = Convert.ToInt32(F_trackbar.Value);
            int calc_intensity = F_intensity_value * 10;

            intensityF.Text = Convert.ToString(calc_intensity) + "%";
        }

        private void G_on_TextChanged(object sender, EventArgs e)
        {
            try
            {
                G_on_interval = Convert.ToInt32(G_on.Text);
            }
            catch
            {
                Console.WriteLine("invalid interval vals");
                return;

            }

            // Ascertain whether values are valid for their purpose
            if (intervals_valid(G_on_interval))
            {
                Console.WriteLine("Interval Values are valid");
            }
        }

        private void G_off_TextChanged(object sender, EventArgs e)
        {
            try
            {
                G_off_interval = Convert.ToInt32(G_off.Text);
            }
            catch
            {
                Console.WriteLine("invalid interval vals");
                return;

            }

            // Ascertain whether values are valid for their purpose
            if (intervals_valid(G_off_interval))
            {
                Console.WriteLine("Interval Values are valid");
            }
        }

        private void G_red_TextChanged(object sender, EventArgs e)
        {
            try
            {
                G_red_value = Convert.ToInt32(G_red.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(G_red_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(G_red_value, G_green_value, G_blue_value);
                G_status.BackColor = newColour;
            }
        }

        private void G_green_TextChanged(object sender, EventArgs e)
        {
            try
            {
                G_green_value = Convert.ToInt32(G_green.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(G_green_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(G_red_value, G_green_value, G_blue_value);
                G_status.BackColor = newColour;
            }
        }

        private void G_blue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                G_blue_value = Convert.ToInt32(G_blue.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(G_blue_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(G_red_value, G_green_value, G_blue_value);
                G_status.BackColor = newColour;
            }
        }

        private void G_trackbar_Scroll(object sender, EventArgs e)
        {
            G_intensity_value = Convert.ToInt32(G_trackbar.Value);
            int calc_intensity = G_intensity_value * 10;

            intensityG.Text = Convert.ToString(calc_intensity) + "%";
        }

        private void H_on_TextChanged(object sender, EventArgs e)
        {
            try
            {
                H_on_interval = Convert.ToInt32(H_on.Text);
            }
            catch
            {
                Console.WriteLine("invalid interval vals");
                return;

            }

            // Ascertain whether values are valid for their purpose
            if (intervals_valid(H_on_interval))
            {
                Console.WriteLine("Interval Values are valid");

            }
        }

        private void H_off_TextChanged(object sender, EventArgs e)
        {
            try
            {
                H_off_interval = Convert.ToInt32(H_off.Text);
            }
            catch
            {
                Console.WriteLine("invalid interval vals");
                return;

            }

            // Ascertain whether values are valid for their purpose
            if (intervals_valid(H_off_interval))
            {
                Console.WriteLine("Interval Values are valid");
            }
        }

        private void H_red_TextChanged(object sender, EventArgs e)
        {
            try
            {
                H_red_value = Convert.ToInt32(H_red.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(H_red_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(H_red_value, H_green_value, H_blue_value);
                H_status.BackColor = newColour;
            }
        }

        private void H_green_TextChanged(object sender, EventArgs e)
        {
            try
            {
                H_green_value = Convert.ToInt32(H_green.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(H_green_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(H_red_value, H_green_value, H_blue_value);
                H_status.BackColor = newColour;
            }
        }

        private void H_blue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                H_blue_value = Convert.ToInt32(H_blue.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(H_blue_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(H_red_value, H_green_value, H_blue_value);
                H_status.BackColor = newColour;
            }
        }

        private void H_trackbar_Scroll(object sender, EventArgs e)
        {
            H_intensity_value = Convert.ToInt32(H_trackbar.Value);
            int calc_intensity = H_intensity_value * 10;

            intensityH.Text = Convert.ToString(calc_intensity) + "%";
        }

        private void I_on_TextChanged(object sender, EventArgs e)
        {
            try
            {
                I_on_interval = Convert.ToInt32(I_on.Text);
            }
            catch
            {
                Console.WriteLine("invalid interval vals");
                return;

            }

            // Ascertain whether values are valid for their purpose
            if (intervals_valid(I_on_interval))
            {
                Console.WriteLine("Interval Values are valid");
            }
        }

        private void I_off_TextChanged(object sender, EventArgs e)
        {
            try
            {
                I_off_interval = Convert.ToInt32(I_off.Text);
            }
            catch
            {
                Console.WriteLine("invalid interval vals");
                return;

            }

            // Ascertain whether values are valid for their purpose
            if (intervals_valid(I_off_interval))
            {
                Console.WriteLine("Interval Values are valid");
            }
        }

        private void I_red_TextChanged(object sender, EventArgs e)
        {
            try
            {
                I_red_value = Convert.ToInt32(I_red.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(I_red_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(I_red_value, I_green_value, I_blue_value);
                I_status.BackColor = newColour;
            }
        }

        private void I_green_TextChanged(object sender, EventArgs e)
        {
            try
            {
                I_green_value = Convert.ToInt32(I_green.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(I_green_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(I_red_value, I_green_value, I_blue_value);
                I_status.BackColor = newColour;
            }
        }

        private void I_blue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                I_blue_value = Convert.ToInt32(I_blue.Text);
            }
            catch
            {
                Console.WriteLine("RGB vals invalid");
                return;
            }
            if (rgb_valid(I_blue_value))
            {
                Console.WriteLine("RGB values are valid");
                Color newColour = Color.FromArgb(I_red_value, I_green_value, I_blue_value);
                I_status.BackColor = newColour;
            }
        }

        private void I_trackbar_Scroll(object sender, EventArgs e)
        {
            I_intensity_value = Convert.ToInt32(I_trackbar.Value);
            int calc_intensity = I_intensity_value * 10;

            intensityI.Text = Convert.ToString(calc_intensity) + "%";
        }

        private void IntensityA_Click(object sender, EventArgs e)
        {

        }

        private void intensityB_Click(object sender, EventArgs e)
        {

        }

        private void intensityC_Click(object sender, EventArgs e)
        {

        }

        private void intensityD_Click(object sender, EventArgs e)
        {

        }

        private void IntensityE_Click(object sender, EventArgs e)
        {

        }

        private void IntensityF_Click(object sender, EventArgs e)
        {

        }

        private void InetnsityG_Click(object sender, EventArgs e)
        {

        }

        private void IntensityH_Click(object sender, EventArgs e)
        {

        }

        private void IntensityI_Click(object sender, EventArgs e)
        {

        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void A_lightButton_Click(object sender, EventArgs e)
        {
            int red_val = Convert.ToInt32(A_red.Text);
            int green_val = Convert.ToInt32(A_green.Text);
            int blue_val = Convert.ToInt32(A_blue.Text);

            int on_interval = Convert.ToInt32(A_on.Text);
            int off_interval = Convert.ToInt32(A_off.Text);


            try
            {
                intervals_valid(on_interval);
                intervals_valid(off_interval);
            }
            catch
            {
                A_on.Text = "";
                A_off.Text = "";
                return;
            }

            Thread test = new Thread(light_loop);

            if (rgb_valid(red_val) && rgb_valid(green_val) && rgb_valid(blue_val) && startStop == false)
            {
                //Thread On_interval = new Thread(light_loop(red_val, green_val, blue_val, on_interval, off_interval));
                startStop = true;
                Console.WriteLine("start");
                test.Start();

                A_on.Enabled = false;
                A_off.Enabled = false;
                A_red.Enabled = false;
                A_blue.Enabled = false;
                A_green.Enabled = false;
                A_trackbar.Enabled = false;
            }
            else if (startStop == true)
            {
                startStop = false;
                Console.WriteLine("ausdihadhsi");
                test.Abort();
                Color newColour = Color.FromArgb(A_red_value, A_green_value, A_blue_value);
                A_status.BackColor = newColour;

                A_on.Enabled = true;
                A_off.Enabled = true;
                A_red.Enabled = true;
                A_blue.Enabled = true;
                A_green.Enabled = true;
                A_trackbar.Enabled = true;
            }


        }

        private void fileOpen_Click(object sender, EventArgs e)
        {
            
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {

        }
    }
}
