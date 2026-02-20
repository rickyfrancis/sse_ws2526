using BooksService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using System.Xml;

namespace BooksService.Controllers
{
    [Route("")]
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;

        private const string _atom = @"<service xmlns:atom=""http://www.w3.org/2005/Atom"" xmlns=""http://localhost:5000"">
                                           <atom:link rel=""books"" href=""/books"" />
                                       </service>";
        
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // GET /
        [HttpGet("")]
        [Produces("application/xml")]
        public IActionResult Index()
        {
            var doc = new XmlDocument();
            doc.LoadXml(_atom);
            return Ok(doc);
        }

        // GET /books
        [HttpGet("books")]
        public Book[] GetAllBooks()
        {
            return _bookRepository.GetAll();
        }

        // GET /books/ofAuthor/.../withTitle/...
        [HttpGet("books/ofAuthor/{author}/withTitle/{title}")]
        public Book[] GetBooksByAuthorAndTitle(string author, string title)
        {
            var books = _bookRepository.GetAll().Where(book =>
                book.Author.ToLowerInvariant().Contains(author.ToLowerInvariant()) &&
                book.Title.ToLowerInvariant().Contains(title.ToLowerInvariant())
            ).ToArray();

            return books;
        }

        // GET /books/ofAuthor/.../hasGenre/...
        [HttpGet("books/ofAuthor/{author}/hasGenre/{genre}")]
        public Book[] GetBooksByAuthorAndGenre(string author, string genre)
        {
            var books = _bookRepository.GetAll().Where(book =>
                book.Author.ToLowerInvariant().Contains(author.ToLowerInvariant()) &&
                book.Genre.ToLowerInvariant().Contains(genre.ToLowerInvariant())
            ).ToArray();

            return books;
        }

        // GET /books/ofAuthor/.../withTitle/...
        [HttpGet("books/withTitle/{title}/hasGenre/{genre}")]
        public Book[] GetBooksByTitleAndGenre(string title, string genre)
        {
            var books = _bookRepository.GetAll().Where(book =>
                book.Title.ToLowerInvariant().Contains(title.ToLowerInvariant()) &&
                book.Genre.ToLowerInvariant().Contains(genre.ToLowerInvariant())
            ).ToArray();

            return books;
        }
    }
}
