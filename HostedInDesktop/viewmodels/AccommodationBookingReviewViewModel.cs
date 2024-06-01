using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Data.Services.Responses;
using HostedInDesktop.Utils;
using HostedInDesktop.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels;

public partial class AccommodationBookingReviewViewModel : ObservableObject, IQueryAttributable
{
    IReviewsService _reviewsService = new ReviewsService();
    IUserService _userService = new UserService();
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        booking = query["Booking"] as Booking;
        Rating = 0;
        ShowDescriptionError = false;
        ShowRatingError = false;
    }

    public Booking booking;

    [ObservableProperty]
    private String review;
    [ObservableProperty]
    private float rating;
    [ObservableProperty]
    private bool showDescriptionError;
    [ObservableProperty]
    private bool showRatingError;

    public AccommodationBookingReviewViewModel()
    {
        
    }

    private bool ValidateReview()
    {
        bool canSaved = true;
        if (String.IsNullOrEmpty(review))
        {
            ShowDescriptionError = true;    
            canSaved = false;
        }
        else
        {
            ShowDescriptionError = false;
        }
        if (Rating <= 0)
        {
            ShowRatingError = true;
            canSaved = false;
        }
        else
        {
            ShowRatingError = false;
        }
        return canSaved;

    }


    [RelayCommand]
    private async void OnRateBookingClicked()
    {
        if (ValidateReview())
        {
            try
            {
                Review review = new Review();
                review.rating = Rating;
                review.reviewDescription = Review;
                review.accommodation = booking.accommodation._id;
                review.guestUser = await GetUserGuest();
                await SaveReviewAsync(review);
                await Shell.Current.GoToAsync("..");
            }
            catch (UnauthorizedAccessException)
            {
                await Shell.Current.DisplayAlert("La sesión caducó", "La sesión caducó debido a inactividad.", "Ir a inicio de sesión");
                await Shell.Current.GoToAsync("///Login");
            }
            catch (ApiException aex)
            {
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

    private async Task<User> GetUserGuest()
    {       
        var userId = App.user._id;
        return await _userService.GetUserById(userId);  
        
    }

    private async Task SaveReviewAsync(Review review)
    {      
        try
        {
            ReviewResponse result = await _reviewsService.CreateAccommodationBookingReview(review);
            if (result != null)
            {
                await Shell.Current.DisplayAlert("Exito", result.Message, "Ok");
            }
        }
        catch (UnauthorizedAccessException)
        {
            await Shell.Current.DisplayAlert("La sesión caducó", "La sesión caducó debido a inactividad.", "Ir a inicio de sesión");
            await Shell.Current.GoToAsync("///Login");
        }
    }

    [RelayCommand]
    public async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }



}
