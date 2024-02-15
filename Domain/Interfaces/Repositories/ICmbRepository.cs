using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface ICmbRepository
    {
        Task<Cmb> GetAsync(int id);
        Task<List<Cmb>> GetAllAsync(ExtendedCmb? entity = null);
        Task<IQueryable<Cmb>> GetAllAsync(ExtendedCmb? entity = null, int pageNumber = 1, int pageSize = 0);
        Task<Cmb> AddAsync(ExtendedCmb entity);
        Task<Cmb> UpdateByIdAsync(int id, ExtendedCmb entity);
        Task<Cmb> UpdateAsync(ExtendedCmb entity);
        Task<Cmb> DeleteByIdAsync(int id);
        Task<Cmb> DeleteAsync(ExtendedCmb entity);
        Task<List<Cmb>> BatchUpdateAsync(List<ExtendedCmb> entities);
        Task SaveChangesAsync();
    }
}
