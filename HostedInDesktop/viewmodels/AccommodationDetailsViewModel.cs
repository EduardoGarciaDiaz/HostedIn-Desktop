using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HostedInDesktop.Abstract;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Enums;
using HostedInDesktop.Messages;
using HostedInDesktop.Utils;
using HostedInDesktop.viewmodels.ModelObservable;
using HostedInDesktop.Views;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Projections;
using Mapsui.Tiling;
using Mapsui.UI.Maui;
using Newtonsoft.Json;
using Syncfusion.Maui.Core.Carousel;
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
        private readonly MultimediaServiceImpl _multimediaService = new MultimediaServiceImpl();
        ISharedService _sharedService = new SharedService();

        [ObservableProperty]
        private Accommodation accommodationData;

        partial void OnAccommodationDataChanged(Accommodation value)
        {
            
        }

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private ObservableCollection<UserReview> userReview;

        [ObservableProperty]
        private string accommodationPrice;

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

        [ObservableProperty]
        private bool isImage;

        [ObservableProperty]
        private bool isVideo;

        [ObservableProperty]
        private int index;

        public ObservableCollection<ImageSource> MultimediaItems { get; } = new ObservableCollection<ImageSource>();

        [ObservableProperty]
        private string videoFilePath;

        [ObservableProperty]
        private ImageSource imageSource;

        [ObservableProperty]
        private bool isCarouselButtonEnabled;

        public AccommodationDetailsViewModel(ISharedService sharedService)
        {
            _sharedService = sharedService;
            WeakReferenceMessenger.Default.Register<AccommodationSelectedMessage>(this, (r, m) =>
            {
                AccommodationData = m.Value;
                LoadAccommodationData();
                UserReview = new ObservableCollection<UserReview>();
                GetReviews();
                isImage = true;
                isVideo = false;
                index = 0;

                OnPropertyChanged(nameof(AccommodationData));
            });

            LoadAccommodationData();
            UserReview = new ObservableCollection<UserReview>();
            GetReviews();
            isImage = true;
            isVideo = false;
            index = 0;
        }

        [RelayCommand]
        public async void GoBack()
        {
            App.contentToShow = new ExploreView();
            await Shell.Current.GoToAsync(nameof(GuestView));
        }

        [RelayCommand]
        public async void GoToBooking()
        {
            if (AccommodationData != null)
            {
                _sharedService.Add<Accommodation>(AccommodationDetailsViewModel.ACCOMMODATION_KEY, AccommodationData);
                WeakReferenceMessenger.Default.Send(new AccommodationBookingMessage(AccommodationData));
                await Shell.Current.GoToAsync(nameof(AccommodationBooking));
            }
        }

        private async void LoadAccommodationData()
        {
            try
            {
                IsLoading = true;

                AccommodationData = _sharedService.GetValue<Accommodation>(ACCOMMODATION_KEY);
                LoadAccommodationPrice();
                LoadAccommodationType();
                LoadAccommodationBasics();
                LoadAccommodationServices();
                LoadLocation();
                LoadHostData();
                IsCarouselButtonEnabled = false;
                await LoadAccommodationMultimedias();
                IsCarouselButtonEnabled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void GetReviews()
        {
            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    IsLoading = true;

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
                finally
                {
                    IsLoading = false;
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

        private void LoadAccommodationPrice()
        {
            double nightPrice = AccommodationData.nightPrice;
            string price = $"${nightPrice} MXN por noche";
            AccommodationPrice = price;
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

        private async Task LoadAccommodationMultimedias()
        {
            ImageSource imageSource;
            MultimediaItems.Clear();

            for (int i = 0; i < 4; i++)
            {
                if (i == 3)
                {
                    await LoadAccommodationMultimedia(i, true);
                }
                else
                {
                    await LoadAccommodationMultimedia(i, false);
                }
            }

            ImageSource = MultimediaItems[0];
        }

        private async Task LoadAccommodationMultimedia(int multimediaIndex, bool isVideo)
        {
            ImageSource imageSource1;
            imageSource1 = ImageSource.FromFile("img_provisional.png");

            try
            {
                IsLoading = true;

                var imageBytes1 = await _multimediaService.LoadMainImageAccommodation(AccommodationData._id, multimediaIndex);

                if (imageBytes1 != null)
                {
                    imageSource1 = ImageSource.FromStream(() => new MemoryStream(imageBytes1));
                }
                MultimediaItems.Add(imageSource1);

                if (isVideo)
                    VideoFilePath = await ImageHelper.SaveVideoToFileAsync(imageBytes1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [RelayCommand]
        private void GoBackCarousel()
        {
            Index--;
            if (Index == -1)
            {
                IsVideo = true;
                IsImage = false;
                Index = 3;

                return;
            }
            else if (Index == 2)
            {
                IsVideo = false;
                IsImage = true;
            }

            if (MultimediaItems.Count > 0)
            {
                ImageSource = MultimediaItems[Index];
            }
        }

        [RelayCommand]
        private void GoAheadCarousel()
        {
            Index++;
            if (Index == 3)
            {
                IsVideo = true;
                IsImage = false;
                return;
            }
            else if (Index == 4)
            {
                IsVideo = false;
                IsImage = true;
                Index = 0;

            }

            if (MultimediaItems.Count > 0)
            {
                ImageSource = MultimediaItems[Index];
            }
        }

    }
}
