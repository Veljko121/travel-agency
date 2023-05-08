﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;

namespace TravelAgency.Services
{
    public class CreatedTourFromStatisticService
    {
        public INewTourNotificationRepository INewTourNotificationRepository { get; set; }
        public IUserRepository IUserRepository { get; set; }
        public ITourRequestRepository ITourRequestRepository { get; set; }
        public ITourRepository ITourRepository { get; set; }
        public CreatedTourFromStatisticService() 
        {
            INewTourNotificationRepository = Injector.Injector.CreateInstance<INewTourNotificationRepository>();
            IUserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            ITourRequestRepository = Injector.Injector.CreateInstance<ITourRequestRepository>();
            ITourRepository = Injector.Injector.CreateInstance<ITourRepository>();
            LinkTourNotifications();
        }
        private void LinkTourNotifications()
        {
            foreach(NewTourNotification notification in INewTourNotificationRepository.GetAll()) 
            {
                if (notification.Tour == null)
                {
                    notification.Tour = ITourRepository.GetById(notification.TourId);
                }
            }
        }
        public void MakeNotificationsForLanguage(Tour tour)
        {
            foreach(User user in IUserRepository.GetAll())
            {
                if (user.Role == Roles.Guest2)
                {
                    foreach (TourRequest request in GetNotAcceptedRequests(user.Id))
                    {
                        if (request.Language == tour.Language)
                        {
                            NewTourNotification createdTour = new NewTourNotification(tour.Id, user.Id, false, tour, true, false);
                            INewTourNotificationRepository.Save(createdTour);
                            break;
                        }
                    }
                }
            }
        }
        public void MakeNotificationsForLocation(Tour tour)
        {
            foreach (User user in IUserRepository.GetAll())
            {
                if (user.Role == Roles.Guest2)
                {
                    foreach (TourRequest request in GetNotAcceptedRequests(user.Id))
                    {
                        if (request.LocationId == tour.LocationId)
                        {
                            NewTourNotification createdTour = new NewTourNotification(tour.Id, user.Id, false, tour, false, true);
                            INewTourNotificationRepository.Save(createdTour);
                            break;
                        }
                    }
                }
            }
        }

        private List<TourRequest> GetNotAcceptedRequests(int id)
        {
            List <TourRequest> result = new List <TourRequest>();
            foreach (TourRequest request in ITourRequestRepository.GetAll())
            {
                if (request.GuestId == id && request.Status != RequestStatus.Accepted)
                {
                    result.Add(request);
                }
            }
            return result;
        }
        public bool NewTourNotificationExists(int guestId)
        {
            return INewTourNotificationRepository.NewTourNotificationExists(guestId);
        }
        public List<NewTourNotification> GetNewTourNotifications(int guestId)
        {
            return INewTourNotificationRepository.GetNewTourNotifications(guestId);
        }
        public void UpdateNotification(NewTourNotification notification)
        {
            INewTourNotificationRepository.Update(notification);
        }
    }
}
