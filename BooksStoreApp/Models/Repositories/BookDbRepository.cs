using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksStoreApp.Models.Repositories
{
    public class BookDbRepository : IBooksStoreRepository<Book>
    {

        BookStoreDbContext db;

        public BookDbRepository(BookStoreDbContext _db)
        {
            db = _db;
        }
        public void Add(Book entity)
        {
            db.Books.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var book = Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
        }

        public Book Find(int id)
        {
            var book = db.Books.Include(a => a.Author).SingleOrDefault(b => b.Id == id);
            return book;
        }

        public IList<Book> List()
        {
            return db.Books.Include(a=>a.Author).ToList();
        }

        public void Update(int id, Book newEntity)
        {
            db.Update(newEntity);
            db.SaveChanges();
        }

        public List<Book> Search(string str)
        {
            var res = db.Books.Include(a => a.Author).Where(b => 
                         b.Title.Contains(str)
                      || b.Description.Contains(str) 
                      || b.Author.FullName.Contains(str)
            );

            return res.ToList();
        }
    }
}
