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

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        Bitmap DrawArea;
        Graphics g;
        SolidBrush pinkBrush = new SolidBrush(Color.HotPink);

        List<List<bool>> beginTab = new List<List<bool>> ();
        Random rnd = new Random();


        public Form1()
        {
            InitializeComponent();
            
            DrawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = DrawArea;
            g = Graphics.FromImage(DrawArea);

            for(int i = 0; i<50; i++)
               beginTab.Add(Enumerable.Repeat(false, 50).ToList());

            comboBox1.Items.Add("Niezmienny");
            comboBox1.Items.Add("Glider");
            comboBox1.Items.Add("Oscylator");
            comboBox1.Items.Add("Losowy");
            comboBox1.Items.Add("Własny");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int czas = int.Parse(textBox1.Text);
            g.Clear(Color.Transparent);
            if (comboBox1.Text == "Niezmienny")
            {
                fromFile("niezmienny.txt");
            }
            else if (comboBox1.Text == "Glider")
            {
                fromFile("glider.txt");
            }
            else if (comboBox1.Text == "Oscylator")
            {
                fromFile("oscylator.txt");
            }
            else if (comboBox1.Text == "Losowy")
            {
                for (int i = 0; i < 50; i++)
                {
                    for (int j = 0; j < 50; j++)
                    {
                        int z = rnd.Next(0, 4);
                        if (z>=3)
                            beginTab[i][j] = true;
                        else
                            beginTab[i][j] = false;
                    }
                }
            }
            else if (comboBox1.Text == "Własny")
            {
                fromFile("wlasny.txt");
            }
            ///////////////////////////////////////////////////////////////
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    if (beginTab[i][j] == true)
                        g.FillRectangle(pinkBrush, i*10, j*10, 10, 10);
                }
            }
            pictureBox1.Image = DrawArea;
            Wait(czas+300);
            while(true)
            {
                List<List<bool>> nextTab = new List<List<bool>>();
                for (int a = 0; a < 50; a++)
                    nextTab.Add(Enumerable.Repeat(false, 50).ToList());

                for (int j = 0; j < 50; j++)
                {    
                    for (int k = 0; k < 50; k++)
                    { 
                        nextTab[j][k] = getDot(j, k);
                    }
                }
                beginTab = nextTab;
                g.Clear(Color.Transparent);
                for (int z = 0; z < 50; z++)
                {
                    for (int j = 0; j < 50; j++)
                    {
                        if (beginTab[z][j] == true)
                            g.FillRectangle(pinkBrush, z * 10, j * 10, 10, 10);
                    }
                }
                pictureBox1.Image = DrawArea;
                Wait(czas);
            }
            
        }

        public void fromFile(string filename)
        {
            string[] lines = File.ReadAllLines(filename);

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '*')
                    {
                        beginTab[j][i] = true;
                    }
                    else
                    {
                        beginTab[j][i] = false;
                    }
                }
            }
        }

        public bool getDot(int x, int y)
        {
            return beginTab[x][y] && getNeighbourCount(x, y) == 2 || getNeighbourCount(x, y) == 3;
        }

        public int mod(int x, int m)
        { 
            m = Math.Abs(m);       
            return (x % m + m) % m;
        }

        public int getNeighbourCount(int x, int y)
        {
            int nc = 0;

            if (beginTab[mod(x + 1, 50)][y])
            {
                nc++;
            }
            if (beginTab[mod(x + 1, 50)][mod(y + 1, 50)])
            {
                nc++;
            }
            if (beginTab[x][mod(y + 1, 50)])
            {
                nc++;
            }
            if (beginTab[x][mod(y - 1, 50)])
            {
                nc++;
            }
            if (beginTab[mod(x + 1, 50)][mod(y - 1, 50)])
            {
                nc++;
            }
            if (beginTab[mod(x - 1, 50)][y])
            {
                nc++;
            }
            if (beginTab[mod(x - 1, 50)][mod(y - 1, 50)])
            {
                nc++;
            }
            if (beginTab[mod(x - 1, 50)][mod(y + 1, 50)])
            {
                nc++;
            }
            return nc;
        }

        public void Wait(int ms)
        {
            DateTime start = DateTime.Now;
            while ((DateTime.Now - start).TotalMilliseconds < ms)
                Application.DoEvents();

        }
    }
}
