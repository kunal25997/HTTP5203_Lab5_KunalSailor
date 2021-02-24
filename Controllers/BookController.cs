using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace HTTP5203_Lab5_KunalSailor.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            IList<Models.Book> BooksList = new List<Models.Book>();

            string path = Request.PathBase + "App_Data/books.xml";
            XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                doc.Load(path);
                XmlNodeList books = doc.GetElementsByTagName("book");

                foreach (XmlElement b in books)
                {
                    Models.Book book = new Models.Book();
                    book.Title = b.GetElementsByTagName("title")[0].InnerText;
                    book.Firstname = b.GetElementsByTagName("firstname")[0].InnerText;
                    book.Lastname = b.GetElementsByTagName("lastname")[0].InnerText;

                    BooksList.Add(book);
                }
            }
            return View(BooksList);
        }
        [HttpGet]
        public IActionResult Add()
        {
            var book = new Models.Book();
            return View(book);
        }

        [HttpPost]
        public IActionResult Add(Models.Book b)
        {
            string path = Request.PathBase + "App_Data/books.xml";
            XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                doc.Load(path);
                XmlElement book = CreateBookElement(doc, b);

                doc.DocumentElement.AppendChild(book);
            }
            else
            {
                XmlNode dec = doc.CreateXmlDeclaration("1.0", "utf-8", "");
                doc.AppendChild(dec);
                XmlNode root = doc.CreateElement("books");

                XmlElement book = CreateBookElement(doc, b);
                root.AppendChild(book);
                doc.AppendChild(book);
            }
            doc.Save(path);
                return View();
        }

        private XmlElement CreateBookElement(XmlDocument doc, Models.Book newBook)
        {
            XmlElement book = doc.CreateElement("book");

            XmlNode Title = doc.CreateElement("title");
            Title.InnerText = newBook.Title;

            
            XmlNode Authorname = doc.CreateElement("author");

            XmlNode Firstname = doc.CreateElement("firstname");
            Firstname.InnerText = newBook.Firstname;

            XmlNode Lastname = doc.CreateElement("lastname");
            Lastname.InnerText = newBook.Lastname;

            Authorname.AppendChild(Firstname);
            Authorname.AppendChild(Lastname);

            book.AppendChild(Authorname);

            return book;
        } 
    }
}
