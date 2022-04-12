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

            if (fio == string.Empty || maskedTextBox1.Text.Length < 10 || adress == string.Empty || maskedTextBox2.Text.Length < 15 || maskedTextBox3.Text.Length < 12 || comboBox1.Text == string.Empty)
            {
                MessageBox.Show($"Введеные не все данные");
            }

            else
            {
                try
                {
                    string update_insert_sotrud = $"INSERT INTO Sotrudnik (FIO, Data_Rojdeniya, Adres, Telefon, INN, Kod_Doljnosti) " +
                                 $"VALUES ('{fio}', '{data_rojd}', '{adress}', '{telef}', '{inn}', '{comboBox1.SelectedIndex + 1}')";
                    // устанавливаем соединение с БД
                    conn.Open();
                    // объект для выполнения SQL-запроса
                    MySqlCommand command = new MySqlCommand(update_insert_sotrud, conn);
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

        public void GetComboBoxListSotrud()
        {
            //Формирование списка статусов
            DataTable list_sotrudnik_table = new DataTable();
            MySqlCommand list_sotrudnik_command = new MySqlCommand();
            //Открываем соединение
            conn.Open();
            //Формируем столбцы для комбобокса списка ЦП
            list_sotrudnik_table.Columns.Add(new DataColumn("Kod_Doljnosti", System.Type.GetType("System.Int32")));
            list_sotrudnik_table.Columns.Add(new DataColumn("Nazvanie_Doljnosti", System.Type.GetType("System.String")));
            //Настройка видимости полей комбобокса
            comboBox1.DataSource = list_sotrudnik_table;
            comboBox1.DisplayMember = "Nazvanie_Doljnosti";
            comboBox1.ValueMember = "Kod_Doljnosti";
            //Формируем строку запроса на отображение списка статусов прав пользователя
            string sql_list_sotrud = "SELECT Kod_Doljnosti, Nazvanie_Doljnosti FROM Doljnost";
            list_sotrudnik_command.CommandText = sql_list_sotrud;
            list_sotrudnik_command.Connection = conn;
            //Формирование списка ЦП для combobox'a
            MySqlDataReader list_sotrudnik_reader;
            try
            {
                //Инициализируем ридер
                list_sotrudnik_reader = list_sotrudnik_command.ExecuteReader();
                while (list_sotrudnik_reader.Read())
                {
                    DataRow rowToAdd = list_sotrudnik_table.NewRow();
                    rowToAdd["Kod_Doljnosti"] = Convert.ToInt32(list_sotrudnik_reader[0]);
                    rowToAdd["Nazvanie_Doljnosti"] = list_sotrudnik_reader[1].ToString();
                    list_sotrudnik_table.Rows.Add(rowToAdd);
                }
                list_sotrudnik_reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка чтения списка ЦП \n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void Sotridnik2_Load(object sender, EventArgs e)
        {
            GetComboBoxListSotrud();
            MaximizeBox = false;
        }
    }
}
