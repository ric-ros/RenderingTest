using RenderingTest.Client.Models;
using RenderingTest.Client.ViewModels;

namespace RenderingTest.Client
{
    public class DataService
    {
        private QuoteBatch _quoteBatch;

        public DataService()
        {
            var route = new RouteViewModel() { Currency = "c", Destination = "dest", Id = Guid.NewGuid(), Origin = "or" };
            _quoteBatch = new();
            _quoteBatch.Routes.Add(route);
            for (int i = 0; i < 10; i++)
            {
                _quoteBatch.Charges.Add(new() { AssociatedRouteId = route.Id, Cost = i, Id = Guid.NewGuid(), MarkUp = i, Name = $"charge {i}" });
            }
        }

        public Task SetBatchAsync(ICollection<ChargeViewModel> charges, ICollection<RouteViewModel> routes, CancellationToken cancellationToken)
        {
            _quoteBatch.Charges = charges;
            _quoteBatch.Routes = routes;
            return Task.CompletedTask;
        }
        public Task<QuoteBatchModel> GetBatchAsync(string reference, CancellationToken cancellationToken)
            => Task.FromResult(
                new QuoteBatchModel()
                {
                    Reference = reference,
                    Charges = _quoteBatch.Charges.Select(x => new DesignerSectionModel() { Id = x.Id, DisplayInfo = $"{x.Name} = {(x.Cost + x.MarkUp).ToString("C")}" }).ToList(),
                    Routes = _quoteBatch.Routes.Select(x => new DesignerSectionModel() { Id = x.Id, DisplayInfo = $"{x.Origin} -> {x.Destination}" }).ToList(),
                });

        public Task<ICollection<ChargeViewModel>> GetChargesAsync(string reference, Guid routeId, CancellationToken cancellationToken)
            => Task.FromResult((ICollection<ChargeViewModel>)_quoteBatch.Charges.Where(c => c.AssociatedRouteId == routeId).ToList()); //HACK

        public Task<ICollection<RouteViewModel>> GetRoutesAsync(string reference, CancellationToken cancellationToken)
            => Task.FromResult((ICollection<RouteViewModel>)_quoteBatch.Routes.ToList()); //HACK

        public Task AddChargeAsync(ChargeViewModel charge, CancellationToken cancellationToken)
        {
            _quoteBatch.Charges.Add(charge);
            return Task.CompletedTask;
        }

        public Task AddRouteAsync(RouteViewModel Route, CancellationToken cancellationToken)
        {
            _quoteBatch.Routes.Add(Route);
            return Task.CompletedTask;
        }

        private class QuoteBatch
        {
            public string Reference { get; set; } = "TGLAU0000123";

            public ICollection<ChargeViewModel> Charges { get; set; } = new List<ChargeViewModel>();

            public ICollection<RouteViewModel> Routes { get; set; } = new List<RouteViewModel>();
        }
    }

}
