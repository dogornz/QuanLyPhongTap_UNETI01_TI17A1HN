using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    public class GoiTap
    {
        [Key]
        public int IdGoiTap { get; set; }
        [Required(ErrorMessage = "Tên gói tập không được để trống")]
        [StringLength(50, MinimumLength = 5)]
        public string TenGoiTap { get; set; }
        [Required(ErrorMessage = "Giá gói tập không được để trống")]
        public decimal GiaGoiTap { get; set; }
        [Required(ErrorMessage = "Thời hạn không được để trống")]
        public int ThoiHan { get; set; } // số ngày
        public bool ConApDung { get; set; } = true;
        [ForeignKey("LoaiGoiTapId")]
        [Required(ErrorMessage = "Loại gói tập không được để trống")]  
        
        public int LoaiGoiTapId { get; set; }
        public ICollection<PhieuDangKy> PhieuDangKy { get; set; }
        public LoaiGoiTap LoaiGoiTap { get; set; }
    }
}
