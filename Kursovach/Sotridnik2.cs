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
    public partial class Sotridnik2 : Form
    {
        public Sotridnik2()
        {
            InitializeComponent();
        }

        MySqlConnection conn = new MySqlConnection(Con.C());

        private void button1_Click(object sender, EventArgs e)
        {
            //Определяем значение переменных для записи в БД
            string fio = textBox3.Text;
            string data_rojd = maskedTextBox1.Text;
            string adress = textBox5.Text;
            string telef = maskedTextBox2.Text;
            string inn = maskedTextBox3.Text;
            //Формируем запрос на изменение

            if (fio == string.Empty || maskedTextBox1.Text.Length < 10 || adress == string.Empty || maskedTextBox2.Text.Length < 15 || maskedTextBox3.Text.Length < 14 || comboBox1.Text == string.Empty)
            {
                MessageBox.Show($"Введеные не все данные");
            }

            else
            {
                try
                {
                    string sql_update_current_stud = $"INSERT INTO Sotrudnik (FIO, Data_Rojdeniya, Adres, Telefon, INN, Kod_Doljnosti) " +
                                 $"VALUES ('{fio}', '{data_rojd}', '{adress}', '{telef}', '{inn}', '{comboBox1.SelectedIndex + 1}')";
                    // устанавливаем соединение с БД
                    conn.Open();
                    // объект для выполнения SQL-запроса
                    MySqlCommand command = new MySqlCommand(sql_update_current_stud, conn);
                    // выполняем запрос
                    command.ExecuteNonQuery();
                    // закрываем подключение к БД
                }

                finally
                {
                    MessageBox.Show($"Сотрудник успешно добавлен");
                    conn.Close();
                    this.Close();
                }
            }
        }

        private void Sotridnik2_Load(object sender, EventArgs e)
        {

        }
    }
}
