﻿using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Models
{
    public class CustomerSatisfaction : BaseModel
    {
        public CustomerSatisfaction(QrCode qrCode)
        {
            QrCode = qrCode;
            Description = "";
        }

        public DateTime? SubmittedAt { get; set; }
        public QrCode QrCode { get; set; }
        public short Rating { get; set; }
        public string Description { get; set; }

        public override bool Save()
        {
            return QueryBuilder
                .Table(nameof(CustomerSatisfaction))
                .Insert(
                    new Dictionary<string, object>
                    {
                        {"ID", Id},
                        {"QRCodeID", QrCode.Id},
                        {"Rating", Rating},
                        {"Description", Description},
                        {"SubmittedAt", DateTime.Now}
                    }
                );
        }

        public override bool Delete()
        {
            return false;
        }
    }
}