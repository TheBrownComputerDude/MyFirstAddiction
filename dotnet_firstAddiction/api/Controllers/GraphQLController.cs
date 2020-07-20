using System.Threading.Tasks;
using api.Schema;
using GraphQL;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("graphql")]
    [ApiController]
    public class GraphQLController : Controller
    {
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            var inputs = query.Variables.ToInputs();

            var schema = new GraphQL.Types.Schema
            {
                Query = new CoreQueries(),
                Mutation = new CoreMutations()
            };

            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = schema;
                _.Query = query.Query;
                _.OperationName = query.OperationName;
                _.Inputs = inputs;
            });

            if (result.Errors?.Count > 0)
            {
                return BadRequest();
            }

            return Ok(result);
        }

    }
}