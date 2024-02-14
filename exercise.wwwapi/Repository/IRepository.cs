namespace workshop.webapi.Repository
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> Get();
        Task<T> GetById(object id);
        Task<T> Delete(T entity);
        Task<T> Insert(T entity);
        Task<T> Update(T entity);
    }
}
