namespace APBD25_CW8;

public class ReadFromFiele
{
    public static string FileRead(string path)
    {
        return File.ReadAllText(path);
    }
}