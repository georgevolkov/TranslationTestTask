using AutoMapper;
using TranslateAppDAL.Domain;
using TranslateApplication.Models;

namespace TranslateApplication.Automapper
{
   public class TranslationProfile : Profile
   {
      public TranslationProfile()
      {
         CreateMap<FillTranslationModel, Translations>();
         CreateMap<Translations, FillTranslationModel>();

         CreateMap<TranslateModel, Translations>()
            .ForMember(x => x.SourceWord, o => o.MapFrom(opt => opt.SourceWord))
            .ForMember(x => x.Translation, o => o.MapFrom(opt => opt.DestinationWord));
         CreateMap<Translations, TranslateModel>()
            .ForMember(x => x.SourceWord, o => o.MapFrom(opt => opt.SourceWord))
            .ForMember(x => x.DestinationWord, o => o.MapFrom(opt => opt.Translation)); ;
      }
   }
}
