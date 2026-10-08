using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Repositories
{
    public interface IHoiVienRepository : IRepository<HoiVien>
    {
        List<HoiVien> Search(string keyword);

        bool UpdateTrangThai(string maHv, string trangThai);
    }
    
}
