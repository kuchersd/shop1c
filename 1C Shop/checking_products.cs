using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Linq;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace _1C_Shop
{
    public partial class checking_products : Form
    {
        [Table(Name = "Products")]
        public class Products
        {
            [Column(IsPrimaryKey = true, IsDbGenerated = true)]
            public int Id { get; set; }
            [Column(Name = "Name")]
            public string Name { get; set; }
            [Column(Name = "Code")]
            public string Code { get; set; }
            [Column(Name = "Description")]
            public string Description { get; set; }
            [Column(Name = "First_Price")]
            public double First_Price { get; set; }
            [Column(Name = "Second_Price")]
            public double Second_Price { get; set; }
            [Column(Name = "Price_Kg")]
            public double Price_Kg { get; set; }
            [Column(Name = "Count")]
            public int Count { get; set; }
            [Column(Name = "Count_Kg")]
            public double Count_Kg { get; set; }
        }

        SqlDataAdapter adapter;
        SqlConnection con;
        DataSet data_set;
        SqlCommand sql_cmd;
        SqlDataReader ans;
        public checking_products()
        {
            InitializeComponent();
            sql_cmd = new SqlCommand();
            data_set = new DataSet();
            con = new SqlConnection(connectionString);
        }

        static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Kreal\source\repos\1C Shop\1C Shop\Database.mdf;Integrated Security=True";

        public partial class Shop : DataContext
        {
            public Table<Products> Products;
            public Shop(string connection) : base(connection) { }
        }

        private void checking_products_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "databaseDataSet3.Products". При необходимости она может быть перемещена или удалена.
            //this.productsTableAdapter.Fill(this.databaseDataSet3.Products);
            string cmd = @"select * from Products";
            sql_cmd.Connection = con;
            sql_cmd.CommandText = cmd;
            //
            adapter = new SqlDataAdapter(cmd, con);
            data_set = new DataSet();
            adapter.Fill(data_set);
            dataGridView1.DataSource = data_set.Tables[0];
            label8.Text = dataGridView1.RowCount.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //this.productsTableAdapter.Fill(this.databaseDataSet3.Products);
            string cmd = @"select * from Products";
            sql_cmd.Connection = con;
            sql_cmd.CommandText = cmd;
            //
            adapter = new SqlDataAdapter(cmd, con);
            data_set = new DataSet();
            adapter.Fill(data_set);
            dataGridView1.DataSource = data_set.Tables[0];
            label8.Text = dataGridView1.RowCount.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //select Products.* from Products where (Name like N'%Тетрадка%' or Code like N'%4820039294951%') 
            //dataGridView1.Rows.Clear();
            string cmd = @"select Products.* from Products where (Name like N'%" + textBox1.Text + "%' or Code like N'" + textBox1.Text + "%') ";
            sql_cmd.Connection = con;
            sql_cmd.CommandText = cmd;
            //
            adapter = new SqlDataAdapter(cmd, con);
            data_set = new DataSet();
            adapter.Fill(data_set);
            dataGridView1.DataSource = data_set.Tables[0];
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Shop db = new Shop(connectionString);

            if (radioButton1.Checked)
            {
                Products products = new Products
                {
                    Name = textBox2.Text,
                    Code = textBox3.Text,
                    Description = textBox4.Text,
                    First_Price = Convert.ToDouble(textBox5.Text),
                    Second_Price = Convert.ToDouble(textBox7.Text),
                    Price_Kg = -1,
                    Count = Convert.ToInt32(textBox6.Text),
                    Count_Kg = -1
                };
                db.Products.InsertOnSubmit(products);
            }
            if(radioButton2.Checked)
            {
                Products products = new Products
                {
                    Name = textBox2.Text,
                    Code = textBox3.Text,
                    Description = textBox4.Text,
                    First_Price = -1,
                    Price_Kg = Convert.ToDouble(textBox5.Text),
                    Second_Price = Convert.ToDouble(textBox7.Text),
                    Count = -1,
                    Count_Kg = Convert.ToInt32(textBox6.Text)
                };
                db.Products.InsertOnSubmit(products);
            }
            if(radioButton1.Checked == false && radioButton2.Checked == false)
                MessageBox.Show("Вы не выбрали тип товара!");
            try
            {
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // Make some adjustments.
                // ...
                // Try again.
                db.SubmitChanges();
                MessageBox.Show(ex.ToString());
            }
            textBox2.Clear(); textBox3.Clear(); textBox4.Clear(); textBox5.Clear(); textBox6.Clear(); textBox7.Clear(); radioButton1.Checked = false; radioButton2.Checked = false;
        }
    }
}
