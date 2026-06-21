using LogisticsERP.API.Data;
using LogisticsERP.API.enums;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;
namespace LogisticsERP.API.Repositories
{
        public class RosterRepo : IRosterRepo
        {
            private readonly AppDbContext _context;

            public RosterRepo(AppDbContext context)
            {
                _context = context;
            }

            public async Task<List<DutyRoster>> GetByMonthYearAsync(int month, int year)
            {
                return await _context.DutyRosters
                    .Where(x => x.Month == month && x.Year == year)
                    .Include(x => x.Entries)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();
            }

            public async Task<DutyRoster?> GetWithEntriesAsync(string rosterId)
            {
                return await _context.DutyRosters
                    .Include(x => x.Entries)
                        .ThenInclude(e => e.Driver)
                    .Include(x => x.Entries)
                        .ThenInclude(e => e.Vehicle)
                    .FirstOrDefaultAsync(x => x.RosterId == rosterId);
            }

            public async Task<List<DutyRosterEntry>> GetEntriesByRosterAsync(string rosterId)
            {
                return await _context.DutyRosterEntries
                    .Include(x => x.Driver)
                    .Include(x => x.Vehicle)
                    .Where(x => x.RosterId == rosterId)
                    .OrderBy(x => x.DutyDate).ThenBy(x => x.ShiftStart)
                    .ToListAsync();
            }

            public async Task<List<DutyRosterEntry>> GetEntriesByDateAsync(DateTime date)
            {
                return await _context.DutyRosterEntries
                    .Include(x => x.Driver)
                    .Include(x => x.Vehicle)
                    .Where(x => x.DutyDate.Date == date.Date)
                    .OrderBy(x => x.ShiftStart)
                    .ToListAsync();
            }

            public async Task<List<DutyRosterEntry>> GetEntriesByDriverAsync(string driverId)
            {
                return await _context.DutyRosterEntries
                    .Include(x => x.Driver)
                    .Include(x => x.Vehicle)
                    .Where(x => x.DriverId == driverId)
                    .OrderByDescending(x => x.DutyDate)
                    .ToListAsync();
            }

            public async Task<List<DutyRosterEntry>> GetEntriesByVehicleAsync(string vehicleId)
            {
                return await _context.DutyRosterEntries
                    .Include(x => x.Driver)
                    .Include(x => x.Vehicle)
                    .Where(x => x.VehicleId == vehicleId)
                    .OrderByDescending(x => x.DutyDate)
                    .ToListAsync();
            }

            public async Task<List<DutyRosterEntry>> GetEntriesByStatusAsync(RosterEntryStatus status)
            {
                return await _context.DutyRosterEntries
                    .Include(x => x.Driver)
                    .Include(x => x.Vehicle)
                    .Where(x => x.Status == status)
                    .OrderByDescending(x => x.DutyDate)
                    .ToListAsync();
            }

            public async Task<List<DutyRosterEntry>> GetEntriesByDateRangeAsync(DateTime from, DateTime to)
            {
                return await _context.DutyRosterEntries
                    .Include(x => x.Driver)
                    .Include(x => x.Vehicle)
                    .Where(x => x.DutyDate >= from && x.DutyDate <= to)
                    .OrderBy(x => x.DutyDate).ThenBy(x => x.ShiftStart)
                    .ToListAsync();
            }

            public async Task<bool> HasConflictAsync(string driverId, DateTime date, ShiftType shiftType, string? excludeEntryId = null)
            {
                var query = _context.DutyRosterEntries
                    .Where(x => x.DriverId == driverId
                             && x.DutyDate.Date == date.Date
                             && x.ShiftType == shiftType
                             && x.Status != RosterEntryStatus.Cancelled);

                if (excludeEntryId != null)
                    query = query.Where(x => x.EntryId != excludeEntryId);

                return await query.AnyAsync();
            }

            public async Task AddEntriesAsync(List<DutyRosterEntry> entries)
            {
                await _context.DutyRosterEntries.AddRangeAsync(entries);
            }
        }
}
