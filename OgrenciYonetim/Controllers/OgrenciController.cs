using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using OgrenciYonetim.Models; 
using System.Collections.Generic;

namespace OgrenciYonetim.Controllers
{
    public class OgrenciController : Controller
    {
        
        string baglantiAdresi = @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=OkulDB; Integrated Security=True";


        public IActionResult Index()
        {
           
            List<Ogrenci> liste = new List<Ogrenci>();

            using (SqlConnection baglan = new SqlConnection(baglantiAdresi))
            {
                baglan.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Ogrenciler", baglan);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read()) 
                {
                    
                    liste.Add(new Ogrenci
                    {
                        ID = (int)dr["ID"],
                        Ad = dr["Ad"].ToString(),
                        Soyad = dr["Soyad"].ToString(),
                        Bolum = dr["Bolum"].ToString()
                    });
                }
            }
            
            return View(liste);
        }

       
        [HttpGet]
        public IActionResult Ekle()
        {
           
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(Ogrenci ogr)
        {
            
            using (SqlConnection baglan = new SqlConnection(baglantiAdresi))
            {
                baglan.Open();
               
                string sorgu = "INSERT INTO Ogrenciler (Ad, Soyad, Bolum) VALUES (@ad, @soyad, @bolum)";
                SqlCommand cmd = new SqlCommand(sorgu, baglan);
                cmd.Parameters.AddWithValue("@ad", ogr.Ad);
                cmd.Parameters.AddWithValue("@soyad", ogr.Soyad);
                cmd.Parameters.AddWithValue("@bolum", ogr.Bolum);
                cmd.ExecuteNonQuery();
            }
            
            return RedirectToAction("Index");
        }

        
        public IActionResult Sil(int id)
        {
            
            using (SqlConnection baglan = new SqlConnection(baglantiAdresi))
            {
                baglan.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Ogrenciler WHERE ID=@id", baglan);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        
    }
}