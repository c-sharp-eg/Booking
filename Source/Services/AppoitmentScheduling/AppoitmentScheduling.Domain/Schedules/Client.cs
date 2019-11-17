﻿using CommonDomain.Model;
using System;

namespace AppoitmentScheduling.Domain.Schedules
{
    internal class Client : Entity<Guid>, IUser
    {
        public string Name { get; protected set; }
    }
}
