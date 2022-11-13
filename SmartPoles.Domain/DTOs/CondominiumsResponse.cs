namespace SmartPoles.Domain.DTOs
{
    public class CondominiumsResponse
    {
        public CondominiumsResponse()
        {
        }

        public CondominiumsResponse(List<Condominium> condominiums)
        {
            Condominiums = condominiums;
        }
        public List<Condominium> Condominiums { get; set; } = new List<Condominium>();
    }

    public class Condominium
    {
        public Condominium()
        {
        }
        public int Code { get; set; }
        public string Name { get; set; }
    }
}