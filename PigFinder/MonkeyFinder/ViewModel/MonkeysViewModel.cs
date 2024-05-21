namespace MonkeyFinder.ViewModel;
using MonkeyFinder.Services;

public partial class MonkeysViewModel : BaseViewModel
{
    public ObservableCollection<Monkey> Monkeys { get; } = new();
    MonkeyService monkeyService;
    [ObservableProperty]
    bool isRefreshing;

    [RelayCommand]
    async Task GetMonkeysAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var monkeys = await monkeyService.GetMonkeys();

            if(connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("No Connectivity!",$"Please cgecj your internet and try again","OK");
                return;
            }

            if (Monkeys.Count != 0)
                Monkeys.Clear();

            foreach (var monkey in monkeys)
                Monkeys.Add(monkey);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get monkeys: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    async Task GoToDetails(Monkey monkey)
    {
        if (monkey == null)
            return;

        await Shell.Current.GoToAsync(nameof(DetailsPage), true, new Dictionary<string, object>
        {
            {"Monkey", monkey }
        });
    }
    [RelayCommand]
    async Task GetClosestMonkey()
    {
        if (IsBusy || Monkeys.Count == 0)
            return;

        try
        {
            var location = await geolocation.GetLastKnownLocationAsync();
            if(location == null)
            {
                location = await geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });
            }
            var first = Monkeys.OrderBy(m => location.CalculateDistance(new Location(m.Lattitude, m.Longitude), DistanceUnits.Miles))
                .FirstOrDefault();

            await Shell.Current.DisplayAlert("", first.Name + " " + first.Location, "OK");
        }

        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to query location: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
    }
    IConnectivity connectivity;
    IGeolocation geolocation;
    public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
    {
        Title = "Pig Finder";
        this.monkeyService = monkeyService;
        this.connectivity = connectivity;
        this.geolocation = geolocation;
    }
}
