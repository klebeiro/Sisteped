using System.Text;
using SistepedApi.DTOs.Response;

namespace SistepedApi.Helpers
{
    public static class CsvHelper
    {
        /// <summary>
        /// Converte relatório de frequência para CSV
        /// </summary>
        public static string ConvertAttendanceReportToCsv(AttendanceReportResponseDTO report)
        {
            var csv = new StringBuilder();
            
            // Cabeçalho com estatísticas
            csv.AppendLine("RELATÓRIO DE FREQUÊNCIA");
            csv.AppendLine($"Total de Registros: {report.TotalRecords}");
            csv.AppendLine($"Total Presente: {report.TotalPresent}");
            csv.AppendLine($"Total Ausente: {report.TotalAbsent}");
            csv.AppendLine($"Percentual de Frequência: {report.AttendancePercentage}%");
            csv.AppendLine();
            
            // Cabeçalho das colunas
            csv.AppendLine("ID,Data,Presente,ID Aluno,Nome Aluno,Matrícula,ID Matéria,Nome Matéria,Código Matéria,ID Grade Curricular,Nome Grade Curricular,ID Responsável,Nome Responsável");
            
            // Dados
            foreach (var item in report.Items)
            {
                csv.AppendLine($"{item.AttendanceId}," +
                    $"{item.Date:yyyy-MM-dd}," +
                    $"{(item.Present ? "Sim" : "Não")}," +
                    $"{item.StudentId}," +
                    $"\"{EscapeCsvField(item.StudentName)}\"," +
                    $"{item.Enrollment}," +
                    $"{item.ClassId}," +
                    $"\"{EscapeCsvField(item.ClassName)}\"," +
                    $"{item.ClassCode}," +
                    $"{(item.GridId?.ToString() ?? "")}," +
                    $"\"{EscapeCsvField(item.GridName ?? "")}\"," +
                    $"{item.GuardianId}," +
                    $"\"{EscapeCsvField(item.GuardianName)}\"");
            }
            
            return csv.ToString();
        }

        /// <summary>
        /// Converte resumo de frequência por aluno para CSV
        /// </summary>
        public static string ConvertStudentAttendanceSummaryToCsv(IEnumerable<StudentAttendanceSummaryDTO> summary)
        {
            var csv = new StringBuilder();
            
            csv.AppendLine("RESUMO DE FREQUÊNCIA POR ALUNO");
            csv.AppendLine();
            csv.AppendLine("ID Aluno,Nome Aluno,Matrícula,Total Registros,Total Presente,Total Ausente,Percentual Frequência");
            
            foreach (var item in summary)
            {
                csv.AppendLine($"{item.StudentId}," +
                    $"\"{EscapeCsvField(item.StudentName)}\"," +
                    $"{item.Enrollment}," +
                    $"{item.TotalRecords}," +
                    $"{item.TotalPresent}," +
                    $"{item.TotalAbsent}," +
                    $"{item.AttendancePercentage}%");
            }
            
            return csv.ToString();
        }

        /// <summary>
        /// Converte resumo de frequência por turma para CSV
        /// </summary>
        public static string ConvertGradeAttendanceSummaryToCsv(IEnumerable<GradeAttendanceSummaryDTO> summary)
        {
            var csv = new StringBuilder();
            
            csv.AppendLine("RESUMO DE FREQUÊNCIA POR TURMA");
            csv.AppendLine();
            csv.AppendLine("ID Turma,Nome Turma,Nível,Turno,Total Registros,Total Presente,Total Ausente,Percentual Frequência");
            
            foreach (var item in summary)
            {
                csv.AppendLine($"{item.GradeId}," +
                    $"\"{EscapeCsvField(item.GradeName)}\"," +
                    $"{item.GradeLevel}," +
                    $"\"{item.ShiftName}\"," +
                    $"{item.TotalRecords}," +
                    $"{item.TotalPresent}," +
                    $"{item.TotalAbsent}," +
                    $"{item.AttendancePercentage}%");
            }
            
            return csv.ToString();
        }

        /// <summary>
        /// Converte relatório de notas para CSV
        /// </summary>
        public static string ConvertGradeReportToCsv(GradeReportResponseDTO report)
        {
            var csv = new StringBuilder();
            
            // Cabeçalho com estatísticas
            csv.AppendLine("RELATÓRIO DE NOTAS");
            csv.AppendLine($"Total de Registros: {report.TotalRecords}");
            csv.AppendLine($"Total Avaliado: {report.TotalGraded}");
            csv.AppendLine($"Total Pendente: {report.TotalPending}");
            csv.AppendLine($"Média Geral: {(report.AverageScore?.ToString("F2") ?? "N/A")}");
            csv.AppendLine($"Maior Nota: {(report.HighestScore?.ToString("F2") ?? "N/A")}");
            csv.AppendLine($"Menor Nota: {(report.LowestScore?.ToString("F2") ?? "N/A")}");
            csv.AppendLine();
            
            // Cabeçalho das colunas
            csv.AppendLine("ID,Nota,Observações,Data Criação,ID Aluno,Nome Aluno,Matrícula,ID Atividade,Título Atividade,Data Aplicação,Nota Máxima,ID Matéria,Nome Matéria,Código Matéria,ID Turma,Nome Turma,Turno,ID Grade Curricular,Nome Grade Curricular,ID Responsável,Nome Responsável");
            
            // Dados
            foreach (var item in report.Items)
            {
                csv.AppendLine($"{item.StudentActivityId}," +
                    $"{(item.Score?.ToString("F2") ?? "")}," +
                    $"\"{EscapeCsvField(item.Remarks ?? "")}\"," +
                    $"{item.CreatedAt:yyyy-MM-dd HH:mm:ss}," +
                    $"{item.StudentId}," +
                    $"\"{EscapeCsvField(item.StudentName)}\"," +
                    $"{item.Enrollment}," +
                    $"{item.ActivityId}," +
                    $"\"{EscapeCsvField(item.ActivityTitle)}\"," +
                    $"{item.ApplicationDate:yyyy-MM-dd}," +
                    $"{item.MaxScore}," +
                    $"{item.ClassId}," +
                    $"\"{EscapeCsvField(item.ClassName)}\"," +
                    $"{item.ClassCode}," +
                    $"{item.GradeId}," +
                    $"\"{EscapeCsvField(item.GradeName)}\"," +
                    $"\"{item.ShiftName}\"," +
                    $"{(item.GridId?.ToString() ?? "")}," +
                    $"\"{EscapeCsvField(item.GridName ?? "")}\"," +
                    $"{item.GuardianId}," +
                    $"\"{EscapeCsvField(item.GuardianName)}\"");
            }
            
            return csv.ToString();
        }

