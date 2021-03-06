﻿using DomainModel;

namespace ApplicationServices
{
    public interface IMuscleService : IBaseService<Muscle, long>
    {
        Muscle GetByName(string name);
    }
}