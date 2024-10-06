using System;
using System.Collections.Generic;
using System.Linq;

namespace TestConsoleApp
{
    public class Province
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CastingSchedule
    {
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int ProvinceId { get; set; }
        public int Capacity { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var candidateService = new CandidateService();

            // Chamada de exemplo para testar o método ScheduleNotification
            candidateService.ScheduleNotification(1, 1);
            candidateService.ScheduleNotification(2, 1);
            candidateService.ScheduleNotification(3,1);
            candidateService.ScheduleNotification(4,1);
        }
    }

    public class CandidateService
    {
        private readonly List<CastingSchedule> _castingSchedules = new List<CastingSchedule>
        {
            new CastingSchedule { Id = 1, StartDateTime = DateTime.Parse("2024-05-15 10:00:00"), EndDateTime = DateTime.Parse("2024-05-15 12:00:00"), ProvinceId = 1, Capacity = 1},
            new CastingSchedule { Id = 2, StartDateTime = DateTime.Parse("2024-05-15 15:00:00"), EndDateTime = DateTime.Parse("2024-05-15 20:00:00"), ProvinceId = 1, Capacity = 3},
            new CastingSchedule { Id = 3, StartDateTime = DateTime.Parse("2024-05-15 14:00:00"), EndDateTime = DateTime.Parse("2024-05-15 16:00:00"), ProvinceId = 2, Capacity = 5 }
            // Adicione mais horários de casting conforme necessário para testar
        };

        private readonly List<Province> _provinces = new List<Province>
        {
            new Province { Id = 1, Name = "Luanda" },
            new Province { Id = 2, Name = "Huambo" }
            // Adicione mais províncias conforme necessário para testar
        };

        public void ScheduleNotification(int candidateId, int provinceId)
        {
            var castingSchedule = _castingSchedules
                .Where(x => x.ProvinceId == provinceId && x.Capacity > 0)
                .OrderBy(x => x.StartDateTime)
                .FirstOrDefault();

            if (castingSchedule == null)
            {
                // Lógica para lidar com o caso em que não há horário de casting disponível para a província
                Console.WriteLine("Não há horário de casting disponível para a província selecionada.");
                return;
            }

            var province = _provinces.FirstOrDefault(x => x.Id == provinceId);

            var dateTime = castingSchedule.StartDateTime;

            // Simulando o envio de notificação
            SendNotification(candidateId, province.Name, dateTime, castingSchedule.Id);

            // Reduzindo a capacidade do horário de casting
            castingSchedule.Capacity--;

            //Console.WriteLine($"Notificação enviada com sucesso para o candidato {candidateId} em {dateTime}.\n");
        }

        private void SendNotification(int candidateId, string provinceName, DateTime dateTime,  int horarioId)
        {
            // Lógica para enviar a notificação
            Console.WriteLine($"Notificação enviada para o candidato {candidateId} em {provinceName} no horario {horarioId} em {dateTime}.");
        }
    }
}
