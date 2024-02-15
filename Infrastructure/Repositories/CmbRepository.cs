using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;

namespace Infrastructure.Repositories
{
    public class CmbRepository : Repository<Cmb>, ICmbRepository
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        #region Public functions
        public CmbRepository(DataContext dataContext, IHttpContextAccessor httpContextAccessor) : base(dataContext)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #region Create
        public async Task<Cmb> AddAsync(ExtendedCmb entity)
        {
            var parameters = GetParameters(Constants.SP_CREATEPROCESSFLAG, entity);
            await _dataContext.Database.ExecuteSqlRawAsync(Helpers.GetSPCommand(Constants.SP_CREATEGENCMB, parameters));

            return entity;
        }
        #endregion

        #region Read
        public async Task<List<Cmb>> GetAllAsync(ExtendedCmb? entity = null)
        {
            var parameters = GetParameters(Constants.SP_READPROCESSFLAG, entity);
            var result = _dataContext.Cmbs.FromSqlRaw(Helpers.GetSPCommand(Constants.SP_READGENCMB, parameters));

            return await GetFinalAllAsync(result);
        }

        public async Task<IQueryable<Cmb>> GetAllAsync(ExtendedCmb? entity = null, int pageNumber = 1, int pageSize = 0)
        {
            var result = await GetAllAsync(entity);

            if (pageSize == 0)
            {
                return result.AsQueryable();
            }
            else
            {
                return result.AsQueryable().Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
        }

        public async Task<Cmb> GetAsync(int id)
        {
            var entity = new ExtendedCmb { Id = id };
            var result = await GetAllAsync(entity);

            return GetFirstOrDefault(result, o => o.Id == id);
        }
        #endregion

        #region Update
        public async Task<Cmb> UpdateByIdAsync(int id, ExtendedCmb entity)
        {
            var parameters = GetParameters(Constants.SP_UPDATEPROCESSFLAG, entity, id);
            await _dataContext.Database.ExecuteSqlRawAsync(Helpers.GetSPCommand(Constants.SP_UPDATEGENCMB, parameters));

            return entity;
        }

        public Task<Cmb> UpdateAsync(ExtendedCmb entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Cmb>> BatchUpdateAsync(List<ExtendedCmb> entities)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Delete
        public Task<Cmb> DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Cmb> DeleteAsync(ExtendedCmb entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Others
        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
        #endregion

        #endregion

        #region Private functions
        private static List<Parameter> GetParameters(string processFlag, ExtendedCmb? entity = null, int? id = null)
        {
            return new List<Parameter>()
            {
                new Parameter { Name = "@P_PROCESS_FLAG", Value = Helpers.GetFinalParameterValue(processFlag, Constants.SP_PARAM_NULLVALUE, "'") },
                new Parameter { Name = "@P_FIELD", Value = Helpers.GetFinalParameterValue(entity?.Field, Constants.SP_PARAM_NULLVALUE, "'") },
            };
        }
        #endregion
    }
}
