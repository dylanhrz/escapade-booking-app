using Escapade.Booking.Core.Data;
using Escapade.Booking.Core.Entities;
using Escapade.Booking.Core.Services.Interfaces;
using Escapade.Booking.Core.Services.Models.RequestModels;
using Microsoft.EntityFrameworkCore;

namespace Escapade.Booking.Core.Services.Models;

public class UserService : IUserService
{
    private readonly EscapadeDbContext _escapadeDbContext;

    public UserService(EscapadeDbContext escapadeDbContext)
    {
        _escapadeDbContext = escapadeDbContext;
    }
    
    public async Task<ResultModel<User>> LoginAsync(UserLoginRequestModel requestModel)
    {
        
        var user = await _escapadeDbContext.Users
            .Include(u => u.UserRole)
            .FirstOrDefaultAsync(u => u.Email == requestModel.Email && u.Password == requestModel.Password);

        if (user == null)
        {
            return new ResultModel<User>
            {
                IsSuccess = false,
                Errors = new List<string> { "Ongeldige login gegevens" }
            };
        }

        return new ResultModel<User>
        {
            IsSuccess = true,
            Items = new List<User> { user }
        };
    }
}