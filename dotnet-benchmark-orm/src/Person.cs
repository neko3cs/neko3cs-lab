using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetOrmBench;

[Table("Person", Schema = "Person")]
public class Person
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int BusinessEntityID { get; set; }

    [Required]
    [Column(TypeName = "nchar")]
    [MaxLength(2)]
    public string PersonType { get; set; } = null!;

    public bool NameStyle { get; set; }

    [MaxLength(8)]
    public string? Title { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [MaxLength(50)]
    public string? MiddleName { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = null!;

    [MaxLength(10)]
    public string? Suffix { get; set; }

    public int EmailPromotion { get; set; }

    [Column(TypeName = "xml")]
    public string? AdditionalContactInfo { get; set; }

    [Column(TypeName = "xml")]
    public string? Demographics { get; set; }

    public Guid rowguid { get; set; }

    public DateTime ModifiedDate { get; set; }
}