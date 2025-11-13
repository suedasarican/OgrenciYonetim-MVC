using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using OgrenciYonetim.Models; // Modelimizi buraya çağırdık
using System.Collections.Generic;

namespace OgrenciYonetim.Controllers
{
    public class OgrenciController : Controller
    {
        // Veritabanı adresimiz (LocalDB)
        string baglantiAdresi = @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=OkulDB; Integrated Security=True";

        // === 1. LİSTELEME İŞLEMİ (READ) ===
        public IActionResult Index()
        {
            // Öğrencileri içine dolduracağımız boş bir liste yapıyoruz
            List<Ogrenci> liste = new List<Ogrenci>();

            using (SqlConnection baglan = new SqlConnection(baglantiAdresi))
            {
                baglan.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Ogrenciler", baglan);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read()) // Satır satır oku
                {
                    // Her satırı bir Öğrenci nesnesine dönüştür ve listeye ekle
                    liste.Add(new Ogrenci
                    {
                        ID = (int)dr["ID"],
                        Ad = dr["Ad"].ToString(),
                        Soyad = dr["Soyad"].ToString(),
                        Bolum = dr["Bolum"].ToString()
                    });
                }
            }
            // IActionResult Dönüşü: "Bu listeyi al ve Index.cshtml sayfasına götür"
            return View(liste);
        }

        // === 2. EKLEME SAYFASINI GÖSTERME (GET) ===
        [HttpGet]
        public IActionResult Ekle()
        {
            // Sadece boş formu gösterir
            return View();
        }

        // === 3. EKLEME İŞLEMİNİ YAPMA (POST) ===
        [HttpPost]
        public IActionResult Ekle(Ogrenci ogr)
        {
            // Formdan gelen veriler 'ogr' nesnesinin içinde gelir
            using (SqlConnection baglan = new SqlConnection(baglantiAdresi))
            {
                baglan.Open();
                // Parametreli INSERT sorgusu (Güvenli yöntem)
                string sorgu = "INSERT INTO Ogrenciler (Ad, Soyad, Bolum) VALUES (@ad, @soyad, @bolum)";
                SqlCommand cmd = new SqlCommand(sorgu, baglan);
                cmd.Parameters.AddWithValue("@ad", ogr.Ad);
                cmd.Parameters.AddWithValue("@soyad", ogr.Soyad);
                cmd.Parameters.AddWithValue("@bolum", ogr.Bolum);
                cmd.ExecuteNonQuery();
            }
            // İş bitince listeye geri yönlendir
            return RedirectToAction("Index");
        }

        // === 4. SİLME İŞLEMİ ===
        public IActionResult Sil(int id)
        {
            // URL'den gelen ID'yi alıp siliyoruz
            using (SqlConnection baglan = new SqlConnection(baglantiAdresi))
            {
                baglan.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Ogrenciler WHERE ID=@id", baglan);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // Düzenleme kısmını da aynı mantıkla aşağıda View oluştururken anlatacağım...
    }
}