using RenderingTest.Client.Models;

namespace RenderingTest.Client.ViewModels
{
    public class ChargeSectionViewModel
    {
        private readonly Guid _associatedRouteId;
        private readonly DataService _dataService;

        public Guid AssociatedRouteId { get => _associatedRouteId; }

        public ICollection<ChargeViewModel> Charges { get; set; } = new List<ChargeViewModel>();

        public ChargeSectionViewModel(DataService dataService, Guid associatedRouteId)
        {
            _associatedRouteId = associatedRouteId;
            _dataService = dataService;
        }

        public async Task InitialiseAsync(string batchReference, CancellationToken cancellationToken)
        {
            Charges = await _dataService.GetChargesAsync(batchReference, AssociatedRouteId, cancellationToken);
        }

        public DesignerSectionModel AddCharge(string name)
        {
            var charge = new ChargeViewModel() { AssociatedRouteId = AssociatedRouteId, Id = Guid.NewGuid(), Name = name };
            Charges.Add(charge);
            return new DesignerSectionModel() { Id = charge.Id, DisplayInfo = charge.Name };
        }

        public DesignerSectionModel RemoveCharge(ChargeViewModel charge)
        {
            Charges.Remove(charge);
            return new DesignerSectionModel() { Id = charge.Id, DisplayInfo = charge.Name };
        }
    }
}
