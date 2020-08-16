using System;
using System.Linq;
using TranslateAppDAL;

namespace TranslateApplication.Models.Translation
{
   public class TranslateWordProcessor
   {
      private readonly TranslateDbContext _dbContext;

      public TranslateWordProcessor(TranslateDbContext _dbContext)
      {
         this._dbContext = _dbContext;
      }

      public string TranslateFromLanguage(TranslateModel model)
      {
         var result = _dbContext.Translations
            .FirstOrDefault(x => string.Equals(x.SourceWord, model.SourceWord, StringComparison.CurrentCultureIgnoreCase) && x.SourceLanguageId == (int) model.SourceLanguage)
            ?.Translation;

         return result;
      }
   }
}
