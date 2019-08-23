namespace fftoolkit.DB.Models
{
    public class DraftParticipant
    {
        public int DraftParticipantId { get; set; }
        public int DraftId { get; set; }

        public int? UserId { get; set; }
        public virtual User User { get; set; }

        public int DraftPosition { get; set; }
        public string Name { get; set; }
    }
}