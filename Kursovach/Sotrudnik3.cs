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

        private void Sotrudnik3_Load(object sender, EventArgs e)
        {
            GetComboBoxListSotrud();
            GetComboBoxListSotrud2();
            MaximizeBox = false;
        }
        MySqlConnection conn = new MySqlConnection(Con.C());
        //DataAdapter представляет собой объект Command , получающий данные из источника данных
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        //Объявление BindingSource, основная его задача, это обеспечить унифицированный доступ к источнику данных.
        private BindingSource bSource = new BindingSource();
        //DataSet - расположенное в оперативной памяти представление данных, обеспечивающее согласованную реляционную программную 
        //модель независимо от источника данных.DataSet представляет полный набор данных, включая таблицы, содержащие, упорядочивающие 
        //и ограничивающие данные, а также связи между таблицами.
        private DataSet ds = new DataSet();
        //Представляет одну таблицу данных в памяти.
        private DataTable table = new DataTable();


        public void GetComboBoxListSotrud()
        {
            //Формирование списка статусов
            DataTable list_sotrudnik_table = new DataTable();
            MySqlCommand list_sotrudnik_command = new MySqlCommand();
            //Открываем соединение
            conn.Open();
            //Формируем столбцы для комбобокса списка ЦП
            list_sotrudnik_table.Columns.Add(new DataColumn("Kod_Sotrudnika", System.Type.GetType("System.Int32")));
            list_sotrudnik_table.Columns.Add(new DataColumn("FIO", System.Type.GetType("System.String")));
            //Настройка видимости полей комбобокса
            comboBox2.DataSource = list_sotrudnik_table;
            comboBox2.DisplayMember = "FIO";
            comboBox2.ValueMember = "Kod_Sotrudnika";
            //Формируем строку запроса на отображение списка статусов прав пользователя
            string sql_list_sotrud = "SELECT Kod_Sotrudnika, FIO FROM Sotrudnik";
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
                    rowToAdd["Kod_Sotrudnika"] = Convert.ToInt32(list_sotrudnik_reader[0]);
                    rowToAdd["FIO"] = list_sotrudnik_reader[1].ToString();
                    list_sotrudnik_table.Rows.Add(rowToAdd);
                }
                list_sotrudnik_reader.Close();
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

        public void GetComboBoxListSotrud2()
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            string sotrud = comboBox2.SelectedValue.ToString();
            //Меняем фио сотруднику
            string FIO = textBox4.Text;
            //Получаем новое количество
            string Data_Rojdeniya = maskedTextBox1.Text;
            //Получаем новоый рейтинг
            string Adres = textBox6.Text;
            //Получаем новоый рейтинг
            string Telefon = maskedTextBox2.Text;

            string INN = maskedTextBox3.Text;


            if (FIO == string.Empty || maskedTextBox1.Text.Length < 10 || Adres == string.Empty || maskedTextBox2.Text.Length < 15 || maskedTextBox3.Text.Length < 12 || comboBox1.Text == string.Empty)
            {
                MessageBox.Show($"Введеные не все данные");
            }

            else
            {
                try
                {
                    conn.Open();
                    string update_sotrud = $"UPDATE Sotrudnik SET FIO = '{FIO}', Data_Rojdeniya = '{Data_Rojdeniya}', Adres = '{Adres}', Telefon = '{Telefon}', INN = '{INN}', Kod_Doljnosti = {comboBox1.SelectedIndex + 1} WHERE Kod_Sotrudnika = {sotrud}";
                    // объект для выполнения SQL-запроса
                    MySqlCommand command_sotrud = new MySqlCommand(update_sotrud, conn);
                    // выполняем запрос
                    command_sotrud.ExecuteNonQuery();
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
        }
    }
}