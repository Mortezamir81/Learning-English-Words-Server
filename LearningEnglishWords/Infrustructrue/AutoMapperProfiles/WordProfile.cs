using ViewModels.Requests;
using ViewModels.Responses;

namespace Infrustructrue.AutoMapperProfiles
{
	public class WordProfile : AutoMapper.Profile
	{
		public WordProfile() : base()
		{
			//For Words && AddWordRequestViewModel
			CreateMap<Domain.Entities.Word, AddWordRequestViewModel>();

			CreateMap<AddWordRequestViewModel, Domain.Entities.Word>();

			CreateMap<Domain.Entities.Word, GetWordResponseViewModel>();

			CreateMap<GetWordResponseViewModel, Domain.Entities.Word>();

			CreateMap<Domain.Entities.Word, GetWordResponseViewModel>()
				.ForMember(dest => dest.WordType, opt => opt.MapFrom(current => current.WordType.Type))
					.ForMember(dest => dest.VerbTense, opt => opt.MapFrom(current => current.VerbTense.Tense));
		}
	}
}
