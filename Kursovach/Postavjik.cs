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
    public partial class Postavjik : Form
    {
        public Postavjik()
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

        public void GetListPostavjik()
        {
            //Запрос для вывода строк в БД
            string sql = $"SELECT Kod_Postavjika AS 'Код поставщика', Nazvanie_P AS 'Поставщик', Telefon AS 'Номер телефона', Rascetniy_Schet AS 'Расчётный счёт', Product AS 'Сырье', Kolvo AS 'Количество' FROM Postavjik";
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

            dataGridView1.Columns[0].FillWeight = 9;
            dataGridView1.Columns[1].FillWeight = 15;
            dataGridView1.Columns[2].FillWeight = 10;
            dataGridView1.Columns[3].FillWeight = 17;
            dataGridView1.Columns[4].FillWeight = 10;
            dataGridView1.Columns[5].FillWeight = 9;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.AllowUserToAddRows = false;

            int count_rows = dataGridView1.RowCount - 0;
            label10.Text = (count_rows).ToString();

            dataGridView1.RowHeadersVisible = false;
            conn.Close();
        }
        

        private void Postavjik_Load(object sender, EventArgs e)
        {
            GetListPostavjik();
        }

        private void dataGridView1_CellMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!e.RowIndex.Equals(-1) && !e.ColumnIndex.Equals(-1) && e.Button.Equals(MouseButtons.Left))
            {
                dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];

                dataGridView1.CurrentRow.Selected = true;

                index_rows5 = dataGridView1.SelectedCells[0].RowIndex.ToString();

                id_rows5 = dataGridView1.Rows[Convert.ToInt32(index_rows5)].Cells[0].Value.ToString();
            }
        }

        public void DeleteSotrudnik(string id_postavjik)
        {
            //Формируем строку запроса на добавление строк
            string sql_delete_user = $"DELETE FROM Postavjik WHERE Kod_Postavjika = '{id_postavjik}'";
            //Посылаем запрос на обновление данных
            MySqlCommand delete_user = new MySqlCommand(sql_delete_user, conn);
            try
            {
                conn.Open();
                delete_user.ExecuteNonQuery();
                MessageBox.Show("Удаление прошло успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.Rows.RemoveAt(Convert.ToInt32(index_rows5));
            }
            catch
            {
                MessageBox.Show($"Ошибка в удалении");
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
            GetListPostavjik();
        }

        private void Уволить_сотрудника_Click(object sender, EventArgs e)
        {
            DeleteSotrudnik(id_rows5);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Postavjik2 p = new Postavjik2();
            p.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Postavjik3 p = new Postavjik3();
            p.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Reload();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Sotrudnik3 s = new Sotrudnik3();
            s.ShowDialog();
        }
    }
}
