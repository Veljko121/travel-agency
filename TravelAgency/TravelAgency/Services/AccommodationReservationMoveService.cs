﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Injector;
using TravelAgency.Model;
using TravelAgency.Repository;
using TravelAgency.RepositoryInterfaces;

namespace TravelAgency.Services
{
    public class AccommodationReservationMoveService
    {
        public IAccommodationReservationMoveRequestRepository MoveRequestRepository { get; set; }
        public IAccommodationReservationRepository ReservationRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IAccommodationRepository AccommodationRepository { get; set; }

        public AccommodationReservationMoveService()
        {
            MoveRequestRepository = Injector.Injector.CreateInstance<IAccommodationReservationMoveRequestRepository>();
            ReservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            UserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            AccommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            AccommodationRepository.LinkOwners(UserRepository.GetOwners());
            ReservationRepository.LinkGuests(UserRepository.GetUsers());
            ReservationRepository.LinkAccommodations(AccommodationRepository.GetAll());
            MoveRequestRepository.LinkReservations(ReservationRepository.GetAll());
        }

        public bool CreateMoveRequest(AccommodationReservationMoveRequest moveRequest)
        {
            if(moveRequest.IsValid)
            {
                MoveRequestRepository.Save(moveRequest);
                return true;
            }
            return false;
        }



    }
}
