using HotDinner.Application.Common.Interfaces.Services;

namespace HotDinner.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
        
    }
}