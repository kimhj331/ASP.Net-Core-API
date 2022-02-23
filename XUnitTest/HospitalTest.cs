//using ButlerLee.API.Contracts.IServices;
//using ButlerLee.API.Models;
//using FluentAssertions;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Xunit;

//namespace XUnitTest
//{
//    public class HospitalTest : IClassFixture<CustomWebApplicationFactory>
//    {
//        private readonly HttpClient _client;
//        private readonly IServiceWrapper _serviceWrapper;

//        public HospitalTest(CustomWebApplicationFactory factory)
//        {
//            _client = factory.CreateDefaultClient();
//            _serviceWrapper = factory.ServiceWrapper;
//        }

//        [Fact]
//        public async Task GetAllHospitals()
//        {

//            //int pageNumber = 1;
//            //int pageSize = 1000;
//            //var response =
//            //    await _client.GetAsync($"api/Hospitals?" +
//            //        $"pageNumber={pageNumber}&" +
//            //        $"pageSize={pageSize}");

//            //var result = response.Content.ReadAsStringAsync().Result;
//            //IEnumerable<Hospital> customers = JsonConvert.DeserializeObject<IEnumerable<Hospital>>(result);

//            //result.Should().NotBeNull();
//            //response.StatusCode.Should().Be(HttpStatusCode.OK);

//            ////Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
//            ////Assert.True(customers != null);
//        }
//    }
//}
