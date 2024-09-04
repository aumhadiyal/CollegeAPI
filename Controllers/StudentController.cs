using CollegeApp.Data;
using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController(CollegeDBContext dbContext, ILogger<StudentController> logger) : Controller
    {


        private readonly ILogger<StudentController> _logger = logger;
        private readonly CollegeDBContext _dbContext = dbContext;



        // POST: api/Student/Create
        // Adds a new student to the repository.
        // Returns 201 Created if successful, 400 Bad Request if validation fails, or 500 Internal Server Error for other errors.
        [HttpPost("Create", Name = "CreateStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> AddStudent([FromBody] StudentDTO? newstudent)
        {
            // Check if the provided model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Validate the Admission Date (should not be in the past)
            if (newstudent.AdmissionDate < DateTime.Now)
            {
                ModelState.AddModelError("Invalid Admission Date", "Admission date is of past");
                return BadRequest(ModelState);
            }
            var id = new Guid();
            var std = new Student()
            {
                Id = id,
                Address = newstudent.Address,
                Email = newstudent.Email,
                AdmissionDate = newstudent.AdmissionDate,
                Name = newstudent.Name
            };

            _dbContext.Students.Add(std);
            _dbContext.SaveChanges();

            // Return the created student information with a 201 status
            return CreatedAtRoute("GetStudentById", new { id = std.Id }, std);
        }

        // PUT: api/Student/Update
        // Updates an existing student's information in the repository.
        // Returns 204 No Content if successful, 400 Bad Request if validation fails, or 500 Internal Server Error for other errors.
        [HttpPut("Update", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> UpdateStudent([FromBody] Student existingstudent)
        {
            // Check if the provided model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Find the student by ID in the repository
            var std = _dbContext.Students.FirstOrDefault(x => x.Id == existingstudent.Id);
            if (std is null)
            {
                return NotFound();
            }

            // Update student details
            std.Name = existingstudent.Name;
            std.Email = existingstudent.Email;
            std.Address = existingstudent.Address;
            std.AdmissionDate = existingstudent.AdmissionDate;

            // Return no content to indicate success
            return NoContent();
        }


        // PATCH: api/Student/UpdatePartial/{id}
        // Partially updates an existing student's information in the repository.
        // Returns 204 No Content if successful, 400 Bad Request if validation fails, or 500 Internal Server Error for other errors.
        [HttpPatch("UpdatePartial/{id:int}", Name = "UpdateStudentPartial")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudentPartial([FromRoute] int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            // Check if the provided model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Validate the student ID (should be positive)
            if (id < 0)
            {
                return BadRequest();
            }

            // Find the student by ID in the repository
            var std = _dbContext.Students.FirstOrDefault(x => x.Id.Equals(id));
            if (std is null)
            {
                return NotFound();
            }

            // Map the existing student entity to a DTO
            var updatedStudent = new StudentDTO()
            {
                Name = std.Name,
                Email = std.Email,
                Address = std.Address,
                AdmissionDate = std.AdmissionDate,
            };

            // Apply the JSON Patch document to the DTO
            patchDocument.ApplyTo(updatedStudent, ModelState);

            // Check if the patch operation was successful
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Update the student entity with the patched values
            std.Name = updatedStudent.Name;
            std.Email = updatedStudent.Email;
            std.Address = updatedStudent.Address;
            std.AdmissionDate = updatedStudent.AdmissionDate;

            // Return no content to indicate success
            return NoContent();
        }



        // GET: api/Student/All
        // Retrieves all students from the repository.
        // Returns 200 OK with the list of students, or 500 Internal Server Error for other errors.
        [HttpGet("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<StudentDTO>> GetStudents()
        {
            _logger.LogInformation("Gell All Students Triggered");
            // Map the list of students to StudentDTO objects
            var students = _dbContext.Students.ToList();

            // Return the list of students
            return Ok(students);
        }

        // GET: api/Student/{id}
        // Retrieves a student by their ID.
        // Returns 200 OK if found, 400 Bad Request if the ID is invalid, or 404 Not Found if the student doesn't exist.
        [HttpGet("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO?> GetStudentById([FromRoute] int id)
        {
            // Validate the student ID (should be positive)
            if (id < 0)
            {
                _logger.LogWarning("Bad Request Error");
                return BadRequest();
            }

            // Find the student by ID in the repository
            var std = _dbContext.Students.FirstOrDefault(x => x.Id.Equals(id));
            if (std is null)
            {
                _logger.LogError("Not Found Error");
                return NotFound();
            }

            // Map the student to a StudentDTO object
            var stdd = new StudentDTO
            {
                Name = std.Name,
                Email = std.Email,
                Address = std.Address,
                AdmissionDate = std.AdmissionDate,
            };

            // Return the student details
            return Ok(stdd);
        }

        // GET: api/Student/{name}
        // Retrieves a student by their name.
        // Returns 200 OK if found, or 404 Not Found if the student doesn't exist.
        [HttpGet("{name}", Name = "GetStudentByName")]
        public ActionResult<StudentDTO?> GetStudentbyName([FromRoute] string name)
        {
            // Find the student by name in the repository
            var std = _dbContext.Students.FirstOrDefault(x => x.Name == name);
            if (std is null)
            {
                return NotFound();
            }

            // Map the student to a StudentDTO object
            var stdd = new StudentDTO
            {
                Name = std.Name,
                Email = std.Email,
                Address = std.Address,
                AdmissionDate = std.AdmissionDate,
            };

            // Return the student details
            return Ok(stdd);
        }

        // DELETE: api/Student/{id}
        // Deletes a student by their ID.
        // Returns 200 OK if successful, 400 Bad Request if the ID is invalid, or 404 Not Found if the student doesn't exist.
        [HttpDelete("{id:int}", Name = "DeleteStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteStudent(int id)
        {
            // Validate the student ID (should be positive)
            if (id < 0)
            {
                return BadRequest();
            }

            // Find the student by ID in the repository
            var std = _dbContext.Students.FirstOrDefault(x => x.Id.Equals(id));
            if (std is null)
            {
                return NotFound();
            }

            // Remove the student from the repository
            _dbContext.Students.Remove(std);

            // Return OK to indicate success
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
