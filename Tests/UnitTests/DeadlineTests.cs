using LibraryCore.Entities;

namespace UnitTests
{
	public class DeadlineTests
	{
		[Fact]
		public void GetDeadline_15DaysFromNow_16thDay()
		{
			var deadline = new DeadlinePeriod(new TimeSpan(15, 0, 0, 0), "15 days");
			var now = new DateTime(2021, 1, 1);

			var deadlineTime = deadline.GetDeadline(now);

			Assert.Equal(new DateTime(2021,1,16), deadlineTime);
		}
	}
}