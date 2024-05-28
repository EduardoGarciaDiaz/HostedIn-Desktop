using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Abstract;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Enums;
using HostedInDesktop.Utils;
using HostedInDesktop.viewmodels.ModelObservable;
using HostedInDesktop.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels
{
    public partial class AccommodationDetailsViewModel : ObservableObject
    {
        public const string ACCOMMODATION_KEY = "AccommodationData";
        private readonly IReviewsService _reviewsService = new ReviewsService();
        ISharedService _sharedService = new SharedService();

        [ObservableProperty]
        private Accommodation accommodationData;

        [ObservableProperty]
        private ObservableCollection<UserReview> userReview;

        [ObservableProperty]
        private string accommodationType;

        [ObservableProperty]
        private string accommodationBasics;

        [ObservableProperty]
        private string accommodationServices;

        [ObservableProperty]
        private string accommodationScore;

        [ObservableProperty]
        private string withOutReviews;

        [ObservableProperty]
        private ImageSource profilePhotoHost;

        [ObservableProperty]
        private string latitude;

        [ObservableProperty]
        private string longitude;

        public AccommodationDetailsViewModel(ISharedService sharedService)
        {
            _sharedService = sharedService;
            LoadAccommodationData();
            UserReview = new ObservableCollection<UserReview>();
            GetReviews();

        }

        [RelayCommand]
        public async void GoBack()
        {
            await Shell.Current.GoToAsync(nameof(GuestView));
        }

        private void LoadAccommodationData()
        {
            try
            {
                AccommodationData = _sharedService.GetValue<Accommodation>(ACCOMMODATION_KEY);
                LoadAccommodationType();
                LoadAccommodationBasics();
                LoadAccommodationServices();
                LoadLocation();
                LoadHostData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void GetReviews()
        {
            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(AccommodationData._id))
                    {
                        List<Review> reviews = await _reviewsService.GetReviewsOfAccommodation(AccommodationData._id);  
                        string reviewsJson = JsonConvert.SerializeObject(reviews);
                        ImageSource profile = "ic_user.png";

                        foreach (Review review in reviews)
                        {

                            if (review.guestUser.profilePhoto != null && review.guestUser.profilePhoto.data != null && review.guestUser.profilePhoto.data.Length > 0)
                            {
                                byte[] imageData = review.guestUser.profilePhoto.data;
                                profile = ImageSource.FromStream(() => new MemoryStream(imageData));
                            }
                            else
                            {
                                profile = "ic_user.png";
                            }

                            UserReview.Add(new UserReview
                            {
                                ProfilePhoto = profile,
                                GuestName = review.guestUser.fullName,
                                ValueRating = review.rating,
                                Description = review.reviewDescription
                            });
                        }

                        CalculateScore(reviews);

                    }
                }
                catch (ApiException aex)
                {
                    Console.WriteLine(aex.StackTrace);
                    await Shell.Current.DisplayAlert("Error", aex.Message, "Ok");
                    return;
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Error ", ex.Message, "Ok");
                    return;
                }
            }
        }

        private void CalculateScore(List<Review> reviews)
        {
            List<float> scores = new List<float>();

            foreach (var review in reviews)
            {
                float score = review.rating;
                if (score > 0)
                {
                    scores.Add(score);
                }
            }

            if (scores.Count > 0)
            {
                double averageScore = scores.Average();
                AccommodationScore = averageScore.ToString();
            }
        }

        private void LoadAccommodationType()
        {
            string type = AccommodationData.accommodationType;

            if (!string.IsNullOrEmpty(type))
            {
                string accommodationTypeDescription = AccommodationTypeDescriptions.GetDescriptionForType(type);
                if (!string.IsNullOrEmpty(accommodationTypeDescription))
                {
                    AccommodationType = accommodationTypeDescription;
                }
            }
        }

        private void LoadAccommodationBasics()
        {
            const string delimiter = " · ";

            string guestsNumber = AccommodationData.guestsNumber.ToString();
            string roomsNumber = AccommodationData.roomsNumber.ToString();
            string bedsNumber = AccommodationData.bedsNumber.ToString();
            string bathroomsNumber = AccommodationData.bathroomsNumber.ToString();

            string[] basics = { guestsNumber, roomsNumber, bedsNumber, bathroomsNumber };
            string[] basicsNames = { " Huéspedes", " Habitaciones", " Camas", " Baños" };

            var basicsDetails = basics.Zip(basicsNames, (basic, name) => basic + name).ToArray();

            AccommodationBasics = string.Join(delimiter, basicsDetails);
        }

        private void LoadAccommodationServices()
        {
            const string delimiter = " · ";
            var services = AccommodationData.accommodationServices;

            if (services != null && services.Length > 0)
            {
                var accommodationServices = new List<string>();

                foreach (var service in services)
                {
                    var description = AccommodationServiceDescriptions.GetDescriptionForService(service);
                    if (!string.IsNullOrEmpty(description))
                    {
                        accommodationServices.Add(description);
                    }
                }

                if (accommodationServices.Count > 0)
                {
                    AccommodationServices = string.Join(delimiter, accommodationServices);
                }
                else
                {
                    AccommodationServices = string.Join(delimiter, services);
                }
            }
        }

        private void LoadLocation()
        {
            Latitude = AccommodationData.location.coordinates[1].ToString();
            Longitude = AccommodationData.location.coordinates[0].ToString();
        }

        private void LoadHostData()
        {
            ProfilePhotoHost = "ic_user.png";

            if (AccommodationData.user.profilePhoto != null && AccommodationData.user.profilePhoto.data != null && AccommodationData.user.profilePhoto.data.Length > 0)
            {
                byte[] imageData = AccommodationData.user.profilePhoto.data;
                ProfilePhotoHost = ImageSource.FromStream(() => new MemoryStream(imageData));
            }
        }

    }
}
