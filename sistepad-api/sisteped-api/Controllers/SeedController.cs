using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistepedApi.Infra.Data;
using SistepedApi.Models;
using SistepedApi.Models.Enums;
using System.Net;

namespace SistepedApi.Controllers
{
    /// <summary>
    /// Controller para popular o banco de dados com dados de teste.
    /// ATENÇÃO: Remover em produção!
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SeedController : ControllerBase
    {
        private readonly SistepedDbContext _context;

        public SeedController(SistepedDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Popula o banco de dados com dados de teste.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SeedDatabase()
        {
            try
            {
                // Verifica se já existe dados
                if (await _context.Users.AnyAsync())
                {
                    return BadRequest("O banco de dados já possui dados. Execute DELETE primeiro se quiser recriar.");
                }

                // ===== USUÁRIOS =====
                var (hashCoord, saltCoord) = PasswordHasher.Hash("Senha@123");
                var (hashProf1, saltProf1) = PasswordHasher.Hash("Senha@123");
                var (hashProf2, saltProf2) = PasswordHasher.Hash("Senha@123");
                var (hashResp1, saltResp1) = PasswordHasher.Hash("Senha@123");
                var (hashResp2, saltResp2) = PasswordHasher.Hash("Senha@123");
                var (hashResp3, saltResp3) = PasswordHasher.Hash("Senha@123");

                // Coordenador
                var coordenador = new User
                {
                    Email = "coordenador@escola.com",
                    Name = "Maria Silva - Coordenadora",
                    Role = UserRole.Coordinator,
                    Status = true,
                    CreatedAt = DateTime.Now
                };
                _context.Users.Add(coordenador);
                await _context.SaveChangesAsync();

                _context.UserCredentials.Add(new UserCredential
                {
                    UserId = coordenador.Id,
                    PasswordHash = Convert.ToBase64String(hashCoord) + ":" + Convert.ToBase64String(saltCoord),
                    Role = UserRole.Coordinator
                });

                // Professor 1
                var professor1 = new User
                {
                    Email = "professor1@escola.com",
                    Name = "João Santos - Professor de Matemática",
                    Role = UserRole.Teacher,
                    Status = true,
                    CreatedAt = DateTime.Now
                };
                _context.Users.Add(professor1);
                await _context.SaveChangesAsync();

                _context.UserCredentials.Add(new UserCredential
                {
                    UserId = professor1.Id,
                    PasswordHash = Convert.ToBase64String(hashProf1) + ":" + Convert.ToBase64String(saltProf1),
                    Role = UserRole.Teacher
                });

                // Professor 2
                var professor2 = new User
                {
                    Email = "professor2@escola.com",
                    Name = "Ana Costa - Professora de Português",
                    Role = UserRole.Teacher,
                    Status = true,
                    CreatedAt = DateTime.Now
                };
                _context.Users.Add(professor2);
                await _context.SaveChangesAsync();

                _context.UserCredentials.Add(new UserCredential
                {
                    UserId = professor2.Id,
                    PasswordHash = Convert.ToBase64String(hashProf2) + ":" + Convert.ToBase64String(saltProf2),
                    Role = UserRole.Teacher
                });

                // Responsável 1
                var responsavel1 = new User
                {
                    Email = "responsavel1@email.com",
                    Name = "Carlos Oliveira",
                    Role = UserRole.Guardian,
                    Status = true,
                    CreatedAt = DateTime.Now
                };
                _context.Users.Add(responsavel1);
                await _context.SaveChangesAsync();

                _context.UserCredentials.Add(new UserCredential
                {
                    UserId = responsavel1.Id,
                    PasswordHash = Convert.ToBase64String(hashResp1) + ":" + Convert.ToBase64String(saltResp1),
                    Role = UserRole.Guardian
                });

                // Responsável 2
                var responsavel2 = new User
                {
                    Email = "responsavel2@email.com",
                    Name = "Fernanda Lima",
                    Role = UserRole.Guardian,
                    Status = true,
                    CreatedAt = DateTime.Now
                };
                _context.Users.Add(responsavel2);
                await _context.SaveChangesAsync();

                _context.UserCredentials.Add(new UserCredential
                {
                    UserId = responsavel2.Id,
                    PasswordHash = Convert.ToBase64String(hashResp2) + ":" + Convert.ToBase64String(saltResp2),
                    Role = UserRole.Guardian
                });

                // Responsável 3
                var responsavel3 = new User
                {
                    Email = "responsavel3@email.com",
                    Name = "Roberto Almeida",
                    Role = UserRole.Guardian,
                    Status = true,
                    CreatedAt = DateTime.Now
                };
                _context.Users.Add(responsavel3);
                await _context.SaveChangesAsync();

                _context.UserCredentials.Add(new UserCredential
                {
                    UserId = responsavel3.Id,
                    PasswordHash = Convert.ToBase64String(hashResp3) + ":" + Convert.ToBase64String(saltResp3),
                    Role = UserRole.Guardian
                });

                await _context.SaveChangesAsync();

                // ===== GRIDS (Grades de Agrupamento) =====
                var grid2025 = new Grid
                {
                    Year = 2025,
                    Name = "Grade Curricular 2025",
                    Status = true,
                    CreatedAt = DateTime.Now
                };
                _context.Grids.Add(grid2025);

                var grid2026 = new Grid
                {
                    Year = 2026,
                    Name = "Grade Curricular 2026",
                    Status = true,
                    CreatedAt = DateTime.Now
                };
                _context.Grids.Add(grid2026);
                await _context.SaveChangesAsync();

                // ===== GRADES (Séries) =====
                var serie1A = new Grade
                {
                    Name = "1º Ano A",
                    Level = 1,
                    Shift = 1, // Manhã
                    Status = true,
                    GridId = grid2025.Id,
                    CreatedAt = DateTime.Now
                };
                _context.Grades.Add(serie1A);

                var serie1B = new Grade
                {
                    Name = "1º Ano B",
                    Level = 1,
                    Shift = 2, // Tarde
                    Status = true,
                    GridId = grid2025.Id,
                    CreatedAt = DateTime.Now
                };
                _context.Grades.Add(serie1B);

                var serie2A = new Grade
                {
                    Name = "2º Ano A",
                    Level = 2,
                    Shift = 1, // Manhã
                    Status = true,
                    GridId = grid2025.Id,
                    CreatedAt = DateTime.Now
                };
                _context.Grades.Add(serie2A);

                var serie3A = new Grade
                {
                    Name = "3º Ano A",
                    Level = 3,
                    Shift = 3, // Noite
                    Status = true,
                    GridId = grid2026.Id,
                    CreatedAt = DateTime.Now
                };
                _context.Grades.Add(serie3A);

                await _context.SaveChangesAsync();

                // ===== CLASSES (Matérias) =====
                var matematica = new Class
                {
                    Code = "MAT001",
                    Name = "Matemática",
                    WorkloadHours = 80,
                    Status = true,
                    CreatedAt = DateTime.Now
                };
                _context.Classes.Add(matematica);

                var portugues = new Class
                {
                    Code = "POR001",
                    Name = "Português",
                    WorkloadHours = 80,
                    Status = true,
                    CreatedAt = DateTime.Now
                };
                _context.Classes.Add(portugues);

                var ciencias = new Class
                {
                    Code = "CIE001",
                    Name = "Ciências",
                    WorkloadHours = 60,
                    Status = true,
                    CreatedAt = DateTime.Now
                };
                _context.Classes.Add(ciencias);

                var historia = new Class
                {
                    Code = "HIS001",
                    Name = "História",
                    WorkloadHours = 40,
                    Status = true,
                    CreatedAt = DateTime.Now
                };
                _context.Classes.Add(historia);

                var geografia = new Class
                {
                    Code = "GEO001",
                    Name = "Geografia",
                    WorkloadHours = 40,
                    Status = true,
                    CreatedAt = DateTime.Now
                };
                _context.Classes.Add(geografia);

                await _context.SaveChangesAsync();

                // ===== GRADE_CLASSES (Série x Matéria) =====
                _context.GradeClasses.AddRange(new[]
                {
                    new GradeClass { GradeId = serie1A.Id, ClassId = matematica.Id, CreatedAt = DateTime.Now },
                    new GradeClass { GradeId = serie1A.Id, ClassId = portugues.Id, CreatedAt = DateTime.Now },
                    new GradeClass { GradeId = serie1A.Id, ClassId = ciencias.Id, CreatedAt = DateTime.Now },
                    new GradeClass { GradeId = serie1B.Id, ClassId = matematica.Id, CreatedAt = DateTime.Now },
                    new GradeClass { GradeId = serie1B.Id, ClassId = portugues.Id, CreatedAt = DateTime.Now },
                    new GradeClass { GradeId = serie2A.Id, ClassId = matematica.Id, CreatedAt = DateTime.Now },
                    new GradeClass { GradeId = serie2A.Id, ClassId = portugues.Id, CreatedAt = DateTime.Now },
                    new GradeClass { GradeId = serie2A.Id, ClassId = historia.Id, CreatedAt = DateTime.Now },
                    new GradeClass { GradeId = serie3A.Id, ClassId = matematica.Id, CreatedAt = DateTime.Now },
                    new GradeClass { GradeId = serie3A.Id, ClassId = portugues.Id, CreatedAt = DateTime.Now },
                    new GradeClass { GradeId = serie3A.Id, ClassId = geografia.Id, CreatedAt = DateTime.Now },
                });
                await _context.SaveChangesAsync();

                // ===== CLASS_TEACHERS (Professor x Matéria) =====
                _context.ClassTeachers.AddRange(new[]
                {
                    new ClassTeacher { ClassId = matematica.Id, TeacherId = professor1.Id, CreatedAt = DateTime.Now },
                    new ClassTeacher { ClassId = ciencias.Id, TeacherId = professor1.Id, CreatedAt = DateTime.Now },
                    new ClassTeacher { ClassId = portugues.Id, TeacherId = professor2.Id, CreatedAt = DateTime.Now },
                    new ClassTeacher { ClassId = historia.Id, TeacherId = professor2.Id, CreatedAt = DateTime.Now },
                    new ClassTeacher { ClassId = geografia.Id, TeacherId = professor2.Id, CreatedAt = DateTime.Now },
                });
                await _context.SaveChangesAsync();

                // ===== STUDENTS (Alunos) =====
                var aluno1 = new Student
                {
                    Enrollment = "2025001",
                    Name = "Pedro Oliveira",
                    BirthDate = new DateTime(2015, 3, 15),
                    GuardianId = responsavel1.Id,
                    Status = true,
                    CreatedAt = DateTime.Now
                };
                _context.Students.Add(aluno1);

                var aluno2 = new Student
                {
                    Enrollment = "2025002",
                    Name = "Laura Oliveira",
                    BirthDate = new DateTime(2016, 7, 22),
                    GuardianId = responsavel1.Id,
                    Status = true,
                    CreatedAt = DateTime.Now
                };
                _context.Students.Add(aluno2);

                var aluno3 = new Student
                {
                    Enrollment = "2025003",
                    Name = "Gabriel Lima",
                    BirthDate = new DateTime(2015, 1, 10),
                    GuardianId = responsavel2.Id,
                    Status = true,
                    CreatedAt = DateTime.Now
                };
                _context.Students.Add(aluno3);

                var aluno4 = new Student
                {
                    Enrollment = "2025004",
                    Name = "Sofia Almeida",
                    BirthDate = new DateTime(2014, 11, 5),
                    GuardianId = responsavel3.Id,
                    Status = true,
                    CreatedAt = DateTime.Now
                };
                _context.Students.Add(aluno4);

                var aluno5 = new Student
                {
                    Enrollment = "2025005",
                    Name = "Lucas Almeida",
                    BirthDate = new DateTime(2013, 8, 30),
                    GuardianId = responsavel3.Id,
                    Status = true,
                    CreatedAt = DateTime.Now
                };
                _context.Students.Add(aluno5);

                var aluno6 = new Student
                {
                    Enrollment = "2025006",
                    Name = "Beatriz Santos",
                    BirthDate = new DateTime(2015, 4, 18),
                    GuardianId = responsavel2.Id,
                    Status = true,
                    CreatedAt = DateTime.Now
                };
                _context.Students.Add(aluno6);

                await _context.SaveChangesAsync();

                // ===== STUDENT_GRADES (Aluno x Série) =====
                _context.StudentGrades.AddRange(new[]
                {
                    new StudentGrade { StudentId = aluno1.Id, GradeId = serie1A.Id, CreatedAt = DateTime.Now },
                    new StudentGrade { StudentId = aluno2.Id, GradeId = serie1A.Id, CreatedAt = DateTime.Now },
                    new StudentGrade { StudentId = aluno3.Id, GradeId = serie1B.Id, CreatedAt = DateTime.Now },
                    new StudentGrade { StudentId = aluno4.Id, GradeId = serie2A.Id, CreatedAt = DateTime.Now },
                    new StudentGrade { StudentId = aluno5.Id, GradeId = serie3A.Id, CreatedAt = DateTime.Now },
                    new StudentGrade { StudentId = aluno6.Id, GradeId = serie1A.Id, CreatedAt = DateTime.Now },
                });
                await _context.SaveChangesAsync();

                // ===== ATTENDANCES (Frequência) =====
                var hoje = DateTime.Today;
                var attendances = new List<Attendance>();

                // Últimos 10 dias de aula
                for (int dia = 0; dia < 10; dia++)
                {
                    var data = hoje.AddDays(-dia);
                    
                    // Pula fim de semana
                    if (data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)
                        continue;

                    // Aluno 1 - 1º Ano A (sempre presente)
                    attendances.Add(new Attendance { StudentId = aluno1.Id, GradeId = serie1A.Id, Date = data, Present = true, CreatedAt = DateTime.Now });
                    
                    // Aluno 2 - 1º Ano A (falta às vezes)
                    attendances.Add(new Attendance { StudentId = aluno2.Id, GradeId = serie1A.Id, Date = data, Present = dia % 3 != 0, CreatedAt = DateTime.Now });
                    
                    // Aluno 3 - 1º Ano B
                    attendances.Add(new Attendance { StudentId = aluno3.Id, GradeId = serie1B.Id, Date = data, Present = dia % 4 != 0, CreatedAt = DateTime.Now });
                    
                    // Aluno 4 - 2º Ano A
                    attendances.Add(new Attendance { StudentId = aluno4.Id, GradeId = serie2A.Id, Date = data, Present = true, CreatedAt = DateTime.Now });
                    
                    // Aluno 5 - 3º Ano A (falta muito)
                    attendances.Add(new Attendance { StudentId = aluno5.Id, GradeId = serie3A.Id, Date = data, Present = dia % 2 == 0, CreatedAt = DateTime.Now });
                    
                    // Aluno 6 - 1º Ano A
                    attendances.Add(new Attendance { StudentId = aluno6.Id, GradeId = serie1A.Id, Date = data, Present = dia != 5, CreatedAt = DateTime.Now });
                }

                _context.Attendances.AddRange(attendances);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Banco de dados populado com sucesso!",
                    Usuarios = new
                    {
                        Coordenador = new { Email = "coordenador@escola.com", Senha = "Senha@123" },
                        Professor1 = new { Email = "professor1@escola.com", Senha = "Senha@123" },
                        Professor2 = new { Email = "professor2@escola.com", Senha = "Senha@123" },
                        Responsavel1 = new { Email = "responsavel1@email.com", Senha = "Senha@123" },
                        Responsavel2 = new { Email = "responsavel2@email.com", Senha = "Senha@123" },
                        Responsavel3 = new { Email = "responsavel3@email.com", Senha = "Senha@123" }
                    },
                    Resumo = new
                    {
                        TotalUsuarios = 6,
                        TotalGrids = 2,
                        TotalSeries = 4,
                        TotalMaterias = 5,
                        TotalAlunos = 6,
                        TotalFrequencias = attendances.Count
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao popular banco: {ex.Message}");
            }
        }

        /// <summary>
        /// Remove todos os dados do banco (CUIDADO!).
        /// </summary>
        [HttpDelete]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ClearDatabase()
        {
            try
            {
                // Ordem correta para evitar erros de FK
                _context.Attendances.RemoveRange(_context.Attendances);
                _context.StudentGrades.RemoveRange(_context.StudentGrades);
                _context.ClassTeachers.RemoveRange(_context.ClassTeachers);
                _context.GradeClasses.RemoveRange(_context.GradeClasses);
                _context.Students.RemoveRange(_context.Students);
                _context.Grades.RemoveRange(_context.Grades);
                _context.Grids.RemoveRange(_context.Grids);
                _context.Classes.RemoveRange(_context.Classes);
                _context.UserCredentials.RemoveRange(_context.UserCredentials);
                _context.Users.RemoveRange(_context.Users);

                await _context.SaveChangesAsync();

                return Ok(new { Message = "Banco de dados limpo com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao limpar banco: {ex.Message}");
            }
        }
    }
}
