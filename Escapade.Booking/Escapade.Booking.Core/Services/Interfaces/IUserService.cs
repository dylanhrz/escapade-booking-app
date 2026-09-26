using Escapade.Booking.Core.Entities;
using Escapade.Booking.Core.Services.Models;
using Escapade.Booking.Core.Services.Models.RequestModels;

namespace Escapade.Booking.Core.Services.Interfaces;

public interface IUserService
{
    Task<ResultModel<User>> LoginAsync(UserLoginRequestModel requestModel);
}