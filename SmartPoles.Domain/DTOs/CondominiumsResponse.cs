namespace SmartPoles.Domain.DTOs
{
    public class CondominiumsResponse
    {
        public CondominiumsResponse()
        {
        }
        public List<Condominium> Condominiums { get; set; }
    }

    public class Condominium
    {
        public Condominium()
        {
        }
        public double Code { get; set; }
        public string Name { get; set; }
    }
}