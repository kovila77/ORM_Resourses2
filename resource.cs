namespace ORM_Resourses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.resources")]
    public partial class resource
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public resource()
        {
            buildings_resources_consume = new HashSet<buildings_resources_consume>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [System.ComponentModel.DisplayName("Ресурс")]
        public int resources_id { get; set; }

        [Required]
        [System.ComponentModel.DisplayName("Ресурс")]
        public string resources_name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [System.ComponentModel.Browsable(false)]
        public virtual ICollection<buildings_resources_consume> buildings_resources_consume { get; set; }
    }
}
