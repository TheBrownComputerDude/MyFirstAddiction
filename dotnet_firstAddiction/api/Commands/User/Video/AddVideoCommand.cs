namespace api.Commands
{
    using MediatR;

    public class AddVideoCommand : IRequest<Dtos.Video>
    {
        public string Location { get; set; }
    }
}