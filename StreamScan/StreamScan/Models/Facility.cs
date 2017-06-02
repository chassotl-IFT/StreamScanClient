namespace StreamScan.Models
{
    public class Facility
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address{ get; set; }
        public int Npa { get; set; }
        public string City { get; set; }
        public int Fk_Enterprise { get; set; }
    }
}