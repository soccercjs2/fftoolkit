namespace fftoolkit.DB.Models
{
    public class DraftPick
    {
        public int DraftPickId { get; set; }
        public int DraftId { get; set; }

        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }

        public int Round { get; set; }
        public int Pick { get; set; }
    }
}
