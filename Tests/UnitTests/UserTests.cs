using LibraryCore.Entities;

namespace UnitTests
{
	public class UserTests
   {
		[Fact]
		public void FullName_AllThreeNames()
      {
         var user = new User("Robert", "Cecil", "Martin");

         Assert.Equal("Robert Cecil Martin", user.FullName);
      }

		[Fact]
		public void FullName_TwoNames()
		{
			var user = new User("Robert", "Martin");

			Assert.Equal("Robert Martin", user.FullName);
		}
	}
}
