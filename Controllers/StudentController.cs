using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController(CollegeDBContext dbContext, ILogger<StudentController> logger,IMapper mapper) : Controller
    {
        private readonly ILogger<StudentController> _logger = logger;
        private readonly CollegeDBContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        // POST: api/Student/Create
        [HttpPost("Create", Name = "CreateStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> AddStudent([FromBody] StudentDTO? newStudentDTO)
        {
            if (!ModelState.IsValid) return BadRequest();

            if (newStudentDTO.AdmissionDate < DateTime.Now)
            {
                ModelState.AddModelError("Invalid Admission Date", "Admission date is of past");
                return BadRequest(ModelState);
            }


            var newStudent = _mapper.Map<Student>(newStudentDTO);

            await _dbContext.Students.AddAsync(newStudent);
            await _dbContext.SaveChangesAsync();

            return CreatedAtRoute("GetStudentById", new { id = newStudent.Id }, newStudent);
        }

        // PUT: api/Student/Update
        [HttpPut("Update/{id:int}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> UpdateStudent([FromRoute] int id,[FromBody] StudentDTO existingStudent)
        {
            if (!ModelState.IsValid) return BadRequest();

            var foundStudent = await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (foundStudent is null) return NotFound();

            _mapper.Map(existingStudent, foundStudent);

            _dbContext.Update(foundStudent);  // Explicitly updating
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/Student/UpdatePartial/{id}
        [HttpPatch("UpdatePartial/{id:int}", Name = "UpdateStudentPartial")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStudentPartial([FromRoute] int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (!ModelState.IsValid) return BadRequest();

            if (id < 0) return BadRequest();

            var foundStudent = await _dbContext.Students.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (foundStudent is null) return NotFound();

            var updatedStudent = _mapper.Map<StudentDTO>(foundStudent);

            patchDocument.ApplyTo(updatedStudent, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            _mapper.Map(updatedStudent, foundStudent);

            _dbContext.Update(foundStudent);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Student/All
        [HttpGet("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            _logger.LogInformation("Get All Students Triggered");
            var students = await _dbContext.Students.AsNoTracking().ToListAsync(); // Read-only query
            var studentsDTO = _mapper.Map <List<StudentDTO>>(students);
            return Ok(studentsDTO);
        }

        // GET: api/Student/{id}
        [HttpGet("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO?>> GetStudentById([FromRoute] int id)
        {
            if (id < 0) return BadRequest();

            var foundStudent = await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id)); // Read-only query
            if (foundStudent is null) return NotFound();

            var foundStudentDTO = _mapper.Map<StudentDTO>(foundStudent);
            return Ok(foundStudentDTO);
        }

        // GET: api/Student/{name}
        [HttpGet("{name}", Name = "GetStudentByName")]
        public async Task<ActionResult<StudentDTO?>> GetStudentbyName([FromRoute] string name)
        {
            var foundStudent = await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name); // Read-only query
            if (foundStudent is null) return NotFound();

            var foundStudentDTO = _mapper.Map<StudentDTO>(foundStudent);  
            return Ok(foundStudentDTO);
        }

        // DELETE: api/Student/{id}
        [HttpDelete("{id:int}", Name = "DeleteStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (id < 0) return BadRequest();

            var foundStudent = await _dbContext.Students.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (foundStudent is null) return NotFound();

            _dbContext.Students.Remove(foundStudent);
            await _dbContext.SaveChangesAsync(); // Save changes after deletion

            return Ok(true);
        }

        [HttpGet("Logger", Name = "Logger Request")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult LogCheck()
        {
            _logger.LogTrace("Trace Logged");
            _logger.LogDebug("Debug Logged");
            _logger.LogInformation("Information Logged");
            _logger.LogWarning("Warning Logged");
            _logger.LogError("Error Logged");
            _logger.LogCritical("Critical Logged");
            return Ok();
        }
    }
}
