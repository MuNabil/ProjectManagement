using Domain.DTOs;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevelopersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DevelopersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var developer = await _unitOfWork.Developers.GetById(id);

            if (developer == null)
                return NotFound("Invalid ID");

            return Ok(developer);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _unitOfWork.Developers.GetAll());
        }

        [HttpGet("GetPopularDevelopers")]
        public IActionResult GetPopularDevelopers([FromQuery] int count)
        {
            return Ok(_unitOfWork.Developers.GetPopularDevelopers(count));
        }

        [HttpGet("CountAll")]
        public async Task<IActionResult> CountAll()
        {
            return Ok(await _unitOfWork.Developers.Count());
        }

        [HttpGet("CountByType")]
        public async Task<IActionResult> CountByType([FromQuery] string type)
        {
            var developers = await _unitOfWork.Developers.Count(d => d.Type.ToLower() == type.Trim().ToLower());
            return Ok(developers);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] DeveloperDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var developer = new Developer();
            developer.Name = model.Name;
            if (!string.IsNullOrEmpty(model.Type))
                developer.Type = model.Type;
            else
                developer.Type = "Software";

            if (model.Followers.HasValue)
                developer.Followers = model.Followers;
            else
                developer.Followers = 0;

            await _unitOfWork.Developers.Add(developer);
            _unitOfWork.Commit();

            return Ok(developer);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var developer = await _unitOfWork.Developers.GetById(id);

            if (developer == null)
                return NotFound($"NO developer was found with id: {id}");

            _unitOfWork.Developers.Delete(developer);
            _unitOfWork.Commit();

            return Ok(developer);
        }
    }
}
