namespace ViewModels.Requests
{
	public class AddWordRequestViewModel
	{
		public string Word { get; set; }
		public string Source { get; set; }
		public int WordTypeId { get; set; }
		public int VerbTenseId { get; set; }
		public string Description { get; set; }
		public string PersianTranslation { get; set; }
		public string EnglishTranslation { get; set; }
	}
}
