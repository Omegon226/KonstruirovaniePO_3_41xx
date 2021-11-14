namespace DAL.EFClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ticket")]
    public partial class Ticket
    {
        public int ID { get; set; }

        public DateTime? DateOfIssue { get; set; }

        [StringLength(50)]
        public string IdentificationInformation { get; set; }

        public int? SeatNumberOnTheTransport { get; set; }

        [StringLength(50)]
        public string FullName { get; set; }

        public int CruiseID { get; set; }

        public int UserID { get; set; }

        public DateTime? RaceDepartureTime { get; set; }

        public virtual Cruise Cruise { get; set; }

        public virtual User User { get; set; }
    }
}
