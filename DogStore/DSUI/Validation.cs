using Serilog;
using System;
using System.Text.RegularExpressions;

namespace DSUI
{
    public class Validation : IValidation
    {
        /// <summary>
        /// Makes sure address is in valid form
        /// </summary>
        /// <param name="message">message to prompt user</param>
        /// <returns>correctly formatted address string</returns>
        public string ValidateAddress(string message)
        {
            string enteredString = "";
            bool repeat = true;
            do
            {
                Console.WriteLine(message);
                try
                {
                    enteredString = Console.ReadLine();
                    if (Regex.IsMatch(enteredString, @"^[\w\s]+,\s\w{2}$"))
                    {
                        repeat = false;
                    }
                    else
                    {
                        Console.WriteLine("Incorrect Format, use Cityname, ST");
                    }
                }
                catch (Exception e)
                {
                    Log.Debug(e.Message);
                    Console.WriteLine("Not a valid input, please try again");
                }
            } while (repeat);
            return enteredString;
        }

        /// <summary>
        /// Makes sure double entered for the dog is valid
        /// </summary>
        /// <param name="message">message to prompt user</param>
        /// <returns>double to be set as price</returns>
        public double ValidateDouble(string message)
        {
            double enteredDouble = 0;
            bool repeat = true;
            do
            {
                Console.WriteLine(message);
                try
                {
                    enteredDouble = Double.Parse(Console.ReadLine());
                    if (enteredDouble > 0)
                    {
                        repeat = false;
                    }
                    else
                    {
                        Console.WriteLine("Must be positive");
                    }
                }
                catch (Exception e)
                {
                    Log.Debug(e.Message);
                    Console.WriteLine("Not a valid input, please try again");
                }
            } while (repeat);
            return enteredDouble;
        }

        /// <summary>
        /// Makes sure int for quantity is a valid int
        /// </summary>
        /// <param name="message">prompt for user to enter int</param>
        /// <returns>valid int</returns>
        public int ValidateInt(string message)
        {
            int enteredInt = 0;
            bool repeat = true;
            do
            {
                Console.WriteLine(message);
                try
                {
                    enteredInt = Int32.Parse(Console.ReadLine());
                    if (enteredInt > 0)
                    {
                        repeat = false;
                    }
                    else
                    {
                        Console.WriteLine("Must be positive");
                    }
                }
                catch (Exception e)
                {
                    Log.Debug(e.Message);
                    Console.WriteLine("Not a valid input, please try again");
                }
            } while (repeat);
            return enteredInt;
        }

        /// <summary>
        /// Makes sure user is inputting a valid string
        /// </summary>
        /// <param name="message">prompt for user to enter in a string</param>
        /// <returns>valid string</returns>
        public string ValidateString(string message)
        {
            string entererdString = "";
            bool repeat = true;
            do
            {
                Console.WriteLine(message);
                try
                {
                    entererdString = Console.ReadLine();
                    if (String.IsNullOrWhiteSpace(entererdString))
                    {
                        Console.WriteLine("Please put in a valid string");
                    }
                    else
                    {
                        repeat = false;
                    }
                }
                catch (Exception e)
                {
                    Log.Debug(e.Message);
                    Console.WriteLine("Something went wrong, try again");
                }
            } while (repeat);
            return entererdString;
        }

        /// <summary>
        /// Makes sure user is entering a valid name
        /// </summary>
        /// <param name="message">message to prompt user to enter in name</param>
        /// <returns>valid name</returns>
        public string ValidateName(string message)
        {
            string enteredString = "";
            bool repeat = true;
            do
            {
                Console.WriteLine(message);
                try
                {
                    enteredString = Console.ReadLine();
                    if (Regex.IsMatch(enteredString, @"^[a-zA-Z]{2,}\s[a-zA-Z]{1,}$"))
                    {
                        repeat = false;
                    }
                    else
                    {
                        Console.WriteLine("Incorrect Format, use Firstname Lastname");
                    }
                }
                catch (Exception e)
                {
                    Log.Debug(e.Message);
                    Console.WriteLine("Not a valid input, please try again");
                }
            } while (repeat);
            return enteredString;
        }

        /// <summary>
        /// Method that makes sure user is entering in a valid phone number
        /// </summary>
        /// <param name="message">prompt for user to enter in phone number</param>
        /// <returns>long value representing user's phone number</returns>
        public long ValidatePhone(string message)
        {
            long phoneNumber = 0;
            string enteredString;
            bool repeat = true;
            do
            {
                Console.WriteLine(message);
                try
                {
                    enteredString = Console.ReadLine();
                    if (Regex.IsMatch(enteredString, @"^[0-9]{10}$"))
                    {
                        repeat = false;
                        phoneNumber = Int64.Parse(enteredString);
                    }
                    else
                    {
                        Console.WriteLine("Incorrect Format, use 1234567890");
                    }
                }
                catch (Exception e)
                {
                    Log.Debug(e.Message);
                    Console.WriteLine("Not a valid input, please try again");
                }
            } while (repeat);
            return phoneNumber;
        }

        /// <summary>
        /// makes sure user is entering in a valid character either m or f for gender
        /// </summary>
        /// <param name="message"> prompt for the user to enter in gender</param>
        /// <returns>char representing gender</returns>
        public char ValidateGender(string message)
        {
            char gender = 'm';
            string enteredString;
            bool repeat = true;
            do
            {
                Console.WriteLine(message);
                try
                {
                    enteredString = Console.ReadLine();
                    if ((enteredString.ToCharArray()[0] == 'm') || (enteredString.ToCharArray()[0] == 'f'))
                    {
                        repeat = false;
                        gender = enteredString.ToCharArray()[0];
                    }
                    else
                    {
                        Console.WriteLine("Please enter either m or f");
                    }
                }
                catch (Exception e)
                {
                    Log.Debug(e.Message);
                    Console.WriteLine("Not a valid input, please try again");
                }
            } while (repeat);
            return gender;
        }

        /// <summary>
        /// Allows user to choose which query they want to preform on the order histories
        /// </summary>
        /// <param name="message">prompt for user to enter in integer</param>
        /// <returns>int corresponding to selected option</returns>
        public int ValidateOrderSearchOptions(string message)
        {
            int enteredInt = 0;
            bool repeat = true;
            do
            {
                Console.WriteLine(message);
                try
                {
                    Console.WriteLine("[1] to sort by Date (Oldest to Newest)");
                    Console.WriteLine("[2] to sort by Date (Newest to Oldest)");
                    Console.WriteLine("[3] to sort by Cost (Least to Most Expensive)");
                    Console.WriteLine("[4] to sort by Cost (Most to Least Expensive)");
                    enteredInt = Int32.Parse(Console.ReadLine());
                    if (enteredInt == 1 || enteredInt == 2 || enteredInt == 3 || enteredInt == 4) repeat = false;
                    else Console.WriteLine("Not a valid number, try again");
                }
                catch (Exception e)
                {
                    Log.Debug(e.Message);
                    Console.WriteLine("Not a valid input, please try again");
                }
            } while (repeat);
            return enteredInt;
        }
    }
}