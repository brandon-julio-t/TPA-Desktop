using System;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Core.Commands
{
    public class CreateCreditCardRequestCommand : ICommand
    {
        private readonly Document _document;

        public CreateCreditCardRequestCommand(Document document)
        {
            _document = document;
        }

        public void Execute()
        {
            /*
             * TODO:
             * create credit card,
             * create expense request,
             * add to finance team notification
             */        
        }
    }
}