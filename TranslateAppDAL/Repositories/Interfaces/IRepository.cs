using System.Linq;
using System.Threading.Tasks;

namespace TranslateAppDAL.Repositories.Interfaces
{
   public interface IRepository<TEntity> where TEntity : class, new()
   {
      IQueryable<TEntity> GetAllTranslations();

      Task<TEntity> AddAsync(TEntity entity);

      Task<TEntity> UpdateAsync(TEntity entity);
   }
}
