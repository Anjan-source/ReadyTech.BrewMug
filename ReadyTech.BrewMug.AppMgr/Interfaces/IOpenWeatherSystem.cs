using ReadyTech.BrewMug.AppMgr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyTech.BrewMug.AppMgr.Interfaces
{
    public interface IOpenWeatherSystem
    {
        public Task<RootObject> GetWheatherTemparatureAsync();
    }
}
