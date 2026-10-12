using System.ComponentModel.DataAnnotations;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    public class LoaiGoiTap
    {
        [Key]
        public int MaLoaiGoi { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên loại gói tập")]
        [StringLength(100, ErrorMessage = "Tên loại gói không được vượt quá 100 ký tự")]
        [Display(Name = "Tên loại gói tập")]
        public string TenLoaiGoi { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Mô tả đặc quyền")]
        public string? MoTa { get; set; }

        [StringLength(50)]
        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; } = "Unlock";

        [Display(Name = "Số gói trực thuộc")]
        public int SoGoiTrucThuoc { get; set; } = 0;

        [Display(Name = "Ngày cập nhật")]
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;
    }
}
