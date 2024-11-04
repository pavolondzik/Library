using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LibraryCore.Entities
{
   public class User
   {
		public User()
		{

		}
		public User(string? firstName, string? lastName)
		{
			FirstName = firstName;
			LastName = lastName;
		}

		public User(string? firstName, string? middleNames, string? lastName)
		{
			FirstName = firstName;
			LastName = lastName;
			MiddleNames = middleNames;
		}

		public int Id { get; set; }

		/// <summary>
		/// User first name
		/// </summary>
		[Required]
		[StringLength(100, MinimumLength = 2)]
		public string? FirstName { get; set; }

		/// <summary>
		/// User last name
		/// </summary>
		[Required]
		[StringLength(100, MinimumLength = 2)]
		public string? LastName { get; set; }

		/// <summary>
		/// All user middle names
		/// </summary>
		[StringLength(100, MinimumLength = 1)]
		public string? MiddleNames { get; set; }

		/// <summary>
		/// User email used for automatic sending of the reminder
		/// </summary>
		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Invalid email address.")]
		public string? Email { get; set; }

      public string FullName
      {
         get {
				var fullname = new StringBuilder();
            char space = ' ';
            if(!string.IsNullOrWhiteSpace(FirstName))
            {
					fullname.Append(FirstName);
				}
				if (!string.IsNullOrWhiteSpace(MiddleNames))
				{
					fullname.Append(space).Append(MiddleNames);
				}
				if (!string.IsNullOrWhiteSpace(LastName))
				{
					fullname.Append(space).Append(LastName);
				}
            return fullname.ToString();
			}
      }
   }
}
