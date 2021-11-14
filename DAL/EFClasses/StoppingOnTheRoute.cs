namespace DAL.EFClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StoppingOnTheRoute")]
    public partial class StoppingOnTheRoute
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StoppingOnTheRoute()
        {
            StopSequences = new HashSet<StopSequences>();
        }

        public int ID { get; set; }

        public int StopLocalityID { get; set; }

        public virtual Locality Locality { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StopSequences> StopSequences { get; set; }
    }
}
