using Application.Dtos;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CmbController : ControllerBase
    {
        protected readonly ICmbService _cmbService;
        public CmbController(ICmbService cmbService)
        {
            _cmbService = cmbService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _cmbService.Get(id);

            return response;
        }

        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery] ReadCmbRequest request)
        {
            var response = await _cmbService.Get(request);

            return response;
        }

        [HttpPost()]
        public async Task<IActionResult> Create(CreateCmbRequest request)
        {
            var response = await _cmbService.Create(request);

            return response;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCmbRequest request)
        {
            var response = await _cmbService.Update(id, request);

            return response;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _cmbService.Delete(id);

            return response;
        }
    }
}
