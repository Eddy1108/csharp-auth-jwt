using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using workshop.webapi.DataModels;
using workshop.webapi.DataTransfer.Requests;
using workshop.webapi.Repository;

namespace workshop.webapi.Endpoints
{
    public static class PostEndpoint
    {
        public static void ConfigureCarEndpoint(this WebApplication app)
        {

            var post = app.MapGroup("posts");
            post.MapGet("/", GetAll);
            post.MapPost("/", AddPost).AddEndpointFilter(async (invocationContext, next) =>
            {
                var post = invocationContext.GetArgument<PostPostRequest>(1);

                if (string.IsNullOrEmpty(post.AuthorId) || string.IsNullOrEmpty(post.Content))
                {
                    return Results.BadRequest("You must enter a AuthorId AND Content");
                }
                return await next(invocationContext);
            }); ;
            post.MapGet("/{id}", GetById);
            post.MapPut("/{id}", Update);
            post.MapDelete("/{id}", Delete);

        }
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> Delete(IRepository<Post> repository, ClaimsPrincipal user, int id)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find Post with Id:{id}");
            }
            var result = await repository.Delete(entity);
            return result != null ? TypedResults.Ok(new { DateTime=DateTime.Now, User=user.Email(), Car=new { AuthorId = result.AuthorId, Model = result.Content }}) : TypedResults.BadRequest($"Post wasn't deleted");
        }
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Update(IRepository<Post> repository, int id, PostPatchRequest model)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find Post with Id:{id}");
            }
            entity.AuthorId = !string.IsNullOrEmpty(model.AuthorId) ? model.AuthorId : entity.AuthorId;
            entity.Content = !string.IsNullOrEmpty(model.Content) ? model.Content : entity.Content;

            var result = await repository.Update(entity);

            return result != null ? TypedResults.Ok(new { AuthorId = result.AuthorId, Content = result.Content }) : TypedResults.BadRequest("Couldn't save to the database?!");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAll(IRepository<Post> repository)
        {
            var entities = await repository.Get();
            List<Object> results = new List<Object>();
            foreach (var post in entities)
            {
                results.Add(new { Id = post.Id, AuthorId = post.AuthorId, Content = post.Content });
            }
            return TypedResults.Ok(results);
        }
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetById(IRepository<Post> repository, int id)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find Post with Id:{id}");
            }
            return TypedResults.Ok(new { Id = entity.Id, AuthorId = entity.AuthorId, Content = entity.Content });
        }
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> AddPost(IRepository<Post> repository, PostPostRequest model)
        {
            var results = await repository.Get();

            if (results.Any(x => x.Content.Equals(model.Content, StringComparison.OrdinalIgnoreCase)))
            {
                return Results.BadRequest("Post with provided name already exists");
            }

            var entity = new Post() { AuthorId = model.AuthorId, Content = model.Content };
            await repository.Insert(entity);
            return TypedResults.Created($"/{entity.Id}", new { AuthorId = entity.AuthorId, Content = entity.Content });

        }
    }
}
