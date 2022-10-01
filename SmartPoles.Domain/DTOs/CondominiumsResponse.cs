namespace SmartPoles.Domain.DTOs
{
    public class CondominiumsResponse
    {
        public IEnumerable<Condominium> Condominiums { get; set; }
    }

    public class Condominium
    {
        public double Code { get; set; }
        public string Name { get; set; }
    }
}