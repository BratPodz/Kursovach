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
    public partial class Otchet : Form
    {
        public Otchet()
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

        private void Otchet_Load(object sender, EventArgs e)
        {
            GetListOtchet();
            MaximizeBox = false;
        }
        public void GetListOtchet()
        {
            string sql = $"SELECT Kod_Prodaji AS 'Код продажи', Production AS 'Название продукта', Kolichestvo AS 'Количество', Prodaja.Data_Prodaji AS 'Дата продажи', " +
                $"Prodaja.Name_Komp AS 'Наименование заказчика'  FROM Product INNER JOIN Prodaja ON Product.Kod_Producta = Prodaja.id";
            ////Открываем соединение
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
            dataGridView1.Columns[2].FillWeight = 3;
            dataGridView1.Columns[3].FillWeight = 3;
            dataGridView1.Columns[4].FillWeight = 3;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            int count_rows = dataGridView1.RowCount - 0;

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;

            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Otchet2 o = new Otchet2();
            o.ShowDialog();
        }
    }
}
