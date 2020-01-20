using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksStoreApp.Models.Repositories
{
    public class AuthorDbRepository : IBooksStoreRepository<Author>
    {
        //Initialiser Database:
        public BookStoreDbContext db;

        public AuthorDbRepository(BookStoreDbContext _db)
        {
            db = _db;
        }

        public void Add(Author entity)
        {
            db.Authors.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var author = Find(id);
            db.Authors.Remove(author);
            db.SaveChanges();
        }

        public Author Find(int id)
        {
            var author = db.Authors.SingleOrDefault(a => a.Id == id);
            return author;
        }

        public IList<Author> List()
        {
            return db.Authors.ToList();
        }

        public List<Author> Search(string str)
        {
            var res = db.Authors.Where(a => a.FullName.Contains(str));
            return res.ToList();
        }

        public void Update(int id, Author newEntity)
        {
            db.Update(newEntity);
            db.SaveChanges();
        }
    }
}
