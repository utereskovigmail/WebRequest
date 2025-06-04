namespace WebRequest.Model;

public class Category
{
    public string title {get; set;}
    public string urlSlug {get; set;}
    public int priority {get; set;}
    public string image {get; set;}
    
    public override string ToString() => $"{title} - {urlSlug}, {priority} - {image}";
}
