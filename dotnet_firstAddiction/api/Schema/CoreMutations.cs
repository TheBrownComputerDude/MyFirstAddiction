namespace api.Schema
{
    using api.Commands;
    using api.Dtos;
    using GraphQL.Types;
    using MediatR;

    public class CoreMutations : ObjectGraphType
    {
        public CoreMutations(IMediator mediator)
        {
            this.FieldAsync<UserInfoType>(
                "updateHandleName",
                "updates the handle name for user.",
                new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInfoInput>>
                    {
                        Name = "input",
                    }
                ),
                async context => {
                    return await mediator.Send(new UpdateUserInfoCommand()
                        {
                            HandlerName = "test"
                        });
                });
        }

    }
}