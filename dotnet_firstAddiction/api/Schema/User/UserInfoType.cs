using api.Dtos;
using GraphQL.Types;

namespace api.Schema
{
    public class UserInfoType : ObjectGraphType<UserInfo>
    {
        public UserInfoType()
        {
            this.Field(u => u.HandleName);
        }
        
    }
}