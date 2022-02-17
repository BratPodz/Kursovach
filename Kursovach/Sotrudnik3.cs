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
    public partial class Sotrudnik3 : Form
    {
        public Sotrudnik3()
        {
            InitializeComponent();
        }

        MySqlConnection conn = new MySqlConnection(Con.C());
        //DataAdapter представляет собой объект Command , получающий данные из источника данных.
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        //Объявление BindingSource, основная его задача, это обеспечить унифицированный доступ к источнику данных.
        private BindingSource bSource = new BindingSource();
        //DataSet - расположенное в оперативной памяти представление данных, обеспечивающее согласованную реляционную программную 
        //модель независимо от источника данных.DataSet представляет полный набор данных, включая таблицы, содержащие, упорядочивающие 
        //и ограничивающие данные, а также связи между таблицами.
        private DataSet ds = new DataSet();
        //Представляет одну таблицу данных в памяти.
        private DataTable table = new DataTable();

        private void button2_Click(object sender, EventArgs e)
        {
            //Получаем айди сотрудника
            string Kod_Sotrudnika = textBox1.Text;
            //Меняем фио сотруднику
            string FIO = textBox4.Text;
            //Получаем новое количество
            string Data_Rojdeniya = maskedTextBox1.Text;
            //Получаем новоый рейтинг
            string Adres = textBox6.Text;
            //Получаем новоый рейтинг
            string Telefon = maskedTextBox2.Text;

            string INN = maskedTextBox3.Text;


            try
            {
                conn.Open();
                // запрос обновления данных
                string query2 = $"UPDATE Sotrudnik SET FIO = '{FIO}', Data_Rojdeniya = '{Data_Rojdeniya}', Adres = '{Adres}', Telefon = '{Telefon}', INN = '{INN}', Kod_Doljnosti = {comboBox1.SelectedIndex + 1}   WHERE Kod_Sotrudnika = {Kod_Sotrudnika}";
                // объект для выполнения SQL-запроса
                MySqlCommand command = new MySqlCommand(query2, conn);
                // выполняем запрос
                command.ExecuteNonQuery();
                // закрываем подключение к БД
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


        private void button1_Click(object sender, EventArgs e)
        {
            //Объявлем переменную для запроса в БД
            string Kod_Sotrudnika = textBox1.Text;
            // устанавливаем соединение с БД
            conn.Open();
            // запрос
            string sql = $"SELECT FIO, INN FROM Sotrudnik WHERE Kod_Sotrudnika={Kod_Sotrudnika}";
            // объект для выполнения SQL-запроса
            MySqlCommand command = new MySqlCommand(sql, conn);
            // объект для чтения ответа сервера
            MySqlDataReader reader = command.ExecuteReader();
            // читаем результат
            while (reader.Read())
            {
                // элементы массива [] - это значения столбцов из запроса SELECT
                label1.Text = reader[0].ToString();
                label2.Text = reader[1].ToString();

            }
            reader.Close(); // закрываем reader
            // закрываем соединение с БД
            conn.Close();
        }
    }
}
