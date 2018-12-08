using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace challenges.Models
{
    public class Activity
    {
        [Key]
        public virtual int ActivityId { get; set; }
        [Required]
        public virtual string ActivityName { get; set; }
    }
}
