﻿using System;
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
    public partial class Prodaja : Form
    {
        public Prodaja()
        {
            InitializeComponent();
        }

        private void Prodaja_Load(object sender, EventArgs e)
        {
            GetListProduct();
            GetComboBoxListProdaja();
            MaximizeBox = false;
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

        public void GetComboBoxListProdaja()
        {
            //Формирование списка статусов
            DataTable list_prodaja_table = new DataTable();
            MySqlCommand list_prodaja_command = new MySqlCommand();
            //Открываем соединение
            conn.Open();
            //Формируем столбцы для комбобокса списка ЦП
            list_prodaja_table.Columns.Add(new DataColumn("Kod_Producta", System.Type.GetType("System.Int32")));
            list_prodaja_table.Columns.Add(new DataColumn("Production", System.Type.GetType("System.String")));
            //Настройка видимости полей комбобокса
            comboBox1.DataSource = list_prodaja_table;
            comboBox1.DisplayMember = "Production";
            comboBox1.ValueMember = "Kod_Producta";
            //Формируем строку запроса на отображение списка статусов прав пользователя
            string sql_prodaja = "SELECT Kod_Producta, Production FROM Product";
            list_prodaja_command.CommandText = sql_prodaja;
            list_prodaja_command.Connection = conn;
            //Формирование списка ЦП для combobox'a
            MySqlDataReader list_prodaja_reader;
            try
            {
                //Инициализируем ридер
                list_prodaja_reader = list_prodaja_command.ExecuteReader();
                while (list_prodaja_reader.Read())
                {
                    DataRow rowToAdd = list_prodaja_table.NewRow();
                    rowToAdd["Kod_Producta"] = Convert.ToInt32(list_prodaja_reader[0]);
                    rowToAdd["Production"] = list_prodaja_reader[1].ToString();
                    list_prodaja_table.Rows.Add(rowToAdd);
                }
                list_prodaja_reader.Close();
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



        public void GetListProduct()
        {
            //Запрос для вывода строк в БД
            string sql_product = $"SELECT Kod_Producta AS 'Код Продукта', Production AS 'Название Продукта', Cena AS 'Цена', ost AS 'Остаток' FROM Product";
            //Открываем соединение
            conn.Open();
            //Объявляем команду, которая выполнить запрос в соединении conn
            MyDA.SelectCommand = new MySqlCommand(sql_product, conn);
            //Заполняем таблицу записями из БД
            MyDA.Fill(table);
            //Указываем, что источником данных в bindingsource является заполненная выше таблица
            bSource.DataSource = table;
            //Указываем, что источником данных ДатаГрида является bindingsource 
            dataGridView1.DataSource = bSource;

            dataGridView1.Columns[0].FillWeight = 5;
            dataGridView1.Columns[1].FillWeight = 25;
            dataGridView1.Columns[2].FillWeight = 5;
            dataGridView1.Columns[3].FillWeight = 5;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;

            conn.Close();

        }
        public void reload_list()
        {
            table.Clear();
            GetListProduct();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Ввод артикуля
            string pcod = comboBox1.SelectedValue.ToString();
            //Кол-во товара
            int kol = Convert.ToInt32(textBox2.Text);

            string name = textBox4.Text;

            string date_of_operation = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            int ooos = 0;
            int cena = 0;
            int itog = 0;
            int sale = 0;

            conn.Open();

            string ost = $"SELECT ost, Cena, itog, sale FROM Product WHERE Kod_Producta = {pcod}";

            MySqlCommand commanda = new MySqlCommand(ost, conn);
            MySqlDataReader reader = commanda.ExecuteReader();

            while (reader.Read())
            {
                ooos = Convert.ToInt32(reader[0]);
                cena = Convert.ToInt32(reader[1]);
                itog = Convert.ToInt32(reader[2]);
                sale = Convert.ToInt32(reader[3]);
            }
            reader.Close();
            
            if (kol <= ooos)
            {
                sale = kol + sale;
                ooos = ooos - kol;
                itog = cena * kol + itog;
                try
                {
                    string update_prodaja = $"UPDATE Product SET sale = {sale}, ost = {ooos}, itog = {itog} WHERE Kod_Producta = {pcod}";
                    MySqlCommand command = new MySqlCommand(update_prodaja, conn);
                    command.ExecuteNonQuery();

                    string insert_prodaja = $"INSERT INTO Prodaja (Kolichestvo, Data_Prodaji, Name_Komp, id)" +
                    $"VALUES ('{kol}','{date_of_operation}','{name}','{pcod}')";
                    MySqlCommand command2 = new MySqlCommand(insert_prodaja, conn);
                    // выполняем запрос
                    command2.ExecuteNonQuery();
                }
                finally
                {
                    MessageBox.Show($"Заказ успешно создан");
                }
            }

            else if (kol > ooos)
            {
                MessageBox.Show($"Товара нет в наличии");
            }
            conn.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            reload_list();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }
    }
}
