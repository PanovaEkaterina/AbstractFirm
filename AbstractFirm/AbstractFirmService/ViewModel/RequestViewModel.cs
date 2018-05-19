namespace AbstractFirmService.ViewModel
{
    public class RequestViewModel
    {
        public int Id { get; set; }

        public int KlientId { get; set; }

        public string KlientFIO { get; set; }

        public int PackageId { get; set; }

        public string PackageName { get; set; }

        public int? LawyerId { get; set; }

        public string LawyerName { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }

        public string Status { get; set; }

        public string DateCreate { get; set; }

        public string DateLawyer { get; set; }
    }
}
