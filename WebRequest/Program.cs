using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebRequest.Model;

class Program
{
    static void ViewAll()
    {
        string url = $"https://lohika.itstep.click/api/Users/all";

        var request = (HttpWebRequest)System.Net.WebRequest.Create(url);
        request.Method = "GET";

        using var response = (HttpWebResponse)request.GetResponse();
        using var reader = new StreamReader(response.GetResponseStream());
        string data = reader.ReadToEnd();

        // Assuming the API returns a list of users, not a single object
        var list = JsonConvert.DeserializeObject<List<UserItemModel>>(data);
        foreach (var item in list)
        {
            Console.WriteLine(item);
        }
    }
    static void ViewById(int id)
    {
        string url = $"https://lohika.itstep.click/api/Users/get/{id}";

        var request = (HttpWebRequest)System.Net.WebRequest.Create(url);
        request.Method = "GET";

        using var response = (HttpWebResponse) request.GetResponse();
        using var reader = new StreamReader(response.GetResponseStream());
        string data = reader.ReadToEnd();

        // Assuming the API returns a list of users, not a single object
        var l = JsonConvert.DeserializeObject<UserItemModel>(data);
        Console.WriteLine(l);
    }
    
    static UserItemModel ReadUserFromConsole()
    {
        UserItemModel user = new UserItemModel();

        Console.Write("First Name: ");
        user.FirstName = Console.ReadLine();

        Console.Write("Second Name: ");
        user.SecondName = Console.ReadLine();

        Console.Write("Email: ");
        user.Email = Console.ReadLine();

        Console.Write("Phone: ");
        user.Phone = Console.ReadLine();

        Console.Write("Photo (URL or base64 string): ");
        user.Photo = Console.ReadLine();

        Console.Write("Password: ");
        user.Password = Console.ReadLine();

        Console.Write("Confirm Password: ");
        user.ConfirmPassword = Console.ReadLine();

        return user;
    }

    static void Create()
    {
        string url = "https://lohika.itstep.click/api/Users/create";

        var request = (HttpWebRequest)System.Net.WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/json";

        while (true)
        {
            try
            {
                var user = ReadUserFromConsole();

                string jsonData = JsonConvert.SerializeObject(user);

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(jsonData);
                }

            
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    Console.WriteLine("Success: " + result);
                    break;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    string responseText = reader.ReadToEnd();
                    var errorResponse = JsonConvert.DeserializeObject<ApiErrorResponse>(responseText);

                    foreach (var error in errorResponse.Errors)
                    {
                        Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value)}");
                    }
                }
            }
        }

    
    }

    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("1) Create User; 2) View All Users 3) View user by id 4)break;");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Create();
                
                    break;
                case 2:
                    ViewAll();
                
                    break;
                case 3:
                    Console.WriteLine("Enter ID: ");
                    int id = Convert.ToInt32(Console.ReadLine());
                    ViewById(id);
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
        
        
    }
    
    public class ApiErrorResponse
    {
        public Dictionary<string, List<string>> Errors { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string TraceId { get; set; }
    }

}

