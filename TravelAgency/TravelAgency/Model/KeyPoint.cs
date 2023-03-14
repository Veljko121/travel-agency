﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Model
{
    public class KeyPoint : Serializer.ISerializable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int TourOccurrenceId { get; set; }
        public string Name { get; set; }
        public List<User> Guests { get; set; }

        private bool isChecked;
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                if (value != isChecked)
                {
                    isChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public KeyPoint(int id, string name,  List<User> guests, int tourOccurrenceId)
        {
            Id = id;
            Name = name;
            Guests = guests;
            TourOccurrenceId = tourOccurrenceId;
        }

        public KeyPoint()
        {
            Name = "";
            Guests = new List<User>();
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TourOccurrenceId.ToString(), Name, isChecked.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TourOccurrenceId = int.Parse(values[1]);
            Name = values[2];
            IsChecked = bool.Parse(values[3]);
        }
    }
}
