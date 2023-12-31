﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class TourRatingFormViewModel : INotifyPropertyChanged
    {
        private string url;
        public string GuideKnowledge { get; set; }
        public string GuideLanguage { get; set; }
        public string Interesting { get; set; }
        public string AdditionalComment { get; set; }
        public string Url
        {
            get { return url; }
            set { url = value; OnPropertyChanged(); }
        }
        public string SelectedUrl { get; set; }
        public ObservableCollection<string> Urls { get; set; }
        public string Description { get; set; }
        private TourRatingService tourRatingService;
        public ButtonCommandNoParameter AddUrlCommand { get; set; }
        public ButtonCommandNoParameter RemoveUrlCommand { get; set; }

        private int currentGuestId;
        private int occurrenceId;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public TourRatingFormViewModel(TourOccurrence occurrence, int guestId)
        {
            GuideKnowledge = "5";
            GuideLanguage = "5";
            Interesting = "5";
            Urls = new ObservableCollection<string>();
            Description = "Guide: "+occurrence.Guide.Username+".  " +occurrence.Tour.Name + " in " + occurrence.Tour.Location.Country +
                ", " + occurrence.Tour.Location.City + ". " + occurrence.Tour.Description;
            tourRatingService = new TourRatingService();
            AddUrlCommand = new ButtonCommandNoParameter(AddUrl);
            RemoveUrlCommand = new ButtonCommandNoParameter(RemoveUrl);
            currentGuestId = guestId;
            occurrenceId = occurrence.Id;
            UpdateHelpText();
        }
        private void AddUrl()
        {
            if (Url != null && Url != "")
            {
                Urls.Add(Url);
                Url = "";
            }
        }
        private void RemoveUrl()
        {
            if (SelectedUrl != null)
            {
                Urls.Remove(SelectedUrl);
            }
        }
        public void SubmitRating()
        {
            int guideKnowledge;
            int guideLanguage;
            int interesting;
            guideKnowledge = int.Parse(GuideKnowledge);
            guideLanguage = int.Parse(GuideLanguage);
            interesting = int.Parse(Interesting);
            TourRating tourRating = new TourRating(currentGuestId, occurrenceId, guideKnowledge, guideLanguage, interesting, AdditionalComment, null);
            TourRating savedTourRating = tourRatingService.SaveTourRating(tourRating);
            savePhotos(savedTourRating.Id);
        }
        private void savePhotos(int id)
        {
            for (int i = 0; i < Urls.Count; i++)
            {
                TourRatingPhoto photo = new TourRatingPhoto();
                photo.Link = Urls[i];
                photo.TourRatingId = id;
                tourRatingService.SaveTourRatingPhoto(photo);
            }
        }
        private void UpdateHelpText()
        {
            string file = @"../../../Resources/HelpTexts/TourRatingHelp.txt";
            Guest2MainViewModel.HelpText = File.ReadAllText(file);
        }
    }
}
