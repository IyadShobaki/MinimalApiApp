using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using TodoLibrary.DataAccess;

namespace MinimalApi.Endpoints;

public static class TodoEndpoints
{
    public static void AddTodoEndpoints(this WebApplication app)
    {
        app.MapGet("/api/Todos", GetAllTodos);

        app.MapPost("/api/Todos", CreateTodo)
            .AddEndpointFilter(async (context, next) =>
            {

                var task = context.GetArgument<string>(1);

                if (string.IsNullOrWhiteSpace(task))
                {
                    return Results.BadRequest("Task can not be null.");
                }
                Regex taskRegex = new Regex(@"[^0-9A-Za-z]");

                if (taskRegex.IsMatch(task) || task.Length < 6)
                {

                    return Results.BadRequest("Task can only contains letters and numbers, and it must be at least 6 characters long.");
                }
                return await next(context);
            }).AllowAnonymous();

        app.MapDelete("/api/Todos/{id}", DeleteTodo);//.RequireAuthorization();
    }

    //[Authorize] // Either we add Authorize on top of the methid or add .RequireAuthorization(); like above
    private async static Task<IResult> GetAllTodos(ITodoData data)
    {
        var output = await data.GetAllAssigned(1);
        return Results.Ok(output);
    }
    private async static Task<IResult> CreateTodo(ITodoData data, [FromBody] string task)
    {
        var output = await data.Create(1, task);
        return Results.Ok(output);
    }
    private async static Task<IResult> DeleteTodo(ITodoData data, int id)
    {
        await data.Delete(1, id);
        return Results.Ok();
    }
}
