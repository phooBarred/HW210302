using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _210302HW.Data
{
    public class AdsManager
    {
        private string _connectionString;

        public AdsManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Ad> GetAds()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                var ads = new List<Ad>();

                cmd.CommandText = @"SELECT * FROM Ads";
                connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Ad ad = new();
                    ad.Id = (int)reader["Id"];
                    ad.ListedBy = (string)reader["ListedBy"];
                    ad.PhoneNumber = (string)reader["PhoneNumber"];
                    ad.Text = (string)reader["Text"];
                    ad.DateCreated = (DateTime)reader["DateCreated"];
                    ads.Add(ad);
                }

                return ads;
            }
        }

        public void AddAd(Ad ad)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO Ads (ListedBy, PhoneNumber, Text, DateCreated)
                                    VALUES (@listedBy, @phone, @text, GETDATE()) SELECT SCOPE_IDENTITY()";
                object listedBy = ad.ListedBy;
                if (listedBy == null)
                {
                    listedBy = DBNull.Value;
                }
                cmd.Parameters.AddWithValue("@listedBy", listedBy);
                cmd.Parameters.AddWithValue("@phone", ad.PhoneNumber);
                cmd.Parameters.AddWithValue("@text", ad.Text);

                connection.Open();
                ad.Id = (int)(decimal)cmd.ExecuteScalar();
            }
        }
        public void DeleteAd(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"DELETE FROM Ads WHERE ID = @id";
                cmd.Parameters.AddWithValue("@id", id);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    public class Ad
    {
        public int Id { get; set; }
        public string ListedBy { get; set; }
        public string PhoneNumber { get; set; }
        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
        public bool CanDelete { get; set; }

    }
}
