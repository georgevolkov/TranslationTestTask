using System;
using System.Linq;
using System.Threading.Tasks;
using TranslateAppDAL.Domain;
using TranslateAppDAL.Repositories.Interfaces;

namespace TranslateAppDAL.Repositories
{
   public class TranslationRepository : Repository<Translations>, ITranslationRepository
   {
      private readonly TranslateDbContext _translateDbContext;

      public TranslationRepository(TranslateDbContext translateDbContext) : base(translateDbContext)
      {
         _translateDbContext = translateDbContext;
      }

      public int SaveTranslation(Translations model)
      {
         _translateDbContext.Translations.Add(model);
         return _translateDbContext.SaveChanges();
      }

      public async Task DeleteTranslation(int id)
      {
         var model = _translateDbContext.Translations.FirstOrDefault(x => x.Id == id);
         var reverseModel = _translateDbContext.Translations.FirstOrDefault(x =>
            string.Equals(x.SourceWord, model.Translation, StringComparison.OrdinalIgnoreCase)
            && string.Equals(x.Translation, model.SourceWord, StringComparison.OrdinalIgnoreCase));

         if (model == null)
            throw new NullReferenceException($"Не найдена модель с id: {id}");

         if (reverseModel == null)
            throw new NullReferenceException($"Не заполнено обратное слово для: {model.SourceWord}");

         _translateDbContext.Translations.Remove(model);
         _translateDbContext.Translations.Remove(reverseModel);

         await _translateDbContext.SaveChangesAsync();
      }

      public async Task UpdateTranslation(Translations model)
      {
         var reverseModel = _translateDbContext.Translations.FirstOrDefault(x =>
                               string.Equals(x.SourceWord, model.Translation, StringComparison.OrdinalIgnoreCase)
                               && string.Equals(x.Translation, model.SourceWord, StringComparison.OrdinalIgnoreCase)) ??
                            new Translations();

         reverseModel.SourceWord = model.Translation;
         reverseModel.SourceLanguageId = model.TranslationLanguageId;
         reverseModel.Translation = model.SourceWord;
         reverseModel.TranslationLanguageId = model.SourceLanguageId;
         await UpdateAsync(model);
         await UpdateAsync(reverseModel);
      }

      public async Task CreateTranslation(Translations model)
      {
         var reversedTranslation = new Translations
         {
            SourceLanguageId = model.TranslationLanguageId, SourceWord = model.Translation,
            Translation = model.SourceWord, TranslationLanguageId = model.SourceLanguageId
         };
         await AddAsync(model);
         await AddAsync(reversedTranslation);
      }
   }
}
