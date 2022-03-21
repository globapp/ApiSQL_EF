namespace Api.Models
{
    public class MapAddress
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Idcustomer { get; set; }
        public Int16 AddressType { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? Address { get; set; }
        public string? Adby { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

    }
}
