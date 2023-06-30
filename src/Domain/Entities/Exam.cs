using System;
using System.Collections.Generic;

namespace Domain.Entities
{
	public class Exam : Base.Entity
	{
		public Exam() : base()
		{
		}


		public User User { get; set; }
		public Guid? UserId { get; set; }
		public DateTime? PocessingExamDate { get; set; }
		public CompleteResult CompleteResult { get; set; }
		public List<PrimitiveResult> PrimitiveResults { get; set; }
	}

	public class PrimitiveResult : Base.Entity
	{
		public PrimitiveResult() : base()
		{
		}


		public bool IsCorrect { get; set; }
		public bool IsUnanswer { get; set; }
		public string Question { get; set; }
		public string YourAnswer { get; set; }
		public string CorrectAnswer { get; set; }
	}

	public class CompleteResult : Base.Entity
	{
		public CompleteResult() : base()
		{
		}


		public int UnanswerCount { get; set; }
		public int QuestionsCount { get; set; }
		public int CorrectAnswersCount { get; set; }
		public int IncorrectAnswersCount { get; set; }
	}
}
