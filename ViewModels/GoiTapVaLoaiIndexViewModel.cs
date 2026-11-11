using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.ViewModels
{
    public class GoiTapVaLoaiIndexViewModel
    {
        public string ActiveTab { get; set; } = "goi";
        public IEnumerable<LoaiGoiTap> LoaiGoiTaps { get; set; } = new List<LoaiGoiTap>();
        public GoiTapIndexViewModel GoiTapFilter { get; set; } = new();
    }
}

