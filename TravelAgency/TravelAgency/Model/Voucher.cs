﻿using System;
using System.Globalization;

namespace TravelAgency.Model
{
    public class Voucher : Serializer.ISerializable
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public int GuideId { get; set; }
        public bool IsUsed { get; set; }
        public DateTime Deadline { get; set; }
        public Voucher()
        {
        }

        public Voucher(int id, int guestId, int guideId, DateTime dateTime)
        {
            Id = id;
            GuestId = guestId;
            GuideId = guideId;
            IsUsed = false;
            Deadline = dateTime;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuestId.ToString(), GuideId.ToString(), IsUsed.ToString(), Deadline.ToString("dd-MM-yyyy HH-mm") };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            GuestId = int.Parse(values[1]);
            GuideId = int.Parse(values[2]);
            IsUsed = bool.Parse(values[3]);
            Deadline = DateTime.ParseExact(values[4], "dd-MM-yyyy HH-mm", CultureInfo.InvariantCulture);
        }
    }
}
