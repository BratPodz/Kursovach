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
    public partial class Form2 : Form
    {
        public Form2()
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

        public void GetListSotrunkik()
        {
            //Запрос для вывода строк в БД
            string sql = $"SELECT Kod_Sotrudnika AS 'Код сотрудника', FIO AS 'Фио', Data_Rojdeniya AS 'Дата рождения', Adres AS 'Адрес', Telefon AS 'Телефон', INN AS 'ИНН' FROM Sotrudnik";
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
            dataGridView1.Columns[2].FillWeight = 9;
            dataGridView1.Columns[3].FillWeight = 15;
            dataGridView1.Columns[4].FillWeight = 15;
            dataGridView1.Columns[5].FillWeight = 9;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            int count_rows = dataGridView1.RowCount - 0;
            label10.Text = (count_rows).ToString();

            dataGridView1.RowHeadersVisible = false;
            conn.Close();

        }
        private void Form2_Load(object sender, EventArgs e)
        {
            GetListSotrunkik();
            GetComboBoxList();
        }

        public void Reload()
        {
            //Чистим виртуальную таблицу
            table.Clear();
            //Вызываем метод получения записей, который вновь заполнит таблицу
            GetListSotrunkik();
        }
        public void DeleteSotrudnik(string id_sotrudnik)
        {
            //Формируем строку запроса на добавление строк
            string sql_delete_sotrudnik = $"DELETE FROM Sotrudnik WHERE Kod_Sotrudnika = '{id_sotrudnik}'";
            //Посылаем запрос на обновление данных
            MySqlCommand delete_sotrudnik = new MySqlCommand(sql_delete_sotrudnik, conn);
            try
            {
                conn.Open();
                delete_sotrudnik.ExecuteNonQuery();
                MessageBox.Show("Удаление прошло успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.Rows.RemoveAt(Convert.ToInt32(index_rows5));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления строки \n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            finally
            {
                conn.Close();
                //Вызов метода обновления ДатаГрида
                Reload();
            }
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
        public void GetComboBoxList()
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
            comboBox1.DataSource = list_sotrudnik_table;
            comboBox1.DisplayMember = "FIO";
            comboBox1.ValueMember = "Kod_Sotrudnika";
            //Формируем строку запроса на отображение списка статусов прав пользователя
            string sql_list_users = "SELECT Kod_Sotrudnika, FIO FROM Sotrudnik";
            list_sotrudnik_command.CommandText = sql_list_users;
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
        private void GetList(int id_sotrud)
        {
            conn.Open();
            //Строка запроса
            string commandStr = $"SELECT FIO, Nazvanie_Doljnosti, Oklad FROM Sotrudnik INNER JOIN Doljnost ON Sotrudnik.Kod_Doljnosti = Doljnost.Kod_Doljnosti WHERE FIO = '{comboBox1.Text}'";
            //Команда для получения списка
            MySqlCommand get_list = new MySqlCommand(commandStr, conn);
            //Ридер для хранения списка строк
            MySqlDataReader reader_list = get_list.ExecuteReader();
            //Читаем ридер
            while (reader_list.Read())
            {
                //Формируем строку для вывода красивого сообщения в MessageBox
                string s = "";
                s += "ФИО: " + reader_list[0].ToString() + "\n";
                s += "Должность: " + reader_list[1].ToString() + "\n";
                s += "Оклад: " + reader_list[2].ToString();
                MessageBox.Show(s);
            }
            //Закрываем ридер
            reader_list.Close();
            //Закрываем соединение
            conn.Close();   
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DeleteSotrudnik(id_rows5);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Reload();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Sotridnik2 s = new Sotridnik2();
            s.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int id_sotrud = Convert.ToInt32(comboBox1.SelectedValue);
            //Посылаем ID сотрудника в метод вывода информациии
            GetList(id_sotrud);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Sotrudnik3 s = new Sotrudnik3();
            s.ShowDialog();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            int id_sotrud = Convert.ToInt32(comboBox1.SelectedValue);
            //Посылаем ID сотрудника в метод вывода информациии
            GetList(id_sotrud);
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Sotrudnik3 s = new Sotrudnik3();
            s.ShowDialog();
        }
    }
}