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
    public partial class Otchet2 : Form
    {

        public Otchet2()
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

        private void Otchet2_Load(object sender, EventArgs e)
        {
            GetListOtchet();
            MaximizeBox = false;
        }

        public void GetListOtchet()
        {
            string sql = $"SELECT Production AS 'Продукт', sale AS 'Продано', itog AS 'На сумму' FROM Product";
            //Запрос для вывода строк в БД

            string p = $"Общая прибыль составляет - ";

            conn.Open();
            //Объявляем команду, которая выполнить запрос в соединении conn
            MyDA.SelectCommand = new MySqlCommand(sql, conn);
            //Заполняем таблицу записями из БД
            MyDA.Fill(table);
            //Указываем, что источником данных в bindingsource является заполненная выше таблица
            bSource.DataSource = table;
            //Указываем, что источником данных ДатаГрида является bindingsource 
            dataGridView1.DataSource = bSource;

            dataGridView1.Columns[0].FillWeight = 2;
            dataGridView1.Columns[1].FillWeight = 5;
            //dataGridView1.Columns[2].FillWeight = 2;


            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;

            string sum = $"SELECT SUM(itog) FROM Product";
            MySqlCommand command1 = new MySqlCommand(sum, conn);
            MySqlDataReader reader = command1.ExecuteReader();
            while(reader.Read())
            {
                label3.Text = p + reader[0].ToString();
            }
            reader.Close();
            conn.Close();
        }
    }
}
