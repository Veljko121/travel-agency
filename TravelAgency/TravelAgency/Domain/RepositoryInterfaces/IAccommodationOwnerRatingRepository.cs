﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface IAccommodationOwnerRatingRepository
    {
        public List<AccommodationOwnerRating> GetAll();

        public AccommodationOwnerRating GetById(int id);

        public List<AccommodationOwnerRating> GetByOwner(User owner);

        public List<AccommodationOwnerRating> GetRatingsVisibleToOwner(User owner, IEnumerable<AccommodationGuestRating> guestRatings);

        public int NextId();

        public AccommodationOwnerRating Save(AccommodationOwnerRating rating);

        public void LinkReservations(List<AccommodationReservation> reservations);
    }
}