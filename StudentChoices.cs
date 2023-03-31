using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using SocsFeeds.objects; // Import any necessary namespaces

namespace SocsFeeds
{
    public class StudentChoices
    {
        // Define a static method that retrieves student club data asynchronously
        // Returns a tuple of a List<Choices> object and a string error message
        public static async Task<(List<Choices>, string)> StudentClubs(string academicYear, string term, string category, int schoolID, string apiKey)
        {
            // Construct the URL for the API endpoint using the input parameters
            string socsUrl = $"https://www.socscms.com/socs/xml/proactivityClubParticipationreport.ashx?ID={schoolID}&key={apiKey}";

            // If a category is specified, add it to the URL as a query parameter
            if (!string.IsNullOrEmpty(category))
            {
                socsUrl += $"&Term={term}&AcademicYear={academicYear}&Category=LIKE:{category}";
            }
            else // Otherwise, omit the category query parameter
            {
                socsUrl += $"&Term={term}&AcademicYear={academicYear}";
            }

            // Create an HttpClient object to send the HTTP request
            var client = new HttpClient();
            // Send an HTTP GET request to the API endpoint and await the response
            var response = await client.GetAsync(socsUrl);

            // If the response indicates failure, return an error message
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = $"Error retrieving student club data. Status code: {response.StatusCode}";
                return (null, errorMessage);
            }

            // Otherwise, read the response into a string variable
            var xml = await response.Content.ReadAsStringAsync();
            // Parse the XML into an XDocument object
            var xmlDoc = XDocument.Parse(xml);

            // Extract the relevant data from the XML and construct Choices objects
            var choicesNodes = xmlDoc.Descendants("pupil");
            var choices = new List<Choices>();
            foreach (var node in choicesNodes)
            {
                var choice = new objects.Choices // Create a new Choices object
                {
                    // Populate its properties with data from the XML
                    ActivityName = node.Element("Activity")?.Value,
                    Term = node.Element("Term")?.Value,
                    PupilID = node.Element("PupilID")?.Value,
                    AcademicYear = node.Element("Year")?.Value,
                    Gender = node.Element("Gender")?.Value,
                    DayTime = node.Element("DayTime")?.Value,
                    YearGroups = node.Element("YearGroups")?.Value
                };
                // Add the new Choices object to the list of choices
                choices.Add(choice);
            }
            // Return the list of choices and a null error message
            return (choices, null);
        }

    }
}