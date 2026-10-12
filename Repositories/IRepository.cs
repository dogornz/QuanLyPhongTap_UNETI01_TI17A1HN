namespace QuanLyPhongTap_UNETI01_TI17A1HN.Repositories
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();

        T? GetById(object id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Save();
    }
}
