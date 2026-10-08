using System.ComponentModel.DataAnnotations;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    public class LoaiGoiTap
    {
        [Key]
        public int LoaiGoiTapId { get; set; }
        [Required(ErrorMessage = "Tên loại gói tập không được để trống")]
        public string TenLoaiGoi { get; set; }
        public string MoTa { get; set; }
        [Required(ErrorMessage = "Trạng thái không được để trống")]
        public string TrangThai { get; set; }  // (Active/Inactive)

        public ICollection<GoiTap> GoiTap { get; set; }
    }
}
