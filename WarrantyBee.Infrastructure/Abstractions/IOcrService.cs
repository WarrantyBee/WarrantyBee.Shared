namespace WarrantyBee.Shared.Infrastructure.Abstractions;

/// <summary>
/// Defines a service for extracting structured data from documents and images using Optical Character Recognition (OCR).
/// </summary>
public interface IOcrService
{
    /// <summary>
    /// Parses a receipt image and extracts key information such as store name, date, and product details.
    /// </summary>
    /// <param name="imageStream">The stream of the receipt image.</param>
    /// <returns>A <see cref="ParsedReceiptData"/> object containing the extracted fields.</returns>
    Task<ParsedReceiptData> ParseReceiptAsync(Stream imageStream);
}

/// <summary>
/// Represents the structured data extracted from a receipt.
/// </summary>
public class ParsedReceiptData
{
    /// <summary>
    /// Gets or sets the name of the merchant or store.
    /// </summary>
    public string? MerchantName { get; set; }

    /// <summary>
    /// Gets or sets the date of purchase extracted from the receipt.
    /// </summary>
    public DateTime? PurchaseDate { get; set; }

    /// <summary>
    /// Gets or sets the product SKU or model number if identified.
    /// </summary>
    public string? Sku { get; set; }

    /// <summary>
    /// Gets or sets the confidence score of the OCR operation (0.0 to 1.0).
    /// </summary>
    public double Confidence { get; set; }
}
