using FitnessCenter.Web.Models;

namespace FitnessCenter.Web.Models.ViewModels
{
    public class AppointmentCreateViewModel
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }

        public int TrainerId { get; set; }
        public string TrainerName { get; set; }

        public DateTime StartTime { get; set; }

        public decimal Price { get; set; }
        public int Duration { get; set; }

        public IEnumerable<Trainer> Trainers { get; set; }
    }
}
