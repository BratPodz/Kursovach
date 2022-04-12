using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary2;
using MySql.Data.MySqlClient;

namespace Kursovach
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Postavjik f = new Postavjik();
            f.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PriceList price = new PriceList();
            price.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Prodaja prodaja = new Prodaja();
            prodaja.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Otchet otchet = new Otchet();
            otchet.ShowDialog();
        }
    }
}
