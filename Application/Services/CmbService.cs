using Application.Dtos;
using Application.Dtos.Common;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Helpers;

namespace Application.Services
{
    public class CmbService : ICmbService
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IMapper _mapper;

        protected readonly IEmailService _emailService;

        protected readonly ICmbRepository _cmbRepository;
        public CmbService(IHttpContextAccessor httpContextAccessor, IMapper mapper, IEmailService emailService, ICmbRepository cmbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;

            _emailService = emailService;

            _cmbRepository = cmbRepository;
        }

        public async Task<IActionResult> Get(int id)
        {
            var cmb = await _cmbRepository.GetAsync(id);

            if (cmb == null)
            {
                return new NotFoundObjectResult(Constants.MSG_CMBNOTFOUND);
            }

            var cmbResponse = _mapper.Map<CmbResponse>(cmb);

            return await Task.Run(() => new OkObjectResult(cmbResponse));
        }

        public async Task<IActionResult> Get(ReadCmbRequest request)
        {
            var mappedLeaveEntitlement = _mapper.Map<ExtendedCmb>(request);
            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            // Define parameters for GetAllAsync() to return IQueryable not List
            var cmb = await PagedList<Cmb>.ToPagedList(await _cmbRepository.GetAllAsync(mappedLeaveEntitlement, 1, 0), pageNumber, pageSize);

            var metadata = new
            {
                cmb.TotalCount,
                cmb.PageSize,
                cmb.CurrentPage,
                cmb.TotalPages,
                cmb.HasNext,
                cmb.HasPrevious
            };

            _httpContextAccessor.HttpContext.Response.Headers.Add(Constants.PAGINATION_HEADER_NAME, Helpers.SerializeObject(metadata));

            return await Task.Run(() => new OkObjectResult(cmb));
        }

        public Task<IActionResult> Create(CreateCmbRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> Update(int id, UpdateCmbRequest request)
        {
            // Send test
            await _emailService.SendVerificationCode("user@email.com", "1111", new EmailSetting { });

            var cmb = await _cmbRepository.GetAsync(id);

            if (cmb != null)
            {
                if (id != cmb.Id)
                {
                    return new BadRequestObjectResult(Constants.MSG_CMBEXISTS);
                }
            }

            var mappedCmb = _mapper.Map<ExtendedCmb>(request);

            await _cmbRepository.UpdateByIdAsync(id, mappedCmb);

            var updateCmb = await _cmbRepository.GetAsync(id);

            var cmbResponse = _mapper.Map<CmbResponse>(updateCmb);

            return await Task.Run(() => new OkObjectResult(cmbResponse));
        }

        public Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
