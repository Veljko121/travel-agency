﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Observer;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class TourRepository : ITourRepository
    {
        private const string FilePath = "../../../Resources/Data/tours.csv";
        private readonly Serializer<Tour> _serializer;
        private List<Tour> tours;

        public TourRepository()
        {
            _serializer = new Serializer<Tour>();
            tours = _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            if (tours.Count == 0)
            {
                return 1;
            }
            return tours[tours.Count - 1].Id + 1;
        }

        public List<Tour> GetAll()
        {
            return tours;
        }
        public Tour GetById(int id) 
        {
            return tours.Find(t => t.Id == id);
        }
        public Tour Save(Tour tour)
        {
            tour.Id = NextId();
            tours.Add(tour);
            _serializer.ToCSV(FilePath, tours);
            return tour;
        }
    }
}
