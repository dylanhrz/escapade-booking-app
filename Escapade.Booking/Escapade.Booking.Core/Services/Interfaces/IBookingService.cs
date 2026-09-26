using Escapade.Booking.Core.Services.Models;
using Escapade.Booking.Core.Services.Models.RequestModels;

namespace Escapade.Booking.Core.Services.Interfaces;

public interface IBookingService
{
    Task<ResultModel<Entities.Booking>> GetAllAsync();
    Task<ResultModel<Entities.Booking>> GetByIdAsync(int id);
    Task<ResultModel<Entities.Booking>> GetByTokenAsync(Guid token);
    Task<ResultModel<Entities.Booking>> CreateBookingAsync(BookingCreateRequestModel requestModel);
    Task<BaseResultModel> ApproveBookingAsync(int id);
    Task<BaseResultModel> DeleteBookingAsync(int id);
    Task<BaseResultModel> SaveChangesAsync();
}