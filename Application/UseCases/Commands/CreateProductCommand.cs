using MediatR;

namespace Application.UseCases.Commands
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Review { get; set; }
    }
}
