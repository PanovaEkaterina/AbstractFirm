namespace AbstractFirmModel
{
    public class ArchiveBlank
    {
        public int Id { get; set; }

        public int ArchiveId { get; set; }

        public int BlankId { get; set; }

        public int Count { get; set; }

        public virtual Archive Archive { get; set; }

        public virtual Blank Blank { get; set; }
    }
}
