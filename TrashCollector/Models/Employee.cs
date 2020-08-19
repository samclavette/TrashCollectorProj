﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrashCollector.Models
{
    public class Employee
    {
        [Key]
        public int Id{ get; set; }

        public string Name { get; set; }

        public string ZipCode { get; set; }

        public int MyProperty { get; set; }
    }
}
