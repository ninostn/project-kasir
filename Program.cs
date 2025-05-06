using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace kasir
{
    public class Barang
    {
        public string Nama { get; set; }
        public int Harga { get; set; }
        public int Stok { get; set; }
    }

    public class User
    {
        public string Nama { get; set; }
        public string Password { get; set; }
        public int Saldo { get; set; }
    }

    public static class kasir
    {
        public static List<Barang> daftarBarang = new List<Barang>();
        public static List<User> daftarUser = new List<User>();
        public static int adminSaldo = 0;

        public static void Main()
        {
            while (true)
            {
                string user = "";
                string password = "";
                bool isLogin = false;
                bool isAdmin = false;
                bool isUser = false;

                while (!isLogin)
                {
                    Console.WriteLine("====================================");
                    Console.WriteLine("||          HALAMAN MASUK         ||");
                    Console.WriteLine("====================================\n");
                    Console.WriteLine("[1] Login\n[2] Register");
                    Console.Write("\nPilih Menu: ");
                    string pilihan = Console.ReadLine();
                    switch (pilihan)
                    {
                        case "1":
                            Console.Clear();
                            Console.WriteLine("====================================");
                            Console.WriteLine("||              LOGIN             ||");
                            Console.WriteLine("====================================\n");
                            Console.Write("Username: ");
                            user = Console.ReadLine();
                            Console.Write("Password: ");
                            password = Console.ReadLine();
                            if (user == "admin" && password == "admin")
                            {
                                Console.WriteLine("\nLogin Berhasil!");
                                Thread.Sleep(1000);
                                isAdmin = true;
                                isLogin = true;
                            }
                            else
                            {
                                var akun = daftarUser.FirstOrDefault(u => u.Nama == user && u.Password == password);
                                if (akun != null)
                                {
                                    Console.WriteLine("\nLogin Berhasil.");
                                    isLogin = true;
                                    isUser = true;
                                    Thread.Sleep(1000);
                                }
                                else
                                {
                                    Console.WriteLine("\nUsername atau password salah.");
                                    Thread.Sleep(1000);
                                }
                            }
                            Console.Clear();
                            break;

                        case "2":
                            Console.Clear();
                            Console.WriteLine("====================================");
                            Console.WriteLine("||             REGISTER           ||");
                            Console.WriteLine("====================================\n");
                            Console.Write("Username: ");
                            user = Console.ReadLine();
                            Console.Write("Password: ");
                            password = Console.ReadLine();
                            bool userAda = daftarUser.Exists(u => u.Nama == user);

                            if (userAda)
                            {
                                Console.WriteLine("\nNama pengguna sudah digunakan.");
                                Console.WriteLine("Tekan enter untuk kembali...");
                                Console.ReadLine();
                                Console.Clear();
                                break;
                            }
                            else
                            {
                                daftarUser.Add(new User { Nama = user, Password = password, Saldo = 0 });
                                Console.WriteLine("\nRegistrasi berhasil.");
                                Thread.Sleep(1000);
                                Console.Clear();
                                isLogin = true;
                                isUser = true;
                            }
                            break;

                        default:
                            Console.WriteLine("Pilihan tidak valid");
                            Thread.Sleep(900);
                            Console.Clear();
                            break;
                    }
                }

                while (isUser)
                {
                    var akun = daftarUser.FirstOrDefault(u => u.Nama == user && u.Password == password);
                    Console.Clear();
                    Console.WriteLine("====================================");
                    Console.WriteLine("||         MENU PENGGUNA          ||");
                    Console.WriteLine("====================================");
                    Console.WriteLine($"Nama Pengguna: {user}");
                    Console.WriteLine($"Saldo: Rp {akun.Saldo}\n");
                    Console.WriteLine("[1] Beli Barang");
                    Console.WriteLine("[2] Logout");
                    Console.Write("\nPilih menu: ");
                    string pilihan = Console.ReadLine();

                    switch (pilihan)
                    {
                        case "1":
                            if (daftarBarang.Count == 0)
                            {
                                Console.WriteLine("\nDAFTAR BARANG:\n");
                                Console.WriteLine("Belum ada barang yang ditambahkan.");
                                Console.WriteLine("Tekan enter untuk kembali...");
                                Console.ReadLine();
                                break;
                            }
                            else
                            {
                                for (int i = 0; i < daftarBarang.Count; i++)
                                {
                                    var item = daftarBarang[i];
                                    Console.WriteLine($"[{i + 1}] {item.Nama} - Harga: {item.Harga} - Stock {item.Stok}");
                                }
                            }
                            Console.WriteLine("\nMasukan nama barang yang ingin dibeli: ");
                            string namaBarang = Console.ReadLine();
                            var barangs = daftarBarang.FirstOrDefault(b => b.Nama.Equals(namaBarang, StringComparison.OrdinalIgnoreCase));
                            if (barangs != null)
                            {
                                Console.Write("Jumlah barang yang ingin dibeli: ");
                                if (int.TryParse(Console.ReadLine(), out int jumlah) && jumlah > 0)
                                {
                                    if (jumlah <= barangs.Stok)
                                    {
                                        int totalHarga = barangs.Harga * jumlah;
                                        if (akun.Saldo >= totalHarga)
                                        {
                                            akun.Saldo -= totalHarga;
                                            barangs.Stok -= jumlah;
                                            adminSaldo += totalHarga;
                                            Console.WriteLine($"Pembelian berhasil! Total harga: Rp {totalHarga}");
                                            Console.WriteLine($"Sisa saldo Anda: Rp {akun.Saldo}");
                                            Console.WriteLine($"Sisa stok {barangs.Nama}: {barangs.Stok}");   
                                            Console.WriteLine("Tekan enter untuk kembali...");
                                            Console.ReadLine();     
                                            Thread.Sleep(1000);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Saldo tidak cukup.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Stok tidak mencukupi.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Input tidak valid.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Barang tidak ditemukan.");
                            }
                            break;

                        case "2":
                            isLogin = false;
                            isUser = false;
                            isAdmin = false;
                            Console.WriteLine("Logout berhasil.");
                            Thread.Sleep(1000);
                            Console.Clear();
                            break;

                        default:
                            Console.WriteLine("Pilihan Tidak Valid.");
                            Thread.Sleep(900);
                            break;
                    }
                }

                while (isAdmin)
                {
                    Console.Clear();
                    Console.WriteLine("====================================");
                    Console.WriteLine("||           MENU ADMIN           ||");
                    Console.WriteLine("====================================");
                    Console.WriteLine($"Saldo Toko : Rp {adminSaldo}\n");
                    Console.WriteLine("[1] Tambah Barang");
                    Console.WriteLine("[2] Lihat Daftar Barang");
                    Console.WriteLine("[3] Tambah Saldo user");
                    Console.WriteLine("[4] Tarik Saldo Toko");
                    Console.WriteLine("[5] Logout");
                    Console.Write("\nPilih menu: ");
                    string pilihan = Console.ReadLine();

                    switch (pilihan)
                    {
                        case "1":
                            Console.Write("Nama barang: ");
                            string nama = Console.ReadLine();
                            Console.Write("Harga barang: ");
                            int harga = int.Parse(Console.ReadLine());
                            Console.Write("Jumlah stok: ");
                            int stok = int.Parse(Console.ReadLine());

                            daftarBarang.Add(new Barang { Nama = nama, Harga = harga, Stok = stok });
                            Console.WriteLine("Barang berhasil ditambahkan!");
                            Thread.Sleep(1000);
                            break;

                        case "2":
                            Console.WriteLine("\nDAFTAR BARANG:\n");
                            if (daftarBarang.Count == 0)
                            {
                                Console.WriteLine("Belum ada barang.");
                            }
                            else
                            {
                                for (int i = 0; i < daftarBarang.Count; i++)
                                {
                                    var item = daftarBarang[i];
                                    Console.WriteLine($"[{i + 1}] {item.Nama} - Rp {item.Harga} - Stok: {item.Stok}");
                                }
                            }
                            Console.WriteLine("Tekan Enter untuk kembali...");
                            Console.ReadLine();
                            break;
                        case "3":
                            if (daftarUser.Count == 0)
                            {
                                Console.WriteLine("\nBelum ada pengguna.");
                                Console.WriteLine("Tekan enter untuk kembali...");
                                Console.ReadLine();
                                break;
                            }

                            Console.WriteLine("\nDAFTAR USER:");
                            for (int i = 0; i < daftarUser.Count; i++)
                            {
                                var us = daftarUser[i];
                                Console.WriteLine($"[{i + 1}] Nama User: {us.Nama} - Saldo: {us.Saldo}");
                            }

                            Console.Write("\nMasukkan nama pengguna yang ingin ditambah saldonya: ");
                            string addNama = Console.ReadLine();
                            var userList = daftarUser.FirstOrDefault(u => u.Nama.Equals(addNama, StringComparison.OrdinalIgnoreCase));

                            if (userList != null)
                            {
                                Console.Write("Masukkan jumlah saldo yang ingin ditambahkan: ");
                                if (int.TryParse(Console.ReadLine(), out int addSaldo) && addSaldo > 0)
                                {
                                    userList.Saldo += addSaldo;
                                    Console.WriteLine($"Saldo berhasil ditambahkan. Saldo {userList.Nama}: Rp {userList.Saldo}");
                                }
                                else
                                {
                                    Console.WriteLine("Input tidak valid.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Pengguna tidak ditemukan.");
                            }

                            Console.WriteLine("Tekan enter untuk kembali...");
                            Console.ReadLine();
                            break;

                        case "5":
                            isLogin = false;
                            isUser = false;
                            isAdmin = false;
                            Console.WriteLine("Logout berhasil.");
                            Thread.Sleep(1000);
                            Console.Clear();
                            break;

                        default:
                            Console.WriteLine("Pilihan tidak valid!");
                            Thread.Sleep(900);
                            break;
                    }
                }
            }
        }
    }
}