using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Linq;
using System.Data.Linq.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace _1C_Shop
{
    public partial class Form1 : Form
    {
        public double to_pay;
        public string user_name;
        public Form1()
        {
            InitializeComponent();
            to_pay = 0;
            user_name = "";
        }

        [Table(Name = "Users")]
        public class Users
        {
            [Column(IsPrimaryKey = true, IsDbGenerated = true)]
            public int Id { get; set; }
            [Column(Name = "name")]
            public string Name { get; set; }
            [Column(Name = "password")]
            public string Password { get; set; }
            [Column(Name = "isroot")]
            public int IsRoot { get; set; }
        }

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
            [Column(Name = "Second_Price")]
            public double Second_Price { get; set; }
            [Column(Name = "Price_Kg")]
            public double Price_Kg { get; set; }
            [Column(Name = "Count")]
            public int Count { get; set; }
            [Column(Name = "Count_Kg")]
            public double Count_Kg { get; set; }
        }

        [Table(Name = "History")]
        public class History
        {
            [Column(IsPrimaryKey = true, IsDbGenerated = true)]
            public int Id { get; set; }
            [Column(Name = "date")]
            public DateTime date { get; set; }
            [Column(Name = "product_and_price")]
            public string product_and_price { get; set; }
            [Column(Name = "total_paid")]
            public double total_paid { get; set; }
            [Column(Name = "was_given")]
            public double was_given { get; set; }
            [Column(Name = "change")]
            public double change { get; set; }
            [Column(Name = "payment_method")]
            public string payment_method { get; set; }
            [Column(Name = "by_user")]
            public string by_user { get; set; }
            [Column(Name = "time")]
            public DateTime time { get; set; }
        }

        static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Kreal\source\repos\1C Shop\1C Shop\Database.mdf;Integrated Security=True";
        private void login_Click(object sender, EventArgs e)
        {
            string password = textBox5.Text;
            DataContext db = new DataContext(connectionString);
            if (password != string.Empty)
            {
                var query1 =
                  from d in db.GetTable<Users>()
                  where d.Password == password
                  select new { pass = d.Password, name = d.Name, isRoot = d.IsRoot };

                foreach (var q1 in query1)
                {
                    label2.Text = q1.name;
                    this.Text = "1C Магазин / Пользователь: " + q1.name;  // Можно писать просто Text.
                    user_name = q1.name;
                    textBox5.Clear();
                    Button_Options_On(q1.name);
                }
            }
            else
            {
                MessageBox.Show("Введите пароль.");
            }
           
        }
        public void Button_Options_On(string Name)
        {
            if (Name == "root")  // Оптимизировать!
            {
                button2.Enabled = true;
                button5.Enabled = true;
                //button6.Enabled = true;
                button7.Enabled = true;
                textBox1.Enabled = true;
                textBox3.Enabled = true;
                button9.Enabled = true;
                checkBox1.Enabled = true;
                button10.Enabled = true;
                button1.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
                button5.Enabled = false;
                //button6.Enabled = false;
                button7.Enabled = false;
                //
                textBox1.Enabled = true;
                textBox3.Enabled = true;
                //
                button9.Enabled = true;
                checkBox1.Enabled = true;
                button10.Enabled = true;
            }
            textBox3.Focus();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox5.Focus();
        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            DataContext db = new DataContext(connectionString);
            if (textBox3.TextLength == 13)
            {
                var query1 =
                 from d in db.GetTable<Products>()
                 where d.Code == textBox3.Text
                 select new { name = d.Name, code = d.Code, desc = d.Description, price = d.Second_Price, price_kg = d.Price_Kg, count = d.Count, count_kg = d.Count_Kg };

                foreach (var q1 in query1)
                {
                    if (q1.price_kg == -1)
                    {
                        listBox1.Items.Add(q1.code); listBox2.Items.Add(q1.name); listBox3.Items.Add(q1.price + " грн."); listBox4.Items.Add(q1.desc);
                        to_pay += q1.price;
                    }
                    if (q1.price_kg != -1)
                    {
                        var str = Interaction.InputBox("Введите окончательный вес продукта В ГРАММАХ:", "Ввод веса.", "100", -1, -1);
                        var pay_for = q1.price * (Convert.ToDouble(str) / 1000);
                        listBox1.Items.Add(q1.code); listBox2.Items.Add(q1.name); listBox3.Items.Add(pay_for + " грн."); listBox4.Items.Add(q1.desc + " (Вес: " + str + " грамм.)");

                        to_pay += pay_for;
                    }
                }
                textBox3.Clear();
                groupBox4.Text = "К оплате: " + to_pay;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear(); listBox2.Items.Clear(); listBox3.Items.Clear(); listBox4.Items.Clear();
            textBox1.Clear(); textBox2.Clear();
            to_pay = 0;
            groupBox4.Text = "К оплате:";
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                double remainder;
                if (textBox1.Text != string.Empty)
                {
                    if (to_pay < Convert.ToDouble(textBox1.Text))
                    {
                        remainder = Convert.ToDouble(textBox1.Text) - to_pay;
                        textBox2.Text = remainder.ToString();
                    }
                    else
                        textBox2.Clear();
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox1.Enabled == true)
                textBox1.Enabled = false;
            else
                textBox1.Enabled = true;
            textBox1.Clear(); textBox2.Clear();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        public partial class Shop : DataContext
        {
            public Table<History> Histories;
            public Shop(string connection) : base(connection) {  }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count != 0 && listBox2.Items.Count != 0 && listBox3.Items.Count != 0 && listBox4.Items.Count != 0)
            {
                string temp = "";
                listBox1.SelectionMode = SelectionMode.MultiSimple;
                listBox2.SelectionMode = SelectionMode.MultiSimple;
                listBox3.SelectionMode = SelectionMode.MultiSimple;
                listBox4.SelectionMode = SelectionMode.MultiSimple;
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    listBox1.SetSelected(i, true);
                    temp += listBox1.SelectedItem.ToString() + " ";
                    listBox2.SetSelected(i, true);
                    temp += listBox2.SelectedItem.ToString() + " ";
                    listBox3.SetSelected(i, true);
                    temp += listBox3.SelectedItem.ToString() + " ";
                    listBox4.SetSelected(i, true);
                    temp += listBox4.SelectedItem.ToString() + " ";
                }
                Shop db = new Shop(connectionString);
                string pmtd_str;
                if (checkBox1.Checked)
                {    
                    pmtd_str = "картой";
                    //
                    History history = new History
                    {
                        date = DateTime.Today,
                        product_and_price = temp,
                        total_paid = to_pay,
                        was_given = to_pay,
                        change = 0,
                        payment_method = pmtd_str,
                        by_user = user_name,
                        time = DateTime.Now
                    };
                    db.Histories.InsertOnSubmit(history);
                }
                else
                {
                    pmtd_str = "наличными";
                    //
                    if (textBox1.Text.Length != 0)
                    {
                        if (Convert.ToDouble(textBox1.Text) > to_pay)
                        {
                            History history = new History
                            {
                                date = DateTime.Today,
                                product_and_price = temp,
                                total_paid = to_pay,
                                was_given = Convert.ToDouble(textBox1.Text),
                                change = Convert.ToDouble(textBox2.Text),
                                payment_method = pmtd_str,
                                by_user = user_name,
                                time = DateTime.Now
                            };
                            db.Histories.InsertOnSubmit(history);
                        }
                        else
                        {
                            MessageBox.Show("Вам дали меньше,чем нужно заплатить!");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введите полученную сумму.");
                        return;
                    }
                }
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
                }
                list_lastoperations.Items.Add(DateTime.Now + " | " + "На сумму: " + to_pay + "грн. | " + "Оплата: " + pmtd_str);
                if(!checkBox1.Checked)
                    list_lastoperations.Items.Add("Дали: "+ textBox1.Text + "грн. | " + "Сдача: " + textBox2.Text + "грн.");
                listBox1.Items.Clear(); listBox2.Items.Clear(); listBox3.Items.Clear(); listBox4.Items.Clear();
                to_pay = 0;
                groupBox4.Text = "К оплате:";
                textBox1.Clear();textBox2.Clear();checkBox1.Checked = false;
            }
            else
            {
                MessageBox.Show("Выберите хотя-бы один товар.");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            checking_day chkng = new checking_day();
            chkng.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checking_products chkngd = new checking_products();
            chkngd.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            settings setts = new settings();
            setts.Show();
        }
    }
}
