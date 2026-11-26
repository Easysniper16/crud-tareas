namespace TareasApi.Endpoints;

public class ActualizarTarea : IEndpoint
{
    public record Cambios(string? Titulo, string? Descripcion, bool? Completada);

    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/tareas/{id:long}", (long id, Cambios dto) =>
        {
            using var conn = Db.Open();
            var actual = Db.Obtener(conn, id);
            if (actual is null)
                return Results.NotFound(new { error = "No encontrada" });

            var titulo = (dto.Titulo ?? actual.Titulo).Trim();
            if (titulo.Length == 0)
                return Results.BadRequest(new { error = "El título es obligatorio" });

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE tareas SET titulo=$t, descripcion=$d, completada=$c WHERE id=$id";
            cmd.Parameters.AddWithValue("$t", titulo);
            cmd.Parameters.AddWithValue("$d", dto.Descripcion ?? actual.Descripcion);
            cmd.Parameters.AddWithValue("$c", (dto.Completada ?? actual.Completada) ? 1 : 0);
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
            return Results.Ok(Db.Obtener(conn, id));
        });
    }
}
