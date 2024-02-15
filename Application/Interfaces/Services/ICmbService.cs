using Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Application.Interfaces.Services
{
    public interface ICmbService
    {
        Task<IActionResult> Get(int id);
        Task<IActionResult> Get(ReadCmbRequest request);
        Task<IActionResult> Create(CreateCmbRequest request);
        Task<IActionResult> Update(int id, UpdateCmbRequest request);
        Task<IActionResult> Delete(int id);
    }
}
