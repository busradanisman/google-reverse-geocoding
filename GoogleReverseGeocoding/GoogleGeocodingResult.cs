using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GoogleReverseGeocoding
{
    class GoogleGeocodingResult
    {
        private string returnLat;
        private string returnLng;
        private string returnId;
        private string geocodingResultArray;
        private string formattedAddress;
        private string placeId;

        public string ReturnLat { get => returnLat; set => returnLat = value; }
        public string ReturnLng { get => returnLng; set => returnLng = value; }
        public string ReturnId { get => returnId; set => returnId = value; }
        public string GeocodingResultArray { get => geocodingResultArray; set => geocodingResultArray = value; }
        public string FormattedAddress { get => formattedAddress; set => formattedAddress = value; }
        public string PlaceId { get => placeId; set => placeId = value; }

        public void Deneme()
        {
            HttpClient client = new HttpClient();
            //string apiKey = ConfigurationManager.AppSettings["Key"];
            HttpResponseMessage response = client.GetAsync(String.Format(ConfigurationManager.AppSettings["GetGoogleApiFromUrl"], ReturnLat, ReturnLng)).Result;
            response.EnsureSuccessStatusCode();
            GeocodingResultArray = response.Content.ReadAsStringAsync().Result;
        }
        public void GetAdressFromLatLng()
        {
            Deneme();
            JObject json = JObject.Parse(GeocodingResultArray);
            JArray jArray = (JArray)json["results"];
            FormattedAddress = jArray[0]["formatted_address"].ToString();
        }
        public void GetPlaceIdFromLatLng()
        {
            Deneme();
            JObject json = JObject.Parse(GeocodingResultArray);
            JArray jArray = (JArray)json["results"];
            PlaceId = jArray[0]["place_id"].ToString();
        }
    }
}
