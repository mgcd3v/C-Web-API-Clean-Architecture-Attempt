using Domain.Interfaces.Repositories.Base;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DataContext _dataContext;

        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public T GetFirstOrDefault(List<T> list, Func<T, bool>? predicate = null)
        {
            if (list == null || list.Count == 0)
            {
                // Return the default value for the type T
                return default;
            }

            // Return the first element in the list
            if(predicate == null)
            {
                return list.FirstOrDefault();
            }
            return list.FirstOrDefault(predicate);
        }

        public async Task<List<T>> GetFinalAllAsync(IQueryable<T> list, bool asNoTracking = true)
        {
            if(asNoTracking)
            {
                return await list.AsNoTracking().ToListAsync();
            }
            else
            {
                return await list.ToListAsync();
            }    
            
        }
    }
}
