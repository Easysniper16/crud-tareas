namespace TareasApi.Endpoints;

public class CrearTarea : IEndpoint
{
    public record NuevaTarea(string? Titulo, string? Descripcion);

    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/tareas", (NuevaTarea dto) =>
        {
            var titulo = dto.Titulo?.Trim();
            if (string.IsNullOrEmpty(titulo))
                return Results.BadRequest(new { error = "El título es obligatorio" });

            using var conn = Db.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO tareas (titulo, descripcion) VALUES ($t, $d); SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("$t", titulo);
            cmd.Parameters.AddWithValue("$d", dto.Descripcion ?? "");
            var id = (long)cmd.ExecuteScalar()!;
            return Results.Created($"/api/tareas/{id}", Db.Obtener(conn, id));
        });
    }
}
