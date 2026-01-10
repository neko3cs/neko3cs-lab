using System.ComponentModel.DataAnnotations;
// ReSharper disable UnusedMember.Global

namespace DotNetOrmBench;

public class DimCustomer
{
    [Key]
    public int CustomerKey { get; set; }
    public int? GeographyKey { get; set; }
    [MaxLength(15)]
    public required string CustomerAlternateKey { get; set; }
    [MaxLength(8)]
    public string? Title { get; set; }
    [MaxLength(50)]
    public string? FirstName { get; set; }
    [MaxLength(50)]
    public string? MiddleName { get; set; }
    [MaxLength(50)]
    public string? LastName { get; set; }
    public bool? NameStyle { get; set; }
    public DateTime? BirthDate { get; set; }
    [MaxLength(1)]
    public string? MaritalStatus { get; set; }
    [MaxLength(10)]
    public string? Suffix { get; set; }
    [MaxLength(1)]
    public string? Gender { get; set; }
    [MaxLength(50)]
    public string? EmailAddress { get; set; }
    public decimal? YearlyIncome { get; set; }
    public byte? TotalChildren { get; set; }
    public byte? NumberChildrenAtHome { get; set; }
    [MaxLength(40)]
    public string? EnglishEducation { get; set; }
    [MaxLength(40)]
    public string? SpanishEducation { get; set; }
    [MaxLength(40)]
    public string? FrenchEducation { get; set; }
    [MaxLength(100)]
    public string? EnglishOccupation { get; set; }
    [MaxLength(100)]
    public string? SpanishOccupation { get; set; }
    [MaxLength(100)]
    public string? FrenchOccupation { get; set; }
    [MaxLength(1)]
    public string? HouseOwnerFlag { get; set; }
    public byte? NumberCarsOwned { get; set; }
    [MaxLength(120)]
    public string? AddressLine1 { get; set; }
    [MaxLength(120)]
    public string? AddressLine2 { get; set; }
    [MaxLength(20)]
    public string? Phone { get; set; }
    public DateTime? DateFirstPurchase { get; set; }
    [MaxLength(15)]
    public string? CommuteDistance { get; set; }
}