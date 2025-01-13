using AutoMapper;
using StudentManagementSystem.DTOs;
using StudentManagementSystem.Models;

namespace StudentManagementSystem
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map Student to StudentDTO and vice versa
            CreateMap<Student, StudentDTO>();
            CreateMap<StudentDTO, Student>();

            // Map University to UniversityDTO and vice versa
            CreateMap<University, UniversityDTO>();
            CreateMap<UniversityDTO, University>();

            // Map Faculty to FacultyDTO and vice versa
            CreateMap<Faculty, FacultyDTO>();
            CreateMap<FacultyDTO, Faculty>();

            // Map Student to PatchStudentDTO and vice versa
            CreateMap<Student, PatchStudentDTO>();
            CreateMap<PatchStudentDTO, Student>();
        }
    }
}