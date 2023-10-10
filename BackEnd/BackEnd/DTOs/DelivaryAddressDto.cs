namespace BackEnd.DTOs
{
    public class DelivaryAddressDto
    {
        public class AddressDTO
        {
            public int? userId { get; set; }
            public string? username { get; set; }
            public int? addressId { get; set; }
            public string country { get; set; }
            public string state { get; set; }
            public string city { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string address { get; set; }
            public string zip { get; set; }
        }

    }
}
