using ValidationDemoApi.CORE.Interfaces;

namespace ValidationDemoApi.DAL
{
    public class FileRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly string _filePath;
        private List<T> _entities = new List<T>();
        private readonly IMapper<T> _mapper;

        public FileRepository(string filePath, IMapper<T> mapper)
        {
            _filePath = filePath;
            _mapper = mapper;

            _entities = Load();
        }

        public void Save()
        {
            using (var writer = new StreamWriter(_filePath))
            {
                foreach (var entity in _entities)
                {
                    var line = _mapper.MapToCSV(entity);
                    writer.WriteLine(line);
                }
            }
        }

        private List<T> Load()
        {
            if (!File.Exists(_filePath))
            {
                return new List<T>();
            }
            var entities = new List<T>();
            using (var reader = new StreamReader(_filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var entity = _mapper.MapToObject(line);
                    entities.Add(entity);
                }
            }
            return entities;
        }


        public T Add(T entity)
        {
            var nextId = 0;
            if (_entities.Any())
            {
                nextId = _entities.Max(e => e.Id);
            }
            entity.Id = nextId + 1;
            _entities.Add(entity);
            Save();
            return entity;
        }

        public void Delete(T entity)
        {
            var existing = GetById(entity.Id);
            if (existing != null)
            {
                _entities.Remove(existing);
                Save();
            }
        }

        public IEnumerable<T> GetAll()
        {
            return _entities;
        }

        public T GetById(int id)
        {
            return _entities.FirstOrDefault(e => e.Id == id);
        }

        public void Update(T entity)
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                var oldEntity = _entities.ElementAt(i);
                if (oldEntity.Id == entity.Id)
                {
                    _entities[i] = entity;
                    Save();
                    break;
                }

            }
        }

        public IEnumerable<T> Filter(Func<T, bool> predicate)
        {
            var entities = _entities.Where(predicate);
            return entities;
        }

        public T? GetOne(Func<T, bool> predicate)
        {
            return _entities.FirstOrDefault(predicate);
        }
    }
}
