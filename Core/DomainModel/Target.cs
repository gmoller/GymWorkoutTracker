﻿namespace DomainModel
{
    public class Target : IDomainIdentifiable<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public MuscleGroup MuscleGroup { get; set; }
    }
}