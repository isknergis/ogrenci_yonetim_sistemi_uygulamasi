using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;


namespace ogrencikayitsistemi
{
    public partial class Form1 : Form
    {

        private NpgsqlConnection conn;

        public Form1()
        {
            InitializeComponent();
            conn = new NpgsqlConnection("Host = localhost; Port = 5432; Username = postgres; Password = nergis; Database = kutuphane; ");

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
                RefreshOgrenciListesi();
            

        }

        private void RefreshOgrenciListesi()
        { //Form her yüklendiğinde öğrenci listesinin güncellenmesini sağlıyor

            try
            {
                conn.Open();
                string query = "SELECT ad, soyad, email, kayit_tarihi, dogum_tarihi, bolum , ogrenci_no FROM ogrenciler";
                NpgsqlCommand command = new NpgsqlCommand(query, conn);
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                listBoxOgrenciler.Items.Clear();

                foreach (DataRow row in dataTable.Rows)
                {
                    // Her bir satırı ListBox'a ekliyoruz
                    string ogrenciBilgisi = $"{row["ad"]} {row["soyad"]} - {row["email"]} -" +
                        $" Kayıt Tarihi: {((DateTime)row["kayit_tarihi"]).ToString("yyyy-MM-dd")} -" +
                        $" Doğum Tarihi: {((DateTime)row["dogum_tarihi"]).ToString("yyyy-MM-dd")}  - " +
                        $"Bölümü: {row["bolum"]} - Öğrenci No: {row["ogrenci_no"]}";

                    listBoxOgrenciler.Items.Add(ogrenciBilgisi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            
            string ad = txtAd.Text;
            string soyad = txtSoyad.Text;
            string email = txtEmail.Text;
            DateTime dogumTarihi = dateTimePickerDogumTarihi.Value;
            string bolum = textbox_bolum.Text;
            string ogrenci_no =textbox_ogrencino.Text;


            try
            {
                conn.Open();
                string query = "INSERT INTO ogrenciler (ad, soyad, email, dogum_tarihi, bolum , ogrenci_no) VALUES (@ad, @soyad, @email, @dogum_tarihi , @bolum , @ogrenci_no)";
                NpgsqlCommand command = new NpgsqlCommand(query, conn);
                command.Parameters.AddWithValue("@ad", ad);
                command.Parameters.AddWithValue("@soyad", soyad);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@dogum_tarihi", dogumTarihi);
                command.Parameters.AddWithValue("@bolum", bolum);
                command.Parameters.AddWithValue("@ogrenci_no", ogrenci_no);

                int rowsAffected = command.ExecuteNonQuery(); // sorguyu çalıştır, etkilenen satır sayısını alır.
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Öğrenci başarıyla kaydedildi.");
                    RefreshOgrenciListesi();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
    }

