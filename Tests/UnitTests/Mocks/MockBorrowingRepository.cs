using LibraryCore.Entities;
using LibraryCore.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UnitTests.Mocks
{
	public class MockBorrowingRepository : ILibraryRepository
	{
		private readonly List<Borrowing> _borrowings;

		public MockBorrowingRepository(List<Borrowing> borrowings)
		{
			_borrowings = borrowings;
		}

		public Task<int> AddBookAsync(Book book)
		{
			throw new NotImplementedException();
		}

		public Task<int> AddBorrowingAsync(Borrowing borrowing)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteBook(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Book?> GetBookAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Book?> GetBookByIsbnAsync(string? isbn)
		{
			throw new NotImplementedException();
		}

		public IAsyncEnumerable<Book> GetBookByTitleAsync(string title)
		{
			throw new NotImplementedException();
		}

		public Task<Borrowing?> GetBorrowingAsync(int id)
		{
			throw new NotImplementedException();
		}

		public async IAsyncEnumerable<Borrowing> GetBorrowingsDayBeforeExpirationAsync()
		{
			foreach (var borrowing in _borrowings.Where(b => b.DateShouldReturn.Date.AddDays(-1) == DateTime.Now.Date))
			{
				yield return borrowing;
				await Task.Yield(); // Simulate async behavior
			}
		}

		public Task<bool> UpdateBook(Book book)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UpdateBorrowingAsync(Borrowing borrowing)
		{
			throw new NotImplementedException();
		}
	}
}
