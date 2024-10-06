using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.Models.DTO
{
    public class VerificationDTOInput
    {
        [Required(ErrorMessage = "Verification status ID is required.")]
        public int VerificationStatusId { get; set; } // Status da verificação (e.g., Em Revisão, Verdadeira, Falsa)

        public Guid? VerifiedByUserId { get; set; } // ID do usuário que fez a verificação (se aplicável)

        public Guid RequestedByUserId { get; set; } // ID do usuário que solicitou a verificação

        public string MainLink { get; set; } // Link principal associado à verificação

        public string SecundaryLink { get; set; } // Link secundário associado à verificação

        public string Obs { get; set; } // Observações sobre a verificação

        public string PublishedChannel { get; set; } // Canal de publicação original

        public DateTime PublishedDate { get; set; } // Data de publicação original da notícia

        public DateTime? VerificationDate { get; set; } // Data em que a verificação foi realizada (opcional)

        [Required(ErrorMessage = "News ID is required.")]
        public int NewsId { get; set; } // Identificador da notícia que está sendo verificada
    }

}
