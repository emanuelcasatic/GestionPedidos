using System;
using System.Text.Json;
using GestionPedidos.Models;

namespace GestionPedidos.Persistence;

public class GestorJSON
{
    private readonly string _rutaDeArchivo = Path.Combine(AppContext.BaseDirectory, "pedidos.json");

    public List<Pedido> LeerPedidos()
    {
        List<Pedido>? pedidos = null;
        try
        {
            using StreamReader streamReader = new StreamReader(_rutaDeArchivo);
            string jsonPedidos = streamReader.ReadToEnd();

            if(!string.IsNullOrWhiteSpace(jsonPedidos.Trim()))
            {
                pedidos = JsonSerializer.Deserialize<List<Pedido>>(json: jsonPedidos, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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