using Dtat.Results;
using System.Collections.Generic;
using ViewModels.Requests;
using ViewModels.Responses;

namespace Services
{
	public partial  class WordServices
	{
		#region Check Validation Methods
		//ForAddNewWordValidation
		public Result AddNewWordValidation
			(AddWordRequestViewModel addWordRequestViewModel)
		{
			var result =
				new Dtat.Results.Result();

			if (addWordRequestViewModel == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNull,
					nameof(addWordRequestViewModel));

				result.AddErrorMessage(errorMessage);
				return result;
			}

			if (string.IsNullOrWhiteSpace(addWordRequestViewModel.Word))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(addWordRequestViewModel.Word), nameof(addWordRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			if (string.IsNullOrWhiteSpace(addWordRequestViewModel.Source))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(addWordRequestViewModel.Source), nameof(addWordRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			if (string.IsNullOrWhiteSpace(addWordRequestViewModel.EnglishTranslation))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(addWordRequestViewModel.EnglishTranslation), nameof(addWordRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			if (string.IsNullOrWhiteSpace(addWordRequestViewModel.PersianTranslation))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(addWordRequestViewModel.PersianTranslation), nameof(addWordRequestViewModel));

				result.AddErrorMessage(errorMessage);
			}

			if (addWordRequestViewModel.WordTypeId <= 0 || addWordRequestViewModel.WordTypeId >= 8)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.InvalidWordTypeValue);

				result.AddErrorMessage(errorMessage);
			}

			return result;

		}

		public Result UpdateWordValidation(AddWordRequestViewModel word)
		{
			var result =
				new Dtat.Results.Result();

			if (word == null)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNull,
					nameof(word));

				result.AddErrorMessage(errorMessage);
				return result;
			}

			if (string.IsNullOrWhiteSpace(word.Word))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(word.Word), nameof(word));

				result.AddErrorMessage(errorMessage);
			}

			if (string.IsNullOrWhiteSpace(word.Source))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(word.Source), nameof(word));

				result.AddErrorMessage(errorMessage);
			}

			if (string.IsNullOrWhiteSpace(word.EnglishTranslation))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(word.EnglishTranslation), nameof(word));

				result.AddErrorMessage(errorMessage);
			}

			if (string.IsNullOrWhiteSpace(word.PersianTranslation))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn,
					nameof(word.PersianTranslation), nameof(word));

				result.AddErrorMessage(errorMessage);
			}

			if (word.WordTypeId <= 0 || word.WordTypeId >= 8)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.InvalidWordTypeValue);

				result.AddErrorMessage(errorMessage);
			}

			return result;

		}

		public Result<GetWordResponseViewModel> GetWordValidation(string word)
		{
			var result =
				new Dtat.Results.Result<GetWordResponseViewModel>();

			if (string.IsNullOrWhiteSpace(word))
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNullWithIn, nameof(word), nameof(word));

				result.AddErrorMessage(errorMessage);
			}

			return result;
		}

		public Result<List<GetExamResponseViewModel>> 
			GetExamValidation(GetExamRequestViewModel getExamRequestViewModel)
		{
			var result =
				new Dtat.Results.Result<List<GetExamResponseViewModel>>();

			if (getExamRequestViewModel.QuestionsCount < 1 || getExamRequestViewModel.QuestionsCount > 100)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.InvalidQuestionCountValue);

				result.AddErrorMessage(errorMessage);
			}

			return result;
		}

		public Result<ExamProcessingResponseViewModel>
			ExamProcessingValidation(List<ExamProcessingRequestViewModel> examProcessingRequestViewModels)
		{
			var result =
				new Dtat.Results.Result<ExamProcessingResponseViewModel>();

			if (examProcessingRequestViewModels == null || examProcessingRequestViewModels.Count == 0)
			{
				string errorMessage = string.Format
					(Resources.Messages.ErrorMessages.MostNotBeNull, examProcessingRequestViewModels);

				result.AddErrorMessage(errorMessage);
			}

			var index = 0;
			foreach (var exam in examProcessingRequestViewModels)
			{
				if (string.IsNullOrWhiteSpace(exam.Question))
				{
					string errorMessage = string.Format
						(Resources.Messages.ErrorMessages.MostNotBeNullWithIn, 
							nameof(exam.Question), nameof(examProcessingRequestViewModels) + nameof(exam.Question) + $"[{index}]");

					result.AddErrorMessage(errorMessage);
				}

				index++;
			}

			return result;
		}

		#endregion /Check Validation Methods
	}
}
