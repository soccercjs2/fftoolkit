namespace fftoolkit.DB.Models
{
    public class DraftInvite
    {
        public int DraftInviteId { get; set; }
        public int DraftId { get; set; }
        public string EmailAddress { get; set; }
        public string Guid { get; set; }
        public bool Active { get; set; }
        public bool Accepted { get; set; }
    }
}
