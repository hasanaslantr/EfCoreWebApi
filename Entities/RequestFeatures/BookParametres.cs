namespace Entities.RequestFeatures
{
    public class BookParametres : RequestParameters
    {
        public uint MinPrice { get; set; }
        public uint MaxPrice { get; set; } = 1000;
        public bool ValidRriceRange => MaxPrice > MinPrice;
        public String? SearchTerm { get; set; }

        public BookParametres()
        {
            OrderBy = "id";
        }

    }
}
