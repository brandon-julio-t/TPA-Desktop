using System.Collections.ObjectModel;
using System.Windows;
using TPA_Desktop.Core.Commands;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Repositories;

namespace TPA_Desktop.Views.Departments.Customer_Service.CreditCards
{
    public partial class RequestCreditCardWindow
    {
        private readonly RequestCreditCardWindowViewModel _viewModel = new RequestCreditCardWindowViewModel();

        public RequestCreditCardWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void HandleSubmit(object sender, RoutedEventArgs e)
        {
            new CreateCreditCardRequestCommand(_viewModel.Document).Execute();
        }
    }

    public class RequestCreditCardWindowViewModel
    {
        private readonly DocumentTypeRepository _documentTypeRepository = new DocumentTypeRepository();
        private readonly CreditCardCompanyRepository _creditCardCompanyRepository = new CreditCardCompanyRepository();
        public ObservableCollection<DocumentType> DocumentTypes { get; set; }
        public ObservableCollection<CreditCardCompany> CreditCardCompanies { get; set; }
        public DocumentType? SelectedDocumentType { get; set; }
        public DocumentType? SelectedCreditCardCompany { get; set; }
        public Document Document { get; set; } = new Document();

        public RequestCreditCardWindowViewModel()
        {
            DocumentTypes = new ObservableCollection<DocumentType>(_documentTypeRepository.FindAll());
            CreditCardCompanies = new ObservableCollection<CreditCardCompany>(_creditCardCompanyRepository.FindAll());
        }
    }
}