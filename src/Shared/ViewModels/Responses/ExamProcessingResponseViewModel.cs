using Domain.Entities;
using System.Collections.Generic;

namespace ViewModels.Responses
{
	public class ExamProcessingResponseViewModel
	{
		public CompleteResult CompleteResult { get; set; }
		public List<PrimitiveResult> PrimitiveResults { get; set; }
	}
}
