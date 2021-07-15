using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using API_BubbleSort.Data;
using Newtonsoft.Json;
using System;

namespace API_BubbleSort.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// API route to get all employee data
        /// </summary>
        private const string apiRoute = "https://dummy.restapiexample.com/api/v1/employees";

        private List<Employee> _employeeList;

        /// <summary>
        /// Property to hold list of deserialized employees
        /// </summary>
        public List<Employee> EmployeeList
        {
            get
            {
                this.GetEmployeeData();
                return _employeeList;
            }
            set
            {
                _employeeList = value;
            }
        }

        /// <summary>
        /// Set the property with data from the API
        /// </summary>
        private void GetEmployeeData()
        {
            try
            {
                //http request to api
                using(var httpClient = new HttpClient())
                {
                    //call .result to run synchronously
                    var msg = httpClient.GetAsync(apiRoute).Result;
                    if(msg.IsSuccessStatusCode)
                    {
                        string responseString = msg.Content.ReadAsStringAsync().Result;

                        //deserialize the api data
                        API_JsonRequest req = JsonConvert.DeserializeObject<API_JsonRequest>(responseString);
                        EmployeeList = req.Employees;
                    }
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, "API call failed");
            }
        }

        /// <summary>
        /// On index page load
        /// </summary>
        public void OnGet()
        {
            ViewData["Title"] = "Employees";

            //add result to the view based on query parameter
            string sortOrder = Request.Query["sort"];

            if(!String.IsNullOrEmpty(sortOrder))
            {
                if(sortOrder.Trim().ToLower() == "name")
                {
                    //bubble sort by name
                    ViewData["listOfEmployees"] = BubbleSortByName(EmployeeList);
                }
                else if(sortOrder.Trim().ToLower() == "salary")
                {
                    //bubble sort by salary
                    ViewData["listOfEmployees"] = BubbleSortBySalary(EmployeeList);
                }
                else
                {
                    //garbage was passed in the query parameter; return default order
                    ViewData["listOfEmployees"] = EmployeeList;
                }
            }
            else
            {
                //default sort order
                ViewData["listOfEmployees"] = EmployeeList;
            }
        }

        /// <summary>
        /// Bubble sort the list by employee name
        /// </summary>
        /// <param name="initEmployeeList">Initial unsorted list</param>
        /// <returns>Sorted list</returns>
        public List<Employee> BubbleSortByName(List<Employee> initEmployeeList)
        {





            return initEmployeeList;
        }

        /// <summary>
        /// Bubble sort the list by employee salary
        /// </summary>
        /// <param name="initEmployeeList">Initial unsorted list</param>
        /// <returns>Sorted list</returns>
        public List<Employee> BubbleSortBySalary(List<Employee> initEmployeeList)
        {





            return initEmployeeList;
        }
    }
}