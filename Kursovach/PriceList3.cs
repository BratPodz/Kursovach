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
    public partial class PriceList3 : Form
    {

        MySqlConnection conn = new MySqlConnection(Con.C());

        public PriceList3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Объявлем переменную для запроса в БД
            string Kod_Product = textBox1.Text;
            // устанавливаем соединение с БД
            conn.Open();
            // запрос
            string sql = $"SELECT Production FROM Product WHERE Kod_Producta = {Kod_Product}";
            // объект для выполнения SQL-запроса
            MySqlCommand command = new MySqlCommand(sql, conn);
            // объект для чтения ответа сервера
            MySqlDataReader reader = command.ExecuteReader();
            // читаем результат
            while (reader.Read())
            {
                // элементы массива [] - это значения столбцов из запроса SELECT
                label1.Text = reader[0].ToString();
            }
            reader.Close(); // закрываем reader
            // закрываем соединение с БД
            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Получаем айди сотрудника
            string Kod_Producta = textBox1.Text;
            //Меняем фио сотруднику
            string Kolichesto = textBox2.Text;

            string Cena = textBox3.Text;

            try
            {
                conn.Open();
                // запрос обновления данных
                string query2 = $"UPDATE Product SET ost = '{Kolichesto}', Cena = '{Cena}' WHERE Kod_Producta = '{Kod_Producta}'";
                // объект для выполнения SQL-запроса
                MySqlCommand command = new MySqlCommand(query2, conn);
                // выполняем запрос
                command.ExecuteNonQuery();
                // закрываем подключение к БД6
                MessageBox.Show($"Изменение прошло успешно!");
                conn.Close();
            }
            catch
            {
                MessageBox.Show($"Ошибка в добавлении");
            }
            finally
            {
                conn.Close();
                this.Close();
            }
        }
    }
}
