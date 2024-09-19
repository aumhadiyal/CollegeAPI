using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public interface IStudentRepository
{
    Task<Student> AddAsync(StudentDTO studentDTO);
    Task<StudentDTO> UpdateAsync(int studentId, StudentDTO studentDTO);
    Task<StudentDTO> UpdateStudentPartialAsync(int studentId, JsonPatchDocument<StudentDTO> patchDocument, ModelStateDictionary modelState);
    Task<StudentDTO> GetByIdAsync(int studentId);
    Task<List<Student>> GetAllAsync();
    Task<List<StudentDTO>> GetByNameAsync(string studentName);
    Task<bool> DeleteAsync(int studentId);
}
