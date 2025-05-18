using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Data.Entities;
using HotelManagement.Data.Interfaces;
using HotelManagement.Services.DTOs;
using HotelManagement.Services;

namespace HotelManagement.Services.Services
{
    public class RoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<RoomDTO?> GetRoomByIdAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            return room != null ? MapToDTO(room) : null;
        }

        public async Task<RoomDTO> CreateRoomAsync(RoomDTO roomDTO)
        {
            var room = new Room
            {
                Number = roomDTO.Number,
                Floor = roomDTO.Floor,
                Type = roomDTO.Type
            };

            await _roomRepository.AddAsync(room);
            await _roomRepository.SaveAsync();

            roomDTO.Id = room.Id;
            return roomDTO;
        }

        private static RoomDTO MapToDTO(Room room) => new()
        {
            Id = room.Id,
            Number = room.Number,
            Floor = room.Floor,
            Type = room.Type
        };
    }
}
