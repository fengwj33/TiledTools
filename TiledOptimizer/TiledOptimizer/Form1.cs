using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace TiledOptimizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "TMX File|*.tmx";

            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = this.openFileDialog1.FileName;
                textBox1.Text = FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string TMXFileName;
            
            int[,] outm;
            
            int width, height;
            string data;
            TMXFileName = textBox1.Text;
            StreamReader sr = new StreamReader(TMXFileName);
            XmlDocument doc = new XmlDocument();
            doc.Load(XmlReader.Create(sr));






            //--------------



            XmlNodeList L_Node = doc.SelectNodes("map/layer");
            XmlDocument doc2 = new XmlDocument();
            doc2.Load(XmlReader.Create(TMXFileName));
            for (int nid = 0; nid < L_Node.Count; nid++)
            {
                XmlElement layerAttrib = (XmlElement)L_Node[nid];
                int.TryParse(layerAttrib.GetAttribute("width"), out width);
                int.TryParse(layerAttrib.GetAttribute("height"), out height);
                data = ((XmlElement)layerAttrib.FirstChild).InnerText;




                DataOpt(data, out outm, width, height);


                XmlNodeList Nodes = doc2.SelectNodes("map/layer");
                XmlElement layerData = (XmlElement)Nodes[nid].FirstChild;
                string outstr = "";
                for (int y = 1; y <= height; y++)
                {
                    for (int x = 1; x <= width; x++)
                    {
                        outstr += outm[x, y].ToString();
                        if (!(x == width && y == height))
                            outstr += ",";
                    }
                    outstr += "\r\n";
                }
                layerData.InnerText = outstr;
            }

            /*
            XmlElement layerAttrib = (XmlElement)doc.SelectSingleNode("map/layer");
            int.TryParse(layerAttrib.GetAttribute("width"),out width);
            int.TryParse(layerAttrib.GetAttribute("height"), out height);
            data = ((XmlElement)doc.SelectSingleNode("map/layer/data")).InnerText ;
     
            
            
            
            DataOpt(data,out outm, width, height);
            XmlDocument doc2 = new XmlDocument();
            doc2.Load(XmlReader.Create(TMXFileName));
            XmlElement layerData = (XmlElement)doc2.SelectSingleNode("map/layer/data");
            string outstr="";
            for (int y = 1; y <= height; y++)
            {
                for (int x = 1; x <= width; x++)
                {
                    outstr += outm[x, y].ToString();
                    if (!(x == width && y == height))
                        outstr += ",";
                }
                outstr += "\r\n";
            }
            layerData.InnerText = outstr;
            */



            
            sr.Close();
            
            
            
            
            doc2.Save(TMXFileName+"out.tmx");
            MessageBox.Show("finish");
            

        }
        private void DataOpt(string data,out int[,] outm,int width,int height)
        {
            int delta=1;
            int[,] s;
            string[] splData;
            splData = data.Split(',');
            s = new int[width + 2, height + 2];
            outm = new int[width + 2, height + 2];
            int pointer = 0;
            for (int y = 1; y <= height; y++)
            {
                for (int x = 1; x <= width; x++)
                {

                    int.TryParse(splData[pointer], out s[x, y]);
                    pointer++;
                }
            }
            for (int y = 1; y <= height; y++)
            {
                for (int x = 1; x <= width; x++)
                {
                    if (s[x, y] == 0)
                    {
                        outm[x, y] = 0;
                        continue;
                    }
                    else
                    {
                        delta = s[x, y];
                    }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 1; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 2; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 3; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 4; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 5; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 6; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 7; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 8; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 9; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 10; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 11; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 12; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 13; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 14; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 15; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 16; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 17; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 18; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 19; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 20; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 21; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 22; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 23; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 24; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 25; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 26; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 27; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 28; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 29; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 30; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 31; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 32; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 33; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 34; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 35; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 36; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 37; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 38; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 39; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 40; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 41; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 42; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 43; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 44; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 45; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 46; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 47; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 48; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 49; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 50; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 51; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 52; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 53; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 54; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 55; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 56; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 57; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 58; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 59; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 60; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 61; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 62; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 63; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 64; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 65; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 66; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 67; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 68; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 69; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 70; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 71; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 72; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 73; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 74; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 75; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 76; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 77; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 78; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 79; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 80; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 81; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 82; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 83; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 84; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 85; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 86; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 87; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 88; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 89; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 90; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 91; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 92; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 93; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 94; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 95; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 96; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 97; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 98; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 99; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 100; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 101; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 102; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 103; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 104; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 105; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 106; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 107; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 108; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 109; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 110; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 111; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 112; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 113; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 114; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 115; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 116; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 117; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 118; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 119; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 120; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 121; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 122; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 123; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 124; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 125; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 126; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 127; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] == 0)
                    { outm[x, y] = 128; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 129; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 130; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 131; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 132; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 133; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 134; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 135; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 136; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 137; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 138; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 139; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 140; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 141; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 142; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 143; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 144; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 145; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 146; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 147; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 148; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 149; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 150; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 151; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 152; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 153; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 154; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 155; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 156; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 157; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 158; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 159; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 160; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 161; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 162; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 163; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 164; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 165; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 166; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 167; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 168; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 169; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 170; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 171; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 172; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 173; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 174; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 175; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 176; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 177; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 178; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 179; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 180; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 181; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 182; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 183; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 184; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 185; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 186; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 187; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 188; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 189; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 190; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 191; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] == 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 192; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 193; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 194; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 195; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 196; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 197; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 198; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 199; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 200; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 201; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 202; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 203; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 204; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 205; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 206; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 207; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 208; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 209; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 210; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 211; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 212; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 213; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 214; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 215; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 216; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 217; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 218; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 219; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 220; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 221; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 222; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 223; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] == 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 224; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 225; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 226; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 227; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 228; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 229; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 230; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 231; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 232; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 233; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 234; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 235; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 236; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 237; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 238; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 239; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] == 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 240; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 241; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 242; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 243; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 244; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 245; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 246; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 247; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] == 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 248; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 249; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 250; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 251; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] == 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 252; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 253; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] == 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 254; continue; }
                    if (s[x - 1, y - 1] == 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 255; continue; }
                    if (s[x - 1, y - 1] != 0 && s[x, y - 1] != 0 && s[x + 1, y - 1] != 0 && s[x - 1, y] != 0 && s[x + 1, y] != 0 && s[x - 1, y + 1] != 0 && s[x, y + 1] != 0 && s[x + 1, y + 1] != 0)
                    { outm[x, y] = 256; continue; }
                }
            }
            for (int y = 1; y <= height; y++)
            {
                for (int x = 1; x <= width; x++)
                {
                    if (outm[x, y] != 0)
                        outm[x, y] += delta - 1;
                }
            }
        }
    }
}
