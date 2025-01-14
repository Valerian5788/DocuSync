using DocuSync.Domain.Entities.Enums;
using DocuSync.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Application.Common.Models
{
    public class DocumentTypeDto
    {
        public string Name { get; set; } = string.Empty;
        public DocumentFrequency Frequency { get; set; }

        public static DocumentTypeDto FromEntity(DocumentType entity) =>
            new DocumentTypeDto
            {
                Name = entity.Name,
                Frequency = entity.Frequency
            };
    }
}
