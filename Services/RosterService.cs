using AutoMapper;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Roster;
using LogisticsERP.API.enums;
using LogisticsERP.API.Helpers;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.Services
{
    public class RosterService : ServiceBaseFunctions, IRosterService
        {
            private readonly IGenericRepo<DutyRoster> _rosterRepo;
            private readonly IGenericRepo<DutyRosterEntry> _entryRepo;
            private readonly IRosterRepo _rosterSpecificRepo;
            private readonly IGenericRepo<Driver> _driverRepo;
            private readonly IGenericRepo<Vehicle> _vehicleRepo;
            private readonly IMapper _mapper;
            private readonly AppDbContext _context;

            public RosterService(
                IGenericRepo<DutyRoster> rosterRepo,
                IGenericRepo<DutyRosterEntry> entryRepo,
                IRosterRepo rosterSpecificRepo,
                IGenericRepo<Driver> driverRepo,
                IGenericRepo<Vehicle> vehicleRepo,
                IMapper mapper,
                AppDbContext context)
            {
                _rosterRepo = rosterRepo;
                _entryRepo = entryRepo;
                _rosterSpecificRepo = rosterSpecificRepo;
                _driverRepo = driverRepo;
                _vehicleRepo = vehicleRepo;
                _mapper = mapper;
                _context = context;
            }

            // ── CREATE ROSTER ─────────────────────────────────────
            public async Task<ApiResponse<RosterResponseDto>> CreateRosterAsync(RosterCreateDto dto)
            {
                try
                {
                    if (dto.Month < 1 || dto.Month > 12)
                        return Fail<RosterResponseDto>("Month must be between 1 and 12.");

                    var roster = _mapper.Map<DutyRoster>(dto);
                    roster.Status = RosterStatus.Draft;

                    await _rosterRepo.AddAsync(roster);
                    await _context.SaveChangesAsync();

                    return Ok(_mapper.Map<RosterResponseDto>(roster), "Roster created successfully.");
                }
                catch (Exception ex)
                {
                    return Fail<RosterResponseDto>(ex.InnerException?.Message ?? ex.Message);
                }
            }

            // ── UPDATE ROSTER ─────────────────────────────────────
            public async Task<ApiResponse<RosterResponseDto>> UpdateRosterAsync(string id, RosterUpdateDto dto)
            {
                try
                {
                    var roster = await _rosterRepo.GetByIdAsync(id);
                    if (roster == null)
                        return Fail<RosterResponseDto>("Roster not found.");

                    if (dto.Title != null) roster.Title = dto.Title;
                    if (dto.Notes != null) roster.Notes = dto.Notes;
                    if (dto.Status.HasValue) roster.Status = dto.Status.Value;
                    if (dto.ApprovedBy != null) roster.ApprovedBy = dto.ApprovedBy;

                    await _rosterRepo.Update(roster);
                    await _context.SaveChangesAsync();

                    return Ok(_mapper.Map<RosterResponseDto>(roster), "Roster updated successfully.");
                }
                catch (Exception ex)
                {
                    return Fail<RosterResponseDto>(ex.InnerException?.Message ?? ex.Message);
                }
            }

            // ── GET ROSTER BY ID ──────────────────────────────────
            public async Task<ApiResponse<RosterResponseDto>> GetRosterByIdAsync(string id)
            {
                try
                {
                    var roster = await _rosterSpecificRepo.GetWithEntriesAsync(id);
                    if (roster == null)
                        return Fail<RosterResponseDto>("Roster not found.");

                    var result = _mapper.Map<RosterResponseDto>(roster);
                    result.TotalEntries = roster.Entries.Count;

                    return Ok(result, "Roster fetched successfully.");
                }
                catch (Exception ex)
                {
                    return Fail<RosterResponseDto>(ex.InnerException?.Message ?? ex.Message);
                }
            }

            // ── GET ALL ROSTERS ───────────────────────────────────
            public async Task<ApiResponse<List<RosterResponseDto>>> GetAllRostersAsync()
            {
                try
                {
                    var rosters = await _rosterRepo.GetAllAsync();
                    var result = _mapper.Map<List<RosterResponseDto>>(rosters);
                    return Ok(result, $"{result.Count} roster(s) found.");
                }
                catch (Exception ex)
                {
                    return Fail<List<RosterResponseDto>>(ex.InnerException?.Message ?? ex.Message);
                }
            }

            // ── DELETE ROSTER ─────────────────────────────────────
            public async Task<ApiResponse<bool>> DeleteRosterAsync(string id)
            {
                try
                {
                    var roster = await _rosterRepo.GetByIdAsync(id);
                    if (roster == null)
                        return Fail<bool>("Roster not found.");

                    if (roster.Status == RosterStatus.Approved)
                        return Fail<bool>("Approved roster cannot be deleted.");

                    await _rosterRepo.Delete(id);
                    await _context.SaveChangesAsync();
                    return Ok(true, "Roster deleted successfully.");
                }
                catch (Exception ex)
                {
                    return Fail<bool>(ex.InnerException?.Message ?? ex.Message);
                }
            }

            // ── PUBLISH ROSTER ────────────────────────────────────
            public async Task<ApiResponse<RosterResponseDto>> PublishRosterAsync(string id)
            {
                try
                {
                    var roster = await _rosterRepo.GetByIdAsync(id);
                    if (roster == null)
                        return Fail<RosterResponseDto>("Roster not found.");

                    if (roster.Status != RosterStatus.Draft)
                        return Fail<RosterResponseDto>("Only draft rosters can be published.");

                    roster.Status = RosterStatus.Published;
                    await _rosterRepo.Update(roster);
                    await _context.SaveChangesAsync();

                    return Ok(_mapper.Map<RosterResponseDto>(roster), "Roster published successfully.");
                }
                catch (Exception ex)
                {
                    return Fail<RosterResponseDto>(ex.InnerException?.Message ?? ex.Message);
                }
            }

            // ── APPROVE ROSTER ────────────────────────────────────
            public async Task<ApiResponse<RosterResponseDto>> ApproveRosterAsync(string id, string approvedBy)
            {
                try
                {
                    var roster = await _rosterRepo.GetByIdAsync(id);
                    if (roster == null)
                        return Fail<RosterResponseDto>("Roster not found.");

                    if (roster.Status != RosterStatus.Published)
                        return Fail<RosterResponseDto>("Only published rosters can be approved.");

                    roster.Status = RosterStatus.Approved;
                    roster.ApprovedBy = approvedBy;

                    await _rosterRepo.Update(roster);
                    await _context.SaveChangesAsync();

                    return Ok(_mapper.Map<RosterResponseDto>(roster), "Roster approved successfully.");
                }
                catch (Exception ex)
                {
                    return Fail<RosterResponseDto>(ex.InnerException?.Message ?? ex.Message);
                }
            }

            // ── CLOSE ROSTER ──────────────────────────────────────
            public async Task<ApiResponse<RosterResponseDto>> CloseRosterAsync(string id)
            {
                try
                {
                    var roster = await _rosterRepo.GetByIdAsync(id);
                    if (roster == null)
                        return Fail<RosterResponseDto>("Roster not found.");

                    roster.Status = RosterStatus.Closed;
                    await _rosterRepo.Update(roster);
                    await _context.SaveChangesAsync();

                    return Ok(_mapper.Map<RosterResponseDto>(roster), "Roster closed successfully.");
                }
                catch (Exception ex)
                {
                    return Fail<RosterResponseDto>(ex.InnerException?.Message ?? ex.Message);
                }
            }

            // ── CREATE ENTRY ──────────────────────────────────────
            public async Task<ApiResponse<RosterEntryResponseDto>> CreateEntryAsync(RosterEntryCreateDto dto)
            {
                try
                {
                    var roster = await _rosterRepo.GetByIdAsync(dto.RosterId);
                    if (roster == null)
                        return Fail<RosterEntryResponseDto>("Roster not found.");

                    if (roster.Status == RosterStatus.Closed)
                        return Fail<RosterEntryResponseDto>("Cannot add entries to a closed roster.");

                    var driver = await _driverRepo.GetByIdAsync(dto.DriverId);
                    if (driver == null)
                        return Fail<RosterEntryResponseDto>("Driver not found.");

                    var vehicle = await _vehicleRepo.GetByIdAsync(dto.VehicleId);
                    if (vehicle == null)
                        return Fail<RosterEntryResponseDto>("Vehicle not found.");

                    // Check for shift conflict
                    var hasConflict = await _rosterSpecificRepo.HasConflictAsync(dto.DriverId, dto.DutyDate, dto.ShiftType);
                    if (hasConflict)
                        return Fail<RosterEntryResponseDto>($"Driver already has a {dto.ShiftType} shift on {dto.DutyDate:dd MMM yyyy}.");

                    var entry = _mapper.Map<DutyRosterEntry>(dto);
                    await _entryRepo.AddAsync(entry);
                    await _context.SaveChangesAsync();

                    return Ok(MapEntryResponse(entry, driver, vehicle), "Roster entry created successfully.");
                }
                catch (Exception ex)
                {
                    return Fail<RosterEntryResponseDto>(ex.InnerException?.Message ?? ex.Message);
                }
            }

            // ── BULK CREATE ENTRIES ───────────────────────────────
            public async Task<ApiResponse<List<RosterEntryResponseDto>>> BulkCreateEntriesAsync(RosterBulkCreateDto dto)
            {
                try
                {
                    var roster = await _rosterRepo.GetByIdAsync(dto.RosterId);
                    if (roster == null)
                        return Fail<List<RosterEntryResponseDto>>("Roster not found.");

                    if (roster.Status == RosterStatus.Closed)
                        return Fail<List<RosterEntryResponseDto>>("Cannot add entries to a closed roster.");

                    var errors = new List<string>();
                    var entries = new List<DutyRosterEntry>();

                    foreach (var entryDto in dto.Entries)
                    {
                        var driver = await _driverRepo.GetByIdAsync(entryDto.DriverId);
                        var vehicle = await _vehicleRepo.GetByIdAsync(entryDto.VehicleId);

                        if (driver == null) { errors.Add($"Driver {entryDto.DriverId} not found."); continue; }
                        if (vehicle == null) { errors.Add($"Vehicle {entryDto.VehicleId} not found."); continue; }

                        var hasConflict = await _rosterSpecificRepo.HasConflictAsync(entryDto.DriverId, entryDto.DutyDate, entryDto.ShiftType);
                        if (hasConflict) { errors.Add($"Driver {driver.FullName} already has {entryDto.ShiftType} shift on {entryDto.DutyDate:dd MMM yyyy}."); continue; }

                        entries.Add(_mapper.Map<DutyRosterEntry>(entryDto));
                    }

                    if (errors.Any() && !entries.Any())
                        return Fail<List<RosterEntryResponseDto>>(string.Join(" | ", errors));

                    await _rosterSpecificRepo.AddEntriesAsync(entries);
                    await _context.SaveChangesAsync();

                    var message = errors.Any()
                        ? $"{entries.Count} entries created. Skipped: {string.Join(" | ", errors)}"
                        : $"{entries.Count} entries created successfully.";

                    return Ok(_mapper.Map<List<RosterEntryResponseDto>>(entries), message);
                }
                catch (Exception ex)
                {
                    return Fail<List<RosterEntryResponseDto>>(ex.InnerException?.Message ?? ex.Message);
                }
            }

            // ── UPDATE ENTRY ──────────────────────────────────────
            public async Task<ApiResponse<RosterEntryResponseDto>> UpdateEntryAsync(string id, RosterEntryUpdateDto dto)
            {
                try
                {
                    var entry = await _entryRepo.GetByIdAsync(id);
                    if (entry == null)
                        return Fail<RosterEntryResponseDto>("Roster entry not found.");

                    // Check shift conflict if driver/date/shift changed
                    if (dto.DriverId != null || dto.DutyDate.HasValue || dto.ShiftType.HasValue)
                    {
                        var driverId = dto.DriverId ?? entry.DriverId;
                        var date = dto.DutyDate ?? entry.DutyDate;
                        var shiftType = dto.ShiftType ?? entry.ShiftType;

                        var hasConflict = await _rosterSpecificRepo.HasConflictAsync(driverId, date, shiftType, id);
                        if (hasConflict)
                            return Fail<RosterEntryResponseDto>($"Driver already has a {shiftType} shift on {date:dd MMM yyyy}.");
                    }

                    if (dto.DriverId != null) entry.DriverId = dto.DriverId;
                    if (dto.VehicleId != null) entry.VehicleId = dto.VehicleId;
                    if (dto.DutyDate.HasValue) entry.DutyDate = dto.DutyDate.Value;
                    if (dto.ShiftType.HasValue) entry.ShiftType = dto.ShiftType.Value;
                    if (dto.ShiftStart.HasValue) entry.ShiftStart = dto.ShiftStart.Value;
                    if (dto.ShiftEnd.HasValue) entry.ShiftEnd = dto.ShiftEnd.Value;
                    if (dto.DutyType.HasValue) entry.DutyType = dto.DutyType.Value;
                    if (dto.Purpose != null) entry.Purpose = dto.Purpose;
                    if (dto.FromLocation != null) entry.FromLocation = dto.FromLocation;
                    if (dto.ToLocation != null) entry.ToLocation = dto.ToLocation;
                    if (dto.OfficerName != null) entry.OfficerName = dto.OfficerName;
                    if (dto.Donor != null) entry.Donor = dto.Donor;
                    if (dto.Notes != null) entry.Notes = dto.Notes;
                    if (dto.Status.HasValue) entry.Status = dto.Status.Value;
                    if (dto.DutyLogId != null) entry.DutyLogId = dto.DutyLogId;

                    await _entryRepo.Update(entry);
                    await _context.SaveChangesAsync();

                    return Ok(_mapper.Map<RosterEntryResponseDto>(entry), "Roster entry updated successfully.");
                }
                catch (Exception ex)
                {
                    return Fail<RosterEntryResponseDto>(ex.InnerException?.Message ?? ex.Message);
                }
            }

            // ── DELETE ENTRY ──────────────────────────────────────
            public async Task<ApiResponse<bool>> DeleteEntryAsync(string id)
            {
                try
                {
                    var entry = await _entryRepo.GetByIdAsync(id);
                    if (entry == null)
                        return Fail<bool>("Roster entry not found.");

                    if (entry.Status == RosterEntryStatus.Completed)
                        return Fail<bool>("Completed entries cannot be deleted.");

                    await _entryRepo.Delete(id);
                    await _context.SaveChangesAsync();
                    return Ok(true, "Roster entry deleted successfully.");
                }
                catch (Exception ex)
                {
                    return Fail<bool>(ex.InnerException?.Message ?? ex.Message);
                }
            }

            // ── MARK IN PROGRESS ──────────────────────────────────
            public async Task<ApiResponse<RosterEntryResponseDto>> MarkAsInProgressAsync(string entryId)
            {
                try
                {
                    var entry = await _entryRepo.GetByIdAsync(entryId);
                    if (entry == null)
                        return Fail<RosterEntryResponseDto>("Entry not found.");

                    if (entry.Status != RosterEntryStatus.Scheduled)
                        return Fail<RosterEntryResponseDto>("Only scheduled entries can be marked as in progress.");

                    entry.Status = RosterEntryStatus.InProgress;
                    await _entryRepo.Update(entry);
                    await _context.SaveChangesAsync();

                    return Ok(_mapper.Map<RosterEntryResponseDto>(entry), "Entry marked as in progress.");
                }
                catch (Exception ex)
                {
                    return Fail<RosterEntryResponseDto>(ex.InnerException?.Message ?? ex.Message);
                }
            }

            // ── MARK COMPLETED ────────────────────────────────────
            public async Task<ApiResponse<RosterEntryResponseDto>> MarkAsCompletedAsync(string entryId, string dutyLogId)
            {
                try
                {
                    var entry = await _entryRepo.GetByIdAsync(entryId);
                    if (entry == null)
                        return Fail<RosterEntryResponseDto>("Entry not found.");

                    entry.Status = RosterEntryStatus.Completed;
                    entry.DutyLogId = dutyLogId;

                    await _entryRepo.Update(entry);
                    await _context.SaveChangesAsync();

                    return Ok(_mapper.Map<RosterEntryResponseDto>(entry), "Entry marked as completed.");
                }
                catch (Exception ex)
                {
                    return Fail<RosterEntryResponseDto>(ex.InnerException?.Message ?? ex.Message);
                }
            }

            // ── MARK MISSED ───────────────────────────────────────
            public async Task<ApiResponse<RosterEntryResponseDto>> MarkAsMissedAsync(string entryId)
            {
                try
                {
                    var entry = await _entryRepo.GetByIdAsync(entryId);
                    if (entry == null)
                        return Fail<RosterEntryResponseDto>("Entry not found.");

                    if (entry.Status == RosterEntryStatus.Completed)
                        return Fail<RosterEntryResponseDto>("Completed entries cannot be marked as missed.");

                    entry.Status = RosterEntryStatus.Missed;
                    await _entryRepo.Update(entry);
                    await _context.SaveChangesAsync();

                    return Ok(_mapper.Map<RosterEntryResponseDto>(entry), "Entry marked as missed.");
                }
                catch (Exception ex)
                {
                    return Fail<RosterEntryResponseDto>(ex.InnerException?.Message ?? ex.Message);
                }
            }

            // ── CANCEL ENTRY ──────────────────────────────────────
            public async Task<ApiResponse<RosterEntryResponseDto>> CancelEntryAsync(string entryId, string reason)
            {
                try
                {
                    var entry = await _entryRepo.GetByIdAsync(entryId);
                    if (entry == null)
                        return Fail<RosterEntryResponseDto>("Entry not found.");

                    if (entry.Status == RosterEntryStatus.Completed)
                        return Fail<RosterEntryResponseDto>("Completed entries cannot be cancelled.");

                    entry.Status = RosterEntryStatus.Cancelled;
                    entry.Notes = $"Cancelled: {reason}";

                    await _entryRepo.Update(entry);
                    await _context.SaveChangesAsync();

                    return Ok(_mapper.Map<RosterEntryResponseDto>(entry), "Entry cancelled successfully.");
                }
                catch (Exception ex)
                {
                    return Fail<RosterEntryResponseDto>(ex.InnerException?.Message ?? ex.Message);
                }
            }

            // ── MONTHLY VIEW ──────────────────────────────────────
            public async Task<ApiResponse<MonthlyRosterViewDto>> GetMonthlyViewAsync(int month, int year)
            {
                try
                {
                    var rosters = await _rosterSpecificRepo.GetByMonthYearAsync(month, year);
                    if (!rosters.Any())
                        return Fail<MonthlyRosterViewDto>($"No roster found for {month}/{year}.");

                    var roster = rosters.First();
                    var entries = await _rosterSpecificRepo.GetEntriesByRosterAsync(roster.RosterId);

                    var dailyGroups = entries
                        .GroupBy(x => x.DutyDate.Date)
                        .OrderBy(g => g.Key)
                        .Select(g => new DailyRosterViewDto
                        {
                            Date = g.Key,
                            TotalAssigned = g.Count(),
                            Completed = g.Count(x => x.Status == RosterEntryStatus.Completed),
                            Missed = g.Count(x => x.Status == RosterEntryStatus.Missed),
                            InProgress = g.Count(x => x.Status == RosterEntryStatus.InProgress),
                            Shifts = _mapper.Map<List<RosterEntryResponseDto>>(g.OrderBy(x => x.ShiftStart).ToList())
                        }).ToList();

                    var view = new MonthlyRosterViewDto
                    {
                        RosterId = roster.RosterId,
                        Title = roster.Title,
                        Month = month,
                        Year = year,
                        Status = roster.Status,
                        TotalDuties = entries.Count,
                        CompletedDuties = entries.Count(x => x.Status == RosterEntryStatus.Completed),
                        MissedDuties = entries.Count(x => x.Status == RosterEntryStatus.Missed),
                        ScheduledDuties = entries.Count(x => x.Status == RosterEntryStatus.Scheduled),
                        DailyView = dailyGroups
                    };

                    return Ok(view, $"Monthly roster for {month}/{year} fetched successfully.");
                }
                catch (Exception ex)
                {
                    return Fail<MonthlyRosterViewDto>(ex.InnerException?.Message ?? ex.Message);
                }
            }

            // ── DAILY VIEW ────────────────────────────────────────
            public async Task<ApiResponse<DailyRosterViewDto>> GetDailyViewAsync(DateTime date)
            {
                try
                {
                    var entries = await _rosterSpecificRepo.GetEntriesByDateAsync(date);

                    var view = new DailyRosterViewDto
                    {
                        Date = date,
                        TotalAssigned = entries.Count,
                        Completed = entries.Count(x => x.Status == RosterEntryStatus.Completed),
                        Missed = entries.Count(x => x.Status == RosterEntryStatus.Missed),
                        InProgress = entries.Count(x => x.Status == RosterEntryStatus.InProgress),
                        Shifts = _mapper.Map<List<RosterEntryResponseDto>>(entries)
                    };

                    return Ok(view, $"Daily roster for {date:dd MMM yyyy} fetched successfully.");
                }
                catch (Exception ex)
                {
                    return Fail<DailyRosterViewDto>(ex.InnerException?.Message ?? ex.Message);
                }
            }

            // ── DRIVER ROSTER VIEW ────────────────────────────────
            public async Task<ApiResponse<DriverRosterViewDto>> GetDriverRosterViewAsync(string driverId, int month, int year)
            {
                try
                {
                    var driver = await _driverRepo.GetByIdAsync(driverId);
                    if (driver == null)
                        return Fail<DriverRosterViewDto>("Driver not found.");

                    var entries = await _rosterSpecificRepo.GetEntriesByDriverAsync(driverId);
                    var filtered = entries
                        .Where(x => x.DutyDate.Month == month && x.DutyDate.Year == year)
                        .ToList();

                    var view = new DriverRosterViewDto
                    {
                        DriverId = driverId,
                        DriverName = driver.FullName,
                        TotalDuties = filtered.Count,
                        CompletedDuties = filtered.Count(x => x.Status == RosterEntryStatus.Completed),
                        MissedDuties = filtered.Count(x => x.Status == RosterEntryStatus.Missed),
                        Duties = _mapper.Map<List<RosterEntryResponseDto>>(filtered)
                    };

                    return Ok(view, $"Driver roster for {month}/{year} fetched successfully.");
                }
                catch (Exception ex)
                {
                    return Fail<DriverRosterViewDto>(ex.InnerException?.Message ?? ex.Message);
                }
            }

            // ── HELPER ────────────────────────────────────────────
            private RosterEntryResponseDto MapEntryResponse(DutyRosterEntry entry, Driver driver, Vehicle vehicle)
            {
                var dto = _mapper.Map<RosterEntryResponseDto>(entry);
                dto.DriverName = driver.FullName;
                dto.VehicleNumber = vehicle.Number;
                return dto;
            }
    }
}
