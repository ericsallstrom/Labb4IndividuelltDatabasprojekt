using Labb4IndividuelltDatabasprojekt.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb4IndividuelltDatabasprojekt.Navigation
{
    // This class is responsible for generating random data to the database.
    public class DataGenerator
    {
        private static readonly Random random = new();
        private static readonly KrutångerHighSchoolDbContext context = new();
        
        // Generates a random and unique phone number.
        public string GeneratePhoneNr()
        {
            string phoneNr;

            do
            {
                // Format: +46 7X-XXX XX XX
                string countryCode = "+46";
                string areaCode = "7" + random.Next(2, 10);
                string restOfPhoneNr = GenerateRandomPhoneNrPart();
                phoneNr = $"{countryCode} {areaCode}-{restOfPhoneNr}";
            } while (!PhoneNrIsUnique(phoneNr));

            return phoneNr;
        }

        // Checks if the generated phone number is uniqe in the db.
        private bool PhoneNrIsUnique(string phoneNr)
        {
            return !context.Students.Any(s => s.PhoneNr == phoneNr);
        }

        // Generates a random part for the phone number.
        private static string GenerateRandomPhoneNrPart()
        {
            StringBuilder sbPhoneNr = new();

            for (int i = 0; i < 7; i++)
            {
                sbPhoneNr.Append(random.Next(10));
            }

            return sbPhoneNr.ToString().Insert(3, " ").Insert(6, " ");
        }

        // Generates a random email address based on the given first name and surname of a student.
        public string GenerateEmail(string firstName, string surname)
        {
            string fullName = string.Join('.', firstName, surname).ToLower();
            string username;

            if (ContainsSpecialCharacters(fullName))
            {
                username = RemoveDiacriticsAndOtherChars(fullName);
            }
            else
            {
                username = fullName;
            }

            string emailDomain = GenerateRandomEmailDomain();
            string email = string.Concat(username, emailDomain);

            return email;
        }

        // Generates a random email domain from a predefined list.
        private string GenerateRandomEmailDomain()
        {
            StringBuilder sbEmail = new();

            string[] emailOptions = { "@hotmail.com", "@gmail.com", "@outlook.com", "@yahoo.com" };

            return sbEmail.Append(emailOptions[random.Next(emailOptions.Length)]).ToString();
        }

        // Remove diacritics and other characters.
        private string RemoveDiacriticsAndOtherChars(string fullName)
        {
            string cleanedName = fullName
                .Replace("å", "a")
                .Replace("ä", "a")
                .Replace("ö", "o")
                .Replace("Å", "A")
                .Replace("Ä", "A")
                .Replace("Ö", "O")
                .Replace("'", "")
                .Replace("¨", "")
                .Replace("^", "")
                .Replace("´", "")
                .Replace("`", "");

            return cleanedName;
        }

        // Checks if the input string contains special characters.
        private bool ContainsSpecialCharacters(string input)
        {
            return input.Any(c => "åäöÅÄÖ'´`¨^".Contains(c));
        }
    }
}
