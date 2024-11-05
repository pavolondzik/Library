using InfrastructureLayer.Data;
using LibraryCore.Entities;
using LibraryCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Repositories
{
	public class LibraryRepository : ILibraryRepository
	{
		private LibraryContext _context;

		public LibraryRepository(LibraryContext dbContext)
		{
			_context = dbContext;
		}

		#region Book
		public async Task<int> AddBookAsync(Book book)
		{
			await _context.Books.AddAsync(book);
			await _context.SaveChangesAsync();
			return book.Id;
		}

		public async Task<Book?> GetBookAsync(int id)
		{
			var book = await _context.Books.FindAsync(id);
			return book;
		}

		public async Task<Book?> GetBookByIsbnAsync(string? isbn)
		{
			var book = await _context.Books.FirstOrDefaultAsync(x => x.ISBN10 == isbn);

			return book;
		}

		public async IAsyncEnumerable<Book> GetBookByTitleAsync(string title)
		{
			await foreach (var entity in _context.Books.Where(x => string.Equals(x.Title, title, StringComparison.OrdinalIgnoreCase)).AsAsyncEnumerable())
			{
				yield return entity;
			}
		}

		public async IAsyncEnumerable<Borrowing> GetBorrowingsDayBeforeExpirationAsync()
		{
			await foreach (var entity in _context.Borrowings
				.Include(x => x.User)
				.Include(x => x.Book)
				.Where(x => x.DateShouldReturn.Date == DateTime.Now.Date.AddDays(-1))
				.AsAsyncEnumerable())
			{
				yield return entity;
			}
		}

		public async Task<bool> UpdateBook(Book book)
		{
			var entity = await _context.Books.FindAsync(book.Id);
			if (entity != null)
			{
				_context.Entry(entity).CurrentValues.SetValues(book);
				return await _context.SaveChangesAsync() > 0;
			}

			return false;
		}

		public async Task<bool> DeleteBook(int id)
		{
			var book = await _context.Books.FindAsync(id);
			if (book == null) return false;

			_context.Books.Remove(book);
			return await _context.SaveChangesAsync() > 0;
		}
		#endregion Book

		#region Borrowing
		public async Task<int> AddBorrowingAsync(Borrowing borrowing)
		{
			await _context.Borrowings.AddAsync(borrowing);
			await _context.SaveChangesAsync();
			return borrowing.Id;
		}

		public async Task<Borrowing?> GetBorrowingAsync(int id)
		{
			var borrowing = await _context.Borrowings.FindAsync(id);
			return borrowing;
		}

		public async Task<bool> UpdateBorrowingAsync(Borrowing borrowing)
		{
			var entity = await _context.Borrowings
			.Include(x => x.Book)
			.Include(y => y.User)
			.FirstOrDefaultAsync(x => x.Id == borrowing.Id);

			if (entity != null)
			{
				_context.Entry(entity).CurrentValues.SetValues(borrowing);
				return await _context.SaveChangesAsync() > 0;
			}

			return false;
		}
		#endregion Borrowing
	}
}
