namespace Infrustructrue.AutoMapperProfiles
{
	public class WordProfile : AutoMapper.Profile
	{
		public WordProfile() : base()
		{
			//For Words && AddWordRequestViewModel
			CreateMap<Word, AddWordRequestViewModel>().ReverseMap();

			CreateMap<Word, GetWordResponseViewModel>().ReverseMap();

			CreateMap<Word, GetWordResponseViewModel>()
				.ForMember(dest => dest.WordType, opt => opt.MapFrom(current => current.WordType.Type))
					.ForMember(dest => dest.VerbTense, opt => opt.MapFrom(current => current.VerbTense.Tense));
		}
	}
}
