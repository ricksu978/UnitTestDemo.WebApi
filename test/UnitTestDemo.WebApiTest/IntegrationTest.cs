using System.Net.Http.Json;
using System.Threading.Tasks.Dataflow;
using UnitTestDemo.WebApiTest.Common;

namespace UnitTestDemo.WebApiTest
{
    public class IntegrationTest
    {
        private readonly WebApiTestServer _server = new();

        [SetUp]
        public async Task Setup()
        {
            await _server.StartAsync();
        }

        [Test]
        public async Task AllTests()
        {
            await EchoGetTest();
            await EchoPostTest();
            await EchoPutTest();
            await EchoDeleteTest();
        }

        private async Task EchoGetTest()
        {
            var apiResponse = await _server.WebApiClient.GetStringAsync("/api/echo?hello=world&foo=bar");

            var socketResponse = await _server.MessageBuffer.ReceiveAsync();

            Assert.That(apiResponse, Is.EqualTo(socketResponse));
        }

        private async Task EchoPostTest()
        {
            var httpResponse = await _server.WebApiClient.PostAsJsonAsync("/api/echo", new { Hello = "World" });

            var apiResponse = await httpResponse.Content.ReadAsStringAsync();
            var socketResponse = await _server.MessageBuffer.ReceiveAsync();

            Assert.That(apiResponse, Is.EqualTo(socketResponse));
        }

        private async Task EchoPutTest()
        {
            var httpResponse = await _server.WebApiClient.PutAsJsonAsync("/api/echo", new { Foo = "Bar" });

            var apiResponse = await httpResponse.Content.ReadAsStringAsync();
            var socketResponse = await _server.MessageBuffer.ReceiveAsync();

            Assert.That(apiResponse, Is.EqualTo(socketResponse));
        }

        private async Task EchoDeleteTest()
        {
            var httpResponse = await _server.WebApiClient.DeleteAsync("/api/echo");

            var apiResponse = await httpResponse.Content.ReadAsStringAsync();
            var socketResponse = await _server.MessageBuffer.ReceiveAsync();

            Assert.That(apiResponse, Is.EqualTo(socketResponse));
        }
    }
}