using System.ComponentModel.DataAnnotations;

namespace LibraryCore.Entities
{
   public class Book
   {
		public Book()
		{
			
		}
		public Book(string? title, string isbn10, BookState bookStatus, int bookCount = 1)
		{
			Title = title;
         ISBN10 = isbn10;
         BookStatus = bookStatus;
         BookCount = bookCount;
		}

		public int Id { get; set; }

		[Required]
		public string? Title { get; set; }
      public string? SubTitle { get; set; }

		[Required]
		public string? Autor { get; set; }
      public string? Description { get; set; }

		/// <summary>
		///  International Standard Book Number - numeric commercial book identifier
		///  E.g.: 0-13-235088-2
		/// </summary>
		[Required]
		[StringLength(20, MinimumLength = 13)]
		public string? ISBN10 { get; set; }

      /// <summary>
      /// ToDO: Should be year published
      /// </summary>
      public DateTime DatePublished { get; set; }
      public string? Publisher { get; set; }
      public string? Language { get; set; }

      public int PagesCount { get; set; }
      public string? Genre { get; set; }

      /// <summary>
      ///  Date Added to Book Collection
      /// </summary>
      public DateTime DateAdded { get; set; }

      /// <summary>
      /// Number of books with this ISBN in library in any given time.
      /// </summary>
      public int BookCount { get; set; }

      #region Navigation Properties
      public int BookStatusId { get; set; }
      public virtual BookState? BookStatus { get; set; }

      #endregion Navigation Properties
   }
}
