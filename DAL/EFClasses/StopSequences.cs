namespace DAL.EFClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StopSequences
    {
        public int ID { get; set; }

        public int? IndexNumber { get; set; }

        public int StoppingID { get; set; }

        public int StopRouteID { get; set; }

        public float? TripPrice { get; set; }

        public TimeSpan? TravelTimeToStop { get; set; }

        public virtual Route Route { get; set; }

        public virtual StoppingOnTheRoute StoppingOnTheRoute { get; set; }
    }
}
