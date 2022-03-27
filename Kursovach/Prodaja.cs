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
        string index_rows5;
        string id_rows5;

        public void GetListProduct()
        {
            //Запрос для вывода строк в БД
            string sql = $"SELECT Kod_Producta AS 'Код Продукта', Production AS 'Название Продукта', Cena AS 'Цена', col AS 'Количество', ost AS 'Остаток' FROM Product";
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

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            int count_rows = dataGridView1.RowCount - 0;

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;

            conn.Close();

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
            string query2 = $"UPDATE Product SET sale = {kol} + sale, itog = itog + {kol}, ost = ost - {kol}, itog = cena * {kol} + itog WHERE Kod_Producta = {pcod}";

            MySqlCommand command = new MySqlCommand(query2, conn);
            // выполняем запрос
            command.ExecuteNonQuery();

            // закрываем подключение к БД
            conn.Close();
            


            try
            {
                
                //Вводим компанию
                string comp = textBox4.Text;
                //Вводим дату покупки
                string cData = maskedTextBox1.Text;

                // устанавливаем соединение с БД
                conn.Open();
                // запрос обновления данных
                string query4 = $"INSERT INTO Prodaja (Kolichestvo, Data_Prodaji, Name_Komp) " +
                                            $"VALUES ('{kol}','{cData}','{comp}')";

                MySqlCommand command3 = new MySqlCommand(query4, conn);
                // выполняем запрос
                command3.ExecuteNonQuery();
                // закрываем подключение к БД
                conn.Close();
                



            }
            catch
            {

            }
            
        }
    }
}
