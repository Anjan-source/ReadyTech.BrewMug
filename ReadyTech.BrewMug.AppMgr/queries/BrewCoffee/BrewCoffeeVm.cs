
namespace ReadyTech.BrewMug.AppMgr.queries.BrewCoffee
{
    using ReadyTech.BrewMug.AppMgr.Enums;
    /// <summary>
    /// BrewCoffeeVm represents model output as per the user needs
    /// </summary>
    public class BrewCoffeeVm
    {
        public BrewCoffee BrewCoffeeInfo { get; set; }
        public CoffeeStatus CoffeStatus {get; set;}
    }
    
}