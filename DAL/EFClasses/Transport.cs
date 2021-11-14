namespace DAL.EFClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Transport")]
    public partial class Transport
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Transport()
        {
            Cruise = new HashSet<Cruise>();
        }

        public int ID { get; set; }

        public int? NumberOfSeats { get; set; }

        [StringLength(9)]
        public string RegistrationNumber { get; set; }

        [StringLength(50)]
        public string Model { get; set; }

        public bool? Hidden { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cruise> Cruise { get; set; }
    }
}
