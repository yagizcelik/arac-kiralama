using System;
using System.Collections.Generic;
using System.Text;

namespace AracKiralamaSistemiApp
{
    public class Arac
    {
        public int AracId { get; set; }
        public string Model { get; set; }
        public bool KiralamaDurumu { get; set; }

        public Arac(int aracId, string model)
        {
            AracId = aracId;
            Model = model;
            KiralamaDurumu = false;
        }
        public void AracDurumuGuncelle(bool durum)
        {
            KiralamaDurumu = durum;
        }

        public override string ToString()
        {
            return $"Araç ID: {AracId}, Model: {Model}, Durum: {(KiralamaDurumu ? "Kirada" : "Müsait")}";
        }
    }

    public class Musteri
    {
        public int MusteriId { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }

        public Musteri(int musteriId, string ad, string soyad)
        {
            MusteriId = musteriId;
            Ad = ad;
            Soyad = soyad;
        }

        public override string ToString()
        {
            return $"Müşteri ID: {MusteriId}, Ad: {Ad}, Soyad: {Soyad}";
        }
    }
    public class Kiralama
    {
        public int KiralamaId { get; set; }
        public Musteri Kiralayan { get; set; }
        public Arac KiralananArac { get; set; }
        public DateTime KiralamaTarihi { get; set; }
        public DateTime? IptalTarihi { get; set; }

        public Kiralama(int kiralamaId, Musteri musteri, Arac arac)
        {
            KiralamaId = kiralamaId;
            Kiralayan = musteri;
            KiralananArac = arac;
            KiralamaTarihi = DateTime.Now;
            IptalTarihi = null;
        }

        public override string ToString()
        {
            string durum = IptalTarihi == null ? "Aktif" : $"İptal Edildi ({IptalTarihi.Value})";
            return $"Kiralama ID: {KiralamaId}, {Kiralayan}, {KiralananArac}, Durum: {durum}";
        }
    }

    public class AracKiralamaSistemi
    {
        public List<Arac> AracListesi { get; set; }
        public List<Musteri> MusteriListesi { get; set; }
        public List<Kiralama> KiralamaListesi { get; set; }
        private int nextKiralamaId = 1;

        public AracKiralamaSistemi()
        {
            AracListesi = new List<Arac>();
            MusteriListesi = new List<Musteri>();
            KiralamaListesi = new List<Kiralama>();
        }

        public void KiralamaYap(int musteriId, int aracId)
        {
            Musteri musteri = MusteriListesi.Find(m => m.MusteriId == musteriId);
            Arac arac = AracListesi.Find(a => a.AracId == aracId);

            if (musteri == null)
            {
                Console.WriteLine("Müşteri bulunamadı.");
                return;
            }
            if (arac == null)
            {
                Console.WriteLine("Araç bulunamadı.");
                return;
            }
            if (arac.KiralamaDurumu)
            {
                Console.WriteLine("Araç şu anda kirada.");
                return;
            }

            Kiralama yeniKiralama = new Kiralama(nextKiralamaId++, musteri, arac);
            KiralamaListesi.Add(yeniKiralama);
            arac.AracDurumuGuncelle(true);

            Console.WriteLine("Kiralama işlemi başarılı:");
            Console.WriteLine(yeniKiralama);
        }

        public void KiralamaIptalEt(int kiralamaId)
        {
            Kiralama kiralama = KiralamaListesi.Find(k => k.KiralamaId == kiralamaId);
            if (kiralama == null)
            {
                Console.WriteLine("Kiralama işlemi bulunamadı.");
                return;
            }
            if (kiralama.IptalTarihi != null)
            {
                Console.WriteLine("Kiralama zaten iptal edilmiş.");
                return;
            }

            kiralama.IptalTarihi = DateTime.Now;
            kiralama.KiralananArac.AracDurumuGuncelle(false);

            Console.WriteLine("Kiralama iptal edildi:");
            Console.WriteLine(kiralama);
        }

        public void KiralamaBilgisi()
        {
            if (KiralamaListesi.Count == 0)
            {
                Console.WriteLine("Kiralama geçmişi boş.");
                return;
            }

            foreach (var kiralama in KiralamaListesi)
            {
                Console.WriteLine(kiralama);
            }
        }

        public void AracEkle(Arac arac)
        {
            AracListesi.Add(arac);
        }

