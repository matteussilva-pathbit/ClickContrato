namespace ClickContrato.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth").WithTags("Auth");

        group.MapPost("/register", async (RegisterRequest request, AuthService authService) =>
            {
                var result = await authService.RegisterAsync(request);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest(new { code = result.ErrorCode, message = result.ErrorMessage });
            })
            .WithName("Register")
            .WithSummary("Registrar novo usuário")
            .WithDescription("""
                             Cria um usuário e retorna um JWT (access token) para autenticação via Bearer.
                             
                             Exemplo de body:
                             {
                               "email": "teste@exemplo.com",
                               "name": "Teste",
                               "password": "SenhaForte123"
                             }
                             """)
            .Accepts<RegisterRequest>("application/json")
            .Produces<AuthResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPost("/login", async (LoginRequest request, AuthService authService) =>
            {
                var result = await authService.LoginAsync(request);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.Unauthorized();
            })
            .WithName("Login")
            .WithSummary("Login")
            .WithDescription("""
                             Valida email/senha e retorna um JWT (access token).
                             
                             Exemplo de body:
                             {
                               "email": "teste@exemplo.com",
                               "password": "SenhaForte123"
                             }
                             """)
            .Accepts<LoginRequest>("application/json")
            .Produces<AuthResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        return app;
    }
}


