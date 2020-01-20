using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BooksStoreApp.Models;
using BooksStoreApp.Models.Repositories;
using BooksStoreApp.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooksStoreApp.Controllers
{
    public class BookController : Controller
    {
        private readonly IBooksStoreRepository<Book> bookRepository;
        private readonly IBooksStoreRepository<Author> authorRepository;
        private readonly IHostingEnvironment hosting;

        public BookController(IBooksStoreRepository<Book> bookRepository, IBooksStoreRepository<Author> authorRepository,IHostingEnvironment hosting)
        {
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
            this.hosting = hosting;
        }

        // GET: Book
        public ActionResult Index()
        {
            var books = bookRepository.List();
            return View(books);
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            var book = bookRepository.Find(id);
            return View(book);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            var model = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return View(model);
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    string fileName = UploadsFile(model.Img)??string.Empty;

                    if (model.AuthorId == -1)
                    { 
                        ViewBag.message = "Please select an author";
                        return View(getAllAuthors());
                    }
                    
                    var author = authorRepository.Find(model.AuthorId);
                    Book book = new Book
                    {
                        Id = model.Id,
                        Title = model.Title,
                        Description = model.Description,
                        Author = author,
                        ImageUrl = fileName
                    };
                    bookRepository.Add(book);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            ModelState.AddModelError("", "You have to fill all the required fields");
            return View(getAllAuthors());
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int id)
        {
            var book = bookRepository.Find(id);
            var authorId = book.Author == null ? book.Author.Id = 0 : book.Author.Id;
            var model = new BookAuthorViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                AuthorId = authorId,
                Authors = authorRepository.List().ToList(),
                ImgUrl=book.ImageUrl
            };
            return View(model);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult Edit(int id,BookAuthorViewModel model)
        {
            try
            {
                string fileName = UploadFile(model.Img,model.ImgUrl);

                Book book = new Book
                {
                    Id=model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    Author = authorRepository.Find(model.AuthorId),
                    ImageUrl = fileName
                };

                bookRepository.Update(model.Id, book);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            var book = bookRepository.Find(id);
            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult confirmDelete(int id)
        {
            try
            {
                bookRepository.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        List<Author> FillSelectList()
        {
            var authors = authorRepository.List().ToList();
            authors.Insert(0, new Author { Id = -1, FullName = "---- Select ----" });
            return authors;
        }

        BookAuthorViewModel getAllAuthors()
        {
            var Vmodel = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return Vmodel;
        }

        string UploadsFile(IFormFile file)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                string fullPath = Path.Combine(uploads, file.FileName);
                file.CopyTo(new FileStream(fullPath, FileMode.Create));

                return file.FileName;
            }
            return null;
        }

        string UploadFile(IFormFile file,string ImgUrl)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "uploads");

                string newPath = Path.Combine(uploads, file.FileName);
                string oldPath = Path.Combine(uploads, ImgUrl);

                if (newPath != oldPath)
                {
                    System.IO.File.Delete(oldPath);
                    file.CopyTo(new FileStream(newPath, FileMode.Create));
                }
                return file.FileName;
            }
            return ImgUrl;
        }

        public ActionResult Search(string str)
        {
            var res = bookRepository.Search(str);
            return View("Index", res);
        }
    }
}