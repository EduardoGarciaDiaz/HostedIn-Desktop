using Grpc.Net.Client;
using HostedInDesktop.Data.GrpcModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Services
{
    public class StaticticsServiceImpl
    {
        public StaticticsServiceImpl() { }

        public async Task<List<AccommodationGrpc>> GetMostBookedAccommodations()
        {
            try
            {
                using var channel = GrpcChannel.ForAddress(Utils.GrcpServerData.BASE_ADDRESS);
                global::StaticticsService.StaticticsServiceClient stub = new(channel);

                GuestRequest request = new GuestRequest()
                {
                    Token = $"Bearer {App.token}"
                };

                MostBookedAccommodationsResponse response = await stub.GetMostBookedAccommodationsAsync(request);
                List<AccommodationGrpc> mostBookedAccommodations = [];
                foreach (var accommodation in response.Accommodations)
                {
                    Console.WriteLine($"number {accommodation.BookingsNumber}");
                    mostBookedAccommodations.Add(new AccommodationGrpc()
                    {
                        Title = accommodation.Title,
                        BookingsNumber = accommodation.BookingsNumber,
                    });
                }
                return mostBookedAccommodations;
                
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<AccommodationGrpc>> GetBestRatedAccommodations()
        {
            try
            {
                using var channel = GrpcChannel.ForAddress(Utils.GrcpServerData.BASE_ADDRESS);
                global::StaticticsService.StaticticsServiceClient stub = new(channel);

                GuestRequest guestRequest = new GuestRequest()
                {
                    Token = $"Bearer {App.token}"
                };

                BestRatedAccommodationsResponse response = await stub.GetBestRatedAccommodationsAsync(guestRequest);
                List<AccommodationGrpc> mostRatedAccommodations = [];
                foreach (var accommodation in response.Accommodations)
                {
                    mostRatedAccommodations.Add(new AccommodationGrpc()
                    {
                        Title = accommodation.Name,
                        Rate = accommodation.Rate
                    });
                }
                return mostRatedAccommodations;

            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);  
                throw;
            }
        }


        public async Task<List<AccommodationGrpc>> GetMostHostBookedAccommodations(string _idHost)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress(Utils.GrcpServerData.BASE_ADDRESS);
                global::StaticticsService.StaticticsServiceClient stub = new(channel);

                HostRequest hostRequest = new HostRequest()
                {
                    IdHost = _idHost,
                    Token = $"Bearer {App.token}"
                };

                MostBookedAccommodationsResponse response = await stub.GetMostBookedAccommodationsOfHostAsync(hostRequest);
                List<AccommodationGrpc> mostBookedAccommodations = [];
                foreach (var accommodation in response.Accommodations)
                {
                    mostBookedAccommodations.Add(new AccommodationGrpc()
                    {
                        Title = accommodation.Title,
                        BookingsNumber = accommodation.BookingsNumber,
                    });
                }
                return mostBookedAccommodations;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<MonthEarning>> GetHostEarnings(string _idHost)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress(Utils.GrcpServerData.BASE_ADDRESS);
                global::StaticticsService.StaticticsServiceClient stub = new(channel);

                HostRequest hostRequest = new HostRequest()
                {
                    IdHost = _idHost,
                    Token = $"Bearer {App.token}"
                };

                EarningsResponse response = await stub.GetEarningsAsync(hostRequest);
                List<MonthEarning> earnings = new List<MonthEarning>();
                foreach(var earning in response.Earnings)
                {
                    earnings.Add(new MonthEarning()
                    {
                        Month = earning.Month,
                        Earnings = earning.Earning_
                    });
                }
                return earnings;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
