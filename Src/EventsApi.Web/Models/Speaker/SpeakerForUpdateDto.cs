using System.ComponentModel.DataAnnotations;

namespace EventsApi.Web.Models.Speaker
{
    public class SpeakerForUpdateDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
