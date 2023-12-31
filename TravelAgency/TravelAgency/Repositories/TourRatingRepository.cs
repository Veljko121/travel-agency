﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class TourRatingRepository : ITourRatingRepository
    {
        private const string FilePath = "../../../Resources/Data/tourRatings.csv";
        private readonly Serializer<TourRating> _serializer;
        private List<TourRating> tourRatings;
        public TourRatingRepository()
        {
            _serializer = new Serializer<TourRating>();
            tourRatings = _serializer.FromCSV(FilePath);
        }

        public TourRatingRepository(TourRatingPhotoRepository tourRatingPhotoRepository)
        {
            _serializer = new Serializer<TourRating>();
            tourRatings = _serializer.FromCSV(FilePath);
        }

        public List<TourRating> GetAll()
        {
            return tourRatings;
        }

        public List<TourRating> GetRatingsByTourOccurrenceId(int id)
        {
            List<TourRating> result = new List<TourRating>();
            foreach (TourRating tourRating in tourRatings)
            {
                if (tourRating.TourOccurrenceId == id)
                {
                    result.Add(tourRating);
                }
            }
            return result;
        }

        public int GetRatingsNumberByGuestId(int id)
        {
            int number = 0;
            foreach (TourRating tourRating in tourRatings)
                if (tourRating.GuestId == id)
                    number++;
            return number;
        }

        public int NextId()
        {
            if (tourRatings.Count == 0)
            {
                return 1;
            }
            return tourRatings[tourRatings.Count - 1].Id + 1;
        }

        public TourRating Save(TourRating tourRating)
        {
            tourRating.Id = NextId();
            tourRatings.Add(tourRating);
            _serializer.ToCSV(FilePath, tourRatings);

            return tourRating;
        }

        public bool IsTourNotRated(int guestId, int occurrenceId)
        {
            return !tourRatings.Exists(x => x.GuestId == guestId && x.TourOccurrenceId == occurrenceId);
        }
        public void UpdateIsValid(TourRating tourRating)
        {
            TourRating oldTourRating = tourRatings.Find(t => t.Id == tourRating.Id);
            oldTourRating.IsValid = tourRating.IsValid;
            _serializer.ToCSV(FilePath, tourRatings);
        }
    }
}
