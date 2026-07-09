using System.Text;
using Microsoft.EntityFrameworkCore;
using BlossomTreeManager.Web.Data;
using BlossomTreeManager.Web.Data.Enums;

namespace BlossomTreeManager.Web.Services;

public class AttendanceExportService(ApplicationDbContext dbContext)
{
    public async Task<string> BuildCsvAsync(Guid schoolId, DateOnly? startDate, DateOnly? endDate, AttendanceStatus? status)
    {
        var query = dbContext.AttendanceRecords
            .AsNoTracking()
            .Where(x => x.SchoolId == schoolId)
            .Include(x => x.Student)
            .Include(x => x.Room)
            .AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(x => x.AttendanceDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(x => x.AttendanceDate <= endDate.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
        }

        var rows = await query
            .OrderByDescending(x => x.AttendanceDate)
            .ThenBy(x => x.Student.LastName)
            .ThenBy(x => x.Student.FirstName)
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("Date,Student,Room,CheckInUtc,CheckOutUtc,Status");

        foreach (var row in rows)
        {
            var studentName = EscapeCsv($"{row.Student.FirstName} {row.Student.LastName}");
            var roomName = EscapeCsv(row.Room.Name);
            sb.AppendLine($"{row.AttendanceDate:yyyy-MM-dd},{studentName},{roomName},{row.CheckInAtUtc:O},{row.CheckOutAtUtc:O},{row.Status}");
        }

        return sb.ToString();
    }

    private static string EscapeCsv(string value)
    {
        if (value.Contains(',') || value.Contains('"'))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        return value;
    }
}
