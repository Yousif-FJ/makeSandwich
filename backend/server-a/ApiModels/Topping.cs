using System.ComponentModel.DataAnnotations;

namespace server_a.ApiModels
{
    public partial class Topping
    {
        public long? Id { get; set; }

        [Required]
        public required string Name { get; set; }
    }
}
