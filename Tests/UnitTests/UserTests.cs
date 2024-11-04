using LibraryCore.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests
{
	public class UserTests : IDisposable
   {
      #region Setup
      public UserTests()
      {

      }

      public void Dispose()
      {

      }
		#endregion Setup

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
