using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace MyLibraryTuto
{
    public partial class BookTbl : Form
    {
        public BookTbl()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mohan\Documents\Mylibrarydb.mdf;Integrated Security=True;Connect Timeout=30");
        private void BookTbl_Load(object sender, EventArgs e)
        {
            populate();
        }
        public void populate()
        {
            Con.Open();
            string query = "select * from BookTbl";
            SqlDataAdapter da = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(da);
            var ds = new DataSet();
            da.Fill(ds);
            BookDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
         if (bookname.Text == "" || author.Text == ""|| publisher.Text == "" || price.Text =="" || Qty.Text=="")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("insert into BookTbl values('" + bookname.Text + "','" + author.Text + "','" + publisher.Text + "'," + price.Text + "," + Qty.Text + ")", Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Book Successfully Added Successfully");
                Con.Close();
               populate();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (bookname.Text == "")
            {
                MessageBox.Show("Enter The Book Name");
            }
            else
            {
                Con.Open();
                string query = "delete from BookTbl where BookName = '" + bookname.Text + "';";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Book Successfully Deleted");
                Con.Close();
                populate();
            }
        }

        private void BookDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           bookname.Text = BookDGV.SelectedRows[0].Cells[0].Value.ToString();
            author.Text = BookDGV.SelectedRows[0].Cells[1].Value.ToString();
            publisher.Text = BookDGV.SelectedRows[0].Cells[2].Value.ToString();
            price.Text = BookDGV.SelectedRows[0].Cells[3].Value.ToString();
            Qty.Text = BookDGV.SelectedRows[0].Cells[4].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (bookname.Text == "" || author.Text == "" || publisher.Text == "" || price.Text == "" || Qty.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                Con.Open();
                string query = "update BookTbl set Author='" + author.Text + "',Publisher='" +publisher.Text + "',Price=" + price.Text+ ",Qty ="+Qty.Text+" where BookName ='" + bookname.Text + "';";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Book Successfully Updated");
                Con.Close();
                populate();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm main = new MainForm();
            main.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
