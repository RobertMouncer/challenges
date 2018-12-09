using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace challenges.Models
{
    public class Activity
    {
        public virtual int ActivityId { get; set; }
        public virtual string ActivityName { get; set; }
    }
}
