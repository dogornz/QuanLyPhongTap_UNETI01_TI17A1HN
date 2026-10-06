using Quanlyphongtap.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quanlyphongtap.Models
{
    public class GoiTap
    {
        public int IdGoiTap { get; set; }
        public string TenGoiTap { get; set; }
        public decimal GiaGoiTap { get; set; }
        public int ThoiHan { get; set; } // số ngày
        public bool ConApDung { get; set; } = true;
        [ForeignKey("LoaiGoiTapId")]  // ← Chỉ rõ FK column
        public int LoaiGoiTapId { get; set; }
        public ICollection<PhieuDangKy> PhieuDangKy { get; set; }
        public LoaiGoiTap LoaiGoiTap { get; set; }
    }
}
