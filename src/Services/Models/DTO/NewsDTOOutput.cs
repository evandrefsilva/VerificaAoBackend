using Data.NewsVerfication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models.DTO
{
    public class NewsDTOOutput
    {
        public int Id { get; set; } // Identificador único da notícia

        public string Title { get; set; } // Título da notícia

        public string Resume { get; set; } // Resumo da notícia

        public string Text { get; set; } // Texto completo da notícia

        public DateTime PublicationDate { get; set; } // Data de publicação

        public bool IsPublished { get; set; } // Status de publicação

        public int Like { get; set; } // Número de likes

        public int UnLike { get; set; } // Número de dislikes

        public int ReadTime { get; set; } // Tempo de leitura estimado

        public string PublishedBy { get; set; } // Nome do usuário que publicou a notícia

        public string TagName { get; set; } // Nome da tag associada

        public VerificationDTOOutput Verification { get; set; } // Detalhes da verificação associada à notícia
    }

}
