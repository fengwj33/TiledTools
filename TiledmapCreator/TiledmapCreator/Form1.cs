using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TiledmapCreator
{
    public partial class Form1 : Form
    {
        public int tileSize = 32;
        Bitmap bm;
        System.Drawing.Graphics g;
        int offsetX, offsetY;
        Bitmap source;
        int[] map = new int[8];
        int Tcols;
        int currentPointer;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            Tcols=16;
            source = new Bitmap(textBox1.Text);
            tileSize = source.Height;
            bm = new Bitmap(Tcols*tileSize,16*tileSize);
            g = System.Drawing.Graphics.FromImage(bm);
            g.Clear(System.Drawing.Color.Transparent);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            //g.DrawImage(source, new Rectangle(0, 0, 16, 16), getRect(12), System.Drawing.GraphicsUnit.Pixel);
            currentPointer = 0;
            int lasPointer;
            for (int i = 0; i < 256; i++)
            {
                int temp = i;
                for (int k = 0; k < 8; k++)
                {
                    int j;
                    j = temp % 2;
                    temp = temp / 2;
                    map[k] = j;
                }
                offsetY = currentPointer/Tcols;
                offsetX = currentPointer - offsetY * Tcols;
                offsetX *= tileSize;
                offsetY *= tileSize;
                lasPointer = currentPointer;
                CreateTile();
                
                /*
                string[] sx = new string[] { "-1", "", "+1", "-1", "+1", "-1", "", "+1" };
                string[] sy = new string[] { "-1", "-1", "-1", "", "", "+1", "+1", "+1"};
                if (lasPointer != currentPointer)
                {
                    lasPointer = currentPointer;
                    textBox3.Text += "if(";
                    int sp = 0;
                    for (int it = 0; it < 8; it++)
                    {
                        if (map[it] == 0)
                        {
                            textBox3.Text += "s[x" + sx[it] + ",y" + sy[it] + "]" + "==0";
                        }
                        else
                        {
                            textBox3.Text += "s[x" + sx[it] + ",y" + sy[it] + "]" + "!=0";
                        }
                        if (it == 7)
                        {
                            textBox3.Text += ")\r\n";
                        }
                        else
                        {
                            textBox3.Text += " && ";
                        }
                    }
                    textBox3.Text += "{  outm[x,y]=" + currentPointer.ToString()+";continue;}\r\n";
                }

                */

                //currentPointer++;
            }
            




            bm.Save(textBox2.Text, System.Drawing.Imaging.ImageFormat.Png);
            MessageBox.Show("转换完成");

        }
        
        public void CreateTile()
        {
            int[] outID=new int[4];
            int s = tileSize / 2;
            if (map[0] == 0 && map[3] == 0 && map[1] == 0)
                outID[0] = 6;
            else if (map[0] == 0 && map[3] == 0 && map[1] == 1)
                outID[0] = 4;
            else if (map[0] == 0 && map[3] == 1 && map[1] == 0)
                outID[0] = 2;
            else if (map[0] == 0 && map[3] == 1 && map[1] == 1)
                outID[0] = 8;
            else if (map[0] == 1 && map[3] == 0 && map[1] == 0)
                outID[0] = 6;
            else if (map[0] == 1 && map[3] == 0 && map[1] == 1)
                outID[0] = 4;
            else if (map[0] == 1 && map[3] == 1 && map[1] == 0)
                outID[0] = 2;
            else if (map[0] == 1 && map[3] == 1 && map[1] == 1)
                outID[0] = 0;
            else
                return;
            if (map[1] == 0 && map[2] == 0 && map[4] == 0)
                outID[1] = 7;
            else if (map[1] == 0 && map[2] == 0 && map[4] == 1)
                outID[1] = 3;
            else if (map[1] == 0 && map[2] == 1 && map[4] == 0)
                outID[1] = 7;
            else if (map[1] == 0 && map[2] == 1 && map[4] == 1)
                outID[1] = 3;
            else if (map[1] == 1 && map[2] == 0 && map[4] == 0)
                outID[1] = 5;
            else if (map[1] == 1 && map[2] == 0 && map[4] == 1)
                outID[1] = 9;
            else if (map[1] == 1 && map[2] == 1 && map[4] == 0)
                outID[1] = 5;
            else if (map[1] == 1 && map[2] == 1 && map[4] == 1)
                outID[1] = 1;
            else
                return;

            if (map[3] == 0 && map[5] == 0 && map[6] == 0)
                outID[2] = 16;
            else if (map[3] == 0 && map[5] == 0 && map[6] == 1)
                outID[2] = 14;
            else if (map[3] == 0 && map[5] == 1 && map[6] == 0)
                outID[2] = 16;
            else if (map[3] == 0 && map[5] == 1 && map[6] == 1)
                outID[2] = 14;
            else if (map[3] == 1 && map[5] == 0 && map[6] == 0)
                outID[2] = 12;
            else if (map[3] == 1 && map[5] == 0 && map[6] == 1)
                outID[2] = 18;
            else if (map[3] == 1 && map[5] == 1 && map[6] == 0)
                outID[2] = 12;
            else if (map[3] == 1 && map[5] == 1 && map[6] == 1)
                outID[2] = 10;
            else
                return;


            if (map[4] == 0 && map[6] == 0 && map[7] == 0)
                outID[3] = 17;
            else if (map[4] == 0 && map[6] == 0 && map[7] == 1)
                outID[3] = 17;
            else if (map[4] == 0 && map[6] == 1 && map[7] == 0)
                outID[3] = 15;
            else if (map[4] == 0 && map[6] == 1 && map[7] == 1)
                outID[3] = 15;
            else if (map[4] == 1 && map[6] == 0 && map[7] == 0)
                outID[3] = 13;
            else if (map[4] == 1 && map[6] == 0 && map[7] == 1)
                outID[3] = 13;
            else if (map[4] == 1 && map[6] == 1 && map[7] == 0)
                outID[3] = 19;
            else if (map[4] == 1 && map[6] == 1 && map[7] == 1)
                outID[3] = 11;
            else
                return;
            g.DrawImage(source, new Rectangle(offsetX,offsetY, s, s), getRect(outID[0]), System.Drawing.GraphicsUnit.Pixel);
            g.DrawImage(source, new Rectangle(offsetX+s, offsetY, s, s), getRect(outID[1]), System.Drawing.GraphicsUnit.Pixel);
            g.DrawImage(source, new Rectangle(offsetX, offsetY + s, s, s), getRect(outID[2]), System.Drawing.GraphicsUnit.Pixel);
            g.DrawImage(source, new Rectangle(offsetX + s, offsetY + s, s, s), getRect(outID[3]), System.Drawing.GraphicsUnit.Pixel);
            currentPointer++;
        }
        public Rectangle getRect(int id)
        {
            Rectangle Rect = new Rectangle() ;
            int s = tileSize / 2;
            int x, y;
            y = id / 10+1;
            x = id - (y - 1) * 10;
            x = x * s;
            y = (y - 1) * s;
            Rect.X = x;
            Rect.Y = y;
            Rect.Width = s;
            Rect.Height = s;
            return Rect;
        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Png File|*.png";
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = this.openFileDialog1.FileName;
                textBox1.Text = FileName;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Png File|*.png";
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = this.saveFileDialog1.FileName;
                textBox2.Text = FileName;
            }
        }

        
    }
}
