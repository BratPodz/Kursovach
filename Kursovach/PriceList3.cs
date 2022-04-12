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

        private void PriceList3_Load(object sender, EventArgs e)
        {
            GetComboBoxListPriceList();
            MaximizeBox = false;
        }

        public void GetComboBoxListPriceList()
        {
            //Формирование списка статусов
            DataTable list_price_table = new DataTable();
            MySqlCommand list_price_command = new MySqlCommand();
            //Открываем соединение
            conn.Open();
            //Формируем столбцы для комбобокса списка ЦП
            list_price_table.Columns.Add(new DataColumn("Kod_Producta", System.Type.GetType("System.Int32")));
            list_price_table.Columns.Add(new DataColumn("Production", System.Type.GetType("System.String")));
            //Настройка видимости полей комбобокса
            comboBox1.DataSource = list_price_table;
            comboBox1.DisplayMember = "Production";
            comboBox1.ValueMember = "Kod_Producta";
            //Формируем строку запроса на отображение списка статусов прав пользователя
            string sql_list_users = "SELECT Kod_Producta, Production FROM Product";
            list_price_command.CommandText = sql_list_users;
            list_price_command.Connection = conn;
            //Формирование списка ЦП для combobox'a
            MySqlDataReader list_price_reader;
            try
            {
                //Инициализируем ридер
                list_price_reader = list_price_command.ExecuteReader();
                while (list_price_reader.Read())
                {
                    DataRow rowToAdd = list_price_table.NewRow();
                    rowToAdd["Kod_Producta"] = Convert.ToInt32(list_price_reader[0]);
                    rowToAdd["Production"] = list_price_reader[1].ToString();
                    list_price_table.Rows.Add(rowToAdd);
                }
                list_price_reader.Close();
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

        private void button1_Click(object sender, EventArgs e)
        {
            //Получаем айди сотрудника
            string Kod_Producta = comboBox1.SelectedValue.ToString();
            //Меняем фио сотруднику
            string Kolichesto = textBox2.Text;

            string Cena = textBox3.Text;

            if (Kod_Producta == string.Empty || Kolichesto == string.Empty || Cena == string.Empty)
            {
                MessageBox.Show($"Введены не все данные");
            }
            
            else
            {
                try
                {
                    conn.Open();
                    // запрос обновления данных
                    string update_pricelist = $"UPDATE Product SET ost = '{Kolichesto}', Cena = '{Cena}' WHERE Kod_Producta = '{Kod_Producta}'";
                    // объект для выполнения SQL-запроса
                    MySqlCommand command = new MySqlCommand(update_pricelist, conn);
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
}
