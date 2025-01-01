﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FunnyWebRazor.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }

}
