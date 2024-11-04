using System.ComponentModel.DataAnnotations;

namespace LibraryCore.Entities
{
   public class Borrowing
   {
      public int Id { get; set; }

		/// <summary>
		/// Date when the book was borrowed
		/// </summary>
		[Required]
		public DateTime DateBorrowed { get; set; }

		/// <summary>
		/// Date the book should be returned at
		/// </summary>
		[Required]
		public DateTime DateShouldReturn { get; set; }

      /// <summary>
      /// Date of book return confirmation is also a date when the book has been returned.
      /// </summary>
      public DateTime DateReturnConfirmation { get; set; }

      #region Navigation Properties
      public int UserId { get; set; }
      public virtual User? User { get; set; }

      public int BookId { get; set; }
      public virtual Book? Book { get; set; }
      #endregion Navigation Properties
   }
}
