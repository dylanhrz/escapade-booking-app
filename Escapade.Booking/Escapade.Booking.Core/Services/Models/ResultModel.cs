namespace Escapade.Booking.Core.Services.Models;

public class ResultModel<T> : BaseResultModel
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
}