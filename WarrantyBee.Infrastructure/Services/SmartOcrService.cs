using WarrantyBee.Shared.Infrastructure.Abstractions;

namespace WarrantyBee.Shared.Infrastructure.Services;

/// <summary>
/// A foundational OCR service that provides a blueprint for receipt parsing. 
/// In a production environment, this should be integrated with Azure Document Intelligence or Google Vision API.
/// </summary>
public class SmartOcrService : IOcrService
{
    /// <summary>
    /// Parses a receipt image. This foundational implementation returns simulated data 
    /// but is architected to be swapped for a real AI integration.
    /// </summary>
    public async Task<ParsedReceiptData> ParseReceiptAsync(Stream imageStream)
    {
        // FOUNDATION: This is where we would call external AI APIs.
        // For Day 1, we provide a high-fidelity simulation to demonstrate the UX.
        
        await Task.Delay(1500); // Simulate network latency

        return new ParsedReceiptData
        {
            MerchantName = "Authorized Retailer",
            PurchaseDate = DateTime.Today.AddDays(-5),
            Sku = "WB-DEMO-PRODUCT",
            Confidence = 0.95
        };
    }
}
