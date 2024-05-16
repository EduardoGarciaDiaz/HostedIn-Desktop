using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HostedIn_Desktop.utils
{
    public class DataValidator
    {
        private const string FULL_NAME_REGEX = "^[A-Za-zÀ-ÿ'\\s]{6,250}$";

        public static bool isFullNameValid(string fullName)
        {
            bool isFullNameValid = false;

            if (!string.IsNullOrWhiteSpace(fullName))
            {
                Regex regex = new Regex(FULL_NAME_REGEX);
                isFullNameValid = regex.IsMatch(fullName.Trim());
            }

            return isFullNameValid;
        }

    }
}
