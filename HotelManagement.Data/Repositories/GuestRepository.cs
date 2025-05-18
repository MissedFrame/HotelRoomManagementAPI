using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Data.Entities;
using HotelManagement.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Data.Repositories
{
    public class GuestRepository : IGuestRepository
    {
        private readonly HotelDbContext _context;

        public GuestRepository(HotelDbContext context) => _context = context;
        public async Task<List<Guest>> GetAllAsync() => await _context.Guests.ToListAsync();
        public async Task<Guest?> GetByIdAsync(int id) => await _context.Guests.FindAsync(id);
        public async Task AddAsync(Guest guest) => await _context.Guests.AddAsync(guest);
        public Task UpdateAsync(Guest guest)
        {
            _context.Guests.Update(guest);
            return Task.CompletedTask;
        }
        public async Task DeleteAsync(int id)
        {
            var guest = await GetByIdAsync(id);
            if (guest != null) _context.Guests.Remove(guest);
        }

        public async Task<bool> HasExistingBookingAsync(string firstName, string lastName, DateTime dob, DateTime checkIn, DateTime checkOut) =>
            await _context.Guests.AnyAsync(g =>
                g.FirstName == firstName &&
                g.LastName == lastName &&
                g.DOB == dob &&
                checkIn <= g.CheckOutDate && checkOut >= g.CheckInDate);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();  
        }
    }
}
