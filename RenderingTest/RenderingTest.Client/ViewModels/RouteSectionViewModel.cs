namespace RenderingTest.Client.ViewModels
{
    public class RouteSectionViewModel
    {
        private readonly DataService _dataService;

        public ICollection<RouteViewModel> Routes { get; set; } = new List<RouteViewModel>();

        public RouteSectionViewModel(DataService dataService)
        {
            _dataService = dataService;
        }

        public async Task InitialiseAsync(string batchReference, CancellationToken cancellationToken)
        {
            Routes = await _dataService.GetRoutesAsync(batchReference, cancellationToken);
        }

        public void AddRoute(RouteViewModel routeViewModel)
        {
            Routes.Add(routeViewModel);
        }
    }
}
