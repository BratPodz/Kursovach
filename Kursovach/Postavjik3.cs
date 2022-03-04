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
    public partial class Postavjik3 : Form
    {
        public Postavjik3()
        {
            InitializeComponent();
        }

        MySqlConnection conn = new MySqlConnection(Con.C());

        private void button1_Click(object sender, EventArgs e)
        {
            //Объявлем переменную для запроса в БД
            string Kod_Postavjik = textBox1.Text;
            // устанавливаем соединение с БД
            conn.Open();
            // запрос
            string sql = $"SELECT Nazvanie_P FROM Postavjik WHERE Kod_Postavjika = {Kod_Postavjik}";
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
            string Kod_Postavjika = textBox1.Text;
            //Меняем фио сотруднику
            string Nazvanie = textBox2.Text;
            //Получаем новое количество
            string Telefon = maskedTextBox1.Text;
            //Получаем новоый рейтинг
            string Rascetniy_Schet = maskedTextBox2.Text;

            string Kolvo = textBox4.Text;





            try
            {
                conn.Open();
                // запрос обновления данных
                string query2 = $"UPDATE Postavjik SET Nazvanie_P = '{Nazvanie}', Telefon = '{Telefon}', Rascetniy_Schet = '{Rascetniy_Schet}', Product = '{comboBox1.Text}', Kolvo = '{Kolvo}' WHERE Kod_Postavjika = '{Kod_Postavjika}'";
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
