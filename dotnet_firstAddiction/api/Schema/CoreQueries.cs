using api.Commands;
using api.Dtos;
using GraphQL.Types;
using MediatR;

namespace api.Schema
{
    public class CoreQueries : ObjectGraphType
    {
        public CoreQueries(IMediator mediator)
        {
        }
    }
}