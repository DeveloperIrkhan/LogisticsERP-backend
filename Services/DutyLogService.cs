using AutoMapper;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.DutyLogs;
using LogisticsERP.API.enums;
using LogisticsERP.API.Helpers;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using LogisticsERP.API.Repositories;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LogisticsERP.API.Services
{
    public class DutyLogService : ServiceBaseFunctions, IDutyLogService
    {
        private readonly IGenericRepo<DutyLogs> _genericRepo;
        private readonly IDutyRepo _dutyRepo;
        private readonly IGenericRepo<Vehicle> _vehicleRepo;
        private readonly IGenericRepo<Driver> _driverRepo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public DutyLogService(
            IGenericRepo<DutyLogs> genericRepo,
            IDutyRepo dutyRepo,
            IGenericRepo<Vehicle> vehicleRepo,
            IGenericRepo<Driver> driverRepo,
            IMapper mapper,
            AppDbContext context)
        {
            _genericRepo = genericRepo;
            _dutyRepo = dutyRepo;
            _vehicleRepo = vehicleRepo;
            _driverRepo = driverRepo;
            _mapper = mapper;
            _context = context;
        }
        public async Task<ApiResponse<DutyResponseDto>> CreateAsync(DutyCreateDto dto)
        {
            try
            {
                var vehicle = await _vehicleRepo.GetByIdAsync(dto.VehicleId);
                if (vehicle == null)
                    return Fail<DutyResponseDto>("vehicle not found");
                var driver = await _driverRepo.GetByIdAsync(dto.DriverId);
                if (driver == null)
                    return Fail<DutyResponseDto>("driver not found");

                var duty = _mapper.Map<DutyLogs>(dto);
                duty.Status = DutyStatus.Pending;

                await _genericRepo.AddAsync(duty);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<DutyResponseDto>(duty), "Duty Create Successfully");
            }
            catch (Exception ex)
            {
                return Fail<DutyResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<DutyResponseDto>> UpdateAsync(string Id, DutyUpdateDto dto)
        {
            try
            {
                var duty = await _genericRepo.GetByIdAsync(Id);
                if (duty == null)
                    return Fail<DutyResponseDto>("Duty not found.");

                //updating the desire fields
                if (dto.FromLocation != null) duty.FromLocation = dto.FromLocation;
                if (dto.ToLocation != null) duty.ToLocation = dto.ToLocation;
                if (dto.Purpose != null) duty.Purpose = dto.Purpose;
                if (dto.OfficerName != null) duty.OfficerName = dto.OfficerName;
                if (dto.Donor != null) duty.Donor = dto.Donor;
                if (dto.Remarks != null) duty.Remarks = dto.Remarks;
                if (dto.DutyType.HasValue) duty.DutyType = dto.DutyType.Value;
                if (dto.Status.HasValue) duty.Status = dto.Status.Value;
                if (dto.DateIn.HasValue) duty.DateIn = dto.DateIn.Value;
                if (dto.KillometerOut.HasValue) duty.KillometerOut = dto.KillometerOut.Value;
                if (dto.KillometerIn.HasValue) duty.KillometerIn = dto.KillometerIn.Value;
                if (dto.TotalKm.HasValue) duty.TotalKm = dto.TotalKm.Value;
                if (dto.TotalHours.HasValue) duty.TotalHours = dto.TotalHours.Value;

                await _genericRepo.Update(duty);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<DutyResponseDto>(duty), "Duty updated successfully.");

            }
            catch (Exception exp)
            {
                return Fail<DutyResponseDto>(exp.InnerException?.Message ?? exp.Message);
            }
        }

        public async Task<ApiResponse<DutyResponseDto>> GetByIdAsync(string id)
        {
            try
            {
                var duty = await _dutyRepo.getFullDutyById(id);
                if (duty == null)
                    return Fail<DutyResponseDto>("Duty not found.");

                return Ok(_mapper.Map<DutyResponseDto>(duty), "Duty fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<DutyResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<DutyResponseDto>>> GetAllAsync()
        {
            try
            {
                var duties = await _genericRepo.GetAllAsync();
                var result = _mapper.Map<List<DutyResponseDto>>(duties);
                return Ok(result, $"{result.Count} duty record(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<DutyResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            try
            {
                var duty = await _genericRepo.GetByIdAsync(id);
                if (duty == null)
                    return Fail<bool>("Duty not found.");

                await _genericRepo.Delete(id);
                await _context.SaveChangesAsync();
                return Ok(true, "Duty deleted successfully.");
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<DutyResponseDto>>> GetByDriverAsync(string driverId)
        {
            try
            {
                var existingDriver = await _genericRepo.GetByIdAsync(driverId);
                if (existingDriver == null) return Fail<List<DutyResponseDto>>("driver not found");
                var drivers = _mapper.Map<List<DutyResponseDto>>(existingDriver);
                return Ok(drivers, $"{drivers.Count} duty record(s) found for driver.");
            }
            catch (Exception ex)
            {
                return Fail<List<DutyResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<DutyResponseDto>>> GetByStatusAsync(DutyStatus status)
        {
            try
            {
                var duties = await _dutyRepo.GetByStatusAsync(status);
                if (duties == null) return Fail<List<DutyResponseDto>>("no duty found for this status!");

                var result = _mapper.Map<List<DutyResponseDto>>(duties);
                return Ok(result, $"{result.Count} duty record(s) found with status {status}.");
            }
            catch (Exception ex)
            {
                return Fail<List<DutyResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<DutyResponseDto>>> GetByVehicleAsync(string vehicleId)
        {
            try
            {
                var duties = await _dutyRepo.GetByVehicleAsync(vehicleId);
                if (duties == null) return Fail<List<DutyResponseDto>>("duties not found");

                var result = _mapper.Map<List<DutyResponseDto>>(duties);
                return Ok(result, $"{result.Count} duty record(s) found for vehicle.");
            }
            catch (Exception ex)
            {
                return Fail<List<DutyResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<DutyResponseDto>> StartDutyAsync(string dutyId)
        {
            try
            {
                var duty = await _genericRepo.GetByIdAsync(dutyId);
                if (duty == null)
                    return Fail<DutyResponseDto>("Duty not found.");
                if (duty.Status == DutyStatus.InProgress)
                    return Fail<DutyResponseDto>("Duty already in progress.");
                if (duty.Status == DutyStatus.Completed)
                    return Fail<DutyResponseDto>("Duty already in Completed.");

                duty.Status = DutyStatus.InProgress;
                duty.DateOut = DateTime.UtcNow;

                await _genericRepo.Update(duty);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<DutyResponseDto>(duty), "Duty started successfully.");
            }
            catch (Exception ex)
            {
                return Fail<DutyResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ApiResponse<DutyResponseDto>> EndDutyAsync(string dutyId, EndDutyDto dto)
        {
            try
            {
                var duty = await _genericRepo.GetByIdAsync(dutyId);
                if (duty == null)
                    return Fail<DutyResponseDto>("Duty not found.");

                if (duty.Status == DutyStatus.Completed)
                    return Fail<DutyResponseDto>("Duty already in Completed.");
                if (duty.Status == DutyStatus.Pending)
                    return Fail<DutyResponseDto>("Duty not started yet.");

                duty.Status = DutyStatus.Completed;
                duty.DateIn = dto.DateIn;
                duty.KillometerIn = dto.KillometerIn;

                if (duty.KillometerOut.HasValue)
                    duty.TotalKm = dto.KillometerIn - duty.KillometerOut.Value;

                duty.TotalHours = (decimal)(duty.DateOut - dto.DateIn).TotalHours;

                if (dto.Remarks != null)
                    duty.Remarks = dto.Remarks;

                await _genericRepo.Update(duty);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<DutyResponseDto>(duty), "Duty started successfully.");
            }
            catch (Exception ex)
            {
                return Fail<DutyResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<DutyResponseDto>>> GetActiveDutiesAsync()
        {
            try
            {
                var duties = await _dutyRepo.GetActiveDutiesAsync();
                var result = _mapper.Map<List<DutyResponseDto>>(duties);
                return Ok(result, $"{result.Count} active duty record(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<DutyResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<DutyResponseDto>> ApproveDutyAsync(string dutyId, string approvedBy)
        {
            try
            {
                var duty = await _genericRepo.GetByIdAsync(dutyId);
                if (duty == null)
                    return Fail<DutyResponseDto>("Duty not found.");

                if (duty.Status != DutyStatus.Completed)
                    return Fail<DutyResponseDto>("Only completed duties can be approved.");
                duty.ApprovedBy = approvedBy;
                duty.Status = DutyStatus.Approved;

                await _genericRepo.Update(duty);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<DutyResponseDto>(duty), "Duty approved successfully.");

            }
            catch (Exception ex)
            {
                return Fail<DutyResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<DutyResponseDto>>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            try
            {
                if (from > to)
                    return Fail<List<DutyResponseDto>>("'From' date cannot be greater than 'To' date.");

                var duties = await _dutyRepo.GetByDateRangeAsync(from, to);
                var result = _mapper.Map<List<DutyResponseDto>>(duties);
                return Ok(result, $"{result.Count} duty record(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<DutyResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<DutyResponseDto>> CancelDutyAsync(string dutyId, string reason)
        {
            try
            {
                var duty = await _genericRepo.GetByIdAsync(dutyId);
                if (duty == null)
                    return Fail<DutyResponseDto>("Duty not found.");

                if (duty.Status == DutyStatus.Completed)
                    return Fail<DutyResponseDto>("Completed duty cannot be cancelled.");

                if (duty.Status == DutyStatus.Cancelled)
                    return Fail<DutyResponseDto>("Duty is already cancelled.");

                duty.Status = DutyStatus.Cancelled;
                duty.CancellationReason = reason;
                duty.CancelledAt = DateTime.UtcNow;

                await _genericRepo.Update(duty);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<DutyResponseDto>(duty), "Duty cancelled successfully.");
            }
            catch (Exception ex)
            {
                return Fail<DutyResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }

        }


        public async Task<byte[]> ExportDutyPdfAsync(
    string id)
        {
            var reportData = await GetByIdAsync(id) ?? null;
            if (reportData == null)
                throw new Exception("Duty record not found.");
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Portrait());

                    page.MarginHorizontal(30);
                    page.MarginVertical(25);

                    page.DefaultTextStyle(x =>
                        x.FontFamily("Arial")
                         .FontSize(9)
                         .FontColor(Colors.Grey.Darken3));

                    page.Header()
                        .Element(c =>
                            ComposeHeader(
                                c,
                                "Duty Report",
                                reportData.Data.DutyId));

                    page.Content()
                        .PaddingTop(15)
                        .Element(c =>
                            ComposeDutyReport(
                                c,
                                reportData?.Data));

                    page.Footer()
                        .Element(ComposeFooter);
                });
            }).GeneratePdf();
        }


        //-----------------helper----------------------------------------
        private void ComposeHeader(
    IContainer container,
    string title,
    string dutyId)
        {
            container
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten1)
                .PaddingBottom(10)
                .Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item()
                            .Text("PAKISTAN RED CRESCENT SOCIETY")
                            .FontSize(16)
                            .Bold()
                            .FontColor(Colors.Red.Medium);

                        col.Item()
                            .PaddingTop(2)
                            .Text(title)
                            .FontSize(11)
                            .FontColor(Colors.Grey.Darken1);
                    });

                    row.ConstantItem(180)
                        .AlignRight()
                        .Column(col =>
                        {
                            col.Item()
                                .Text($"Generated: {DateTime.Now:dd MMM yyyy}")
                                .FontSize(8)
                                .FontColor(Colors.Grey.Medium);

                            col.Item()
                                .Text($"Time: {DateTime.Now:hh:mm tt}")
                                .FontSize(8)
                                .FontColor(Colors.Grey.Medium);

                            col.Item()
                                .PaddingTop(4)
                                .Text($"Duty ID: {dutyId}")
                                .FontSize(8)
                                .Bold()
                                .FontColor(Colors.Grey.Darken2);
                        });
                });
        }

        private static string NullToDash(string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? "-"
                : value;
        }

        private static string FormatDecimal(decimal? value)
        {
            return value.HasValue
                ? value.Value.ToString("0.##")
                : "-";
        }

        private static string GetDriverName(DutyResponseDto duty)
        {
            if (duty.Driver == null)
                return "-";

            // Change these properties according to your DriverResponseDto
            return duty.Driver.FullName ?? "-";
        }

        private void ComposeDutyReport(
    IContainer container,
    DutyResponseDto duty)
        {
            container.Column(column =>
            {

                column.Item()
                    .Element(c => ComposeSummary(c, duty));

                column.Item().PaddingTop(12);

                column.Item()
                    .Element(c => SectionTitle(c, "Vehicle & Driver"));

                column.Item()
                    .PaddingTop(6)
                    .Element(c =>
                        InformationGrid(
                            c,

                            ("Vehicle No", duty.Vehicle?.Number ?? "-"),
                            ("Vehicle Model", duty.Vehicle?.ModelName ?? "-"),
                            ("Driver", GetDriverName(duty)),
                            ("Officer", NullToDash(duty.OfficerName))
                        ));

                column.Item().PaddingTop(12);

                // ---------------------------------------------------------
                // JOURNEY DETAILS
                // ---------------------------------------------------------

                column.Item()
                    .Element(c => SectionTitle(c, "Journey Details"));

                column.Item()
                    .PaddingTop(6)
                    .Element(c =>
                        InformationGrid(
                            c,

                            ("From", NullToDash(duty.FromLocation)),
                            ("To", NullToDash(duty.ToLocation)),
                            ("Purpose", NullToDash(duty.Purpose)),
                            ("Donor", NullToDash(duty.Donor))
                        ));

                column.Item().PaddingTop(12);
                column.Item()
                    .Element(c => SectionTitle(c, "Timing & Distance"));
                column.Item()
                    .PaddingTop(6)
                    .Element(c =>
                        InformationGrid(
                            c,

                            ("Date Out", duty.DateOut.ToString("dd MMM yyyy")),
                            ("Out Time", duty.DateOut.ToString("hh:mm tt")),
                            ("Date In", duty.DateIn?.ToString("dd MMM yyyy") ?? "-"),
                            ("In Time", duty.DateIn?.ToString("hh:mm tt") ?? "-"),

                            ("KM Out", FormatDecimal(duty.KillometerOut)),
                            ("KM In", FormatDecimal(duty.KillometerIn)),
                            ("Total KM", FormatDecimal(duty.TotalKm)),
                            ("Total Hours", FormatDecimal(duty.TotalHours))
                        ));

                column.Item().PaddingTop(12);


                column.Item()
                    .Element(c => SectionTitle(c, "Authorization"));

                column.Item()
                    .PaddingTop(6)
                    .Element(c =>
                        InformationGrid(
                            c,

                            ("Duty Type", duty.DutyType.ToString()),
                            ("Status", duty.Status.ToString()),
                            ("Approved By", NullToDash(duty.ApprovedBy)),
                            ("Created At", duty.CreatedAt.ToString("dd MMM yyyy hh:mm tt"))
                        ));


                if (!string.IsNullOrWhiteSpace(duty.Remarks))
                {
                    column.Item().PaddingTop(12);

                    column.Item()
                        .Element(c => SectionTitle(c, "Remarks"));

                    column.Item()
                        .PaddingTop(6)
                        .Background(Colors.Grey.Lighten4)
                        .Border(1)
                        .BorderColor(Colors.Grey.Lighten2)
                        .Padding(10)
                        .Text(duty.Remarks)
                        .FontSize(9);
                }


                if (!string.IsNullOrWhiteSpace(duty.CancellationReason))
                {
                    column.Item().PaddingTop(12);

                    column.Item()
                        .Element(c => SectionTitle(c, "Cancellation Information"));

                    column.Item()
                        .PaddingTop(6)
                        .Background(Colors.Red.Lighten5)
                        .Border(1)
                        .BorderColor(Colors.Red.Lighten2)
                        .Padding(10)
                        .Column(col =>
                        {
                            col.Item()
                                .Text("Cancellation Reason")
                                .Bold()
                                .FontSize(8);

                            col.Item()
                                .PaddingTop(3)
                                .Text(duty.CancellationReason)
                                .FontSize(9);

                            if (duty.CancelledAt.HasValue)
                            {
                                col.Item()
                                    .PaddingTop(5)
                                    .Text($"Cancelled At: {duty.CancelledAt:dd MMM yyyy hh:mm tt}")
                                    .FontSize(8)
                                    .FontColor(Colors.Grey.Darken1);
                            }
                        });
                }
            });
        }
        private void SectionTitle(IContainer container, string title)
        {
            container
                .PaddingBottom(2)
                .Row(row =>
                {
                    row.ConstantItem(4)
                        .Background(Colors.Red.Medium);

                    row.RelativeItem()
                        .PaddingLeft(7)
                        .Text(title)
                        .FontSize(11)
                        .Bold()
                        .FontColor(Colors.Grey.Darken3);
                });
        }
        private void InformationGrid(IContainer container, params (string Label, string Value)[] items)
        {
            container
                .Column(column =>
                {
                    for (int i = 0; i < items.Length; i += 4)
                    {
                        var rowItems = items
                            .Skip(i)
                            .Take(4)
                            .ToArray();

                        column.Item()
                            .Row(row =>
                            {
                                foreach (var item in rowItems)
                                {
                                    row.RelativeItem()
                                        .PaddingRight(8)
                                        .Element(c =>
                                            InformationCard(
                                                c,
                                                item.Label,
                                                item.Value
                                            ));
                                }

                                // Fill remaining space
                                for (int j = rowItems.Length; j < 4; j++)
                                {
                                    row.RelativeItem()
                                        .PaddingRight(8);
                                }
                            });

                        if (i + 4 < items.Length)
                        {
                            column.Item().PaddingTop(7);
                        }
                    }
                });
        }
        private void InformationCard(IContainer container, string label, string value)
        {
            container
                .Background(Colors.White)
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(8)
                .MinHeight(42)
                .Column(column =>
                {
                    column.Item()
                        .Text(label.ToUpper())
                        .FontSize(7)
                        .Bold()
                        .FontColor(Colors.Grey.Medium);

                    column.Item()
                        .PaddingTop(4)
                        .Text(string.IsNullOrWhiteSpace(value) ? "-" : value)
                        .FontSize(9)
                        .FontColor(Colors.Grey.Darken3);
                });
        }
        private void ComposeSummary(IContainer container, DutyResponseDto duty)
        {
            container
                .Background(Colors.Grey.Lighten5)
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(12)
                .Row(row =>
                {
                    row.RelativeItem()
                        .Column(col =>
                        {
                            col.Item()
                                .Text("DUTY SUMMARY")
                                .FontSize(9)
                                .Bold()
                                .FontColor(Colors.Grey.Darken1);

                            col.Item()
                                .PaddingTop(4)
                                .Text($"{duty.FromLocation}  →  {duty.ToLocation}")
                                .FontSize(15)
                                .Bold()
                                .FontColor(Colors.Grey.Darken4);

                            col.Item()
                                .PaddingTop(3)
                                .Text(duty.Purpose)
                                .FontSize(9)
                                .FontColor(Colors.Grey.Darken1);
                        });

                    row.ConstantItem(130)
                        .AlignMiddle()
                        .AlignRight()
                        .Element(c =>
                            StatusBadge(
                                c,
                                duty.Status.ToString()
                            ));
                });
        }
        private void StatusBadge(IContainer container, string status)
        {
            var background = status.ToLower() switch
            {
                "completed" => Colors.Green.Lighten4,
                "approved" => Colors.Green.Lighten4,
                "cancelled" => Colors.Red.Lighten4,
                "pending" => Colors.Orange.Lighten4,
                "rejected" => Colors.Red.Lighten4,
                _ => Colors.Grey.Lighten3
            };

            var textColor = status.ToLower() switch
            {
                "completed" => Colors.Green.Darken2,
                "approved" => Colors.Green.Darken2,
                "cancelled" => Colors.Red.Darken2,
                "pending" => Colors.Orange.Darken2,
                "rejected" => Colors.Red.Darken2,
                _ => Colors.Grey.Darken2
            };

            container
                .Background(background)
                .PaddingHorizontal(12)
                .PaddingVertical(6)
                .AlignRight()
                .AlignMiddle()
                .Text(status.ToUpper())
                .FontSize(8)
                .Bold()
                .FontColor(textColor);
        }
        private Action<IContainer> ComposeHeader(string title)
        {
            return container => container
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Medium)
                .PaddingBottom(5)
                .Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("Pakistan Red Crescent Society")
                            .FontSize(14).Bold().FontColor(Colors.Red.Medium);
                        col.Item().Text(title)
                            .FontSize(11).FontColor(Colors.Grey.Darken2);
                    });
                    row.ConstantItem(150).AlignRight().Column(col =>
                    {
                        col.Item().Text($"Generated: {DateTime.Now:dd MMM yyyy}")
                            .FontSize(8).FontColor(Colors.Grey.Medium);
                        col.Item().Text($"Time: {DateTime.Now:hh:mm tt}")
                            .FontSize(8).FontColor(Colors.Grey.Medium);
                    });
                });
        }
        private Action<IContainer> ComposeFooter()
        {
            return container => container
                .BorderTop(1)
                .BorderColor(Colors.Grey.Lighten1)
                .PaddingTop(5)
                .Row(row =>
                {
                    row.RelativeItem().Text("PRCS Fleet Management System")
                        .FontSize(8).FontColor(Colors.Grey.Medium);
                    row.ConstantItem(100).AlignRight()
                        .Text(x =>
                        {
                            x.Span("Page ").FontSize(8).FontColor(Colors.Grey.Medium);
                            x.CurrentPageNumber().FontSize(8).FontColor(Colors.Grey.Medium);
                            x.Span(" of ").FontSize(8).FontColor(Colors.Grey.Medium);
                            x.TotalPages().FontSize(8).FontColor(Colors.Grey.Medium);
                        });
                });
        }
        private void ComposeFooter(IContainer container)
        {
            container
                .BorderTop(1)
                .BorderColor(Colors.Grey.Lighten2)
                .PaddingTop(7)
                .Row(row =>
                {
                    row.RelativeItem()
                        .Text("PRCS Fleet Management System")
                        .FontSize(8)
                        .FontColor(Colors.Grey.Medium);

                    row.ConstantItem(120)
                        .AlignRight()
                        .Text(text =>
                        {
                            text.Span("Page ")
                                .FontSize(8)
                                .FontColor(Colors.Grey.Medium);

                            text.CurrentPageNumber()
                                .FontSize(8)
                                .FontColor(Colors.Grey.Medium);

                            text.Span(" of ")
                                .FontSize(8)
                                .FontColor(Colors.Grey.Medium);

                            text.TotalPages()
                                .FontSize(8)
                                .FontColor(Colors.Grey.Medium);
                        });
                });
        }
    }
}
