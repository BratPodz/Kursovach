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
    public partial class Prodaja : Form
    {
        public Prodaja()
        {
            InitializeComponent();
        }

        private void Prodaja_Load(object sender, EventArgs e)
        {
            GetListProduct();
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


        public void GetListProduct()
        {
            //Запрос для вывода строк в БД
            string sql = $"SELECT Kod_Producta AS 'Код Продукта', Production AS 'Название Продукта', Cena AS 'Цена', ost AS 'Остаток' FROM Product";
            //Открываем соединение
            conn.Open();
            //Объявляем команду, которая выполнить запрос в соединении conn
            MyDA.SelectCommand = new MySqlCommand(sql, conn);
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

            int count_rows = dataGridView1.RowCount - 0;

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;

            conn.Close();

        }
        public void reload_list()
        {
            table.Clear();
            GetListProduct();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Ввод артикуля
            string pcod = textBox1.Text;
            //Кол-во товара
            string kol = textBox2.Text;

            // устанавливаем соединение с БД
            conn.Open();
            // запрос обновления данных

            string query2 = $"UPDATE Product SET sale = {kol} + sale, ost = ost - {kol}, itog = Cena * {kol} + itog WHERE Kod_Producta = {pcod}";
            MySqlCommand command = new MySqlCommand(query2, conn);
            command.ExecuteNonQuery();

            try
            {
                //Вводим компанию
                string comp = textBox4.Text;
                //Вводим дату покупки
                string date_of_operation = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                // устанавливаем соединение с БД
                // запрос обновления данных
                string query4 = $"INSERT INTO Prodaja (Kolichestvo, Data_Prodaji, Name_Komp, id)" +
                                $"VALUES ('{kol}','{date_of_operation}','{comp}','{pcod}')";

                MySqlCommand command3 = new MySqlCommand(query4, conn);
                // выполняем запрос
                command3.ExecuteNonQuery();
                // закрываем подключение к БД

            }
            finally
            {
                MessageBox.Show($"Заказ успешно создан");
                conn.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            reload_list();
        }
    }
}
