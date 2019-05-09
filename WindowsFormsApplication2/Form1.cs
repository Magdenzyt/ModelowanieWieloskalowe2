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

        Timer timer;

        int wys;
        int szer;


        public Form1()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 200;
            timer.Tick += new EventHandler(timer1_Tick);
            DrawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = DrawArea;
            g = Graphics.FromImage(DrawArea);
            

            

            comboBox1.Items.Add("Niezmienny");
            comboBox1.Items.Add("Glider");
            comboBox1.Items.Add("Oscylator");
            comboBox1.Items.Add("Losowy");
            comboBox1.Items.Add("Własny");

        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            List<List<bool>> nextTab = new List<List<bool>>();
            for (int a = 0; a < wys; a++)
                nextTab.Add(Enumerable.Repeat(false, szer).ToList());

            for (int j = 0; j < wys; j++)
            {
                for (int k = 0; k < szer; k++)
                {
                    nextTab[j][k] = getDot(j, k);
                }
            }
            beginTab = nextTab;
            g.Clear(Color.Transparent);
            for (int z = 0; z < wys; z++)
            {
                for (int j = 0; j < szer; j++)
                {
                    if (beginTab[z][j] == true)
                        g.FillRectangle(pinkBrush, z * (500 / wys), j * (500 / szer), 500 / wys, 500 / szer);
                }
            }
            pictureBox1.Image = DrawArea;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (timer.Enabled)
            {
                timer.Stop();
            }
            else
                timer.Start();
            
        }

        public void fromFile(string filename)
        {
            string[] lines = File.ReadAllLines(filename);

            for(int i =0; i<beginTab.Count; i++)
            {
                for(int j = 0; j<beginTab[i].Count; j++)
                {
                    beginTab[i][j] = false;
                }
            }

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

            if (beginTab[mod(x + 1, wys)][y])
            {
                nc++;
            }
            if (beginTab[mod(x + 1, wys)][mod(y + 1, szer)])
            {
                nc++;
            }
            if (beginTab[x][mod(y + 1, szer)])
            {
                nc++;
            }
            if (beginTab[x][mod(y - 1, szer)])
            {
                nc++;
            }
            if (beginTab[mod(x + 1, wys)][mod(y - 1, szer)])
            {
                nc++;
            }
            if (beginTab[mod(x - 1, wys)][y])
            {
                nc++;
            }
            if (beginTab[mod(x - 1, wys)][mod(y - 1, szer)])
            {
                nc++;
            }
            if (beginTab[mod(x - 1, wys)][mod(y + 1, szer)])
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs myszka = (MouseEventArgs)e;
            Point coordinates = myszka.Location;

            int x = coordinates.X / (500/wys);
            int y = coordinates.Y / (500/szer);

            beginTab[x][y] = true;

            g.Clear(Color.Transparent);
            for (int z = 0; z < wys; z++)
            {
                for (int j = 0; j < szer; j++)
                {
                    if (beginTab[z][j] == true)
                        g.FillRectangle(pinkBrush, z * (500 / wys), j * (500 / szer), 500 / wys, 500 / szer);
                }
            }
            pictureBox1.Image = DrawArea;
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            wys = int.Parse(textBox2.Text);
            szer = int.Parse(textBox3.Text);
            for (int i = 0; i < wys; i++)
                beginTab.Add(Enumerable.Repeat(false, szer).ToList());
            int czas = int.Parse(textBox1.Text);
            timer.Interval = czas;
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
                for (int i = 0; i < wys; i++)
                {
                    for (int j = 0; j < szer; j++)
                    {
                        int z = rnd.Next(0, 4);
                        if (z >= 3)
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
            for (int i = 0; i < wys; i++)
            {
                for (int j = 0; j < szer; j++)
                {
                    if (beginTab[i][j] == true)
                        g.FillRectangle(pinkBrush, i * (500 / wys), j * (500 / szer), 500 / wys, 500 / szer);
                }
            }
            pictureBox1.Image = DrawArea;
            Wait(czas + 300);
        }
    }
}
