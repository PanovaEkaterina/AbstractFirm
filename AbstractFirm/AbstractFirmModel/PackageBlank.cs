namespace AbstractFirmModel
{
    public class PackageBlank
    {
        public int Id { get; set; }

        public int PackageId { get; set; }

        public int BlankId { get; set; }

        public int Count { get; set; }

        public virtual Package Package { get; set; }

        public virtual Blank Blank { get; set; }
    }
}
