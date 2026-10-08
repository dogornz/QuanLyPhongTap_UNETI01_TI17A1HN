namespace QuanLyPhongTap_UNETI_TI17A1HN.Models
{
    public class LoaiGoiTap
    {
        public int LoaiGoiTapId { get; set; }
        public string TenLoai { get; set; }

        public ICollection<GoiTap> GoiTap { get; set; }

    }
}
