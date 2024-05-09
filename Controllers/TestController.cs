using Microsoft.AspNetCore.Mvc;

//[ApiController]
//3. public class BookController : ControllerBase
//4. {
//5. private readonly ILogger<BookController> _logger;
//6. private readonly IBookService _bookService;
//7. public BookController(ILogger<BookController> logger, IBookService bookService)
//8. {
//9. _logger = logger;
//10. _bookService = bookService;
//11. }

//12. [HttpGet("/api/books")]
//13. public ActionResult<IEnumerable<BookResponse>> GetAllBooks()