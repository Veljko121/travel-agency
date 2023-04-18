﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using TravelAgency.Model;
using TravelAgency.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class AccommodationRepository : IAccommodationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodations.csv";

        private readonly Serializer<Accommodation> serializer;

        private List<Accommodation> accommodations;

        public AccommodationRepository()
        {
            serializer = new Serializer<Accommodation>();
            accommodations = serializer.FromCSV(FilePath);
        }

        public AccommodationRepository(List<User> users, List<Location> locations, List<AccommodationPhoto> photos) : this()
        {
            LinkOwners(users);
            LinkLocations(locations);
            LinkPhotos(photos);
        }

        public void LinkOwners(List<User> owners)
        {
            foreach (Accommodation accommodation in accommodations)
            {
                foreach (User user in owners)
                {
                    if (accommodation.OwnerId == user.Id)
                    {
                        accommodation.Owner = user;
                        break;
                    }
                }
            }
        }

        public void LinkLocations(List<Location> locations)
        {
            foreach (Accommodation accommodation in accommodations)
            {
                foreach (Location location in locations)
                {
                    if (accommodation.LocationId == location.Id)
                    {
                        accommodation.Location = location;
                        break;
                    }
                }
            }
        }

        public void LinkPhotos(List<AccommodationPhoto> photos)
        {
            foreach (Accommodation accommodation in accommodations)
            {
                foreach (AccommodationPhoto photo in photos)
                {
                    if (accommodation.Id == photo.ObjectId)
                    {
                        accommodation.Photos.Add(photo);
                    }
                }
            }
        }

        public int NextId()
        {
            if (accommodations.Count < 1)
            {
                return 1;
            }
            return accommodations.Max(c => c.Id) + 1;
        }

        public List<Accommodation> GetByOwner(User owner)
        {
            return accommodations.FindAll(c => c.OwnerId == owner.Id);
        }

        public List<Accommodation> GetAll()
        {
            return accommodations;
        }

        public Accommodation Save(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            accommodations.Add(accommodation);
            serializer.ToCSV(FilePath, accommodations);
            return accommodation;
        }

        public List<Accommodation> GetFiltered(AccommodationSearchFilter filter)
        {
            List<Accommodation> searchedAccommodations = GetAll();
            searchedAccommodations = FilterByName(filter.NameFilter, searchedAccommodations);
            searchedAccommodations = FilterByCountry(filter.CountryFilter, searchedAccommodations);
            searchedAccommodations = FilterByCity(filter.CityFilter, searchedAccommodations);
            searchedAccommodations = FilterByType(filter.TypeFilter, searchedAccommodations);
            searchedAccommodations = FilterByGuestNumber(filter.GuestNumberFilter, searchedAccommodations);
            searchedAccommodations = FilterByDayNumber(filter.DayNumberFilter, searchedAccommodations);

            searchedAccommodations = SortBySuperOwnersFirst(searchedAccommodations);

            return searchedAccommodations;
        }

        private List<Accommodation> FilterByName(string nameFilter, List<Accommodation> accommodations)
        {
            if (nameFilter != "")
            {
                return accommodations.Where(accommodation => accommodation.Name.ToLower().Trim().Contains(nameFilter.ToLower().Trim())).ToList();
            }
            return accommodations;
        }

        private List<Accommodation> FilterByCountry(string countryFilter, List<Accommodation> accommodations)
        {
            if (countryFilter != "Not specified")
            {
                return accommodations.Where(accommodation => accommodation.Location.Country.ToLower().Contains(countryFilter.ToLower())).ToList();
            }
            return accommodations;
        }

        private List<Accommodation> FilterByCity(string cityFilter, List<Accommodation> accommodations)
        {
            if (cityFilter != "Not specified")
            {
                return accommodations.Where(accommodation => accommodation.Location.City.ToLower().Contains(cityFilter.ToLower())).ToList();
            }
            return accommodations;
        }

        private List<Accommodation> FilterByType(string typeFilter, List<Accommodation> accommodations)
        {
            switch (typeFilter)
            {
                case "Appartment":
                    return accommodations.Where(accommodation => accommodation.Type == AccommodationType.APARTMENT).ToList();
                case "House":
                    return accommodations.Where(accommodation => accommodation.Type == AccommodationType.HOUSE).ToList();
                case "Hut":
                    return accommodations.Where(accommodation => accommodation.Type == AccommodationType.HUT).ToList();
            }
            return accommodations;
        }

        private List<Accommodation> FilterByGuestNumber(int guestNumberFilter, List<Accommodation> accommodations)
        {
            if (guestNumberFilter > 0)
            {
                return accommodations.Where(accommodation => guestNumberFilter <= accommodation.MaxGuests).ToList();
            }
            return accommodations;
        }

        private List<Accommodation> FilterByDayNumber(int DayFilter, List<Accommodation> accommodations)
        {
            if (DayFilter > 0)
            {
                return accommodations.Where(accommodation => DayFilter >= accommodation.MinDays).ToList();
            }
            return accommodations;
        }

        public List<Accommodation> SortBySuperOwnersFirst(List<Accommodation> accommodations)
        {
            var a = new List<Accommodation>(accommodations);
            List<Accommodation> sortedAccommodations = new List<Accommodation>();

            foreach (var accommodation in accommodations)
            {
                if (accommodation.Owner.IsSuperOwner)
                {
                    sortedAccommodations.Add(accommodation);
                    a.Remove(accommodation);
                }
            }

            foreach (var accommodation in a)
            {
                sortedAccommodations.Add(accommodation);
            }

            return sortedAccommodations;
        }
    }
}
