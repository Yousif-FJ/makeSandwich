using System.ComponentModel.DataAnnotations;

namespace server_a.ApiModels
{
    public partial class Sandwich
    {
        public long? Id { get; set; }

        [Required]
        public required string Name { get; set; }


        public List<Topping>? Toppings { get; set; } 


        [Required]
        public BreadTypeEnum BreadType { get; set; }
    }
    public enum BreadTypeEnum
    {
        Oat = 1,
        Rye = 2,
        Wheat = 3
    }
}