        public void MusteriEkle(Musteri musteri)
        {
            MusteriListesi.Add(musteri);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PerformLogin();
            AracKiralamaSistemi sistem = new AracKiralamaSistemi();

            sistem.AracEkle(new Arac(1, "Mercedes Maybach"));
            sistem.AracEkle(new Arac(2, "BMW M5 cs"));
            sistem.AracEkle(new Arac(3, "Hyundai i30"));
            sistem.AracEkle(new Arac(4, "Audi Rs7"));
            sistem.AracEkle(new Arac(5, "Opel Corsa"));
            sistem.AracEkle(new Arac(6, "Audi Rs6"));

            sistem.MusteriEkle(new Musteri(1, "Yağız", "Çelik"));
            sistem.MusteriEkle(new Musteri(2, "Ahmet", "Doğruyol"));
            sistem.MusteriEkle(new Musteri(3, "Aylin", "Bozkurt"));

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n--- Araç Kiralama Sistemi ---");
                Console.WriteLine("1. Araç listesini görüntüle");
                Console.WriteLine("2. Müşteri listesini görüntüle");
                Console.WriteLine("3. Kiralama işlemi yap");
                Console.WriteLine("4. Kiralama iptal et");
                Console.WriteLine("5. Tüm kiralama geçmişini görüntüle");
                Console.WriteLine("6. Çıkış");
                Console.Write("Seçiminiz: ");
                string secim = Console.ReadLine();
                Console.WriteLine();

                switch (secim)
                {
                    case "1":
                        Console.WriteLine("Araç Listesi:");
                        foreach (var arac in sistem.AracListesi)
                        {
                            Console.WriteLine(arac);
                        }
                        break;
                    case "2":
                        Console.WriteLine("Müşteri Listesi:");
                        foreach (var musteri in sistem.MusteriListesi)
                        {
                            Console.WriteLine(musteri);
                        }
                        break;
                    case "3":
                        Console.Write("Müşteri ID giriniz: ");
                        if (!int.TryParse(Console.ReadLine(), out int musteriId))
                        {
                            Console.WriteLine("Geçersiz Müşteri ID.");
                            break;
                        }
                        Console.Write("Araç ID giriniz: ");
                        if (!int.TryParse(Console.ReadLine(), out int aracId))
                        {
                            Console.WriteLine("Geçersiz Araç ID.");
                            break;
                        }
                        sistem.KiralamaYap(musteriId, aracId);
                        break;
                    case "4":
                        Console.Write("İptal edilecek Kiralama ID giriniz: ");
                        if (!int.TryParse(Console.ReadLine(), out int kiralamaId))
                        {
                            Console.WriteLine("Geçersiz Kiralama ID.");
                            break;
                        }
                        sistem.KiralamaIptalEt(kiralamaId);
                        break;
                    case "5":
                        Console.WriteLine("Kiralama Geçmişi:");
                        sistem.KiralamaBilgisi();
                        break;
                    case "6":
                        Console.WriteLine("Sistemden çıkılıyor...");
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim, lütfen tekrar deneyin.");
                        break;
                }

                Console.WriteLine("\nDevam etmek için bir tuşa basınız...");
                Console.ReadKey();
            }
        }


        static void PerformLogin()
        {
            int maxAttempts = 3;
            int attempt = 0;
            bool isAuthenticated = false;

            while (attempt < maxAttempts && !isAuthenticated)
            {
                Console.Clear();
                WriteHeader();


                Console.Write("Kullanıcı Adı: ");
                string username = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(username))
                {
                    Console.WriteLine("Kullanıcı adı boş olamaz. Tekrar deneyiniz.\n");
                    attempt++;
                    Console.WriteLine("Devam etmek için bir tuşa basınız...");
                    Console.ReadKey();
                    continue;
                }

                Console.Write("Parola: ");
                string password = ReadPassword();
                if (string.IsNullOrEmpty(password))
                {
                    Console.WriteLine("Parola boş olamaz. Tekrar deneyiniz.\n");
                    attempt++;
                    Console.WriteLine("Devam etmek için bir tuşa basınız...");
                    Console.ReadKey();
                    continue;
                }

                if (username.Equals("yagiz", StringComparison.OrdinalIgnoreCase) && password == "yazoo")
                {
                    isAuthenticated = true;
                    Console.WriteLine("\nYönetici olarak giriş başarılı.");
                    Console.WriteLine("\nGiriş işlemi tamamlandı. Ana menüye yönlendiriliyorsunuz...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\nGiriş bilgileri hatalı. Lütfen tekrar deneyiniz.\n");
                    attempt++;
                    if (attempt < maxAttempts)
                    {
                        Console.WriteLine($"Kalan deneme hakkınız: {maxAttempts - attempt}\n");
                        Console.WriteLine("Devam etmek için bir tuşa basınız...");
                        Console.ReadKey();
                    }
                }
            }

            if (!isAuthenticated)
            {
                Console.WriteLine("Çok fazla hatalı giriş denemesi. Program sonlandırılıyor.");
                Environment.Exit(0);
            }
        }

        static void WriteHeader()
        {
            Console.WriteLine("***********************************************");
            Console.WriteLine("            Araç Kiralama Sistemi             ");
            Console.WriteLine("***********************************************\n");
        }


        static string ReadPassword()
        {
            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write("\b \b");
                    password.Remove(password.Length - 1, 1);
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    Console.Write("*");
                    password.Append(key.KeyChar);
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return password.ToString();
        }
    }
}