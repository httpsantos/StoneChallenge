﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Stone.Domain.Entities
{
    public class Inss : EntityBase<int>
    {
        public decimal Minimo { get; set; }
        public decimal Maximo { get; set; }
        public decimal Alicota { get; set; }
    }
}
