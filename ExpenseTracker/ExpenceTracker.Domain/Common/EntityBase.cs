﻿namespace ExpenseTracker.Domain.Common
{
    public abstract class EntityBase
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
    }
}