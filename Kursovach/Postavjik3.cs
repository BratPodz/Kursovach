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
            GetComboBoxListPostavjik();
        }

        MySqlConnection conn = new MySqlConnection(Con.C());

        public void GetComboBoxListPostavjik()
        {
            //Формирование списка статусов
            DataTable list_postavjik_table = new DataTable();
            MySqlCommand list_postavjik_command = new MySqlCommand();
            //Открываем соединение
            conn.Open();
            //Формируем столбцы для комбобокса списка ЦП
            list_postavjik_table.Columns.Add(new DataColumn("Kod_Postavjika", System.Type.GetType("System.Int32")));
            list_postavjik_table.Columns.Add(new DataColumn("Nazvanie_P", System.Type.GetType("System.String")));
            //Настройка видимости полей комбобокса
            comboBox2.DataSource = list_postavjik_table;
            comboBox2.DisplayMember = "Nazvanie_P";
            comboBox2.ValueMember = "Kod_Postavjika";
            //Формируем строку запроса на отображение списка статусов прав пользователя
            string sql_list_postavjik = "SELECT Kod_Postavjika, Nazvanie_P FROM Postavjik";
            list_postavjik_command.CommandText = sql_list_postavjik;
            list_postavjik_command.Connection = conn;
            //Формирование списка ЦП для combobox'a
            MySqlDataReader list_postavjik_reader;
            try
            {
                //Инициализируем ридер
                list_postavjik_reader = list_postavjik_command.ExecuteReader();
                while (list_postavjik_reader.Read())
                {
                    DataRow rowToAdd = list_postavjik_table.NewRow();
                    rowToAdd["Kod_Postavjika"] = Convert.ToInt32(list_postavjik_reader[0]);
                    rowToAdd["Nazvanie_P"] = list_postavjik_reader[1].ToString();
                    list_postavjik_table.Rows.Add(rowToAdd);
                }
                list_postavjik_reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка чтения списка ЦП \n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            finally
            {
                conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Получаем айди сотрудника
            string Kod_Postavjika = comboBox2.SelectedValue.ToString();
            //Меняем фио сотруднику
            string Nazvanie = textBox2.Text;
            //Получаем новое количество
            string Telefon = maskedTextBox1.Text;
            //Получаем новоый рейтинг
            string Rascetniy_Schet = maskedTextBox2.Text;

            string Kolvo = textBox4.Text;

            if (Kod_Postavjika == string.Empty || Nazvanie == string.Empty || Telefon == string.Empty || Rascetniy_Schet == string.Empty || Rascetniy_Schet == string.Empty || Kolvo == string.Empty)
            {
                MessageBox.Show($"Введены не все данные");
            }
            else
            {
                try
                {
                    conn.Open();
                    // запрос обновления данных
                    string query_postavjik = $"UPDATE Postavjik SET Nazvanie_P = '{Nazvanie}', Telefon = '{Telefon}', Rascetniy_Schet = '{Rascetniy_Schet}', " +
                        $"Product = '{comboBox1.Text}', Kolvo = '{Kolvo}' WHERE Kod_Postavjika = '{Kod_Postavjika}'";
                    // объект для выполнения SQL-запроса
                    MySqlCommand command = new MySqlCommand(query_postavjik, conn);
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

        private void Postavjik3_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
        }
    }
}
