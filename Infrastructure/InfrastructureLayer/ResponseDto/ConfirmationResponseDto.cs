namespace InfrastructureLayer.ResponseDto
{
	public record class ConfirmationResponseDto
	{
		public string? BookTitle { get; set; }
		public bool BookReturned { get; set; }
		public int UserId { get; set; }
		public DateTime BookReturnedAt { get; set; }
	}
}
