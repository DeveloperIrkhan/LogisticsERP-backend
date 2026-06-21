using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Vehicle.LogisticsERP.API.DTOs.Reports;
using LogisticsERP.API.enums;
using LogisticsERP.API.Helpers;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LogisticsERP.API.Services
{
    public class ReportService : ServiceBaseFunctions, IReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext appDbContext)
        {
            _context = appDbContext;
            QuestPDF.Settings.License = LicenseType.Community;
        }
        public async Task<ApiResponse<List<VehicleReportDto>>> GetVehicleReportAsync()
        {
            try
            {
                var vehicles = await _context.Vehicles.ToListAsync();
                var fuelRecords = await _context.FuelRecords.ToListAsync();
                var maintenance = await _context.MaintenanceRecords.ToListAsync();
                var expenses = await _context.Expenses.ToListAsync();
                var report = vehicles.Select(v => new VehicleReportDto
                {  
                    VehicleId = v.VehicleId,
                    VehicleNumber = v.Number,
                    ModelName = v.ModelName,
                    Type = v.VehicleType.ToString(),
                    Status = v.Status.ToString(),
                    RegistrationExpiry = v.RegistrationExpiry,
                    InsuranceExpiry = v.InsuranceExpiry,
                    FitnessExpiry = v.FitnessExpiry,
                    TotalFuelCost = fuelRecords.Where(f => f.VehicleId == v.VehicleId).Sum(f => f.TotalCost),
                    TotalMaintenanceCost = maintenance.Where(m => m.VehicleId == v.VehicleId).Sum(m => m.Cost),
                    TotalExpenseCost = expenses.Where(e => e.VehicleId == v.VehicleId).Sum(e => e.Amount),
                    TotalCost = fuelRecords.Where(f => f.VehicleId == v.VehicleId).Sum(f => f.TotalCost)
                                          + maintenance.Where(m => m.VehicleId == v.VehicleId).Sum(m => m.Cost)
                                          + expenses.Where(e => e.VehicleId == v.VehicleId).Sum(e => e.Amount)
                }).ToList();

                return Ok(report, $"{report.Count} vehicle(s) in report.");
            }
            catch (Exception ex)
            {
                return Fail<List<VehicleReportDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        // ── FUEL REPORT ───────────────────────────────────────
        public async Task<ApiResponse<List<FuelReportDto>>> GetFuelReportAsync(ReportFilterDto filter)
        {
            try
            {
                var query = _context.FuelRecords
                    .Include(x => x.Vehicle)
                    .Include(x => x.Driver)
                    .AsQueryable();

                if (filter.VehicleId != null) query = query.Where(x => x.VehicleId == filter.VehicleId);
                if (filter.DriverId != null) query = query.Where(x => x.DriverId == filter.DriverId);
                if (filter.From.HasValue) query = query.Where(x => x.FuelingDate >= filter.From.Value);
                if (filter.To.HasValue) query = query.Where(x => x.FuelingDate <= filter.To.Value);
                if (filter.Year.HasValue) query = query.Where(x => x.FuelingDate.Year == filter.Year.Value);
                if (filter.Month.HasValue) query = query.Where(x => x.FuelingDate.Month == filter.Month.Value);

                var records = await query.OrderByDescending(x => x.FuelingDate).ToListAsync();

                var report = records.Select(x => new FuelReportDto
                {
                    FuelId = x.FuelId,
                    VehicleNumber = x.Vehicle?.Number ?? string.Empty,
                    DriverName = x.Driver?.FullName ?? string.Empty,
                    FuelingDate = x.FuelingDate,
                    Liters = x.Liters,
                    CostPerLiter = x.CostPerLiter,
                    TotalCost = x.TotalCost,
                    StationName = x.StationName,
                    FuelType = x.FuelType,
                    OdoMeterReading = x.OdoMeterReading
                }).ToList();

                return Ok(report, $"{report.Count} fuel record(s) in report.");
            }
            catch (Exception ex)
            {
                return Fail<List<FuelReportDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        // ── MAINTENANCE REPORT ────────────────────────────────
        public async Task<ApiResponse<List<MaintenanceReportDto>>> GetMaintenanceReportAsync(ReportFilterDto filter)
        {
            try
            {
                var query = _context.MaintenanceRecords
                    .Include(x => x.Vehicle)
                    .AsQueryable();

                if (filter.VehicleId != null) query = query.Where(x => x.VehicleId == filter.VehicleId);
                if (filter.From.HasValue) query = query.Where(x => x.MaintenanceDate >= filter.From.Value);
                if (filter.To.HasValue) query = query.Where(x => x.MaintenanceDate <= filter.To.Value);
                if (filter.Year.HasValue) query = query.Where(x => x.MaintenanceDate.Year == filter.Year.Value);
                if (filter.Month.HasValue) query = query.Where(x => x.MaintenanceDate.Month == filter.Month.Value);

                var records = await query.OrderByDescending(x => x.MaintenanceDate).ToListAsync();

                var report = records.Select(x => new MaintenanceReportDto
                {
                    MaintenanceRecordId = x.MaintenanceRecordId,
                    VehicleNumber = x.Vehicle?.Number ?? string.Empty,
                    MaintenanceDate = x.MaintenanceDate,
                    MaintenanceType = x.MaintenanceType,
                    WorkshopName = x.WorkshopName,
                    Cost = x.Cost,
                    CurrentKm = x.CurrentKm,
                    ChangedParts = x.ChangedParts,
                    InvoiceNumber = x.InvoiceNumber,
                    NextMaintenanceDate = x.NextMaintenanceDate
                }).ToList();

                return Ok(report, $"{report.Count} maintenance record(s) in report.");
            }
            catch (Exception ex)
            {
                return Fail<List<MaintenanceReportDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        // ── EXPENSE REPORT ────────────────────────────────────
        public async Task<ApiResponse<List<ExpenseReportDto>>> GetExpenseReportAsync(ReportFilterDto filter)
        {
            try
            {
                var query = _context.Expenses
                    .Include(x => x.Vehicle)
                    .AsQueryable();

                if (filter.VehicleId != null) query = query.Where(x => x.VehicleId == filter.VehicleId);
                if (filter.From.HasValue) query = query.Where(x => x.ExpenseDate >= filter.From.Value);
                if (filter.To.HasValue) query = query.Where(x => x.ExpenseDate <= filter.To.Value);
                if (filter.Year.HasValue) query = query.Where(x => x.ExpenseDate.Year == filter.Year.Value);
                if (filter.Month.HasValue) query = query.Where(x => x.ExpenseDate.Month == filter.Month.Value);

                var records = await query.OrderByDescending(x => x.ExpenseDate).ToListAsync();

                var report = records.Select(x => new ExpenseReportDto
                {
                    ExpenseId = x.ExpenseId,
                    ExpenseName = x.ExpenseName,
                    Amount = x.Amount,
                    ExpenseDate = x.ExpenseDate,
                    Category = x.ExpenseCategory.ToString(),
                    PaymentMode = x.PaymentMode.ToString(),
                    Status = x.ExpenseStatus.ToString(),
                    VehicleNumber = x.Vehicle?.Number,
                    ReceiptNumber = x.ReceiptNumber,
                    ApprovedBy = x.ApprovedBy
                }).ToList();

                return Ok(report, $"{report.Count} expense record(s) in report.");
            }
            catch (Exception ex)
            {
                return Fail<List<ExpenseReportDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        // ── PDF: VEHICLE ──────────────────────────────────────
        public async Task<byte[]> ExportVehicleReportPdfAsync()
        {
            var reportData = (await GetVehicleReportAsync()).Data ?? new();

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header().Element(ComposeHeader("Vehicle Report"));

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                            c.RelativeColumn(1);
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                        });

                        // Header row
                        TableHeader(table, "Vehicle No", "Model", "Type", "Status", "Fuel Cost", "Maintenance Cost", "Total Cost");

                        // Data rows
                        foreach (var item in reportData)
                        {
                            TableRow(table,
                                item.VehicleNumber,
                                item.ModelName,
                                item.Type,
                                item.Status,
                                $"PKR {item.TotalFuelCost:N0}",
                                $"PKR {item.TotalMaintenanceCost:N0}",
                                $"PKR {item.TotalCost:N0}");
                        }
                    });

                    page.Footer().Element(ComposeFooter());
                });
            }).GeneratePdf();
        }

        // ── PDF: FUEL ─────────────────────────────────────────
        public async Task<byte[]> ExportFuelReportPdfAsync(ReportFilterDto filter)
        {
            var reportData = (await GetFuelReportAsync(filter)).Data ?? new();

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header().Element(ComposeHeader("Fuel Report"));

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                            c.RelativeColumn(1);
                            c.RelativeColumn(1);
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                        });

                        TableHeader(table, "Vehicle No", "Driver", "Date", "Liters", "Cost/Ltr", "Station", "Total Cost");

                        foreach (var item in reportData)
                        {
                            TableRow(table,
                                item.VehicleNumber,
                                item.DriverName,
                                item.FuelingDate.ToString("dd MMM yyyy"),
                                item.Liters.ToString("N1"),
                                $"PKR {item.CostPerLiter:N0}",
                                item.StationName,
                                $"PKR {item.TotalCost:N0}");
                        }
                    });

                    page.Footer().Element(ComposeFooter());
                });
            }).GeneratePdf();
        }

        // ── PDF: MAINTENANCE ──────────────────────────────────
        public async Task<byte[]> ExportMaintenanceReportPdfAsync(ReportFilterDto filter)
        {
            var reportData = (await GetMaintenanceReportAsync(filter)).Data ?? new();

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header().Element(ComposeHeader("Maintenance Report"));

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                        });

                        TableHeader(table, "Vehicle No", "Date", "Type", "Workshop", "Changed Parts", "Cost");

                        foreach (var item in reportData)
                        {
                            TableRow(table,
                                item.VehicleNumber,
                                item.MaintenanceDate.ToString("dd MMM yyyy"),
                                item.MaintenanceType ?? "-",
                                item.WorkshopName ?? "-",
                                item.ChangedParts ?? "-",
                                $"PKR {item.Cost:N0}");
                        }
                    });

                    page.Footer().Element(ComposeFooter());
                });
            }).GeneratePdf();
        }

        // ── PDF: EXPENSE ──────────────────────────────────────
        public async Task<byte[]> ExportExpenseReportPdfAsync(ReportFilterDto filter)
        {
            var reportData = (await GetExpenseReportAsync(filter)).Data ?? new();

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header().Element(ComposeHeader("Expense Report"));

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                        });

                        TableHeader(table, "Expense Name", "Date", "Category", "Payment Mode", "Status", "Amount");

                        foreach (var item in reportData)
                        {
                            TableRow(table,
                                item.ExpenseName,
                                item.ExpenseDate.ToString("dd MMM yyyy"),
                                item.Category,
                                item.PaymentMode,
                                item.Status,
                                $"PKR {item.Amount:N0}");
                        }
                    });

                    page.Footer().Element(ComposeFooter());
                });
            }).GeneratePdf();
        }
        // ── PDF: MONTHLY ROSTER ───────────────────────────────
        public async Task<byte[]> ExportMonthlyRosterPdfAsync(int month, int year)
        {
            var rosters = await _context.DutyRosters
                .Where(x => x.Month == month && x.Year == year)
                .FirstOrDefaultAsync();

            if (rosters == null) return Array.Empty<byte>();

            var entries = await _context.DutyRosterEntries
                .Include(x => x.Driver)
                .Include(x => x.Vehicle)
                .Where(x => x.RosterId == rosters.RosterId)
                .OrderBy(x => x.DutyDate)
                .ThenBy(x => x.ShiftStart)
                .ToListAsync();

            var monthName = new DateTime(year, month, 1).ToString("MMMM yyyy");

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header().Element(ComposeHeader($"Monthly Duty Roster — {monthName}"));

                    page.Content().Column(col =>
                    {
                        // Summary box
                        col.Item().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Roster Title").FontSize(8).FontColor(Colors.Grey.Medium);
                                c.Item().Text(rosters.Title).Bold();
                            });
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Status").FontSize(8).FontColor(Colors.Grey.Medium);
                                c.Item().Text(rosters.Status.ToString()).Bold();
                            });
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Total Duties").FontSize(8).FontColor(Colors.Grey.Medium);
                                c.Item().Text(entries.Count.ToString()).Bold();
                            });
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Approved By").FontSize(8).FontColor(Colors.Grey.Medium);
                                c.Item().Text(rosters.ApprovedBy ?? "-").Bold();
                            });
                        });

                        col.Item().PaddingTop(10);

                        // Group by date
                        var grouped = entries.GroupBy(x => x.DutyDate.Date).OrderBy(g => g.Key);

                        foreach (var day in grouped)
                        {
                            // Day header
                            col.Item().Background(Colors.Red.Lighten4)
                                .Padding(5)
                                .Text(day.Key.ToString("dddd, dd MMMM yyyy"))
                                .Bold().FontSize(10);

                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(c =>
                                {
                                    c.RelativeColumn(2);
                                    c.RelativeColumn(2);
                                    c.RelativeColumn(1);
                                    c.RelativeColumn(1);
                                    c.RelativeColumn(1);
                                    c.RelativeColumn(2);
                                    c.RelativeColumn(2);
                                    c.RelativeColumn(1);
                                });

                                TableHeader(table, "Driver", "Vehicle", "Shift", "Start", "End", "From", "To", "Status");

                                foreach (var entry in day.OrderBy(x => x.ShiftStart))
                                {
                                    var bg = entry.Status switch
                                    {
                                        RosterEntryStatus.Completed => Colors.Green.Lighten4,
                                        RosterEntryStatus.Missed => Colors.Red.Lighten4,
                                        RosterEntryStatus.InProgress => Colors.Yellow.Lighten4,
                                        RosterEntryStatus.Cancelled => Colors.Grey.Lighten3,
                                        _ => Colors.White
                                    };

                                    TableColoredRow(table, bg,
                                        entry.Driver?.FullName ?? "-",
                                        entry.Vehicle?.Number ?? "-",
                                        entry.ShiftType.ToString(),
                                        entry.ShiftStart.ToString(@"hh\:mm"),
                                        entry.ShiftEnd.ToString(@"hh\:mm"),
                                        entry.FromLocation ?? "-",
                                        entry.ToLocation ?? "-",
                                        entry.Status.ToString());
                                }
                            });

                            col.Item().PaddingTop(8);
                        }
                    });

                    page.Footer().Element(ComposeFooter());
                });
            }).GeneratePdf();
        }

        // ── PDF: DAILY ROSTER ─────────────────────────────────
        public async Task<byte[]> ExportDailyRosterPdfAsync(DateTime date)
        {
            var entries = await _context.DutyRosterEntries
                .Include(x => x.Driver)
                .Include(x => x.Vehicle)
                .Where(x => x.DutyDate.Date == date.Date)
                .OrderBy(x => x.ShiftStart)
                .ToListAsync();

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header().Element(ComposeHeader($"Daily Duty Roster — {date:dddd, dd MMMM yyyy}"));

                    page.Content().Column(col =>
                    {
                        // Summary row
                        col.Item().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Total Assigned").FontSize(8).FontColor(Colors.Grey.Medium);
                                c.Item().Text(entries.Count.ToString()).Bold();
                            });
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Completed").FontSize(8).FontColor(Colors.Grey.Medium);
                                c.Item().Text(entries.Count(x => x.Status == RosterEntryStatus.Completed).ToString())
                                    .Bold().FontColor(Colors.Green.Medium);
                            });
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("In Progress").FontSize(8).FontColor(Colors.Grey.Medium);
                                c.Item().Text(entries.Count(x => x.Status == RosterEntryStatus.InProgress).ToString())
                                    .Bold().FontColor(Colors.Yellow.Darken2);
                            });
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Missed").FontSize(8).FontColor(Colors.Grey.Medium);
                                c.Item().Text(entries.Count(x => x.Status == RosterEntryStatus.Missed).ToString())
                                    .Bold().FontColor(Colors.Red.Medium);
                            });
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Scheduled").FontSize(8).FontColor(Colors.Grey.Medium);
                                c.Item().Text(entries.Count(x => x.Status == RosterEntryStatus.Scheduled).ToString())
                                    .Bold().FontColor(Colors.Blue.Medium);
                            });
                        });

                        col.Item().PaddingTop(10);

                        // Main table
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.ConstantColumn(30);
                                c.RelativeColumn(2);
                                c.RelativeColumn(2);
                                c.RelativeColumn(1);
                                c.RelativeColumn(1);
                                c.RelativeColumn(1);
                                c.RelativeColumn(1);
                                c.RelativeColumn(2);
                                c.RelativeColumn(2);
                                c.RelativeColumn(2);
                                c.RelativeColumn(1);
                            });

                            TableHeader(table, "#", "Driver", "Vehicle", "Shift", "Start", "End", "Duty Type", "From", "To", "Officer", "Status");

                            var i = 1;
                            foreach (var entry in entries)
                            {
                                var bg = entry.Status switch
                                {
                                    RosterEntryStatus.Completed => Colors.Green.Lighten4,
                                    RosterEntryStatus.Missed => Colors.Red.Lighten4,
                                    RosterEntryStatus.InProgress => Colors.Yellow.Lighten4,
                                    RosterEntryStatus.Cancelled => Colors.Grey.Lighten3,
                                    _ => Colors.White
                                };

                                TableColoredRow(table, bg,
                                    i++.ToString(),
                                    entry.Driver?.FullName ?? "-",
                                    entry.Vehicle?.Number ?? "-",
                                    entry.ShiftType.ToString(),
                                    entry.ShiftStart.ToString(@"hh\:mm"),
                                    entry.ShiftEnd.ToString(@"hh\:mm"),
                                    entry.DutyType.ToString(),
                                    entry.FromLocation ?? "-",
                                    entry.ToLocation ?? "-",
                                    entry.OfficerName ?? "-",
                                    entry.Status.ToString());
                            }
                        });

                        // Signature section
                        col.Item().PaddingTop(30).Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().BorderBottom(1).BorderColor(Colors.Grey.Medium).PaddingBottom(20);
                                c.Item().PaddingTop(5).Text("Prepared By").FontSize(8).FontColor(Colors.Grey.Medium);
                            });
                            row.ConstantItem(50);
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().BorderBottom(1).BorderColor(Colors.Grey.Medium).PaddingBottom(20);
                                c.Item().PaddingTop(5).Text("Approved By").FontSize(8).FontColor(Colors.Grey.Medium);
                            });
                            row.ConstantItem(50);
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().BorderBottom(1).BorderColor(Colors.Grey.Medium).PaddingBottom(20);
                                c.Item().PaddingTop(5).Text("Fleet Manager").FontSize(8).FontColor(Colors.Grey.Medium);
                            });
                        });
                    });

                    page.Footer().Element(ComposeFooter());
                });
            }).GeneratePdf();
        }
        // ── PDF HELPERS ───────────────────────────────────────
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

        private void TableHeader(TableDescriptor table, params string[] headers)
        {
            foreach (var header in headers)
            {
                table.Header(h =>
                {
                    h.Cell().Background(Colors.Red.Medium).Padding(5)
                        .Text(header).FontColor(Colors.White).Bold().FontSize(9);
                });
            }
        }

        private void TableRow(TableDescriptor table, params string[] values)
        {
            var isEven = false;
            foreach (var value in values)
            {
                table.Cell()
                    .Background(isEven ? Colors.Grey.Lighten3 : Colors.White)
                    .Padding(4)
                    .Text(value).FontSize(8);
            }
        }

        private void TableColoredRow(TableDescriptor table, string backgroundColor, params string[] values)
        {
            foreach (var value in values)
            {
                table.Cell()
                    .Background(backgroundColor)
                    .Padding(4)
                    .Text(value).FontSize(8);
            }
        }
    }
}
