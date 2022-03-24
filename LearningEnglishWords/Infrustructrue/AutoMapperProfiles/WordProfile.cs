using ViewModels.Requests;
using ViewModels.Responses;

namespace Infrustructrue.AutoMapperProfiles
{
	public class WordProfile : AutoMapper.Profile
	{
		public WordProfile() : base()
		{
			//For Words && AddWordRequestViewModel
			CreateMap<Domain.Entities.Words, AddWordRequestViewModel>();

			CreateMap<AddWordRequestViewModel, Domain.Entities.Words>();

			CreateMap<Domain.Entities.Words, GetWordResponseViewModel>();

			CreateMap<GetWordResponseViewModel, Domain.Entities.Words>();

			CreateMap<Domain.Entities.Words, GetWordResponseViewModel>()
				.ForMember(dest => dest.WordType, opt => opt.MapFrom(current => current.WordType.Type))
					.ForMember(dest => dest.VerbTense, opt => opt.MapFrom(current => current.VerbTense.Tense));
		}
	}
}
