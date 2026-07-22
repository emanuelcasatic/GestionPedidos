using System;
using System.Text.Json;
using GestionPedidos.Models;

namespace GestionPedidos.Persistence;

public class GestorJSON
{
    private readonly string _rutaDeArchivo = Path.Combine(AppDomain.CurrentDomain, "pedidos.json");

    public List<Pedido> LeerPedidos()
    {
        List<Pedido>? pedidos = null;
        try
        {
            using StreamReader streamReader = new(_rutaDeArchivo, FileAccess.Read);
            string jsonPedidos = streamReader.ReadToEnd();

            if(!string.IsNullOrWhiteSpace(jsonPedidos.Trim()))
            {
                pedidos = JsonSerializer.Deserialize<GestionPedidos>(jsonPedidos, JsonSerializerOptions.ReferenceEquals);
            }

            if(pedidos != null)
            {
                return pedidos;
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        return [];
    }
}