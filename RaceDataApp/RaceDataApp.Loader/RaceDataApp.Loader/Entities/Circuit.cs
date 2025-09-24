using ServiceStack.DataAnnotations;

namespace RaceDataApp.Loader.Entities;

public class Circuit
    {
        [PrimaryKey]
        public int CircuitId { get; set; }

        [Required]
        [Index(Unique = true)]
        public string CircuitRef { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Country { get; set; }

        [DecimalLength(9, 6)]
        public decimal Lat { get; set; }

        [DecimalLength(9, 6)]
        public decimal Lng { get; set; }

        public int Alt { get; set; }

        [CustomField("TEXT")] // Store as Postgres text, handles long URLs
        public string Url { get; set; }
    }