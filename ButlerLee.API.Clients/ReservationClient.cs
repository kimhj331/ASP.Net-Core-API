using ButlerLee.API.Contracts.IClients;
using ButlerLee.API.Extensions;
using ButlerLee.API.Models;
using ButlerLee.API.Models.Enumerations;
using ButlerLee.API.Models.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ButlerLee.API.Clients
{
    public class ReservationClient : IReservationClient
    {
        private readonly HttpClient httpClient;
        private const string defaultUri = "reservations";

        public ReservationClient(IHttpClientFactory clientFactory) => this.httpClient = clientFactory.CreateClient(nameof(HostAwayClient));

        public async Task<HostawayResponse<IEnumerable<Reservation>>> GetReservations(ReservationParameters parameters)
        {
            HttpResponseMessage response =
              await this.httpClient.GetAsync($"{defaultUri}?{parameters.GetQueryString()}");

            await response.EnsureSuccessStatusCodeAsync();
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<HostawayResponse<IEnumerable<Reservation>>>(content);
        }

        public async Task<HostawayResponse<Reservation>> GetReservationById(int reservationId)
        {
            HttpResponseMessage response =
              await this.httpClient.GetAsync($"{defaultUri}/{reservationId}");

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode == false)
            {
                HostawayResponse<Reservation> hostawayResponse = 
                    new HostawayResponse<Reservation>(ResponseStatus.Fail, null);

                return hostawayResponse;
            }

            return JsonConvert.DeserializeObject<HostawayResponse<Reservation>>(content);
        }

        public async Task<HostawayResponse<Reservation>> CreateReservation(Reservation reservation)
        {
            HttpContent requestContent = new StringContent(reservation.ToJson(), Encoding.UTF8);
            HttpResponseMessage response =
                await this.httpClient.PostAsync($"{defaultUri}?forceOverbooking=1", requestContent);

            await response.EnsureSuccessStatusCodeAsync();
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<HostawayResponse<Reservation>>(responseContent);
        }

        public async Task<HostawayResponse<Reservation>> CreateReservationWithCard(Reservation reservation)
        {
            HttpContent requestContent = new StringContent(reservation.ToJson(), Encoding.UTF8);
            HttpResponseMessage response =
                await this.httpClient.PostAsync($"{defaultUri}?validatePaymentMethod=1", requestContent);

            await response.EnsureSuccessStatusCodeAsync();
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<HostawayResponse<Reservation>>(responseContent);
        }

        public async Task<HostawayResponse<Reservation>> UpdateReservation(int reservationId, Reservation reservation)
        {
            HttpContent requestContent = new StringContent(reservation.ToJson(), Encoding.UTF8);
            HttpResponseMessage response =
                await this.httpClient.PutAsync($"{defaultUri}/{reservationId}?forceOverbooking=1", requestContent);

            await response.EnsureSuccessStatusCodeAsync();
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<HostawayResponse<Reservation>>(responseContent);
        }

        public async Task<HostawayResponse<Reservation>> UpdateReservation(int reservationId, string hostNote, bool isPaid) //TODO
        {
            var isPaidObject = new { IsPaid = isPaid, HostNote = hostNote };

            HttpContent requestContent = new StringContent(isPaidObject.ToJson(), Encoding.UTF8);
            HttpResponseMessage response =
                await this.httpClient.PutAsync($"{defaultUri}/{reservationId}?forceOverbooking=1", requestContent);

            await response.EnsureSuccessStatusCodeAsync();
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<HostawayResponse<Reservation>>(responseContent);
        }
       

        public async Task<HostawayResponse<Reservation>> CancelReservaion(int reservationId, CancelledBy cancelledBy)
        {
            var json = JsonConvert.SerializeObject(new { CancelledBy = cancelledBy.ToString().ToLower() });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response =
                await this.httpClient.PutAsync($"{defaultUri}/{reservationId}/statuses/cancelled", data);

            await response.EnsureSuccessStatusCodeAsync();
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<HostawayResponse<Reservation>>(responseContent);
        }

        public async Task DeleteReservation(int reservationId)
        {
            HttpResponseMessage response = 
                await this.httpClient.DeleteAsync($"{defaultUri}/{reservationId}");

            await response.EnsureSuccessStatusCodeAsync();
        }
    }
}
