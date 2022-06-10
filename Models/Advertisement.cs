namespace SimpleApi.Models
{
    public class Advertisement
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        [MaxLength(1024)]
        public string Description { get; set; }

        [Required]
        [MaxLength(255)]
        public string Tags { get; set; }

        public ICollection<Media> Medias { get; set; }
    }
}
