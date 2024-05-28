
namespace ReadyTech.BrewMug.Domain.Models
{
    /// <summary>
    /// BrewCoffeDTO represts Domina model as per the database table
    /// </summary>
    public class BrewCoffeeDTO
    {
        public string Message {  get; set; }
        public DateTimeOffset Prepared {  get; set; }
    }
}
