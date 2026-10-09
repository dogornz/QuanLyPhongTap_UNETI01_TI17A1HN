// Họ và tên: [Điền họ và tên của bạn]
// Mã sinh viên: [Điền mã sinh viên của bạn]
// Nội dung thực hiện: Module 2 - Lớp phân trang dùng chung (Skip/Take trên IQueryable)
using Microsoft.EntityFrameworkCore;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Helpers
{
    public class PhanTrangDanhSach<T>
    {
        public IReadOnlyList<T> Items { get; }
        public int TrangHienTai { get; }
        public int KichThuocTrang { get; }
        public int TongSoMuc { get; }
        public int TongSoTrang { get; }

        public bool CoTrangTruoc => TrangHienTai > 1;
        public bool CoTrangSau => TrangHienTai < TongSoTrang;
        public int MucBatDau => TongSoMuc == 0 ? 0 : (TrangHienTai - 1) * KichThuocTrang + 1;
        public int MucKetThuc => TongSoMuc == 0 ? 0 : MucBatDau + Items.Count - 1;

        private PhanTrangDanhSach(IReadOnlyList<T> items, int tongSoMuc, int trang, int kichThuoc)
        {
            Items = items;
            TongSoMuc = tongSoMuc;
            TrangHienTai = trang;
            KichThuocTrang = kichThuoc;
            TongSoTrang = Math.Max(1, (int)Math.Ceiling(tongSoMuc / (double)kichThuoc));
        }

        /// <summary>
        /// Phân trang ngay trên SQL: CountAsync() đếm tổng, Skip() bỏ qua các trang trước, Take() lấy đúng 1 trang.
        /// Trang nhập sai (&lt;1 hoặc vượt trang cuối) được đưa về trang hợp lệ gần nhất.
        /// </summary>
        public static async Task<PhanTrangDanhSach<T>> TaoAsync(IQueryable<T> nguon, int trang, int kichThuoc)
        {
            if (kichThuoc < 1) kichThuoc = 1;

            int tongSoMuc = await nguon.CountAsync();
            int tongSoTrang = Math.Max(1, (int)Math.Ceiling(tongSoMuc / (double)kichThuoc));
            trang = Math.Clamp(trang, 1, tongSoTrang);

            var items = await nguon
                .Skip((trang - 1) * kichThuoc)
                .Take(kichThuoc)
                .ToListAsync();

            return new PhanTrangDanhSach<T>(items, tongSoMuc, trang, kichThuoc);
        }
    }
}
