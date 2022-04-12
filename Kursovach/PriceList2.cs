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
    public partial class PriceList2 : Form
    {
        public PriceList2()
        {
            InitializeComponent();
        }
        MySqlConnection conn = new MySqlConnection(Con.C());

        private void PriceList2_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string nazvanie_p = textBox1.Text;
            string cena = textBox2.Text;
            string ost = textBox3.Text;
            //Формируем запрос на изменение

            if (nazvanie_p == string.Empty || cena == string.Empty || ost == string.Empty)
            {
                MessageBox.Show($"Введены не все данные");
            }
            else
            {
                string sql_update_pricelist = $"INSERT INTO Product (Production, Cena, ost) " +
                                 $"VALUES ('{nazvanie_p}', '{cena}', '{ost}')";
                // устанавливаем соединение с БД
                conn.Open();
                // объект для выполнения SQL-запроса
                MySqlCommand command = new MySqlCommand(sql_update_pricelist, conn);
                // выполняем запрос
                command.ExecuteNonQuery();
                // закрываем подключение к БД
                conn.Close();
                //Закрываем форму
                MessageBox.Show($"Товар успешно добавлен");
                this.Close();
            }
        }
    }
}
