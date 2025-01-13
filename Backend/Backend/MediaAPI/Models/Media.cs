namespace MediaAPI.Models
{
    public class Media
    {
        // Primary Key
        public int Id { get; set; }

        // Properties
        public string Title { get; set; }
        public string Genre { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }

        // Foreign Key
        public int MediaTypeId { get; set; }
    }
}
