using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Repositories
{
    public interface IGoiTapRepository
    {
        List<GoiTap> GetAll();
        List<GoiTap> GetGoiDangApDung();
        GoiTap? GetById(string maGoiTap);

        List<(string Ten, int SoLuong)> DemTheoLoai(); // THÊM - 8.6
    }
}