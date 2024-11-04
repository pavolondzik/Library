using System.ComponentModel.DataAnnotations;

namespace LibraryCore.Entities
{
   public class BookState
   {
		public BookState()
		{
			
		}

		public BookState(string? state)
		{
			State = state;
		}

		public int Id { get; set; }

      /// <summary>
      /// Condition of the book, for exampple if the book is in a good condition, or is damaged.
      /// </summary>
      [Required]
      public string? State { get; set; }

      #region Navigation Properties
      public virtual ICollection<Book> Books { get; set; } = new List<Book>();
      #endregion Navigation Properties
   }
}
