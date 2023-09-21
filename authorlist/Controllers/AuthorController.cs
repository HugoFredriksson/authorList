using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Security.Permissions;

namespace authorList.Controllers
{
    namespace authorList.Controllers
    {
        [ApiController]
        [Route("[Controller]")]
        public class BookController : Controller
        {
            MySqlConnection connection =
                new MySqlConnection("server=localhost;uid=root;pwd=;database=book_list");

            [HttpGet]
            public ActionResult<List<author>> GetBooks()
            {
                List<author> books = new List<author>();
                try
                {
                    connection.Open();
                    MySqlCommand query = connection.CreateCommand();
                    query.Prepare();
                    query.CommandText = "SELECT * FROM books";
                    MySqlDataReader data = query.ExecuteReader();

                    while (data.Read())
                    {
                        author book = new author();
                        book.Id = data.GetInt32("id");
                        book.Author = data.GetString("author");
                        books.Add(book);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return StatusCode(500, "Something went wrong!");
                }
                return Ok(books);
            }

            [HttpPost]
            public ActionResult PostBook(author book)
            {
                try
                {
                    connection.Open();
                    MySqlCommand query = connection.CreateCommand();
                    query.Prepare();
                    query.CommandText = "INSERT INTO books(author) VALUES(@author)";
                    query.Parameters.AddWithValue("@author", book.Author);
                    int rows = query.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        return StatusCode(500, "Could not post book! tråkigt...");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return StatusCode(500);
                }

                return StatusCode(201);
            }
        }
    }
}
