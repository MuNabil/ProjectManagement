using Domain.DTOs;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _unitOfWork.Projects.Find(p => p.Id == id, new[] { "Developer" });

            if (project == null)
                return NotFound("Invalid ID");

            return Ok(project);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _unitOfWork.Projects.FindAll(p => p.Id.HasValue, new[] { "Developer" }));
        }

        [HttpGet("CountAll")]
        public async Task<IActionResult> CountAll()
        {
            return Ok(await _unitOfWork.Projects.Count());
        }

        [HttpGet("SearchInProjectNameAndDescription")]
        public async Task<IActionResult> SearchInProjectNameAndDescription([FromQuery] string term)
        {
            if (string.IsNullOrEmpty(term))
                return BadRequest("Search For What..!!!!");

            var projects = await _unitOfWork.Projects.FindAll(p => p.Name.ToLower().Contains(term.Trim().ToLower()) || p.Description.ToLower().Contains(term.Trim().ToLower()),
                null, null, new[] { "Developer" });

            return Ok(projects);
        }

        [HttpGet("FindByDateInRange")]
        public async Task<IActionResult> FindByDateInRange([FromQuery] DateRangeDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var projects = await _unitOfWork.Projects.FindAll(p => p.CreatedDate >= model.FromStartDate && p.CreatedDate <= model.ToEndDate,
                null, null, new[] { "Developer" }, p => p.CreatedDate);

            return Ok(projects);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] ProjectDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var developer = await _unitOfWork.Developers.GetById(model.DeveloperId);

            if(developer == null)
                return NotFound($"There is no developer with ID:{model.DeveloperId}");

            if (model.CreatedDate == null)
                model.CreatedDate = DateTime.Now;
            else
                if (model.CreatedDate > DateTime.Now)
                    return BadRequest($"Invalid Date {model.CreatedDate}");

            var project = new Project();
            project.Name = model.Name;
            project.Description = model.Description;
            project.CreatedDate = model.CreatedDate;
            project.DeveloperId = model.DeveloperId;

            await _unitOfWork.Projects.Add(project);
            _unitOfWork.Commit();

            return Ok(project);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _unitOfWork.Projects.GetById(id);

            if (project == null)
                return NotFound($"NO project was found with id: {id}");

            _unitOfWork.Projects.Delete(project);
            _unitOfWork.Commit();

            return Ok(project);
        }
    }
}
