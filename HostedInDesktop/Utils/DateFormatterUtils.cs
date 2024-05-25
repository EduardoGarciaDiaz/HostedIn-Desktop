﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Utils
{
    public class DateFormatterUtils
    {
        private static readonly string NORMAL_DATE_PATTERN = "dd/MM/yyyy";
        private static readonly string MONGODB_DATE_PATTERN = "yyyy-MM-dd'T'HH:mm:ss.SSSXXX";
        private static readonly string NATURAL_DATE = "dddd d 'de' MMMM 'del' yyyy";
        private static readonly string TIME_ZONE = "UTC";

        public static string ParseDateForMongoDB(string dayFirst)
        {
            string formattedDate = dayFirst;
            DateTime? date = ParseStringToDate(dayFirst);

            if (date != null)
            {
                formattedDate = FormatMongoDate(date.Value);
            }
            else
            {
                formattedDate = null;
            }

            return formattedDate;
        }

        public static DateTime? ParseStringToDate(string date)
        {
            DateTime parsedDate;
            if (DateTime.TryParseExact(date, NORMAL_DATE_PATTERN, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                return parsedDate;
            }
            else
            {
                return null;
            }
        }

        public static string ParseMongoDateToNaturalDate(string dateString)
        {
            string formattedDate = "";
            DateTime dateFormatted;
            if (DateTime.TryParseExact(dateString, MONGODB_DATE_PATTERN, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateFormatted))
            {
                formattedDate = dateFormatted.ToString(NATURAL_DATE, new CultureInfo("es-ES"));
            }
            return formattedDate;
        }

        private static string FormatMongoDate(DateTime date)
        {
            return date.ToString(MONGODB_DATE_PATTERN);
        }

        public static string ParseMongoDateToLocal(string mongoDate)
        {
            string formattedDate = mongoDate;
            DateTime? date = ParseMongoDBStringToDate(mongoDate);

            if (date != null)
            {
                formattedDate = FormatNormalDate(date.Value);
            }
            else
            {
                formattedDate = null;
            }

            return formattedDate;
        }

        private static DateTime? ParseMongoDBStringToDate(string mongoDBDate)
        {
            DateTime parsedDate;
            if (DateTime.TryParseExact(mongoDBDate, MONGODB_DATE_PATTERN, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                return parsedDate;
            }
            else
            {
                return null;
            }
        }

        public static string FormatNormalDate(DateTime date)
        {
            return date.ToString(NORMAL_DATE_PATTERN);
        }

        public static string ParseLongToString(long date)
        {
            DateTime newDate = ParseLongToDate(date);
            return newDate.ToString(NORMAL_DATE_PATTERN);
        }

        public static DateTime ParseLongToDate(long date)
        {
            DateTime newDate = new DateTime(date);
            TimeZoneInfo utcTimeZone = TimeZoneInfo.FindSystemTimeZoneById("UTC");
            newDate = newDate.Add(utcTimeZone.BaseUtcOffset);

            return newDate;
        }

        public static long ParseDateToMillis(int year, int month, int day)
        {
            DateTime date = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
            return date.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}