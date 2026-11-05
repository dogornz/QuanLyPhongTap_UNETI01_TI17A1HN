using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    [Table("GoiTap")]
    public class GoiTap
    {
        [Key]
        [StringLength(450)]
        [Display(Name = "Mã gói tập")]
        [Required(ErrorMessage = "Mã gói tập không được để trống!")]
        public string MaGoiTap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên gói tập không được để trống!")]
        [StringLength(100, ErrorMessage = "Tên gói không được vượt quá 100 ký tự!")]
        [Display(Name = "Tên gói tập")]
        public string TenGoiTap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Thời hạn không được để trống!")]
        [Display(Name = "Thời hạn (tháng)")]
        public string ThoiHanThang { get; set; } = string.Empty;

        [Required(ErrorMessage = "Đơn giá không được để trống!")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Đơn giá phải lớn hơn 0!")]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Đơn giá")]
        public decimal DonGia { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số buổi tối đa không được âm!")]
        [Display(Name = "Số buổi tối đa")]
        public int SoBuoiToiDa { get; set; }

        [Display(Name = "Mô tả")]
        public string MoTa { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn trạng thái gói tập!")]
        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; } = "Đang áp dụng";

        [Required(ErrorMessage = "Vui lòng chọn loại gói tập!")]
        [Display(Name = "Loại gói")]
        public int? MaLoaiGoi { get; set; }

        [ForeignKey("MaLoaiGoi")]
        public virtual LoaiGoiTap? LoaiGoiTap { get; set; }
    }
}
