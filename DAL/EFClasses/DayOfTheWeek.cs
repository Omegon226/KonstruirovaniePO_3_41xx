namespace DAL.EFClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DayOfTheWeek")]
    public partial class DayOfTheWeek
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DayOfTheWeek()
        {
            Cruise = new HashSet<Cruise>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string DayOfTheWeekName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cruise> Cruise { get; set; }
    }
}
