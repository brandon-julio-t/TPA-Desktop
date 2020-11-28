using System;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Services
{
    public class CreditCardService
    {
        private readonly CreditCard _creditCard;

        public CreditCardService(CreditCard creditCard)
        {
            _creditCard = creditCard;
        }

        public bool CheckIsActivated()
        {
            using var reader = QueryBuilder
                .Table(nameof(CreditCard))
                .Join(nameof(ExpenseRequest), "CreditCard.ID", "=", "ExpenseRequest.EntityID")
                .Join(nameof(Request), "ExpenseRequest.ID", "=", "Request.ID")
                .Join(nameof(RequestStatus), "Request.RequestStatusID", "=", "RequestStatus.ID")
                .Where("CreditCard.ID", _creditCard.Id.ToString())
                .Select("RequestStatus.Name")
                .Get();

            if (!reader.Read()) throw new Exception($"Credit card with id {_creditCard.Id} doesn't exist.");

            return reader.GetString(0) == "Approved";
        }
    }
}