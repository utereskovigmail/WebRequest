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
        string url = $"https://lohika.itstep.click/api/Categories/list";

        var request = (HttpWebRequest)System.Net.WebRequest.Create(url);
        request.Method = "GET";

        using var response = (HttpWebResponse)request.GetResponse();
        using var reader = new StreamReader(response.GetResponseStream());
        string data = reader.ReadToEnd();
        
        var list = JsonConvert.DeserializeObject<List<Category>>(data);
        foreach (var item in list)
        {
            Console.WriteLine(item);
        }
    }
    
    
    static Category ReadUserFromConsole()
    {
        Category cat = new Category();

        Console.Write("Enter Category Title: ");
        cat.title = Console.ReadLine();
        
        Console.Write("Enter Category UrlSlag: ");
        cat.urlSlug = Console.ReadLine();

        while (true)
        {
            try
            {
                Console.Write("Enter Category Priority: ");
                cat.priority = Convert.ToInt32(Console.ReadLine());
                break;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
        
        
        Console.Write("Enter Category image: ");
        cat.image = Console.ReadLine();

        return cat;
    }

    static void Create()
    {
        string url = "https://lohika.itstep.click/api/Categories/add";

        

        while (true)
        {
            var request = (HttpWebRequest)System.Net.WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
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
                    Console.WriteLine("Error: " + errorResponse.error);
                    Console.WriteLine(errorResponse.invalid);
                    
                    // foreach (var error in errorResponse.Errors)
                    // {
                    //     Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value)}");
                    // }
                    // Console.WriteLine(responseText);
                }
            }
        }

    
    }

    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("1) Create User; 2) View All Users 0)break;");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Create();
                
                    break;
                case 2:
                    ViewAll();
                
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
        public string invalid { get; set; }
        public string error { get; set; }
    }

}

