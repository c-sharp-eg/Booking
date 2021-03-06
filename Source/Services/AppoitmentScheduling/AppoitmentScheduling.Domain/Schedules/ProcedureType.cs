﻿using CommonDomain.Enums;
using CommonDomain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppoitmentScheduling.Domain.Schedules
{
    /// <summary>
    /// Type of procedure (e.g. man haircut, pedicure, ...) created by app admin
    /// </summary>
    internal class ProcedureType : Entity<int>
    {
        public string Name { get; private set; }

        public ProcedureCategory ProcedureCategory { get; private set; }
        
        public Gender TargetGender { get; private set; }
    }
}
