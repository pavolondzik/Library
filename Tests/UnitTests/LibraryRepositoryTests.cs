using LibraryCore.Entities;
using LibraryCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using UnitTests.Mocks;

namespace UnitTests
{
	public class LibraryRepositoryTests
	{
		[Fact]
		public async Task GetBorrowingsDayBeforeExpirationAsync_ExpiringBorrowing_ShouldReturnExpiringBorrowing()
		{
			// Arrange
			User user1 = new("Linus", "Torvalds") { Email = "linus.torvalds@osdl.org" };
			BookState bookState1 = new("New");
			Book book1 = new("Clean Code", "0-13-235088-2", bookState1) { DatePublished = new DateTime(2009, 1, 1), DateAdded = new DateTime(2010, 1, 1), Autor = "Robert C. Martin" };

			Borrowing borrowing1 = new Borrowing
			{
				Id = 1,
				User = user1,
				Book = book1,
				DateBorrowed = DateTime.Now.Date.AddDays(-29),
				DateShouldReturn = DateTime.Now.Date.AddDays(1)
			};
			var borrowingsExpected = new List<Borrowing> { borrowing1 };
			ILibraryRepository _repository = new MockBorrowingRepository(borrowingsExpected);

			// Act
			var borrowings = await _repository.GetBorrowingsDayBeforeExpirationAsync().ToListAsync();

			// Assert
			Assert.Single(borrowings);
			Assert.Equal(1, borrowings.First().Id);
		}
	}
}
