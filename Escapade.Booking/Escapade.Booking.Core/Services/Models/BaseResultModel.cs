namespace Escapade.Booking.Core.Services.Models;

public class BaseResultModel
{
    public bool IsSuccess { get; set; }
    public IEnumerable<string> Errors { get; set; } = new List<string>();
}