        /// <summary>
        /// Converte resumo de notas por aluno para CSV
        /// </summary>
        public static string ConvertStudentGradeSummaryToCsv(IEnumerable<StudentGradeSummaryDTO> summary)
        {
            var csv = new StringBuilder();
            
            csv.AppendLine("RESUMO DE NOTAS POR ALUNO");
            csv.AppendLine();
            csv.AppendLine("ID Aluno,Nome Aluno,Matrícula,Total Atividades,Atividades Avaliadas,Atividades Pendentes,Média,Maior Nota,Menor Nota");
            
            foreach (var item in summary)
            {
                csv.AppendLine($"{item.StudentId}," +
                    $"\"{EscapeCsvField(item.StudentName)}\"," +
                    $"{item.Enrollment}," +
                    $"{item.TotalActivities}," +
                    $"{item.GradedActivities}," +
                    $"{item.PendingActivities}," +
                    $"{(item.AverageScore?.ToString("F2") ?? "N/A")}," +
                    $"{(item.HighestScore?.ToString("F2") ?? "N/A")}," +
                    $"{(item.LowestScore?.ToString("F2") ?? "N/A")}");
            }
            
            return csv.ToString();
        }

        /// <summary>
        /// Converte resumo de notas por atividade para CSV
        /// </summary>
        public static string ConvertActivityGradeSummaryToCsv(IEnumerable<ActivityGradeSummaryDTO> summary)
        {
            var csv = new StringBuilder();
            
            csv.AppendLine("RESUMO DE NOTAS POR ATIVIDADE");
            csv.AppendLine();
            csv.AppendLine("ID Atividade,Título Atividade,Data Aplicação,Nota Máxima,ID Matéria,Nome Matéria,Código Matéria,Total Alunos,Alunos Avaliados,Alunos Pendentes,Média,Maior Nota,Menor Nota");
            
            foreach (var item in summary)
            {
                csv.AppendLine($"{item.ActivityId}," +
                    $"\"{EscapeCsvField(item.ActivityTitle)}\"," +
                    $"{item.ApplicationDate:yyyy-MM-dd}," +
                    $"{item.MaxScore}," +
                    $"{item.ClassId}," +
                    $"\"{EscapeCsvField(item.ClassName)}\"," +
                    $"{item.ClassCode}," +
                    $"{item.TotalStudents}," +
                    $"{item.GradedStudents}," +
                    $"{item.PendingStudents}," +
                    $"{(item.AverageScore?.ToString("F2") ?? "N/A")}," +
                    $"{(item.HighestScore?.ToString("F2") ?? "N/A")}," +
                    $"{(item.LowestScore?.ToString("F2") ?? "N/A")}");
            }
            
            return csv.ToString();
        }

        /// <summary>
        /// Converte resumo de notas por turma para CSV
        /// </summary>
        public static string ConvertGradeGradeSummaryToCsv(IEnumerable<GradeGradeSummaryDTO> summary)
        {
            var csv = new StringBuilder();
            
            csv.AppendLine("RESUMO DE NOTAS POR TURMA");
            csv.AppendLine();
            csv.AppendLine("ID Turma,Nome Turma,Turno,Total Atividades,Total Alunos,Média,Maior Nota,Menor Nota");
            
            foreach (var item in summary)
            {
                csv.AppendLine($"{item.GradeId}," +
                    $"\"{EscapeCsvField(item.GradeName)}\"," +
                    $"\"{item.ShiftName}\"," +
                    $"{item.TotalActivities}," +
                    $"{item.TotalStudents}," +
                    $"{(item.AverageScore?.ToString("F2") ?? "N/A")}," +
                    $"{(item.HighestScore?.ToString("F2") ?? "N/A")}," +
                    $"{(item.LowestScore?.ToString("F2") ?? "N/A")}");
            }
            
            return csv.ToString();
        }

        /// <summary>
        /// Escapa campos CSV que contêm vírgulas, aspas ou quebras de linha
        /// </summary>
        private static string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return string.Empty;

            // Se contém vírgula, aspas ou quebra de linha, envolve em aspas e duplica aspas internas
            if (field.Contains(',') || field.Contains('"') || field.Contains('\n') || field.Contains('\r'))
            {
                return field.Replace("\"", "\"\"");
            }

            return field;
        }
    }
}
