using Entities.Exceptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Services
{
    public class BookManger : IBookService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;

        public BookManger(IRepositoryManager manager, ILoggerService logger)
        {
            _manager = manager;
            _logger = logger;
        } 
        public Book CreateOneBook(Book book)
        {
            _manager.Book.CreateOnebook(book);
            _manager.Save();
            return book;
        }
        public void DeleteOneBook(int id, bool trackChanges)
        {
            var entities = _manager.Book.GetOneBookId(id, trackChanges);
            if (entities is null)
                throw new BookNotFoundExceptions(id);

            _manager.Book.DeleteOnebook(entities);
            _manager.Save();
        }
        public IEnumerable<Book> GetAllBooks(bool trackChanges)
        {
            return _manager.Book.GetAllBooks(trackChanges);
        }
        public Book GetOneBookById(int id, bool trackChanges)
        {
            var book = _manager.Book.GetOneBookId(id, trackChanges);
            if (book is null)
                throw new BookNotFoundExceptions(id);
            return book;
        }
        public void UpdateOneBook(int id, Book book, bool trackChanges)
        {
            var entities = _manager.Book.GetOneBookId(id, trackChanges);
            if (entities is null)
                throw new BookNotFoundExceptions(id);

       
            entities.Title = book.Title;
            entities.Price = book.Price;
            _manager.Book.Update(entities);
            _manager.Save();

        }
    }
}
