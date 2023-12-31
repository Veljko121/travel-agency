﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Observer;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class TourOccurrenceRepository : ISubject, ITourOccurrenceRepository
    {
        private const string FilePath = "../../../Resources/Data/tourOccurrences.csv";
        private readonly Serializer<TourOccurrence> _serializer;
        private List<TourOccurrence> tourOccurrences;
        private List<IObserver> observers;

        public TourOccurrenceRepository()
        {
            _serializer = new Serializer<TourOccurrence>();
            tourOccurrences = _serializer.FromCSV(FilePath);
            observers = new List<IObserver>();
        }

        public int NextId()
        {
            if (tourOccurrences.Count == 0)
            {
                return 1;
            }
            return tourOccurrences[tourOccurrences.Count - 1].Id + 1;
        }

        public List<TourOccurrence> GetAll()
        {
            return tourOccurrences;
        }
        public List<TourOccurrence> GetTodays(int activeGuideId)
        {
            List<TourOccurrence> result = new List<TourOccurrence>();
            foreach (TourOccurrence tourOccurrence in tourOccurrences)
            {
                if (tourOccurrence.DateTime.Date.Equals(DateTime.Now.Date) && tourOccurrence.GuideId == activeGuideId && tourOccurrence.IsDeleted == false)
                {
                    result.Add(tourOccurrence);
                }
            }
            return result;
        }

        public List<TourOccurrence> GetUpcomings(int activeGuideId)
        {
            List<TourOccurrence> result = new List<TourOccurrence>();
            foreach (TourOccurrence tourOccurrence in tourOccurrences)
            {
                if (tourOccurrence.DateTime.Date > DateTime.Now.Date && tourOccurrence.GuideId == activeGuideId && tourOccurrence.IsDeleted == false)
                {
                    result.Add(tourOccurrence);
                }
            }
            return result;
        }

        public List<TourOccurrence> GetOffered()
        {
            List<TourOccurrence> result = new List<TourOccurrence>();
            foreach (TourOccurrence tourOccurrence in tourOccurrences)
            {
                if (tourOccurrence.DateTime.Date >= DateTime.Now.Date && tourOccurrence.CurrentState != CurrentState.Ended && tourOccurrence.IsDeleted == false)
                {
                    tourOccurrence.MakeDetailedRowString();
                    result.Add(tourOccurrence);
                }
            }
            return result;
        }
        public List<TourOccurrence> GetOfferedToursByLocation(Location location)
        {
            List<TourOccurrence> result = GetOffered();
            result.RemoveAll(occurrence => !occurrence.Tour.Location.Equals(location));
            return result;
        }
        public List<TourOccurrence> GetFinishedOccurrencesForGuide(int guideId)
        {
            List<TourOccurrence> result = new List<TourOccurrence>();
            foreach (TourOccurrence occurrence in tourOccurrences)
            {
                if (occurrence.CurrentState == CurrentState.Ended && occurrence.GuideId == guideId)
                {
                    result.Add(occurrence);
                }
            }
            return result;
        }

        public List<TourOccurrence> GetFinishedOccurrencesForGuideByYear(int guideId, int year)
        {
            List<TourOccurrence> result = new List<TourOccurrence>();
            foreach (TourOccurrence occurrence in tourOccurrences)
            {
                if (occurrence.CurrentState == CurrentState.Ended && occurrence.GuideId == guideId && occurrence.DateTime.Year == year)
                {
                    result.Add(occurrence);
                }
            }
            return result;
        }

        public TourOccurrence SaveTourOccurrence(TourOccurrence tourOccurrence, int activeGuideId)
        {
            tourOccurrence.Id = NextId();
            tourOccurrence.GuideId = activeGuideId;
            tourOccurrences.Add(tourOccurrence);
            _serializer.ToCSV(FilePath, tourOccurrences);
            NotifyObservers();
            return tourOccurrence;
        }

        public void UpdateTourOccurrence(TourOccurrence tourOccurrence)
        {
            TourOccurrence oldTourOccurrence = tourOccurrences.Find(t => t.Id == tourOccurrence.Id);
            oldTourOccurrence.CurrentState = tourOccurrence.CurrentState;
            oldTourOccurrence.FreeSpots = tourOccurrence.FreeSpots;
            _serializer.ToCSV(FilePath, tourOccurrences);
            NotifyObservers();
        }

        public void Subscribe(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.Update();
            }
        }

        public TourOccurrence GetById(int id)
        {
            TourOccurrence tourOccurrence = tourOccurrences.Find(t => t.Id == id);
            return tourOccurrence;
        }
        public TourOccurrence GetByTourId(int id)
        {
            TourOccurrence tourOccurrence = tourOccurrences.Find(t => t.TourId == id);
            return tourOccurrence;
        }
        public int Delete(TourOccurrence tourOccurrence)
        {
            TourOccurrence oldTourOccurrence = tourOccurrences.Find(t => t.Id == tourOccurrence.Id);
            if (oldTourOccurrence == null)
            {
                return 0;
            }
            oldTourOccurrence.IsDeleted = true;
            _serializer.ToCSV(FilePath, tourOccurrences);
            NotifyObservers();
            return oldTourOccurrence.Id;
        }
        public void UndoDelete(int canceledTour)
        {
            TourOccurrence oldTourOccurrence = tourOccurrences.Find(t => t.Id == canceledTour);
            if (oldTourOccurrence == null)
            {
                return;
            }
            oldTourOccurrence.IsDeleted = false;
            _serializer.ToCSV(FilePath, tourOccurrences);
            NotifyObservers();
        }
    }
}
