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
    public partial class PriceList : Form
    {
        public PriceList()
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
        string index_rows5;
        string id_rows5;


        public void GetListPrice()
        {
            //Запрос для вывода строк в БДД
            string sql = $"SELECT Kod_Producta AS 'Код продукта', Production AS 'Название', Cena AS 'Цена' FROM Product";
            //Открываем соединение
            conn.Open();
            //Объявляем команду, которая выполнить запрос в соединении conn+
            MyDA.SelectCommand = new MySqlCommand(sql, conn);
            //Заполняем таблицу записями из БД
            MyDA.Fill(table);
            //Указываем, что источником данных в bindingsource является заполненная выше таблица
            bSource.DataSource = table;
            //Указываем, что источником данных ДатаГрида является bindingsource 
            dataGridView1.DataSource = bSource;
            dataGridView1.Columns[0].FillWeight = 5;
            dataGridView1.Columns[1].FillWeight = 15;
            dataGridView1.Columns[2].FillWeight = 15;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.AllowUserToAddRows = false;

            dataGridView1.RowHeadersVisible = false;
            conn.Close();

        }

        public void DeletePrice(string id_product)
        {
            //Формируем строку запроса на добавление строк
            string sql_delete_price = $"DELETE FROM Product WHERE Kod_Producta = '{id_product}'";
            //Посылаем запрос на обновление данных
            MySqlCommand delete_price = new MySqlCommand(sql_delete_price, conn);
            try
            {
                conn.Open();
                delete_price.ExecuteNonQuery();
                MessageBox.Show("Удаление прошло успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.Rows.RemoveAt(Convert.ToInt32(index_rows5));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления строки \n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
                //Вызов метода обновления ДатаГрида
                Reload();
            }
        }

        public void Reload()
        {
            //Чистим виртуальную таблицу
            table.Clear();
            //Вызываем метод получения записей, который вновь заполнит таблицу
            GetListPrice();
        }

        private void PriceList_Load(object sender, EventArgs e)
        {
            GetListPrice();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DeletePrice(id_rows5);
            Reload();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!e.RowIndex.Equals(-1) && !e.ColumnIndex.Equals(-1) && e.Button.Equals(MouseButtons.Left))
            {
                dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];

                dataGridView1.CurrentRow.Selected = true;

                index_rows5 = dataGridView1.SelectedCells[0].RowIndex.ToString();

                id_rows5 = dataGridView1.Rows[Convert.ToInt32(index_rows5)].Cells[0].Value.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Reload();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PriceList2 p = new PriceList2();
            p.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PriceList3 p = new PriceList3();
            p.ShowDialog();
        }
    }
}
