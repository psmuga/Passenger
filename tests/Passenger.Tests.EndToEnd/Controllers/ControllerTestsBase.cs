using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Passenger.Api;
using Passenger.Infrastructure.Commands.Users;
using Passenger.Infrastructure.DTO;
using Xunit;
namespace Passenger.Tests.EndToEnd.Controllers
{
    public abstract class ControllerTestsBase
    {
        protected readonly TestServer Server;
        protected readonly HttpClient Client;

        public ControllerTestsBase()
        {
            Server = new TestServer(new WebHostBuilder()
                    .UseStartup<Startup>());
            Client = Server.CreateClient();
        }

        protected static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return new StringContent(json, Encoding.UTF8,"application/json");
        }
    }
}