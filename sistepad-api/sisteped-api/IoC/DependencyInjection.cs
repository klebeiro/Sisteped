using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Configuration;
using SistepedApi.Services.Interfaces;
using SistepedApi.Infra.Data;
using SistepedApi.Repositories;
using SistepedApi.Services;

namespace SistepedApi.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IGradeService, GradeService>();
            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<IGridGradeService, GridGradeService>();
            services.AddScoped<IGridClassService, GridClassService>();
            services.AddScoped<IClassTeacherService, ClassTeacherService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IStudentGradeService, StudentGradeService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IGridService, GridService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IStudentActivityService, StudentActivityService>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // BASE DE DADOS SQLITE (LOCAL)
            services.AddDbContext<SistepedDbContext>(options =>
                options.UseSqlite("Data Source=sisteped.db"));

            // BASE DE DADOS SQL SERVER (PRODUÇÃO)
            //services.AddDbContext<SistepedDbContext>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserCredentialRepository, UserCredentialRepository>();
            services.AddScoped<IGradeRepository, GradeRepository>();
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IGridGradeRepository, GridGradeRepository>();
            services.AddScoped<IGridClassRepository, GridClassRepository>();
            services.AddScoped<IClassTeacherRepository, ClassTeacherRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IStudentGradeRepository, StudentGradeRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<IGridRepository, GridRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IStudentActivityRepository, StudentActivityRepository>();

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var validatorTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.BaseType != null)
                .Where(t => t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>))
                .ToList();

            foreach (var validatorType in validatorTypes)
            {
                var dtoType = validatorType.BaseType?.GetGenericArguments()[0];
                if (dtoType != null)
                {
                    var interfaceType = typeof(IValidator<>).MakeGenericType(dtoType);
                    services.AddScoped(interfaceType, validatorType);
                }
            }

            return services;
        }

        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sisteped API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header usando o esquema Bearer. Exemplo: 'Bearer {seu token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var secretKey = configuration["Jwt:SecretKey"];
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey ?? "")),
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        RoleClaimType = System.Security.Claims.ClaimTypes.Role
                    };
                });

            return services;
        }

        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                // Política para Coordenadores - acesso total
                .AddPolicy("CoordinatorOnly", policy =>
                    policy.RequireRole("Coordinator"))
                
                // Política para Coordenadores e Professores
                .AddPolicy("CoordinatorOrTeacher", policy =>
                    policy.RequireRole("Coordinator", "Teacher"))
                
                // Política para todos os usuários autenticados
                .AddPolicy("AllAuthenticated", policy =>
                    policy.RequireAuthenticatedUser());

            return services;
        }

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
