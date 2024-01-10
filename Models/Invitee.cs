namespace ServiceReportAPI.Models
{
    public class Invitee
    {
        public int ID { get; set; }
        public string Descripcion { get; set; }
        public int Invitados { get; set; }
        public int Confirmados { get; set; }
        public int InvitadoA { get; set; }
        public int Confirmado { get; set; }
        public bool Activo { get; set; }
    }
}
