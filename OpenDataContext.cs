namespace ORM_Resourses
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class OpenDataContext : DbContext
    {
        public OpenDataContext()
            : base("name=Model1")
        {
        }

        public virtual DbSet<building> buildings { get; set; }
        public virtual DbSet<buildings_resources_consume> buildings_resources_consume { get; set; }
        public virtual DbSet<resource> resources { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<building>()
                .HasMany(e => e.buildings_resources_consume)
                .WithRequired(e => e.building)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<resource>()
                .HasMany(e => e.buildings_resources_consume)
                .WithRequired(e => e.resource)
                .WillCascadeOnDelete(false);
        }
    }
}
