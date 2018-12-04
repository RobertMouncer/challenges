using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace challenges.Models
{
    public class UserGroup
    {
        [Required]
        public virtual int UserGroupId { get; set; }

        public virtual string UserId { get; set; }

        public virtual int GroupId { get; set; }
        public virtual bool isGroup{ get; set; }
    }
}
