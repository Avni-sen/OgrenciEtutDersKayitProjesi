using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ÖğrenciEtütDersKayitProjesi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection db = new SqlConnection(@"Data Source=DESKTOP-704QU67;Initial Catalog=DbEtutDersProje;Integrated Security=True");

        void dersListesi()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * from Tbl_Dersler", db);
            DataTable dt = new DataTable();
            da.Fill(dt);
            CmbDers.ValueMember = "DersId";
            CmbDers.DisplayMember = "DersAd";
            CmbDers.DataSource = dt;
            
        }
       

        void etutListesi()
        {
            SqlDataAdapter da3 = new SqlDataAdapter("execute Etut" , db);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            dataGridView1.DataSource = dt3;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            dersListesi();
            etutListesi();


        }

        private void CmbDers_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlDataAdapter da2 = new SqlDataAdapter("SELECT OgretmenId,(OgretmenAd +' '+OgretmenSoyad) AS ADSOYAD FROM Tbl_Ogretmen WHERE BransId=" + CmbDers.SelectedValue, db);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            CmbOgr.ValueMember = "OgretmenId";
            CmbOgr.DisplayMember = "ADSOYAD";
            CmbOgr.DataSource = dt2;

        }

        private void BtnEtutOlustur_Click(object sender, EventArgs e)
        {
            db.Open();
            SqlCommand komut = new SqlCommand("insert into Tbl_Etut (DersId,OgretmenId,Tarih,Saat) values (@p1,@p2,@p3,@p4)", db);
            komut.Parameters.AddWithValue("@p1", CmbDers.SelectedValue);
            komut.Parameters.AddWithValue("@p2", CmbOgr.SelectedValue);
            komut.Parameters.AddWithValue("@p3", MtxTarih.Text);
            komut.Parameters.AddWithValue("@p4", MtxSaat.Text);

            komut.ExecuteNonQuery();
            db.Close();
            MessageBox.Show("Etüt Kaydı Başarıyla Oluşturuldu.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            TxtEtutId.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
        }

        private void BtnEtutAl_Click(object sender, EventArgs e)
        {
            db.Open();
            SqlCommand komut2 = new SqlCommand("Update Tbl_Etut set OgrenciId=@p1,durum=@p2 where Id=@p3", db);
            komut2.Parameters.AddWithValue("@p1", TxtOgrenci.Text);
            komut2.Parameters.AddWithValue("@p2", "True");
            komut2.Parameters.AddWithValue("@p", TxtEtutId.Text);
            komut2.ExecuteNonQuery();
            db.Close();
            MessageBox.Show("Öğrenciye ders başarıyla tanımlandı.");
        }

        private void BtnFoto_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            pictureBox1.ImageLocation = openFileDialog1.FileName;
            TxtFotograf.Text = pictureBox1.ImageLocation.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            db.Open();
            SqlCommand komut3 = new SqlCommand("insert into Tbl_Ogrenci (OgrenciAd,OgrenciSoyad,Fotograf,Sınıf,Telefon,Mail) values (@p1,@p2,@p3,@p4,@p5,@p6)", db);
            komut3.Parameters.AddWithValue("@p1" ,TxtOgrenciAd.Text);
            komut3.Parameters.AddWithValue("@p2" ,TxtOgrenciSoyad.Text);
            komut3.Parameters.AddWithValue("@p3", pictureBox1.ImageLocation);
            komut3.Parameters.AddWithValue("@p4" ,TxtSınıf.Text);
            komut3.Parameters.AddWithValue("@p5" ,MtxTelefon.Text);
            komut3.Parameters.AddWithValue("@p6" ,TxtMail.Text);
            komut3.ExecuteNonQuery();
            db.Close();
            MessageBox.Show("Öğrenci Kaydı Başarıyla Tamamlandı.","Tamamlandı",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            db.Open();
            SqlCommand komut4 = new SqlCommand("Begin transaction insert into Tbl_Dersler(DersAd) Values(@D1) " +
                "insert into Tbl_Ogretmen(OgretmenAd,OgretmenSoyad,BransId,Telefon,Mail) values (@p1,@p2,(Select count(DersId) from Tbl_Ders),@p4,@p5) commit", db);
            komut4.Parameters.AddWithValue("@D1",TxtDers.Text);
            komut4.Parameters.AddWithValue("@p1",TxtOgretmenAd.Text);
            komut4.Parameters.AddWithValue("@p2",TxtOgretmenSoyad.Text);
            komut4.Parameters.AddWithValue("@p4",MtxTel.Text);
            komut4.Parameters.AddWithValue("@p5", TxtMailOgretmen.Text);
            komut4.ExecuteNonQuery();
            db.Close();
            MessageBox.Show("Öğrenci Kaydı Başarıyla Tamamlandı.", "Tamamlandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            db.Open();
            SqlCommand komut5 = new SqlCommand("insert into Tbl_Dersler (DersAd) values (@p1)", db);
            komut5.Parameters.AddWithValue("@p1", textBox1.Text);
            komut5.ExecuteNonQuery();
            db.Close();
            MessageBox.Show("Ders Kaydı Başarıyla Tamamlandı.", "Tamamlandı", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

       
    }
}
