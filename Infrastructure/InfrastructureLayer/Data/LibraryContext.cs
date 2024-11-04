using System.Data;
using System.Diagnostics.Metrics;
using LibraryCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Data
{
	public class LibraryContext : DbContext
   {
      public LibraryContext() { }

      public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
      {

      }

      public DbSet<User> Users { get; set; } = null!;
      public DbSet<Book> Books { get; set; } = null!;
      public DbSet<BookState> BookStates { get; set; } = null!;
      public DbSet<Borrowing> Borrowings { get; set; } = null!;
      public DbSet<DeadlinePeriod> DeadlinePeriods { get; set; } = null!;
		

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Book>().HasIndex(c => c.ISBN10).IsUnique();

			base.OnModelCreating(modelBuilder);
		}
	}
}
