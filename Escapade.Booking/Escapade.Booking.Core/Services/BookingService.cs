using System.Diagnostics;
using Escapade.Booking.Core.Data;
using Escapade.Booking.Core.Services.Interfaces;
using Escapade.Booking.Core.Services.Models;
using Escapade.Booking.Core.Services.Models.RequestModels;
using Escapade.Booking.Web.Data;
using Escapade.Booking.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Escapade.Booking.Core.Services;

public class BookingService : IBookingService
{
    private readonly EscapadeDbContext _escapadeDbContext;
    private readonly IEmailService _emailService;

    public BookingService(EscapadeDbContext escapadeDbContext, IEmailService emailService)
    {
        _escapadeDbContext = escapadeDbContext;
        _emailService = emailService;
    }

    public async Task<ResultModel<Entities.Booking>> GetAllAsync()
    {
        var bookings = await _escapadeDbContext.Bookings.ToListAsync();
        return new ResultModel<Entities.Booking>
        {
            IsSuccess = true,
            Items = bookings
        };
    }

    public async Task<ResultModel<Entities.Booking>> GetByIdAsync(int id)
    {
        var booking = await _escapadeDbContext.Bookings.FirstOrDefaultAsync(b => b.Id == id);
        if (booking == null)
        {
            return new ResultModel<Entities.Booking>
            {
                IsSuccess = false,
                Errors = new List<string> { "Boeking niet gevonden." }
            };
        }

        return new ResultModel<Entities.Booking>
        {
            IsSuccess = true,
            Items = new List<Entities.Booking> { booking }
        };
    }

    public async Task<ResultModel<Entities.Booking>> GetByTokenAsync(Guid token)
    {
        var booking = await _escapadeDbContext.Bookings
            .FirstOrDefaultAsync(b => b.AccesToken == token);

        if (booking == null)
        {
            return new ResultModel<Entities.Booking>
            {
                IsSuccess = false,
                Errors = new List<string> { "Booking niet gevonden." }
            };
        }

        return new ResultModel<Entities.Booking>
        {
            IsSuccess = true,
            Items = new List<Entities.Booking> { booking }
        };
    }

    public async Task<ResultModel<Entities.Booking>> CreateBookingAsync(BookingCreateRequestModel requestModel)
    {
        var booking = new Entities.Booking()
        {
            Name = requestModel.Name,
            Email = requestModel.Email,
            PhoneNumber = requestModel.PhoneNumber,
            NumberOfGuests = requestModel.NumberOfGuests,
            Comment = requestModel.Comment,
            StartDate = requestModel.StartDate,
            EndDate = requestModel.EndDate,
        };

        _escapadeDbContext.Bookings.Add(booking);

        var saveResult = await SaveChangesAsync();
        if (!saveResult.IsSuccess)
        {
            return new ResultModel<Entities.Booking>
            {
                IsSuccess = false,
                Errors = saveResult.Errors
            };
        }

        return new ResultModel<Entities.Booking>
        {
            IsSuccess = true,
            Items = new List<Entities.Booking> { booking }
        };
    }

    public async Task<BaseResultModel> ApproveBookingAsync(int id)
    {
        var booking = await _escapadeDbContext.Bookings.FindAsync(id);
        
        if (booking == null)
        {
            return new BaseResultModel { IsSuccess = false, Errors = new[] { "Boeking niet gevonden." } };
        }

        booking.Status = Core.Entities.Booking.BookingStatus.Approved;
        
        _escapadeDbContext.Bookings.Update(booking);
    
        return await SaveChangesAsync();
    }

    public async Task<BaseResultModel> DeleteBookingAsync(int id)
    {
        var booking = await _escapadeDbContext.Bookings.FindAsync(id);
        if (booking == null)
        {
            return new BaseResultModel { IsSuccess = false, Errors = new[] { "Boeking niet gevonden." } };
        }

        _escapadeDbContext.Bookings.Remove(booking);
        return await SaveChangesAsync();
    }

    public async Task<BaseResultModel> SaveChangesAsync()
    {
        try
        {
            await _escapadeDbContext.SaveChangesAsync();
            return new BaseResultModel { IsSuccess = true };
        }
        catch (DbUpdateException dbUpdateException)
        {
            Debug.WriteLine(dbUpdateException.Message);
            return new BaseResultModel
            {
                IsSuccess = false,
                Errors = new List<string> { "Er is een fout opgetreden bij het opslaan in de database." }
            };
        }
    }
}