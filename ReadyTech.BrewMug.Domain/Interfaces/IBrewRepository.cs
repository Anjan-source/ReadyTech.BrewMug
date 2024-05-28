
namespace ReadyTech.BrewMug.Data.Interfaces
{
    using ReadyTech.BrewMug.Domain.Models;
    public interface IBrewRepository
    {
        public Task<BrewCoffeeDTO> GetBrewCoffeeAsync();

    }
}
