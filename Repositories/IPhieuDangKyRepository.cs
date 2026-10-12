
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Repositories
{
    public interface IPhieuDangKyRepository
    {
        List<PhieuDangKy> GetAll();

        PhieuDangKy? GetById(string maPhieu);

        void Add(PhieuDangKy phieu);

        void Save();
    }
}
