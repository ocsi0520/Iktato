using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        int besorolas = 0, alszam = 0;
        List<sor> lista = new List<sor>();
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            besorolas = comboBox1.SelectedIndex;
            switch (besorolas)
            {
                case 0:
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    label1.Enabled = false;
                    label2.Enabled = false;
                    label3.Enabled = false;
                    comboBox2.SelectedIndex = 0;
                    comboBox3.SelectedIndex = 0;
                    comboBox4.SelectedIndex = 0;
                    alszam = 0;
                    break;
                case 1:
                    comboBox2.Enabled = true;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    label1.Enabled = true;
                    label2.Enabled = false;
                    label3.Enabled = false;
                    comboBox2.SelectedIndex = 0;
                    comboBox3.SelectedIndex = 0;
                    comboBox4.SelectedIndex = 0;
                    break;
                case 2:
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = true;
                    comboBox4.Enabled = false;
                    label1.Enabled = false;
                    label2.Enabled = true;
                    label3.Enabled = false;
                    comboBox2.SelectedIndex = 0;
                    comboBox3.SelectedIndex = 0;
                    comboBox4.SelectedIndex = 0;
                    break;
                case 3:
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = true;
                    label1.Enabled = false;
                    label2.Enabled = false;
                    label3.Enabled = true;
                    comboBox2.SelectedIndex = 0;
                    comboBox3.SelectedIndex = 0;
                    comboBox4.SelectedIndex = 0;
                    break;
                default:
                    MessageBox.Show("Vmi gebasz van");
                    break;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            alszam = comboBox2.SelectedIndex;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            alszam = comboBox3.SelectedIndex;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            alszam = comboBox4.SelectedIndex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (besorolas == 0 || alszam == 0)
                MessageBox.Show("Gebasz van! Minden mezőben választani kell!");
            else
            {
                richTextBox1.Text = "Eddig " + lista.Count(x => x.al == alszam && x.besor == besorolas) + " van belőle, így ez a " + (lista.Count(x => x.al == alszam && x.besor == besorolas)+1);
                lista.Add(new sor(besorolas, alszam, textBox1.Text, textBox2.Text, monthCalendar1.SelectionRange.Start.ToShortDateString()));
                textBox1.Text="";
                textBox2.Text="";
                monthCalendar1.SelectionStart= DateTime.Now;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            foreach (var item in lista.OrderBy(x=>x.besor).GroupBy(x=>x.besor))
            {
                richTextBox1.Text += "Besorolás alapján: " + item.First().besor.ToString("000") + "\n";
                foreach (var item2 in item.Select(x=>x.al).Distinct().OrderBy(x=>x))
                {
                    richTextBox1.Text += item2.ToString("000") + " - " + item.Count(x=>x.al==item2) + " db\n";
                }
            }
        }

        private void mentésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.DefaultExt = "txt";
            sf.ShowDialog();
            if (sf.FileName != "")
            {
                List<string> vmi = new List<string>();
                foreach (sor item in lista)
                {
                    vmi.Add(item.besor + "|" + item.al + "|" + item.nev + "|" + item.megjegyzes + "|" + item.datum);
                }
                File.WriteAllLines(sf.FileName, vmi);
            }
        }

        private void megnyitásToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("A most használt adatok elvesznek, ha újat nyitsz meg", "FIGYELEM!!!");
            OpenFileDialog of = new OpenFileDialog();
            of.ShowDialog();
            if (of.FileName != "")
            {
                sor.szamlalo = 3063;
                lista.Clear();
                foreach (string item in File.ReadAllLines(of.FileName))
                {
                    string[] asd = item.Split('|');
                    lista.Add(new sor(Convert.ToInt32(asd[0]), Convert.ToInt32(asd[1]),asd[2],asd[3],asd[4]));
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<sor> seged = lista.Where(x => x.nev.ToLower().Contains(textBox1.Text.ToLower()) && x.megjegyzes.ToLower().Contains(textBox2.Text.ToLower())).ToList();
            if (besorolas != 0)
            {
                seged = seged.Where(x => x.besor == besorolas).ToList();
                if (alszam != 0)
                    seged = seged.Where(x => x.al == alszam).ToList();
            }
            if (seged.Count == 0)
            {
                richTextBox1.Text = "Nem található ilyen objektum"; return;
            }
            richTextBox1.Text = "Besorolás | Alszám | Név | Megjegyzés | Dátum | Sorszám\n\n";
            foreach (sor item in seged)
            {
                richTextBox1.Text += item.besor.ToString("000") + " | " + item.al.ToString("000") + " | " + item.nev + " | " + item.megjegyzes + " | " + item.datum + " | " + item.sorszam + "\n";
            }
        }
    }
    class sor
    {
        public static int szamlalo = 3063;
        public int besor, al, sorszam;
        public string nev, megjegyzes,datum;
        public sor(int besor, int al,string nev,string megjegyzes,string datum)
        {
            this.besor = besor;
            this.al = al;
            if (nev == "")
                this.nev = "-";
            else
            this.nev = nev;
            if (megjegyzes == "")
                this.megjegyzes = "-";
            else
            this.megjegyzes = megjegyzes;
            this.datum = datum;
            this.sorszam = ++sor.szamlalo;
        }
    }
}
