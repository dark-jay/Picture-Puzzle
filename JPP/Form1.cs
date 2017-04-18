using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JPP
{
    public partial class Form1 : Form
    {

        Point EmptyPoint;
        Image ToBeResize;

        public Form1()
        {
            InitializeComponent();

            ToBeResize = pictureBox1.Image; //Image.FromFile(Application.StartupPath + "\\pic.jpg");    BY changing it, I let the exe file picture independent. by jayanta
            pictureBox1.BackgroundImage = ToBeResize;
            EmptyPoint.X = 330;
            EmptyPoint.Y = 330;
            AddImagesToButton(ReturnCroppedList(Resize(ToBeResize), 110, 110));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Buttons_Click(object sender, EventArgs e)
        {
            MoveButton((Button)sender);
        }

        private void MoveButton(Button button)
        {
            int x = EmptyPoint.X - button.Location.X;
            int px = x < 0 ? -x : x;

            int y = EmptyPoint.Y - button.Location.Y;
            int py = y < 0 ? -y : y;

            //moving right & left
            if (button.Location.Y.Equals(EmptyPoint.Y) && px.Equals(button.Size.Width))
            {
                button.Location = new Point(button.Location.X + x, button.Location.Y);
                EmptyPoint.X -= x;
            }

            //moving top & buttom
            if (button.Location.X.Equals(EmptyPoint.X) && py.Equals(button.Size.Width))
            {
                button.Location = new Point(button.Location.X, button.Location.Y + y);
                EmptyPoint.Y -= y;
            }
        }

        public void AddImagesToButton(ArrayList pieces)
        {
            int x = 0;
            int[] c = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 }; //********************

            Shuffle(c);

            foreach (Button b in panel1.Controls)
            {
                if (x < c.Length)
                {
                    b.Image = (Image)pieces[c[x]];
                    x++;
                }
            }
        }

        public ArrayList ReturnCroppedList(Bitmap ToBeCropped, int x, int y)
        {
            ArrayList img_pieces = new ArrayList();
            int moveright = 0;
            int movedown = 0;

            for (int k = 0; k < 16; k++)
            {
                Bitmap piece = new Bitmap(x, y);

                for (int i = 0; i < x; i++)
                    for (int j = 0; j < y; j++)
                        piece.SetPixel(i, j, ToBeCropped.GetPixel(i + moveright, j + movedown)); //* **********************
                img_pieces.Add(piece);

                moveright += 110;

                if (moveright == 440) { moveright = 0; movedown += 110; }
                if (movedown == 440) { break; }
            }

            return img_pieces;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Image Files (JPEG,GIF,BMP,PNG,JPG)|*.jpeg;*.bmp;*.png;*.jpg";


            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ToBeResize = Image.FromFile(openFileDialog1.FileName);
                pictureBox1.Image = Resize(ToBeResize);
                AddImagesToButton(ReturnCroppedList(Resize(ToBeResize), 110, 110));

            }
        }

        private Bitmap Resize(Image img)
        {
            int w = 440;   //size of picturebox1
            int h = 440;   //size of picturebox1

            //creat a new Bitmap the size of the new image
            Bitmap bmp = new Bitmap(w, h);
            //creat a new graphic from the bitmap
            Graphics graphic = Graphics.FromImage((Image)bmp);
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //draw the newly resized image
            graphic.DrawImage(img, 0, 0, w, h);
            //dispose and free up th resources
            graphic.Dispose();
            //return the image
            return bmp;
        }

        private void StretchButton_Click(object sender, EventArgs e)
        {
            if (StretchButton.Text == "<<<")
            {
                StretchButton.Text = ">>>";
                this.Size = new Size(513, 567);
            }
            else if (StretchButton.Text == ">>>")
            {
                StretchButton.Text = "<<<";
                this.Size = new Size(965, 567);
            }
        }

        private void ShuffleButton_Click(object sender, EventArgs e)
        {
            AddImagesToButton(ReturnCroppedList(Resize(ToBeResize), 110, 110));
        }

        private int[] Shuffle(int[] array)
        {
            Random r = new Random();

            int start = r.Next(1, array.Length);

            for (int i = 0; i < array.Length; i++)
                for (int j = start; j > 0; j--)
                {
                    int temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            return array;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            (new About()).ShowDialog();
        }


    }
}
