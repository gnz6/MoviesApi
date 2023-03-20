using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs
{
    public class RoomNearFilterDTO
    {


        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Range(-180, 180)]
        public double Longitude { get; set; }

        private int kmDistance = 10;
        private int maxKmDistance = 50;

        public int KmDistance
        {
            get { return kmDistance; }
            set
            {
                kmDistance = (value> maxKmDistance)? maxKmDistance : value;
            }
        }
    }
}
