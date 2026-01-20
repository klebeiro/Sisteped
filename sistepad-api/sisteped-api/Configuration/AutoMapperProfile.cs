using AutoMapper;
using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;
using SistepedApi.Models;

namespace SistepedApi.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User
            CreateMap<User, UserResponseDTO>();
            CreateMap<UserCreateDTO, User>();

            // Grade
            CreateMap<Grade, GradeResponseDTO>();
            CreateMap<GradeCreateDTO, Grade>();
            CreateMap<GradeUpdateDTO, Grade>();

            // Class
            CreateMap<Class, ClassResponseDTO>();
            CreateMap<ClassCreateDTO, Class>();
            CreateMap<ClassUpdateDTO, Class>();

            // GradeClass
            CreateMap<GradeClass, GradeClassResponseDTO>()
                .ForMember(dest => dest.GradeName, opt => opt.MapFrom(src => src.Grade.Name))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.Name));

            // ClassTeacher
            CreateMap<ClassTeacher, ClassTeacherResponseDTO>()
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.Name))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.Name));

            // Student
            CreateMap<Student, StudentResponseDTO>()
                .ForMember(dest => dest.GuardianName, opt => opt.MapFrom(src => src.Guardian.Name));
            CreateMap<StudentCreateDTO, Student>();
            CreateMap<StudentUpdateDTO, Student>();

            // StudentGrade
            CreateMap<StudentGrade, StudentGradeResponseDTO>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name))
                .ForMember(dest => dest.GradeName, opt => opt.MapFrom(src => src.Grade.Name));

            // Attendance
            CreateMap<Attendance, AttendanceResponseDTO>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name))
                .ForMember(dest => dest.GradeName, opt => opt.MapFrom(src => src.Grade.Name));

            // Activity
            CreateMap<Activity, ActivityResponseDTO>()
                .ForMember(dest => dest.GradeName, opt => opt.MapFrom(src => src.Grade.Name))
                .ForMember(dest => dest.TotalStudents, opt => opt.MapFrom(src => src.StudentActivities.Count))
                .ForMember(dest => dest.GradedStudents, opt => opt.MapFrom(src => src.StudentActivities.Count(sa => sa.Score.HasValue)))
                .ForMember(dest => dest.AverageScore, opt => opt.MapFrom(src => 
                    src.StudentActivities.Any(sa => sa.Score.HasValue) 
                        ? src.StudentActivities.Where(sa => sa.Score.HasValue).Average(sa => sa.Score) 
                        : (decimal?)null));
            CreateMap<ActivityCreateDTO, Activity>();
            CreateMap<ActivityUpdateDTO, Activity>();

            // StudentActivity
            CreateMap<StudentActivity, StudentActivityResponseDTO>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name))
                .ForMember(dest => dest.Enrollment, opt => opt.MapFrom(src => src.Student.Enrollment))
                .ForMember(dest => dest.ActivityTitle, opt => opt.MapFrom(src => src.Activity.Title))
                .ForMember(dest => dest.MaxScore, opt => opt.MapFrom(src => src.Activity.MaxScore));
            CreateMap<StudentActivityCreateDTO, StudentActivity>();
            CreateMap<StudentActivityUpdateDTO, StudentActivity>();
        }
    }
}
