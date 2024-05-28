namespace ReadyTech.BrewMug.AppMgr.Interfaces
{
    using ReadyTech.BrewMug.AppMgr.queries.BrewCoffee;
    public interface IBrewService
    {
        public Task<BrewCoffeeVm> GetBrewCoffeeInformationAsync();
    }
}
