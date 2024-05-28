using Moq;
using ReadyTech.BrewMug.AppMgr.Interfaces;
using ReadyTech.BrewMug.AppMgr.queries.BrewCoffee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyTech.BrewMug.AppMgr.Test.queries
{
    [TestClass]
    public class GetBrewCoffeeQueryHandlerTest
    {
        private readonly Mock<IBrewService> _brewService;
        public GetBrewCoffeeQueryHandlerTest()
        {
            _brewService = new Mock<IBrewService>(); ;
        }

        [TestMethod]
        public async Task Get_BrewCoffee_Information_Handler_Test()
        {
           
        }


    }
}
