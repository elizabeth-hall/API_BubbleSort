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

        private static List<Employee> _employeeList;

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
            //assume list is not sorted by name to start
            bool sorted = false;

            while(!sorted)
            {
                //temporarily switch flag to true until determined otherwise
                sorted = true;
                for(int i = 0; i < initEmployeeList.Count - 1; i++)
                {
                    //compare current element and the next
                    if(String.Compare(initEmployeeList[i].Name, initEmployeeList[i + 1].Name, StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        //swap out of order elements
                        Swap(initEmployeeList, i, i + 1);
                        //since there were out of order elements, the list is not yet sorted 
                        sorted = false;
                    }
                }
                //if sorted is false at the end of the for loop, need to iterate over the list again
            }

            return initEmployeeList;
        }

        /// <summary>
        /// Bubble sort the list by employee salary
        /// </summary>
        /// <param name="initEmployeeList">Initial unsorted list</param>
        /// <returns>Sorted list</returns>
        public List<Employee> BubbleSortBySalary(List<Employee> initEmployeeList)
        {
            //assume list is not sorted by name to start
            bool sorted = false;

            while (!sorted)
            {
                //temporarily switch flag to true until determined otherwise
                sorted = true;
                for(int i = 0; i < initEmployeeList.Count - 1; i++)
                {
                    //compare current element and the next
                    if(initEmployeeList[i].Salary > initEmployeeList[i + 1].Salary)
                    {
                        //swap out of order elements
                        Swap(initEmployeeList, i, i + 1);
                        //since there were out of order elements, the list is not yet sorted
                        sorted = false;
                    }
                }
                //if sorted is false at the end of the for loop, need to iterate over the list again
            }

            return initEmployeeList;
        }

        /// <summary>
        /// Used to swap two elements in an employee list
        /// </summary>
        /// <param name="initEmployeeList">List of employees</param>
        /// <param name="indexOne">First index that needs to be swapped</param>
        /// <param name="indexTwo">Second index that needs to be swapped</param>
        /// <returns></returns>
        public List<Employee> Swap(List<Employee> initEmployeeList, int indexOne, int indexTwo)
        {
            //temp var for employee in first index
            Employee temp = initEmployeeList[indexOne];

            //move contents of second index into first index
            initEmployeeList[indexOne] = initEmployeeList[indexTwo];
            //move item that was in first index into second index
            initEmployeeList[indexTwo] = temp;

            //return list with elements swapped
            return initEmployeeList;
        }
    }
}