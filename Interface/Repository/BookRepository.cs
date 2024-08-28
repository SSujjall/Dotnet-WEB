using WEB.Interface.IRepository;
using WEB.Models;

namespace WEB.Interface.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public BookRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
            _connectionString = _configuration.GetConnectionString("TestStudents") ?? throw new ArgumentNullException(nameof(_connectionString), "Connection string 'TestStudents' not found.");
        }
        public Task<List<Book>> GetAllBook()
        {
            throw new NotImplementedException();
        }

        public Task AddBook(Book bookModel)
        {
            throw new NotImplementedException();
        }

        public Task<Book> GetBookById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}