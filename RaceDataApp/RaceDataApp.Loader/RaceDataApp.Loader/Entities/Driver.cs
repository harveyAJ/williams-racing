using ServiceStack.DataAnnotations;

namespace RaceDataApp.Loader.Entities;

public class Driver
{
    [PrimaryKey]
    public int DriverId { get; set; }

    [Required]
    [Index(Unique = true)]
    public string DriverRef { get; set; }

    public int? Number { get; set; }

    //[StringLength(3)] // FIA codes are 3 letters
    public string Code { get; set; }

    [Required]
    public string Forename { get; set; }

    [Required]
    public string Surname { get; set; }

    public DateTime Dob { get; set; }

    public string Nationality { get; set; }

    [CustomField("TEXT")]
    public string Url { get; set; }
}