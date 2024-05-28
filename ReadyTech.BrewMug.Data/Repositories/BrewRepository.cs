using ReadyTech.BrewMug.Data.Interfaces;
using ReadyTech.BrewMug.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyTech.BrewMug.Data.Repositories
{
    public class BrewRepository: IBrewRepository
    {
        public BrewRepository() { }

        public Task<BrewCoffeeDTO> GetBrewCoffeeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
