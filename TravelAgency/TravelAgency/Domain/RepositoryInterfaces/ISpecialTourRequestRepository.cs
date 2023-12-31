﻿using System.Collections.Generic;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface ISpecialTourRequestRepository
    {
        public List<SpecialTourRequest> GetAll();
        public List<SpecialTourRequest> GetByGuestId(int guestId);
        public SpecialTourRequest Save(SpecialTourRequest request);
        public List<SpecialTourRequest> GetPendings();
        public void Update(SpecialTourRequest request);
        public void UndoIfAccepted(int specialRequest);
    }
}
