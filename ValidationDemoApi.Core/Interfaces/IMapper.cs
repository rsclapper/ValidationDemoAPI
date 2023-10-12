namespace ValidationDemoApi.CORE.Interfaces
{
    public interface IMapper<T> where T : class, IEntity
    {
        T MapToObject(string source);
        string MapToCSV(T source);
    }
}
