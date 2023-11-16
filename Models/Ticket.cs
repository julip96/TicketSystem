using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Models
{
    public class Ticket
    {
        [Key]
        public int ID { get; set; }
        public string? OwnerID { get; set; }
        public string? TicketSubject { get; set; }
        public string? TicketContent { get; set; }
        public ProblemStatus Status { get; set; }
        public DateTime CreationTime { get; set; }
    }
    public enum ProblemStatus
    {
        // change to Open, InProgress, Solved -> so other admins can see if someone is working on it
        Solved,
        Open,
        InProgress
    }
}
