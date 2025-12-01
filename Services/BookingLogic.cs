using System;
using System.Collections.Generic;
using DentalFlow.Models;

namespace DentalFlow.Services
{
    public class BookingLogic
    {
        /// <summary>
        /// Kontrollerar om en ny bokningstid är ledig baserat på befintliga bokningar.
        /// </summary>
        public bool IsTimeAvailable(DateTime newStart, int newServiceDuration, List<Booking> existingBookings)
        {
            // Räkna ut sluttid för nya bokningen
            var newEnd = newStart.AddMinutes(newServiceDuration);

            // Om inga bokningar finns är tiden ledig
            if (existingBookings == null || existingBookings.Count == 0)
                return true;

            foreach (var booking in existingBookings)
            {
                if (booking.Service == null)
                    throw new Exception("Booking.Service måste vara laddad i testet.");

                var existingStart = booking.DateTime;
                var existingEnd = booking.DateTime.AddMinutes(booking.Service.DurationMinutes);

                // Kolla om tider överlappar
                bool overlaps =
                    newStart < existingEnd &&
                    newEnd > existingStart;

                if (overlaps)
                    return false;
            }

            return true;
        }
    }
}
