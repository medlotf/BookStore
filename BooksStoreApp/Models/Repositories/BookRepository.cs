using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksStoreApp.Models.Repositories
{
    public class BookRepository : IBooksStoreRepository<Book>
    {
        List<Book> books;

        public BookRepository()
        {
            books = new List<Book>()
                {
                    new Book{Id=1,Title="Title 1", Description="Description 1", Author=new Author()},
                    new Book{Id=2,Title="Title 2", Description="Description 2", Author=new Author()},
                    new Book{Id=3,Title="Title 3", Description="Description 3", Author=new Author()}
                };
        }
        public void Add(Book entity)
        {
            entity.Id = books.Max(b => b.Id) + 1;
            books.Add(entity);
        }

        public void Delete(int id)
        {
            var book = Find(id);
            books.Remove(book);
        }

        public Book Find(int id)
        {
            var book = books.SingleOrDefault(b => b.Id == id);
            return book;
        }

        public IList<Book> List()
        {
            return books;
        }

        public void Update(int id,Book newEntity)
        {
            var book = Find(id);
            book.Title = newEntity.Title;
            book.Description = newEntity.Description;
            book.Author = newEntity.Author;
            book.ImageUrl = newEntity.ImageUrl;
        }
    }
}
