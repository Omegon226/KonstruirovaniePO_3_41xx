namespace DAL.EFClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cruise")]
    public partial class Cruise
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cruise()
        {
            Ticket = new HashSet<Ticket>();
        }

        public int ID { get; set; }

        public int DayOfTheWeekCruiseID { get; set; }

        public int RouteIDOfTheCruise { get; set; }

        public int DriverIDOfTheCruise { get; set; }

        public int TransportIDOfTheCruise { get; set; }

        public TimeSpan? StartTime { get; set; }

        public virtual DayOfTheWeek DayOfTheWeek { get; set; }

        public virtual Driver Driver { get; set; }

        public virtual Route Route { get; set; }

        public virtual Transport Transport { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket> Ticket { get; set; }
    }
}
