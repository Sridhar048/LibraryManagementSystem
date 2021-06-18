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
    public partial class IssueBookForm : Form
    {
        public IssueBookForm()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mohan\Documents\Mylibrarydb.mdf;Integrated Security=True;Connect Timeout=30");
        private void FillStudent()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("select StdId from StudentTbl", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("StdId", typeof(int));
            dt.Load(rdr);
            StdCb.ValueMember = "StdId";
            StdCb.DataSource = dt;
            Con.Close();
        }
        private void FillBook()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("select BookName from BookTbl where Qty>"+0+"", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("BookName", typeof(string));
            dt.Load(rdr);
            Bookcb.ValueMember = "BookName";
            Bookcb.DataSource = dt;
            Con.Close();
        }
        public void populate()
        {
            Con.Open();
            string query = "select * from IssueTbl";
            SqlDataAdapter da = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(da);
            var ds = new DataSet();
            da.Fill(ds);
            IssueBookDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void fetchstddata()
        {
            Con.Open();
            string query = "select * from StudentTbl where StdId=" + StdCb.SelectedValue.ToString() + "";
            SqlCommand cmd = new SqlCommand(query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                stdnameTb.Text = dr["StdName"].ToString();
                stddpmntTb.Text = dr["StdDep"].ToString();
                PhoneTb.Text = dr["StdPhone"].ToString();
            }
            Con.Close();
        }
        private void UpdateBook()
        {
            int Qty,newQty;
            Con.Open();
            string query = "select * from BookTbl where BookName='" + Bookcb.SelectedValue.ToString() + "'";
            SqlCommand cmd = new SqlCommand(query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                 Qty = Convert.ToInt32(dr["Qty"].ToString());
                 newQty = Qty - 1;
                string query1 = "update BookTbl set Qty=" + newQty + " where BookName='" + Bookcb.SelectedValue.ToString() + "';";
                SqlCommand cmd1 = new SqlCommand(query1, Con);
                cmd1.ExecuteNonQuery();
            }
            Con.Close();
        }

        int cnt = 0;
        private void CountBooks()
        {
            Con.Open();
            string query2 = "select Count(*) from IssueTbl where StdId=" + StdCb.SelectedValue.ToString() + "";
            SqlCommand cmd2 = new SqlCommand(query2, Con);
            cnt = (int)cmd2.ExecuteScalar();
            Con.Close();

        }
       
        private void IssueBookForm_Load(object sender, EventArgs e)
        {
            FillStudent();
            FillBook();
            populate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm main = new MainForm();
            main.Show();
        }


        private void StdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            fetchstddata();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CountBooks();
            if (stdnameTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else if(cnt>2)
            {
                MessageBox.Show("NOT MORE THAN 3 BOOKS ARE ISSUED!");
            }
            else
            {
                string issuedate = IssueDate.Value.Month.ToString() + "/" + IssueDate.Value.Day.ToString() +"/"+ IssueDate.Value.Year.ToString();
                Con.Open();
                SqlCommand cmd = new SqlCommand("insert into IssueTbl values(" + StdCb.SelectedValue.ToString() + ",'" + stdnameTb.Text + "','" + stddpmntTb.Text + "','"+PhoneTb.Text+"','"+Bookcb.SelectedValue.ToString()+"','"+issuedate+"')", Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Book Successfully Issued");
                Con.Close();
                UpdateBook();
               populate();

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (IssueNumTb.Text == "")
            {
                MessageBox.Show("Enter The IssueNumber");
            }
            else
            {
                Con.Open();
                string query = "delete from IssueTbl where IssueNum = " + IssueNumTb.Text + ";";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Issue Successfully Canceled");
                Con.Close();
              //  UpdateBookCancellation();
                populate();
            }
        }
        private void IssueBookDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            IssueNumTb.Text = IssueBookDGV.SelectedRows[0].Cells[0].Value.ToString();
            StdCb.SelectedItem = IssueBookDGV.SelectedRows[0].Cells[1].Value.ToString();
            stdnameTb.Text = IssueBookDGV.SelectedRows[0].Cells[2].Value.ToString();
            stddpmntTb.Text = IssueBookDGV.SelectedRows[0].Cells[3].Value.ToString();
            PhoneTb.Text = IssueBookDGV.SelectedRows[0].Cells[4].Value.ToString();
            Bookcb.Text = IssueBookDGV.SelectedRows[0].Cells[5].Value.ToString();

        }



        

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        
    }
}
