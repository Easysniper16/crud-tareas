using Microsoft.Data.Sqlite;

namespace TareasApi;

public record Tarea(long Id, string Titulo, string Descripcion, bool Completada);

public static class Db
{
    private static string Ruta => Environment.GetEnvironmentVariable("TAREAS_DB") ?? "tareas.db";

    public static SqliteConnection Open()
    {
        var conn = new SqliteConnection($"Data Source={Ruta}");
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"CREATE TABLE IF NOT EXISTS tareas (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            titulo TEXT NOT NULL,
            descripcion TEXT NOT NULL DEFAULT '',
            completada INTEGER NOT NULL DEFAULT 0)";
        cmd.ExecuteNonQuery();
        return conn;
    }

    public static Tarea Leer(SqliteDataReader r) =>
        new(r.GetInt64(0), r.GetString(1), r.GetString(2), r.GetInt64(3) != 0);

    public static Tarea? Obtener(SqliteConnection conn, long id)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT id, titulo, descripcion, completada FROM tareas WHERE id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        using var r = cmd.ExecuteReader();
        return r.Read() ? Leer(r) : null;
    }
}
