using api.Dtos;
using GraphQL.Types;

namespace api.Schema
{
    public class UserInfoInput : InputObjectGraphType<UserInfo>
    {
        public UserInfoInput()
        {
            this.Field(u => u.HandleName);
        }
    }
}