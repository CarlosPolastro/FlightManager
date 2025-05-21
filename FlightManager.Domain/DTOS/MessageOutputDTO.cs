using System.Collections.Generic;

namespace FlightManager.Domain.DTOS
{
	public class MessageOutputDTO<T>
	{
		public T Data { get; set; }
		public List<string> Errors { get; set; }
		public bool Success { get { return Errors.Count == 0; } }

		public MessageOutputDTO()
		{
			Errors = new List<string>();
		}
	}
}

