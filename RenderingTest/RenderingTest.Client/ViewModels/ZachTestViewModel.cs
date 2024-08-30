using RenderingTest.Client.Models;

namespace RenderingTest.Client.ViewModels
{
    public class ZachTestViewModel
    {
        private readonly DataService _dataService;

        private QuoteBatchModel _quoteBatchModel;

        public QuoteBatchModel QuoteBatchModel { get => _quoteBatchModel; }

        public ZachTestViewModel(DataService dataService)
            => _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));

        public async Task InitialiseAsync(CancellationToken cancellationToken)
        {
            _quoteBatchModel = await _dataService.GetBatchAsync(string.Empty, default);
        }
    }
}
