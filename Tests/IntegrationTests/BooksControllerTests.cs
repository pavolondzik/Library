using System.ComponentModel;
using Azure;
using InfrastructureLayer.Data;
using InfrastructureLayer.Repositories;
using InfrastructureLayer.ResponseDto;
using LibraryCore.Entities;
using LibraryManagementApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests
{
	/// <summary>
	/// BooksController tests
	/// </summary>
	public class BooksControllerTests : IClassFixture<TestDatabaseFixture>
	{
		#region Set up
		public BooksControllerTests(TestDatabaseFixture fixture)
		{
			Fixture = fixture;
		}

		public TestDatabaseFixture Fixture { get; }

		private BooksController CreateController(LibraryContext context)
		{
			var logger = Fixture.CreateLogger<BooksController>();
			var repository = new LibraryRepository(context);
			return new BooksController(logger, repository);
		}
		#endregion Set up

		[Fact]
		[Description("Add book to DB. Positive scenario. Test rolls back transaction, therefore test doesn't interfere with other tests.")]
		public async Task PostBook_NewBook_ShouldReturnBookId()
		{
			using var context = Fixture.CreateContext();
			var controller = CreateController(context);

			var bookState = context.BookStates.FirstOrDefault(x => x.State == SeedData.BookState1.State);

			var newBook = new Book("MICROSOFT Linq", "978-80-251-2735-3", bookState) {
				SubTitle = "KOMPLETNÍ PRÚVODCE PROGRAMÁTORA",
				Autor = "Paolo Pialorsi, Marco Russo",
				DatePublished = new DateTime(2009, 1, 1),
				Description = "Architektúra, syntaxe a třídy\r\nJak Linq integrovat do svých projektú\r\nParalelizace dotazú, možnosti rozšíření\r\nUkázky v jazicích c# a Visual Basic",
				Genre = "IT",
				Publisher = "Computer Press, a.s."
			};

			context.Database.BeginTransaction();
			var response = (await controller.PostBook(newBook))?.Result as CreatedAtActionResult;
			context.ChangeTracker.Clear();

			int newBookId = (int)((Book)response.Value)?.Id;
			int newBookIdFromRoute = int.Parse(response.RouteValues["Id"].ToString());

			Assert.True(newBookId > 0);
			Assert.Equal(newBookId, newBookIdFromRoute);

			var bookDb = context.Books.Single(b => b.Title == newBook.Title);
			Assert.Equal(newBook.ISBN10, bookDb.ISBN10);
		}

		[Fact]
		[Description("Positive scenario.")]
		public async Task GetBook_ExistingBook_ISBNShouldMatch()
		{
			using var context = Fixture.CreateContext();
			var controller = CreateController(context);

			var book = (await controller.GetBook(1)).Value;

			Assert.Equal("0-13-235088-2", book?.ISBN10);
		}

		[Fact]
		[Description("Update book. Positive scenario. Leaving changes in DB just for kicks.")]
		public async Task PutBook_ExistingBook_ShouldReturnHttpStatus200()
		{
			using var context = Fixture.CreateContext();
			var controller = CreateController(context);

			SeedData.Book1.SubTitle = "A Handbook of Agile Software Craftsmanship";
			SeedData.Book1.Autor = "Robert C. Martin";
			SeedData.Book1.Description = "Writing clean code is what you must do in order to call yourself a professional. There is no reasonable excuse for doing anything less than your best.";
			SeedData.Book1.Publisher = "Addison-Wesley";
			SeedData.Book1.Language = "English";
			SeedData.Book1.Genre = "Information Technology";
			SeedData.Book1.BookCount = 5;

			var okResult = await controller.PutBook(SeedData.Book1.Id, SeedData.Book1);
			int? statusCode = ((IStatusCodeActionResult)okResult).StatusCode;
			Assert.Equal(StatusCodes.Status200OK, statusCode);

			var bookDb = context.Books.Single(b => b.Title == SeedData.Book1.Title);
			Assert.Equal(SeedData.Book1.SubTitle, bookDb.SubTitle);
			Assert.Equal(SeedData.Book1.Autor, bookDb.Autor);
			Assert.Equal(SeedData.Book1.Description, bookDb.Description);
			Assert.Equal(SeedData.Book1.Publisher, bookDb.Publisher);
			Assert.Equal(SeedData.Book1.Language, bookDb.Language);
			Assert.Equal(SeedData.Book1.Genre, bookDb.Genre);
			Assert.Equal(SeedData.Book1.BookCount, bookDb.BookCount);
		}

		[Fact]
		[Description("Positive scenario.")]
		public async Task DeleteBook_ExistingBook_ShouldReturnHttpStatus200()
		{
			using var context = Fixture.CreateContext();
			var controller = CreateController(context);

			context.Database.BeginTransaction();
			var okResult = await controller.DeleteBook(1);
			context.ChangeTracker.Clear();

			int? statusCode = ((IStatusCodeActionResult)okResult).StatusCode;
			Assert.Equal(StatusCodes.Status200OK, statusCode);

			var bookDb = context.Books.FirstOrDefault(b => b.Title == SeedData.Book1.Title);
			Assert.Null(bookDb);
		}

		[Fact]
		public async Task PostBorrowing_NewBorrowing_ShouldReturnBorrowingId()
		{
			using var context = Fixture.CreateContext();
			var controller = CreateController(context);

			var deadline1 = context.DeadlinePeriods.FirstOrDefault(x => x.BorrowingTimeSpan == SeedData.Deadline1.BorrowingTimeSpan);
			var today = DateTime.Today;

			var book = context.Books.Find(SeedData.Book3.Id);
			var user = context.Users.Find(SeedData.User3.Id);

			var borrowing = new Borrowing()
			{
				DateBorrowed = today,
				DateShouldReturn = deadline1.GetDeadline(today),
				User = user,
				Book = book
			};

			context.Database.BeginTransaction();
			var response = (await controller.PostBorrowing(borrowing))?.Result as CreatedAtActionResult;
			context.ChangeTracker.Clear();

			int newBorrowingId = (int)((Borrowing)response.Value)?.Id;
			int newBorrowingIdFromRoute = int.Parse(response.RouteValues["Id"].ToString());

			Assert.True(newBorrowingId > 0);
			Assert.Equal(newBorrowingId, newBorrowingIdFromRoute);

			var borrowingDb = context.Borrowings.Include(x => x.User).Include(x => x.Book).FirstOrDefault(x => x.Id == newBorrowingIdFromRoute);
			Assert.Equal(borrowingDb.DateShouldReturn, borrowingDb.DateShouldReturn);
			Assert.Equal(borrowingDb.User.FullName, borrowingDb.User.FullName);
			Assert.Equal(borrowingDb.Book.ISBN10, borrowingDb.Book.ISBN10);
		}

		[Fact]
		public async Task PutBorrowing_ExistingBorrowing_Confirmation()
		{
			using var context = Fixture.CreateContext();
			var controller = CreateController(context);

			//context.Database.BeginTransaction();
			var okResult = await controller.PutBookReturn(SeedData.Borrowing1.Id);
			//context.ChangeTracker.Clear();

			int? statusCode = ((IStatusCodeActionResult)okResult.Result).StatusCode;
			Assert.Equal(StatusCodes.Status200OK, statusCode);

			var confirmationDto = ((Microsoft.AspNetCore.Mvc.ObjectResult)okResult.Result).Value as ConfirmationResponseDto;

			var borrowingDb = context.Borrowings.Find(SeedData.Borrowing1.Id);
			Assert.Equal(confirmationDto.BookReturnedAt, borrowingDb.DateReturnConfirmation);
		}
	}
}
