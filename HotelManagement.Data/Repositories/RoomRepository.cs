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
    public class RoomRepository : IRoomRepository
    {
        private readonly HotelDbContext _context;

        public RoomRepository(HotelDbContext context) => _context = context;

        public async Task<List<Room>> GetAllAsync()
            => await _context.Rooms.ToListAsync();

        public async Task<Room?> GetByIdAsync(int id)
            => await _context.Rooms.FindAsync(id);

        public async Task AddAsync(Room room)
            => await _context.Rooms.AddAsync(room);

        public Task UpdateAsync(Room room)
        {
            _context.Rooms.Update(room);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var room = await GetByIdAsync(id);
            if (room != null) _context.Rooms.Remove(room);
        }

        public async Task SaveAsync()
            => await _context.SaveChangesAsync();
    }
}
