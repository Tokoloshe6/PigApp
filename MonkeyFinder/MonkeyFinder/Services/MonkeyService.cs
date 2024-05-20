namespace MonkeyFinder.Services;
using System.Net.Http.Json;
public class MonkeyService
{
    List<Monkey> monkeyList = new();
    HttpClient httpClient;

    public async Task<List<Monkey>> GetMonkeys()
    {
        var response = await httpClient.GetAsync("https://raw.githubusercontent.com/tokoloshe6/PigApp/main/PigInfo.json");

        if (response.IsSuccessStatusCode)
        {
            monkeyList = await response.Content.ReadFromJsonAsync(MonkeyContext.Default.ListMonkey);
        }       
        return monkeyList;
    }
    public MonkeyService()
    {
        this.httpClient = new HttpClient();
    }
}
