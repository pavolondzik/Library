using InfrastructureLayer.ResponseDto;
using LibraryCore.Entities;
using LibraryCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementApi.Controllers
{
	[ApiController]
   [Route("api/[controller]")]
   public class BooksController : ControllerBase
   {
      private readonly ILogger<BooksController> _logger;
      private readonly ILibraryRepository _libRepository;


		public BooksController(ILogger<BooksController> logger, ILibraryRepository bookRepository)
      {
         _logger = logger;
         _libRepository = bookRepository;
      }

      /// <summary>
      /// Vytvorenie novej knihy
      /// POST: api/Books
      /// </summary>
      /// <param name="id"></param>
      /// <param name="book"></param>
      /// <returns>Created with book id</returns>
      [HttpPost]
      public async Task<ActionResult<BaseResponseDto>> PostBook(Book book)
      {
         if(book == null || !ModelState.IsValid)
         {
            return BadRequest();
         }

			int bookId = await _libRepository.AddBookAsync(book);

         return CreatedAtAction("PostBook", new BaseResponseDto { Id = bookId }, book);
      }

      /// <summary>
      /// Získanie detailov existujúcej knihy pod¾a ID
      /// </summary>
      /// <param name="id"></param>
      /// <returns>OK with Book or NotFound</returns>
      [HttpGet("{id}")]
      public async Task<ActionResult<Book>> GetBook(int id)
      {
			var book = await _libRepository.GetBookAsync(id);
			
         if (book == null)
         {
            return NotFound();
         }

         return book;
      }

		/// <summary>
		/// Aktualizácia existujúcej knihy
		/// PUT: api/Books/5
		/// </summary>
		/// <param name="id"></param>
		/// <param name="book"></param>
		/// <returns>OK or BadRequest</returns>
		[HttpPut("{id}")]
      public async Task<IActionResult> PutBook(int id, Book book)
      {
         if (book == null || id != book.Id || !ModelState.IsValid)
         {
            return BadRequest();
         }

			await _libRepository.UpdateBook(book);

         return Ok();
      }

		/// <summary>
		/// Odstránenie knihy
		/// DELETE: api/Books/5
		/// </summary>
		/// <param name="id"></param>
		/// <returns>OK or NotFound</returns>
		[HttpDelete("{id}")]
      public async Task<IActionResult> DeleteBook(int id)
      {
         if (await _libRepository.DeleteBook(id))
         {
            return Ok();
         }
         else {
				return NotFound();
			}
      }

		/// <summary>
		/// Vytvorenie novej zápožièky
		/// </summary>
		/// <param name="borrowing"></param>
		/// <returns>Created with borrowing id or BadRequest</returns>
		[HttpPost]
		public async Task<ActionResult<Borrowing>> PostBorrowing(Borrowing borrowing)
      {
			if (borrowing == null || !ModelState.IsValid)
			{
				return BadRequest();
			}

			int borrowingId = await _libRepository.AddBorrowingAsync(borrowing);

			return CreatedAtAction("PostBorrowing", new BaseResponseDto { Id = borrowingId }, borrowing);
		}

		/// <summary>
		/// Confirmation for returning borrowed book (Potvrdenie o vrátení vypožièanej knihy)
      /// Endpoint sa vola vtedy ked uzivatel vracia knihu.
		/// </summary>
		/// <param name="borrowingId"></param>
		/// <returns>OK or NotFound</returns>
		[HttpPut]
		public async Task<ActionResult<ConfirmationResponseDto>> PutBookReturn(int borrowingId)
		{
			var borrowing = await _libRepository.GetBorrowingAsync(borrowingId);
			if (borrowing != null)
			{
            borrowing.DateReturnConfirmation = DateTime.Now;
				bool success = await _libRepository.UpdateBorrowingAsync(borrowing);

				// return confirmation
				return Ok(new ConfirmationResponseDto { BookTitle = borrowing.Book.Title, BookReturned = success,  UserId = borrowing.User.Id, BookReturnedAt = borrowing.DateReturnConfirmation });
			}
			else
			{
				return NotFound();
			}
		}
	}
}
