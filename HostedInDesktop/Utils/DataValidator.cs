using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HostedInDesktop.Utils
{
    public class DataValidator
    {
        private const string FULL_NAME_REGEX = @"^[A-Za-zÀ-ÿ'\s]{6,250}$";
        private const string PHONE_NUMBER_REGEX = @"^\d{10}$";
        private const string OCCUPATION_REGEX = @"^[\w\s\d\S]{4,200}$";
        private const string RESIDENCE_REGEX = @"^[\w\s\d\S]{4,80}$";
        private const string PASSWORD_REGEX = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$";
        private const string EMAIL_REGEX = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        private const string ACCOMMODATION_INFORMATION_REGEX = @"^[\w\s\d\S]{5,500}$";


        public static bool IsFullNameValid(string fullName)
        {
            bool isValid = false;

            if (!string.IsNullOrEmpty(fullName))
            {
                Regex fullNameRegex = new Regex(FULL_NAME_REGEX);
                isValid = fullNameRegex.IsMatch(fullName);
            }

            return isValid;
        }

        public static bool IsPhoneNumberValid(string phoneNumber)
        {
            bool isValid = false;

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                Regex phoneNumberRegex = new Regex(PHONE_NUMBER_REGEX);
                isValid = phoneNumberRegex.IsMatch(phoneNumber);
            }

            return isValid;
        }

        public static bool IsOccupationValid(string occupation)
        {
            bool isValid = false;

            if (!string.IsNullOrEmpty(occupation))
            {
                Regex occupationRegex = new Regex(OCCUPATION_REGEX);
                isValid = occupationRegex.IsMatch(occupation);
            }

            return isValid;
        }

        public static bool IsResidenceValid(string residence)
        {
            bool isValid = false;

            if (!string.IsNullOrEmpty(residence))
            {
                Regex residenceRegex = new Regex(RESIDENCE_REGEX);
                isValid = residenceRegex.IsMatch(residence);
            }

            return isValid;
        }

        public static bool IsMailValid(string email)
        {
            bool isValid = false;

            if (!string.IsNullOrEmpty(email))
            {
                Regex emailRegex = new Regex(EMAIL_REGEX);
                isValid = emailRegex.IsMatch(email);
            }
            return isValid;
        }

        public static bool IsPasswordValid(string password)
        {
            bool isValid = false;
            if (!string.IsNullOrEmpty(password))
            {
                Regex passwordRegex = new Regex(PASSWORD_REGEX);
                isValid = passwordRegex.IsMatch(password);
            }

            return isValid;
        }

        public static bool IsAccommodationInformationValid(string accommodationInfo)
        {
            bool isValid = false;

            if (!string.IsNullOrEmpty(accommodationInfo))
            {
                Regex accommodationInfoRegex = new Regex(ACCOMMODATION_INFORMATION_REGEX);
                isValid = accommodationInfoRegex.IsMatch(accommodationInfo);
            }

            return isValid;
        }
    }
}
