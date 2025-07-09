namespace PersonMicroservice.Models.DTO
{
    public class StockCreateDTO
    {
        public string BloodGroup { get; set; }
        public int Quantity { get; set; }
    }

    public class StockUpdateDTO
    {
        public int Quantity { get; set; }
    }

    public class StockGetDTO
    {
        public string BloodGroup { get; set; }
        public uint Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
