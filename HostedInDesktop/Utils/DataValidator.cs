﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HostedInDesktop.Utils
{
    public class DataValidator
    {
        private const string FULL_NAME_REGEX = @"^[A-Za-zÀ-ÿ'\s]{6,250}$";
        private const string PHONE_NUMBER_REGEX = @"^\d{10}$";
        private const string OCCUPATION_REGEX = @"^[\w\s\d\S]{4,500}$";
        private const string RESIDENCE_REGEX = @"^[\w\s\d\S]{4,50}$";

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
    }
}
