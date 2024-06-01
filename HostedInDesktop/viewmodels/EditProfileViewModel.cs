using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Google.Protobuf;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels
{
    public partial class EditProfileViewModel : ObservableObject
    {
        private readonly IUserService _userService = new UserService();
        private readonly MultimediaServiceImpl _multimediaService = new MultimediaServiceImpl();
        private const int MAX_MB_SIZE = 1;
        private string _uriSelectedImage = null;
        private User _user = App.user;

        [ObservableProperty]
        private User userData;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string fullname;

        [ObservableProperty]
        private ImageSource profilePhoto;

        public EditProfileViewModel()
        {
            UserData = _user;
            LoadUserData();
        }

        [RelayCommand]
        public async void SelectPhoto()
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Seleccione una imagen"
            });

            if (result != null)
            {
                this._uriSelectedImage = result.FullPath;
                ProfilePhoto = ImageSource.FromFile(_uriSelectedImage);
            }
        }

        [RelayCommand]
        public async void EditProfile()
        {
            if (IsLoading)
            {
                return;
            }

            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    IsLoading = true;

                    if (IsUserDataValid())
                    {
                        User user = await _userService.EditAccount(_user._id, CreateUser());
                        string userDetails = JsonConvert.SerializeObject(user);
                        Fullname = user.fullName;
                        await UploadProfilePhoto();
                        await Shell.Current.DisplayAlert("Éxito", "Se ha actualizado tu información", "Ok");
                    }
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
                finally
                {
                    IsLoading = false;
                }
            }
        }

        private User CreateUser()
        {
            User editedUser = new User();
            string userId = UserData._id;
            string birthdateMongoDb = DateFormatterUtils.ParseDateForMongoDB(UserData.birthDate.Trim());

            editedUser._id = userId;
            editedUser.email = UserData.email.Trim();
            editedUser.fullName = UserData.fullName.Trim();
            if (birthdateMongoDb != null)
            {
                editedUser.birthDate = birthdateMongoDb;
            }
            editedUser.password = null;
            editedUser.phoneNumber = UserData.phoneNumber.Trim();
            editedUser.residence = UserData.residence.Trim();
            editedUser.occupation = UserData.occupation.Trim();

            return editedUser;
        }

        public void LoadUserData()
        {
            User userToEdit = App.user;
            if (userToEdit != null)
            {
                Fullname = userToEdit.fullName;
                UserData.fullName = userToEdit.fullName;
                UserData.birthDate = userToEdit.birthDate;
                UserData.phoneNumber = userToEdit.phoneNumber;
                UserData.email = userToEdit.email;
                UserData.occupation = userToEdit.occupation;
                UserData.residence = userToEdit.residence;

                if (userToEdit.profilePhoto != null && userToEdit.profilePhoto.data != null && userToEdit.profilePhoto.data.Length > 0)
                {
                    byte[] imageData = userToEdit.profilePhoto.data;
                    ProfilePhoto = ImageSource.FromStream(() => new MemoryStream(imageData));
                }
                else
                {
                    ProfilePhoto = "ic_user.png";
                }
            }
        }

        private bool IsUserDataValid()
        {
            bool isUserDataValid = true;

            if (!IsPhotoValid())
            {
                isUserDataValid = false;
            }
            else if (!IsFullNameValid())
            {
                isUserDataValid = false;
            }
            else if (!IsBirthdateValid())
            {
                isUserDataValid = false;
                Shell.Current.DisplayAlert("Lo sentimos...", $"Debes ser mayor de edad", "Ok");
            }
            else if (!IsPhoneNumberValid())
            {
                isUserDataValid = false;
            }
            else if (!IsOccupationValid())
            {
                isUserDataValid = false;
            }
            else if (!IsResidenceValid())
            {
                isUserDataValid = false;
            }

            return isUserDataValid;
        }

        private bool IsPhotoValid()
        {
            bool isValid = true;
            if (this._uriSelectedImage != null && !IsPhotoSizeValid(_uriSelectedImage))
            {
                isValid = false;

                Shell.Current.DisplayAlert("Foto no válida", $"La imagen debe ser pesar menos o igual a {MAX_MB_SIZE}MB", "Ok");
            }

            return isValid;
        }

        private bool IsPhotoSizeValid(string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                long fileSizeInBytes = fileInfo.Length;
                long fileSizeInMegabytes = fileSizeInBytes / (1024 * 1024);

                return fileSizeInMegabytes <= MAX_MB_SIZE;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el tamaño del archivo: {ex.Message}");
                return false;
            }
        }

        private bool IsFullNameValid()
        {
            bool isFullNameValid = true;
            string fullName = UserData.fullName.Trim();

            if (string.IsNullOrEmpty(fullName))
            {
                Shell.Current.DisplayAlert("Nombre obligatorio", "Debes ingresar tu nombre completo", " s", "Ok"); isFullNameValid = false;
            }
            else if (!DataValidator.IsFullNameValid(fullName))
            {
                Shell.Current.DisplayAlert("Nombre completo no válido", "Tu nombre completo debe tener al menos 6 caracteres, solo letras mayúsculas, minúsculas y acentos", "Ok");
                 isFullNameValid = false;
            }

            return isFullNameValid;
        }

        private bool IsBirthdateValid()
        {
            bool isBirthdateValid = true;

            string birthdateStr = UserData.birthDate.ToString();

            if (string.IsNullOrEmpty(birthdateStr))
            {
                Shell.Current.DisplayAlert("Fecha de nacimiento obligatoria", "Debes ingresar tu fecha de nacimiento", "Ok");
                isBirthdateValid = false;
            }
            else
            {
                try
                {
                    string[] dateParts = birthdateStr.Split(' ')[0].Split('/');
                    string formattedDate = $"{dateParts[0]}/{dateParts[1]}/{dateParts[2]}";

                    DateTime birthdate = (DateTime)DateFormatterUtils.ParseStringToDate(birthdateStr);

                    DateTime minAgeDate = DateTime.Today.AddYears(-18);

                    isBirthdateValid = birthdate <= minAgeDate;
                }
                catch (FormatException e)
                {
                    Shell.Current.DisplayAlert("Fecha no válida", "El formato de la fecha no es válido", "Ok");
                    isBirthdateValid = false;
                }
            }

            return isBirthdateValid;
        }

        private bool IsPhoneNumberValid()
        {
            bool isPhoneNumberValid = true;
            string phoneNumber = UserData.phoneNumber.Trim();

            if (string.IsNullOrEmpty(phoneNumber))
            {
                Shell.Current.DisplayAlert("Número de teléfono obligatorio", "Debes ingresar tu número de teléfono", "Ok");
                isPhoneNumberValid = false;
            }
            else if (!DataValidator.IsPhoneNumberValid(phoneNumber))
            {
                Shell.Current.DisplayAlert("Número de teléfono no válido", "Por favor, ingresa un número de teléfono válido (10 números)", "Ok");
                isPhoneNumberValid = false;
            }

            return isPhoneNumberValid;
        }

        private bool IsOccupationValid()
        {
            bool isOccupationValid = true;
            string occupation = UserData.occupation.Trim();

            if (!string.IsNullOrEmpty(occupation) && !DataValidator.IsOccupationValid(occupation))
            {
                Shell.Current.DisplayAlert("Ocupación no válida", "Por favor, ingresa una ocupación válida (al menos 4 caracteres)", "Ok");
                isOccupationValid = false;
            }

            return isOccupationValid;
        }

        private bool IsResidenceValid()
        {
            bool isResidenceValid = true;
            string residence = UserData.residence.Trim();

            if (!string.IsNullOrEmpty(residence) && !DataValidator.IsResidenceValid(residence))
            {
                Shell.Current.DisplayAlert("Estancia no válida", "Por favor, ingresa una estancia válida (de 4 a 50 caracteres)", "Ok");
                isResidenceValid = false;
            }

            return isResidenceValid;
        }

        private async Task UploadProfilePhoto()
        {
            if (_uriSelectedImage != null)
            {
                ByteString[] bytesMultimedia = ImageHelper.ConvertPathToByteString(_uriSelectedImage);

                if (bytesMultimedia != null)
                {
                    try
                    {
                        var response = await _multimediaService.UploadProfilePhoto(App.user._id, bytesMultimedia);
                        if (!response.Contains("Upload successful"))
                        {
                            await Shell.Current.DisplayAlert("Ups...", "No se pudo cargar un archivo multimedia, pero puedes modificarlo después", "Ok");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }




    }

    public delegate void DataSavedEventHandler(object sender, DataSavedEventArgs e);

    public class DataSavedEventArgs : EventArgs
    {
        public string UpdatedFullName { get; }
        public byte[] ImageData { get; }

        public DataSavedEventArgs(string updatedFullName, byte[] imageData)
        {
            UpdatedFullName = updatedFullName;
            ImageData = imageData;
        }
    }

}
