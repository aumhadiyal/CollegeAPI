using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace CollegeApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentRepository _studentRepository;

        public StudentController(IStudentRepository studentRepository, ILogger<StudentController> logger)
        {
            _studentRepository = studentRepository;
            _logger = logger;
        }

        // POST: api/Student
        [HttpPost("Create")]
        public async Task<ActionResult<StudentDTO>> AddStudent([FromBody] StudentDTO newStudentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (newStudentDTO.AdmissionDateDTO < DateTime.Now)
            {
                ModelState.AddModelError("Invalid Admission Date", "Admission date cannot be in the past");
                return BadRequest(ModelState);
            }

            var addedStudent = await _studentRepository.AddAsync(newStudentDTO);
            return CreatedAtAction(nameof(GetStudentById), new { id = addedStudent.Id }, newStudentDTO);
        }
            
        // PUT: api/Student/{id}
        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] int id, [FromBody] StudentDTO studentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedStudent = await _studentRepository.UpdateAsync(id, studentDTO);
            if (updatedStudent == null)
                return NotFound();

            return NoContent();
        }

        // PATCH: api/Student/UpdatePartial/{id}
        [HttpPatch("UpdatePartial/{id:int}")]
        public async Task<IActionResult> UpdateStudentPartial([FromRoute] int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var patchedStudent = await _studentRepository.UpdateStudentPartialAsync(id, patchDocument, ModelState);
            if (patchedStudent == null)
                return NotFound();

            return NoContent();
        }

        // GET: api/Student
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var students = await _studentRepository.GetAllAsync();
            return Ok(students);
        }

        // GET: api/Student/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<StudentDTO>> GetStudentById([FromRoute] int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                return NotFound();

            return Ok(student);
        }

        // GET: api/Student/{name}
        [HttpGet("ByName/{name}")]
        public async Task<ActionResult<List<StudentDTO>>> GetStudentByName([FromRoute] string name)
        {
            var students = await _studentRepository.GetByNameAsync(name);
            return Ok(students);
        }

        // DELETE: api/Student/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            var isDeleted = await _studentRepository.DeleteAsync(id);
            if (!isDeleted)
                return NotFound();

            return Ok(isDeleted);
        }
    }
}
