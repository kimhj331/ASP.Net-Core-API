using AutoMapper;
using ButlerLee.API;
using ButlerLee.API.Clients;
using ButlerLee.API.Contracts.IClients;
using ButlerLee.API.Contracts.IRepositories;
using ButlerLee.API.Contracts.IServices;
using ButlerLee.API.Entities;
using ButlerLee.API.Models;
using ButlerLee.API.Repositories;
using ButlerLee.API.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTest
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public CustomWebApplicationFactory()
        {
            this.ClientOptions.AllowAutoRedirect = false;
            this.ClientOptions.BaseAddress = new Uri("https://localhost:44338/");
        }

        public IRepositoryWrapper RepositoryWrapper { get; private set; }

        internal IListingClient ListingClient { get; private set; }
        internal IAmenityClient AmanityClient { get; private set; }
        internal IReservationClient ReservationClient { get; private set; }
        internal ICommonClient CommonClient { get; private set; }

        public IMapper Mapper { get; private set; }
        public IServiceWrapper ServiceWrapper { get; private set; }

        public Configurations Configurations { get; private set; }
        public object Substitute { get; private set; }

        protected override void ConfigureClient(HttpClient client)
        {
            using (var serviceScope = this.Services.CreateScope())
            {
                Configurations = base.Services.GetService<Configurations>();
                var serviceProvider = serviceScope.ServiceProvider;

                var optionsBuilder = new DbContextOptionsBuilder<RepositoryContext>();
                optionsBuilder.UseMySql(Configurations.ConnectionString).EnableSensitiveDataLogging(true);
                var context = new RepositoryContext(optionsBuilder.Options);

                this.RepositoryWrapper = new RepositoryWrapper(context, Configurations);
                this.Mapper = serviceProvider.GetRequiredService<IMapper>();

                //this.ServiceWrapper = new ServiceWrapper(this.RepositoryWrapper, this.Mapper, this.Configurations);
            }

            //client.DefaultRequestHeaders.Authorization =
            //        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetBearerToken().Result);

            base.ConfigureClient(client);
        }
        private async Task<string> GetBearerToken()
        {
            return "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIzNDQ0OSIsImp0aSI6IjdjMGI5YjQ3NjE3MjdhNDFlOGU3ZWY5NmNjMTc2ZWRiZjE0ZjVmMTAxYzQ1ZTA2MjA1YThiOGRmNGIwNjNjYjc5OTE5NDVjYjQ2MmJhNTkxIiwiaWF0IjoxNjQwMzM0MTA2LCJuYmYiOjE2NDAzMzQxMDYsImV4cCI6MTcwMzQwNjEwNiwic3ViIjoiIiwic2NvcGVzIjpbImdlbmVyYWwiXSwic2VjcmV0SWQiOjM3NDB9.K0E8KH43KgZSNTKTghob8YSPhOTIN8hPO4voEPVfuxuv7wAejfDHDw1h6eTjHyXuGBpVk5SXL-qxLmAZRmh8VDPi3a-eOmrG9w4lM1IHmXrzjvnOSdyDn9bRHk4eqDkqaEeZoBjVaDnFoJhw2NOSTk0nj_sKtkrRInzEeEz37sw";// await Login("smpark@saladsoft.com", "111111");
        }

        /*
        public async Task<string> Login(string userName, string password)
        {
            var user = await this.ServiceWrapper.User.Login(userName.ToLower(), password);

            //generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configurations.AppSettings.Token);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }*/
    }
}
