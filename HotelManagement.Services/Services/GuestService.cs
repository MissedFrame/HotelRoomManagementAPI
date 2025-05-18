using HotelManagement.Data.Interfaces;
using HotelManagement.Data.Entities;
using HotelManagement.Services.DTOs;

namespace HotelManagement.Services.Services
{
    public class GuestService
    {
        private readonly IGuestRepository _guestRepository;

        public GuestService(IGuestRepository guestRepository)
        {
            _guestRepository = guestRepository;
        }

        public async Task<List<GuestDTO>> GetAllGuestsAsync()
        {
            var guests = await _guestRepository.GetAllAsync();
            return guests.Select(MapToDTO).ToList();
        }

        public async Task<GuestDTO?> GetGuestByIdAsync(int id)
        {
            var guest = await _guestRepository.GetByIdAsync(id);
            return guest != null ? MapToDTO(guest) : null;
        }

        public async Task<GuestDTO> CreateGuestAsync(GuestDTO guestDTO)
        {
            var existingBooking = await _guestRepository.HasExistingBookingAsync(
                guestDTO.FirstName,
                guestDTO.LastName,
                guestDTO.DOB,
                guestDTO.CheckInDate,
                guestDTO.CheckOutDate);

            if (existingBooking)
                throw new InvalidOperationException("Guest has overlapping booking");

            var guest = new Guest
            {
                FirstName = guestDTO.FirstName,
                LastName = guestDTO.LastName,
                DOB = guestDTO.DOB,
                Address = guestDTO.Address,
                Nationality = guestDTO.Nationality,
                CheckInDate = guestDTO.CheckInDate,
                CheckOutDate = guestDTO.CheckOutDate,
                RoomId = guestDTO.RoomId
            };

            await _guestRepository.AddAsync(guest);
            await _guestRepository.SaveAsync();  // Proper async call
            return MapToDTO(guest);
        }

        private static GuestDTO MapToDTO(Guest guest) => new()
        {
            Id = guest.Id,
            FirstName = guest.FirstName,
            LastName = guest.LastName,
            DOB = guest.DOB,
            Address = guest.Address,
            Nationality = guest.Nationality,
            CheckInDate = guest.CheckInDate,
            CheckOutDate = guest.CheckOutDate,
            RoomId = guest.RoomId
        };
    }

}
