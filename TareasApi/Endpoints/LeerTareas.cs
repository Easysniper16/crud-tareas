namespace TareasApi.Endpoints;

public class LeerTareas : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/tareas", () =>
        {
            using var conn = Db.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, titulo, descripcion, completada FROM tareas ORDER BY id";
            using var r = cmd.ExecuteReader();
            var lista = new List<Tarea>();
            while (r.Read()) lista.Add(Db.Leer(r));
            return Results.Ok(lista);
        });

        app.MapGet("/api/tareas/{id:long}", (long id) =>
        {
            using var conn = Db.Open();
            var tarea = Db.Obtener(conn, id);
            return tarea is null
                ? Results.NotFound(new { error = "No encontrada" })
                : Results.Ok(tarea);
        });
    }
}
