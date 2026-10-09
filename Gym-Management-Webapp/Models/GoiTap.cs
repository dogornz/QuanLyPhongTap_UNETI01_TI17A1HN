using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    /// <summary>Các trạng thái hợp lệ của gói tập. Module 3 dùng DangApDung để kiểm tra khi đăng ký.</summary>
    public static class TrangThaiGoiTap
    {
        public const string DangApDung = "Đang áp dụng";
        public const string TamNgung = "Tạm ngừng";
        public const string NgungApDung = "Ngừng áp dụng";

        public static readonly string[] DanhSach = { DangApDung, TamNgung, NgungApDung };
    }

    public class GoiTap
    {
        [Key]
        public int MaGoiTap { get; set; }

        [Display(Name = "Tên gói tập")]
        [Required(ErrorMessage = "Vui lòng nhập tên gói tập")]
        [StringLength(100, ErrorMessage = "Tên gói tập tối đa 100 ký tự")]
        public string TenGoiTap { get; set; } = string.Empty;

        // Khóa ngoại -> LoaiGoiTap (1 loại gói - n gói tập).
        // Option mặc định của <select> có value = 0 nên Range(1, ...) sẽ báo lỗi nếu chưa chọn.
        [Display(Name = "Loại gói")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn loại gói")]
        public int MaLoaiGoi { get; set; }

        // Navigation property (nullable để không bị tự động coi là [Required] khi model binding)
        [ForeignKey(nameof(MaLoaiGoi))]
        [Display(Name = "Loại gói")]
        public LoaiGoiTap? LoaiGoiTap { get; set; }

        [Display(Name = "Thời hạn (tháng)")]
        [Range(1, 120, ErrorMessage = "Thời hạn phải lớn hơn 0 (tối đa 120 tháng)")]
        public int ThoiHanThang { get; set; }

        [Display(Name = "Đơn giá (VNĐ)")]
        [Range(1, 999999999, ErrorMessage = "Đơn giá phải lớn hơn 0")]
        [Column(TypeName = "decimal(18, 0)")]
        public decimal DonGia { get; set; }

        // null = gói không giới hạn số buổi. Nếu có nhập thì phải > 0.
        [Display(Name = "Số buổi tối đa")]
        [Range(1, 10000, ErrorMessage = "Số buổi tối đa phải lớn hơn 0 (để trống nếu không giới hạn)")]
        public int? SoBuoiToiDa { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(1000, ErrorMessage = "Mô tả tối đa 1000 ký tự")]
        public string? MoTa { get; set; }

        [Display(Name = "Trạng thái")]
        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        [StringLength(30)]
        public string TrangThai { get; set; } = TrangThaiGoiTap.DangApDung;
    }
}
