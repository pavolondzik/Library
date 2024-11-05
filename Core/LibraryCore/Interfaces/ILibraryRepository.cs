using LibraryCore.Entities;

namespace LibraryCore.Interfaces
{
	public interface ILibraryRepository
	{
		/// <summary>
		/// Add book
		/// </summary>
		/// <param name="book">Book primary key</param>
		/// <returns>Id of the book if book was successfully added or 0.</returns>
		Task<int> AddBookAsync(Book book);

		/// <summary>
		/// Retrieve a book by id.
		/// </summary>
		/// <param name="id">Book primary key</param>
		/// <returns>Returns book entity or null if not found.</returns>
		Task<Book?> GetBookAsync(int id);

		/// <summary>
		/// Retrieve a book by ISBN.
		/// </summary>
		/// <param name="isbn"></param>
		/// <returns>Returns book entity or null if not found.</returns>
		Task<Book?> GetBookByIsbnAsync(string? isbn);

		/// <summary>
		/// Retrieve book by the title
		/// </summary>
		/// <param name="title"></param>
		/// <returns>Returns book entities with the same title</returns>
		IAsyncEnumerable<Book> GetBookByTitleAsync(string title);

		/// <summary>
		/// Retrieve books with the same title
		/// </summary>
		/// <returns></returns>
		IAsyncEnumerable<Borrowing> GetBorrowingsDayBeforeExpirationAsync();

		/// <summary>
		/// Update book entity
		/// </summary>
		/// <param name="book"></param>
		/// <returns></returns>
		Task<bool> UpdateBook(Book book);

		/// <summary>
		/// Delete book
		/// </summary>
		/// <param name="id">Book primary key</param>
		/// <returns>True if book was successfully deleted.</returns>
		Task<bool> DeleteBook(int id);

		/// <summary>
		/// Add borrowing
		/// </summary>
		/// <param name="borrowing"></param>
		/// <returns>Id of the borrowing if book was successfully added or 0.</returns>
		Task<int> AddBorrowingAsync(Borrowing borrowing);

		/// <summary>
		/// Retrieve a borrowing by id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Returns borrowing entity or null if not found.</returns>
		Task<Borrowing?> GetBorrowingAsync(int id);

		/// <summary>
		/// Update borrowing entity
		/// </summary>
		/// <param name="borrowing"></param>
		/// <returns>True if borrowing was successfully modified.</returns>
		Task<bool> UpdateBorrowingAsync(Borrowing borrowing);
	}
}