using System.Text.Json;
using AppleStore.Data;
using Microsoft.AspNetCore.Mvc;

namespace AppleStore.Controllers;

public class ManagerController(ApplicationDbContext context) : Controller
{
    public IActionResult Manager()
    {
        if (HttpContext.Session.GetInt32(HomeController.RoleSessionName) != 2) 
            return RedirectToAction("SignIn", "Home");
            
        var totalSales = context.Orders.Count();
            
        var productSales = context.OrderProducts
            .GroupBy(op => op.Product.IDProduct)
            .Select(g => new
            {
                ProductId = g.Key,
                TotalSales = g.Sum(op => op.TotalCount),
                g.First().Product.ProductName
            }).ToList();

        var salesData = new 
        {
            TotalSales = totalSales,
            ProductSales = productSales
        };

        return View(salesData);
    }
    
    [HttpGet]
    public IActionResult ExportToJson()
    {
        var totalSales = context.Orders.Count();

        var productSales = context.OrderProducts
            .GroupBy(op => op.Product.IDProduct)
            .Select(g => new
            {
                ProductId = g.Key,
                TotalSales = g.Sum(op => op.TotalCount),
                g.First().Product.ProductName
            }).ToList();

        var salesData = new
        {
            TotalSales = totalSales,
            ProductSales = productSales
        };

        var json = JsonSerializer.Serialize(salesData, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        var fileName = $"SalesReport_{DateTime.Now:yyyy-MM-dd-HH-mm}.json";
        var fileBytes = System.Text.Encoding.UTF8.GetBytes(json);

        return File(fileBytes, "application/json", fileName);
    }
    
    [HttpGet]
    public IActionResult ExportToExcel()
    {
        var row = 6;
        var totalSales = context.Orders.Count();

        var productSales = context.OrderProducts
            .GroupBy(op => op.Product.IDProduct)
            .Select(g => new
            {
                ProductId = g.Key,
                TotalSales = g.Sum(op => op.TotalCount),
                g.First().Product.ProductName
            }).ToList();

        var salesData = new
        {
            TotalSales = totalSales,
            ProductSales = productSales
        };

        using var package = new OfficeOpenXml.ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Sales Report");
        
        worksheet.Cells[1, 1].Value = "Sales Report";
        worksheet.Cells[1, 1, 1, 3].Merge = true;
        worksheet.Cells[1, 1].Style.Font.Bold = true;
        worksheet.Cells[1, 1].Style.Font.Size = 16;
        worksheet.Cells[3, 1].Value = "Total Sales:";
        worksheet.Cells[3, 2].Value = salesData.TotalSales;
        worksheet.Cells[5, 1].Value = "Product ID";
        worksheet.Cells[5, 2].Value = "Product Name";
        worksheet.Cells[5, 3].Value = "Total Sales";
        worksheet.Cells[5, 1, 5, 3].Style.Font.Bold = true; 
        
        foreach (var product in salesData.ProductSales)
        {
            worksheet.Cells[row, 1].Value = product.ProductId;
            worksheet.Cells[row, 2].Value = product.ProductName;
            worksheet.Cells[row, 3].Value = product.TotalSales;
            row++;
        }

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return File(stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"SalesReport_{DateTime.Now:yyyy-MM-dd-HH-mm}.xlsx");
    }
}