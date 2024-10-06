using Data.Candidates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models.DTO
{
    public class VerificationDTOOutput
    {
        public int Id { get; set; } // Identificador único da verificação

        public string VerificationStatus { get; set; } // Status da verificação (e.g., "Em Revisão", "Verdadeira", "Falsa")

        public string VerifiedBy { get; set; } // Nome do usuário que fez a verificação (se aplicável)

        public DateTime? VerificationDate { get; set; } // Data em que a verificação foi feita

        public string MainLink { get; set; } // Link principal associado à verificação

        public string SecundaryLink { get; set; } // Link secundário associado à verificação

        public string Obs { get; set; } // Observações feitas durante a verificação

        public string PublishedChannel { get; set; } // Canal de publicação original da notícia verificada

        public DateTime? PublishedDate { get; set; } // Data de publicação da notícia verificada
    }
}
