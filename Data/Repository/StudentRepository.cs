using AutoMapper;
using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CollegeDBContext _context;
        private readonly IMapper _mapper;

        public StudentRepository(CollegeDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Student> AddAsync(StudentDTO studentDTO)
        {
            var studentEntity = _mapper.Map<Student>(studentDTO);
            await _context.Students.AddAsync(studentEntity);
            await _context.SaveChangesAsync();
            return studentEntity;
        }

        public async Task<bool> DeleteAsync(int studentId)
        {
            var studentEntity = await _context.Students.FirstOrDefaultAsync(x => x.Id == studentId);
            if (studentEntity == null) return false;
            _context.Students.Remove(studentEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            var students = await _context.Students.ToListAsync();
            return students;
        }

        public async Task<StudentDTO> GetByIdAsync(int studentId)
        {
            var studentEntity = await _context.Students.FirstOrDefaultAsync(x => x.Id == studentId);
            return studentEntity == null ? null : _mapper.Map<StudentDTO>(studentEntity);
        }

        public async Task<List<StudentDTO>> GetByNameAsync(string studentName)
        {
            var students = await _context.Students
                                          .Where(x => x.Name.Contains(studentName))
                                          .ToListAsync();
            return _mapper.Map<List<StudentDTO>>(students);
        }

        public async Task<StudentDTO> UpdateAsync(int studentId, StudentDTO studentDTO)
        {
            var studentEntity = await _context.Students.FirstOrDefaultAsync(x => x.Id == studentId);
            if (studentEntity == null) return null;
            _mapper.Map(studentDTO, studentEntity);
            _context.Students.Update(studentEntity);
            await _context.SaveChangesAsync();
            return _mapper.Map<StudentDTO>(studentEntity);
        }

        public async Task<StudentDTO> UpdateStudentPartialAsync(int studentId, JsonPatchDocument<StudentDTO> patchDocument, ModelStateDictionary modelState)
        {
            var studentEntity = await _context.Students.FirstOrDefaultAsync(x => x.Id == studentId);
            if (studentEntity == null) return null;

            var studentDTO = _mapper.Map<StudentDTO>(studentEntity);
            patchDocument.ApplyTo(studentDTO, modelState);

            if (!modelState.IsValid)
                throw new Exception("Invalid patch document");

            _mapper.Map(studentDTO, studentEntity);
            _context.Students.Update(studentEntity);
            await _context.SaveChangesAsync();
            return studentDTO;
        }
    }
}
