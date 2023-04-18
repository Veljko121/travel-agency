﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;

namespace TravelAgency.RepositoryInterfaces
{
    public interface IAccommodationRepository
    {
        public List<Accommodation> GetAll();
        public Accommodation Save(Accommodation accommodation);
        public Accommodation GetById(int id);
        public int NextId();
        public List<Accommodation> GetByOwner(User owner);
    }
}
