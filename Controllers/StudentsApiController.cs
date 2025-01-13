using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.DTOs;
using StudentManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

[ApiController]
[Route("api/[controller]")]
public class StudentsApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public StudentsApiController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
    {
        var students = await _context.Students
            .Include(s => s.University)
            .Include(s => s.Faculty)
            .ToListAsync();

        var studentDTOs = _mapper.Map<List<StudentDTO>>(students);
        return Ok(studentDTOs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDTO>> GetStudent(int id)
    {
        var student = await _context.Students
            .Include(s => s.University)
            .Include(s => s.Faculty)
            .FirstOrDefaultAsync(s => s.StudentID == id);

        if (student == null)
        {
            return NotFound();
        }

        var studentDTO = _mapper.Map<StudentDTO>(student);
        return Ok(studentDTO);
    }

    [HttpPost]
    public async Task<ActionResult<StudentDTO>> CreateStudent([FromBody] StudentDTO studentDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Map StudentDTO to Student
        var student = _mapper.Map<Student>(studentDTO);

        // Add the student to the database
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        // Map the saved Student back to StudentDTO
        var createdStudentDTO = _mapper.Map<StudentDTO>(student);

        // Return the created student with a 201 Created response
        return CreatedAtAction(nameof(GetStudent), new { id = createdStudentDTO.StudentID }, createdStudentDTO);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<StudentDTO>> UpdateStudentName(int id, [FromBody] UpdateStudentNameDTO updateStudentNameDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Find the student by ID
        var student = await _context.Students
            .Include(s => s.University)
            .Include(s => s.Faculty)
            .FirstOrDefaultAsync(s => s.StudentID == id);

        if (student == null)
        {
            return NotFound();
        }

        // Update the student's name
        student.StudentName = updateStudentNameDTO.StudentName;

        // Save changes to the database
        await _context.SaveChangesAsync();

        // Map the updated student to a DTO
        var updatedStudentDTO = _mapper.Map<StudentDTO>(student);

        // Return the updated student
        return Ok(updatedStudentDTO);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<StudentDTO>> PatchStudent(int id, [FromBody] JsonPatchDocument<PatchStudentDTO> patchDocument)
    {
        if (patchDocument == null)
        {
            return BadRequest("Patch document is null.");
        }

        // Find the student by ID
        var student = await _context.Students
            .Include(s => s.University)
            .Include(s => s.Faculty)
            .FirstOrDefaultAsync(s => s.StudentID == id);

        if (student == null)
        {
            return NotFound($"Student with ID {id} not found.");
        }

        // Map the student to a PatchStudentDTO
        var studentToPatch = _mapper.Map<PatchStudentDTO>(student);

        // Apply the patch document to the DTO
        patchDocument.ApplyTo(studentToPatch, error =>
        {
            // Add any patch errors to the ModelState
            ModelState.AddModelError(error.AffectedObject.ToString(), error.ErrorMessage);
        });

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Map the patched DTO back to the student entity
        _mapper.Map(studentToPatch, student);

        // Save changes to the database
        await _context.SaveChangesAsync();

        // Return the updated student as a DTO
        var updatedStudentDTO = _mapper.Map<StudentDTO>(student);
        return Ok(updatedStudentDTO);
    }
}