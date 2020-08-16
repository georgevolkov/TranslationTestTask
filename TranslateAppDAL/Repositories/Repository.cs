using System;
using System.Linq;
using System.Threading.Tasks;
using TranslateAppDAL.Repositories.Interfaces;

namespace TranslateAppDAL.Repositories
{
   public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
   {
      private readonly TranslateDbContext _translateDbContext;

      public Repository(TranslateDbContext translateDbContext)
      {
         _translateDbContext = translateDbContext;
      }

      public IQueryable<TEntity> GetAllTranslations()
      {
         try
         {
            return _translateDbContext.Set<TEntity>();
         }
         catch (Exception)
         {
            throw new Exception("Невозможно вернуть записи");
         }
      }

      public async Task<TEntity> AddAsync(TEntity entity)
      {
         if (entity == null)
         {
            throw new ArgumentNullException($"{nameof(AddAsync)} сущность не должна быть пустой");
         }

         try
         {
            await _translateDbContext.AddAsync(entity);
            await _translateDbContext.SaveChangesAsync();

            return entity;
         }
         catch (Exception)
         {
            throw new Exception($"{nameof(entity)} не может быть сохранено");
         }
      }

      public async Task<TEntity> UpdateAsync(TEntity entity)
      {
         if (entity == null)
         {
            throw new ArgumentNullException($"{nameof(AddAsync)} сушьность не должна быть пустой");
         }

         try
         {
            _translateDbContext.Update(entity);
            await _translateDbContext.SaveChangesAsync();

            return entity;
         }
         catch (Exception)
         {
            throw new Exception($"{nameof(entity)} не может быть обновлено");
         }
      }
   }
}
