namespace Data;

public class Assignment
{
    public Assignment()
    {
        Emoji = "twa-pile-of-poo";
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool OncePerDay { get; set; }
    public int Weight { get; set; }
    public string Emoji { get; set; }
}
