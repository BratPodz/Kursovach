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

        private void button1_Click(object sender, EventArgs e)
        {
            //Определяем значение переменных для записи в БД
            string kod_postavjika = textBox1.Text;
            string nazvanie = textBox2.Text;
            string telefon = textBox3.Text;
            string rascetniy_schet = textBox4.Text;

            //Формируем запрос на изменение
            string sql_update_current_stud = $"INSERT INTO Postavjik (Kod_Postavjika, Nazvanie, Telefon, Rascetniy_Schet) " +
                                             $"VALUES ('{kod_postavjika}', '{nazvanie}', '{telefon}', '{rascetniy_schet}')";
            // устанавливаем соединение с БД
            conn.Open();
            // объект для выполнения SQL-запроса
            MySqlCommand command = new MySqlCommand(sql_update_current_stud, conn);
            // выполняем запрос
            // закрываем подключение к БД
            conn.Close();
            //Закрываем форму
            this.Close();
        }
    }
}