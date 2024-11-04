namespace InfrastructureLayer.ResponseDto
{
	public class BaseResponseDto
	{
		public int Id { get; set; }
		public BaseResponseDto()
		{
			
		}

		public BaseResponseDto(int id)
		{
			Id = id;
		}
	}
}
