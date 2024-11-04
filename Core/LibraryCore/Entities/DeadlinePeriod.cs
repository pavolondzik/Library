using System.ComponentModel.DataAnnotations;

namespace LibraryCore.Entities
{
   public class DeadlinePeriod
   {
		public DeadlinePeriod()
		{
			
		}


		public DeadlinePeriod(TimeSpan borrowingTimeSpan, string? description)
		{
			BorrowingTimeSpan = DateTime.MinValue + borrowingTimeSpan;
			Description = description;
		}

		public int Id { get; set; }

		/// <summary>
		/// Time span the book can be borrowed for
		/// </summary>
		[Required]
		public DateTime BorrowingTimeSpan { get; set; }

		/// <summary>
		/// Description of the timespan
		/// </summary>
		[Required]
		public string? Description { get; set; }

      public DateTime GetDeadline(DateTime now)
      {
			TimeSpan timeSpan = BorrowingTimeSpan - DateTime.MinValue;
			return now.Date.Add(timeSpan);
		}
   }
}