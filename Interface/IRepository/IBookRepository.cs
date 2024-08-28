using WEB.DTOs;
using WEB.Models;

namespace WEB.Interface.IRepository
{
    public interface IBookRepository
    {
        public Task<List<Book>> GetAllBook();
        public Task AddBook(Book bookModel);
        public Task<Book> GetBookById(Guid id);
    }
}
