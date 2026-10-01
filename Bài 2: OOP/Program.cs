using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP
{
    public abstract class PhuongTien
    {
        private string _maPT;
        private string _tenHang;
        private int _namSanXuat;
        private decimal _giaGoc;

        public string MaPT
        {
            get { return _maPT; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _maPT = "PT000";
                else
                    _maPT = value.Trim();
            }
        }

        public string TenHang
        {
            get { return _tenHang; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tên hãng không được để trống!");
                _tenHang = value.Trim();
            }
        }

        public int NamSanXuat
        {
            get { return _namSanXuat; }
            set
            {
                if (value < 1900 || value > DateTime.Now.Year)
                    throw new ArgumentException("Năm sản xuất không hợp lệ!");
                _namSanXuat = value;
            }
        }

        public decimal GiaGoc
        {
            get { return _giaGoc; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Giá gốc phải lớn hơn 0!");
                _giaGoc = value;
            }
        }

        public PhuongTien(string maPT, string tenHang, int namSanXuat, decimal giaGoc)
        {
            MaPT = maPT;
            TenHang = tenHang;
            NamSanXuat = namSanXuat;
            GiaGoc = giaGoc;
        }

        public abstract decimal TinhGiaLanBanh();

        public virtual string GetInfo()
        {
            return $"[{MaPT}] {TenHang} | Năm SX: {NamSanXuat} | Giá gốc: {GiaGoc:N0} VNĐ";
        }
    }

    // LỚP OTo
    public class OTo : PhuongTien
    {
        private int _soChoNgoi;
        private double _dungTichDongCo;

        public int SoChoNgoi
        {
            get { return _soChoNgoi; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Số chỗ ngồi phải lớn hơn 0!");
                _soChoNgoi = value;
            }
        }

        public double DungTichDongCo
        {
            get { return _dungTichDongCo; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Dung tích động cơ phải lớn hơn 0!");
                _dungTichDongCo = value;
            }
        }

        public OTo(string maPT, string tenHang, int namSanXuat, decimal giaGoc,
                   int soChoNgoi, double dungTichDongCo)
            : base(maPT, tenHang, namSanXuat, giaGoc)
        {
            SoChoNgoi = soChoNgoi;
            DungTichDongCo = dungTichDongCo;
        }

        public override decimal TinhGiaLanBanh()
        {
            if (SoChoNgoi <= 9)
            {
                return GiaGoc + GiaGoc * 0.12m + GiaGoc * 0.30m;
            }
            else
            {
                return GiaGoc + GiaGoc * 0.10m;
            }
        }

        public override string GetInfo()
        {
            return "Ô tô   | " + base.GetInfo()
                 + $" | {SoChoNgoi} chỗ | Động cơ: {DungTichDongCo}L";
        }
    }

    // LỚP XeMay
    public class XeMay : PhuongTien
    {
        private int _dungTichXylanh;

        public int DungTichXylanh
        {
            get { return _dungTichXylanh; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Dung tích xy-lanh phải lớn hơn 0!");
                _dungTichXylanh = value;
            }
        }

        public XeMay(string maPT, string tenHang, int namSanXuat, decimal giaGoc,
                     int dungTichXylanh)
            : base(maPT, tenHang, namSanXuat, giaGoc)
        {
            DungTichXylanh = dungTichXylanh;
        }

        public override decimal TinhGiaLanBanh()
        {
            if (DungTichXylanh < 175)
                return GiaGoc + GiaGoc * 0.02m;   // trước bạ 2%
            else
                return GiaGoc + GiaGoc * 0.05m;   // trước bạ 5%
        }

        public override string GetInfo()
        {
            return "Xe máy | " + base.GetInfo() + $" | {DungTichXylanh}cc";
        }
    }
    // LỚP QUẢN LÝ
    public class QuanLyPhuongTien
    {
        private List<PhuongTien> _danhSach = new List<PhuongTien>();
        public void AddPhuongTien(PhuongTien pt)
        {
            if (pt == null)
                throw new ArgumentNullException(nameof(pt));
            _danhSach.Add(pt);
        }
        public void DisplayAll()
        {
            if (_danhSach.Count == 0)
            {
                Console.WriteLine("Danh sách trống.");
                return;
            }

            foreach (PhuongTien pt in _danhSach)
            {
                Console.WriteLine(pt.GetInfo());
                Console.WriteLine($"=> Giá lăn bánh: {pt.TinhGiaLanBanh():N0} VNĐ");
            }
        }

        public PhuongTien FindMaxGiaLanBanh()
        {
            if (_danhSach.Count == 0)
                return null;

            PhuongTien max = _danhSach[0];
            foreach (PhuongTien pt in _danhSach)
            {
                if (pt.TinhGiaLanBanh() > max.TinhGiaLanBanh())
                    max = pt;
            }
            return max;
        }

        public List<PhuongTien> SearchByName(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<PhuongTien>();

            return _danhSach
                .Where(p => p.TenHang.IndexOf(keyword.Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            //TC01 
            Console.WriteLine("--- TC01: Validation Năm sản xuất ---");
            try
            {
                new OTo("OT99", "Ford", 1850, 500000000m, 5, 2.0);
                Console.WriteLine("FAIL - Vẫn tạo được đối tượng!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"PASS - Bắt được lỗi: {ex.Message}");
            }

            //TC02
            Console.WriteLine("\n--- TC02: Giá lăn bánh Ô tô 5 chỗ ---");
            OTo oto = new OTo("OT01", "Toyota", 2024, 1000000000m, 5, 2.5);
            decimal giaOto = oto.TinhGiaLanBanh();
            Console.WriteLine($"Giá lăn bánh: {giaOto:N0} VNĐ");
            Console.WriteLine(giaOto == 1420000000m ? "PASS" : "FAIL");

            //TC03
            Console.WriteLine("\n--- TC03: Giá lăn bánh Xe máy 150cc ---");
            XeMay xeMay = new XeMay("XM01", "Honda", 2023, 50000000m, 150);
            decimal giaXe = xeMay.TinhGiaLanBanh();
            Console.WriteLine($"Giá lăn bánh: {giaXe:N0} VNĐ");
            Console.WriteLine(giaXe == 51000000m ? "PASS" : "FAIL");

            //TC04
            Console.WriteLine("\n--- TC04: Đa hình với List ---");
            QuanLyPhuongTien ql = new QuanLyPhuongTien();
            ql.AddPhuongTien(oto);
            ql.AddPhuongTien(xeMay);
            ql.DisplayAll();

            //TC05
            Console.WriteLine("\n--- TC05: Tìm giá lăn bánh cao nhất ---");
            PhuongTien max = ql.FindMaxGiaLanBanh();
            Console.WriteLine(max.GetInfo());
            Console.WriteLine($"=> Giá lăn bánh: {max.TinhGiaLanBanh():N0} VNĐ");
            Console.WriteLine(max == oto ? "PASS" : "FAIL");

            Console.WriteLine("\n--- Thêm: SearchByName(\"honda\") ---");
            foreach (PhuongTien pt in ql.SearchByName("honda"))
                Console.WriteLine(pt.GetInfo());

            Console.WriteLine("\nNhấn phím bất kỳ để thoát...");
            Console.ReadKey();
        }
    }
}
