using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
        Task<IEnumerable<Book>> GetAllBooksAsync(bool trackChanges);


        Task<Book> GetOneBookIdAsync(int id,bool trackChanges);


        void CreateOnebook(Book book);

        void UpdateOnebook(Book book);

        void DeleteOnebook(Book book);
    }
}
