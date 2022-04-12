using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ClassLibrary2;


namespace Kursovach
{
    public partial class Postavjik2 : Form
    {
        public Postavjik2()
        {
            InitializeComponent();
        }

        MySqlConnection conn = new MySqlConnection(Con.C());

        private void button2_Click(object sender, EventArgs e)
        {
            //Определяем значение переменных для записи в БД
            string nazvanie = textBox4.Text;
            string telefon = maskedTextBox1.Text;
            string rascetniy_schet = maskedTextBox2.Text;
            string Kolvo = textBox1.Text;
            //Формируем запрос на изменение

            if (nazvanie == string.Empty || telefon == string.Empty || rascetniy_schet == string.Empty || Kolvo == string.Empty)
            {
                MessageBox.Show($"Введены не все данные");
            }
            else
            {
                string sql_update_postavjik = $"INSERT INTO Postavjik (Nazvanie_P, Telefon, Rascetniy_Schet, Product, Kolvo) " +
                                 $"VALUES ('{nazvanie}', '{telefon}', '{rascetniy_schet}', '{comboBox1.Text}', '{Kolvo}')";
                // устанавливаем соединение с БД
                conn.Open();
                // объект для выполнения SQL-запроса
                MySqlCommand command = new MySqlCommand(sql_update_postavjik, conn);
                // выполняем запрос
                command.ExecuteNonQuery();
                // закрываем подключение к БД
                conn.Close();
                //Закрываем форму
                MessageBox.Show($"Поставщик успешно добавлен");
                this.Close();
            }
        }
    }
}