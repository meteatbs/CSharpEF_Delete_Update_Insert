using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EntityFrameworkGridApp
{
    public partial class Form1 : Form
    {
        Customer model = new Customer();
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btniptal_Click(object sender, EventArgs e)
        {
            Clear();
        }
        void Clear()
        {
            txtisim.Text = txtSoyadi.Text = txtSehir.Text = txtAdres.Text = "";
            btnKaydet.Text = "Save";
            btniptal.Enabled = false;
            model.CustomerID = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            PopulateDataGridView();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            model.FirstName = txtisim.Text.Trim();
            model.LastName = txtSoyadi.Text.Trim();
            model.City = txtSehir.Text.Trim();
            model.Address = txtAdres.Text.Trim();
            using (DBEntities db = new DBEntities())
            {
                if (model.CustomerID == 0)//insert
                    db.Customers.Add(model);
                else//update
                    db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();                          
            }
            Clear();
            PopulateDataGridView();
            MessageBox.Show("Basariyla Kaydedildi");
        }
        void PopulateDataGridView()
        {
            dgvMusteri.AutoGenerateColumns = false;
             using(DBEntities db=new DBEntities())
            {
                dgvMusteri.DataSource = db.Customers.ToList<Customer>();
            }
        }

        private void dgvMusteri_DoubleClick(object sender, EventArgs e)
        {
            if(dgvMusteri.CurrentRow.Index != -1)
            {
                model.CustomerID = Convert.ToInt32(dgvMusteri.CurrentRow.Cells["CustomerID"].Value);
                 using(DBEntities db=new DBEntities())
                {
                    model = db.Customers.Where(x => x.CustomerID == model.CustomerID).FirstOrDefault();
                    txtisim.Text = model.FirstName;
                    txtSoyadi.Text = model.LastName;
                    txtSehir.Text = model.City;
                    txtAdres.Text = model.Address;
                }
                btnKaydet.Text = "Guncellendi";
                btnSil.Enabled = true;
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Bu kayıdı silmek istediğinizden emin misiniz?","EF Crud Operation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            using(DBEntities db=new DBEntities())
            {
                    var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                        db.Customers.Attach(model);
                    db.Customers.Remove(model);
                    db.SaveChanges();
                    PopulateDataGridView();
                    Clear();
                    MessageBox.Show("Basariyla Silindi");
            }
        }
    }
}
