using Grpc.Core;
using Microsoft.EntityFrameworkCore;

public class LogService : LogConsumer.LogConsumerBase
{

    public readonly LogDbContext _context;

    public LogService(LogDbContext context)
    {
        _context = context;
    }


    public override async Task<sendLogResponse> sendLog(sendLogRequest request, ServerCallContext callContext)
    {
        try
        {
            var entry = new Entity
            {
                Log = request.Log,
                Stage = request.Stage,
                Timestamp = DateTime.UtcNow
            };

            _context.Logs.Add(entry);
            await _context.SaveChangesAsync();

            var response = new sendLogResponse { Success = true };
            return response;
        }
        catch (Exception)
        {
            return new sendLogResponse { Success = false };
        }
    }

    public override async Task<getLogResponse> getLog(getLogRequest request, ServerCallContext callContext)
    {
        try
        {

            var date = DateOnly.Parse(request.Date);

            var logs = await _context.Logs.Where(e => DateOnly.FromDateTime(e.Timestamp) == date).ToListAsync();

            var result = logs.Select(e => new LogEntry
            {
                Log = e.Log,
                Stage = e.Stage,
                Timestamps = e.Timestamp.ToString("o")
            }).ToList();

            return new getLogResponse { Log = { result } };
        }
        catch (Exception)
        {
            return new getLogResponse();

        }
    }

    public override async Task<clearLogResponse> clearLog(clearLogRequest request, ServerCallContext callContext)
    {

        try
        {
            var date = DateOnly.Parse(request.Date);

            var logs = _context.Logs.Where(e => DateOnly.FromDateTime(e.Timestamp) == date);
            var count =  await logs.CountAsync();

            _context.Logs.RemoveRange(logs);
            var saved = await _context.SaveChangesAsync();

            return new clearLogResponse
            {
                Success = saved > 0,
                DeletedCount = count
            };

        }
        catch
        {
            return new clearLogResponse
            {
                Success = false,
                DeletedCount = 0
            };
        }
    }
}