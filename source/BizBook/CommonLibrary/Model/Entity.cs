using System;
using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.Model
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    public abstract class Entity
    {
        [Key]
        public string Id { get; set; }

        [Index]
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime Created { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public string CreatedFrom { get; set; }

        [Index]
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime Modified { get; set; }

        [Required]
        public string ModifiedBy { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}