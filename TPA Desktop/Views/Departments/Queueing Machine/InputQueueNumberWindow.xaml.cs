using System.Windows;
using TPA_Desktop.Core.Services;

namespace TPA_Desktop.Views.Departments.Queueing_Machine
{
    public partial class InputQueueNumberWindow
    {
        private readonly QueueService _queueService;

        public InputQueueNumberWindow(QueueService queueService)
        {
            _queueService = queueService;
            InitializeComponent();
            DataContext = new InputQueueNumberWindowViewModel();
        }

        private void HandleSubmit(object sender, RoutedEventArgs e)
        {
            var viewModel = (InputQueueNumberWindowViewModel) DataContext;
            
            _queueService.TableName = (viewModel.IsTeller ? "Teller" : "CustomerService") + "Queue";
            _queueService.QueueNumber = viewModel.QueueNumber;
            
            var queueExists = _queueService.CheckQueueNumberExists();
            var satisfactionHasBeenSubmitted = _queueService.CheckSatisfactionHasBeenSubmitted();
            var hasBeenServed = _queueService.CheckHasBeenServed();
            
            if (!queueExists || satisfactionHasBeenSubmitted || !hasBeenServed)
            {
                if (!queueExists) MessageBox.Show("Queue doesn't exists.");
                if (satisfactionHasBeenSubmitted) MessageBox.Show("Satisfaction feedback has been submitted previously.");
                if (!hasBeenServed) MessageBox.Show("This queue number hasn't been served yet.");
                return;
            }
            
            _queueService.SubmitSatisfactionFeedback();
        }
    }

    public class InputQueueNumberWindowViewModel
    {
        public long QueueNumber { get; set; }
        private bool _isTeller = true;

        public bool IsTeller
        {
            get => _isTeller;
            set => _isTeller = value;
        }

        public bool IsCustomerService
        {
            get => !_isTeller;
            set => _isTeller = !value;
        }
    }
}