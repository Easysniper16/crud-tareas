namespace TareasApi.Endpoints;

public class EliminarTarea : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/tareas/{id:long}", (long id) =>
        {
            using var conn = Db.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM tareas WHERE id = $id";
            cmd.Parameters.AddWithValue("$id", id);
            return cmd.ExecuteNonQuery() == 0
                ? Results.NotFound(new { error = "No encontrada" })
                : Results.NoContent();
        });
    }
}
