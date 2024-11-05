using LibraryCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Data
{
	public static class SeedData
	{
		private static DateTime Today = DateTime.Now;
		private static DateTime DatePublished = new DateTime(2009,1,1);

		public static readonly User User1 = new("Steve", "Wozniak") { Email = "steve.wozniak@gmail.abc" };
		public static readonly User User2 = new("Robert", "C.", "Martin") { Email = "robert.c.martin@test.abc" };
		public static readonly User User3 = new("Martin", "Fowler") { Email = "martin.fowler@test.abc" };

		public static readonly BookState BookState1 = new("New");
		public static readonly BookState BookState2 = new("Missing pages");

		public static readonly Book Book1 = new("Clean Code", "0-13-235088-2", BookState1) { DatePublished = DatePublished, DateAdded = Today, Autor = "Robert C. Martin" };
		public static readonly Book Book2 = new("Clean Architecture", "0-13-449416-4", BookState1) { DatePublished = DatePublished, DateAdded = Today, Autor = "Robert C. Martin" };
		public static readonly Book Book3 = new("The Clean Coder", "0-13-708107-3", BookState1) { DatePublished = DatePublished, DateAdded = Today, Autor = "Robert C. Martin" };

		public static readonly DeadlinePeriod Deadline1 = new(new TimeSpan(15,0,0,0), "15 days");
		public static readonly DeadlinePeriod Deadline2 = new(new TimeSpan(30,0,0,0), "One month");
		public static readonly DeadlinePeriod Deadline3 = new(new TimeSpan(90, 0, 0, 0), "Three months");

		public static Book[] Books = new Book[] { Book1, Book2, Book3 };

		public static Borrowing Borrowing1 = new Borrowing
		{
			User = User1,
			Book = Book1,
			DateBorrowed = Today,
			DateShouldReturn = Today.AddDays(90)
		};

		public static async Task InitializeAsync(LibraryContext dbContext)
		{
			if (await dbContext.Users.AnyAsync()) return; // DB has been seeded

			await PopulateTestDataAsync(dbContext);
		}

		public static async Task PopulateTestDataAsync(LibraryContext dbContext)
		{
			Borrowing Borrowing2 = new Borrowing
			{
				User = User2,
				Book = Book2,
				DateBorrowed = Today,
				DateShouldReturn = Deadline3.GetDeadline(Today)
			};

			Borrowing Borrowing3 = new Borrowing
			{
				User = User1,
				Book = Book3,
				DateBorrowed = Today.AddDays(2),
				DateShouldReturn = Deadline2.GetDeadline(Today.AddDays(2))
			};

			dbContext.DeadlinePeriods.AddRange([Deadline1, Deadline2, Deadline3]);
			dbContext.BookStates.AddRange([BookState1, BookState2]);
			dbContext.Books.AddRange(Books);
			dbContext.Users.AddRange([User1, User2, User3]);
			dbContext.Borrowings.AddRange([Borrowing1, Borrowing2, Borrowing3]);

			await dbContext.SaveChangesAsync();
		}
	}
}
