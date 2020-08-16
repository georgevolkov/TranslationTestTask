using System;
using System.Threading.Tasks;
using TranslateAppDAL.Domain;

namespace TranslateAppDAL.Repositories.Interfaces
{
   public interface ITranslationRepository : IRepository<Translations>
   {
      [Obsolete("Use CreateTranslation method", true)]
      int SaveTranslation(Translations model);
      Task DeleteTranslation(int id);
      Task UpdateTranslation(Translations translation);
      Task CreateTranslation(Translations model);
   }
}
