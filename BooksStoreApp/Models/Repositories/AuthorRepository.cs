using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksStoreApp.Models.Repositories
{
    public class AuthorRepository : IBooksStoreRepository<Author>
    {
        List<Author> authors;

        public AuthorRepository()
        {
            authors = new List<Author>()
            {
                new Author{Id=1,FullName="Cobban"},
                new Author{Id=2,FullName="simo"}
            };
        }

        public void Add(Author entity)
        {
            entity.Id = authors.Max(a => a.Id) + 1;
            authors.Add(entity);
        }

        public void Delete(int id)
        {
            var author = Find(id);
            authors.Remove(author);
        }

        public Author Find(int id)
        {
            var author = authors.SingleOrDefault(a => a.Id == id);
            return author;
        }

        public IList<Author> List()
        {
            return authors;
        }

        public List<Author> Search(string str)
        {
            var res = authors.Where(a => a.FullName.Contains(str));
            return res.ToList();
        }

        public void Update(int id, Author newEntity)
        {
            var author = Find(id);
            author.FullName = newEntity.FullName;
        }
    }
}
