using System.Collections.ObjectModel;
using System.Windows;
using TPA_Desktop.Core.Commands;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;
using TPA_Desktop.Core.Repositories;

namespace TPA_Desktop.Views.Departments.Customer_Service.CreditCards
{
    public partial class RequestCreditCardWindow : ICustomerAccountStore
    {
        private readonly RequestCreditCardWindowViewModel _viewModel = new RequestCreditCardWindowViewModel();

        public RequestCreditCardWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void HandleSubmit(object sender, RoutedEventArgs e)
        {
            new CreateCreditCardRequestCommand(this, _viewModel).Execute();
        }

        public Customer? Customer { get; set; }
        public Account? Account { get; set; }
    }

    public class RequestCreditCardWindowViewModel
    {
        public ObservableCollection<DocumentType> DocumentTypes { get; set; }
        public ObservableCollection<CreditCardCompany> CreditCardCompanies { get; set; }
        public DocumentType? SelectedDocumentType { get; set; }
        public CreditCardCompany? SelectedCreditCardCompany { get; set; }
        public Document Document { get; set; } = new Document();

        public RequestCreditCardWindowViewModel()
        {
            ReadOnlyRepository<DocumentType> documentTypeRepository = new DocumentTypeRepository();
            ReadOnlyRepository<CreditCardCompany> creditCardCompanyRepository = new CreditCardCompanyRepository();

            DocumentTypes = new ObservableCollection<DocumentType>(documentTypeRepository.FindAll());
            CreditCardCompanies = new ObservableCollection<CreditCardCompany>(creditCardCompanyRepository.FindAll());
        }
    }
}