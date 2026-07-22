using GestionPedidos.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

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
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                options.Converters.Add(new JsonStringEnumConverter());

                pedidos = JsonSerializer.Deserialize<List<Pedido>>(jsonPedidos, options);
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

    public bool GuardarPedidos(List<Pedido> pedidos)
    {
        if (pedidos == null || pedidos.Count <= 0)
        {
            return false;
        }

        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new JsonStringEnumConverter());
            string json = JsonSerializer.Serialize(pedidos, options);
            using StreamWriter streamWriter = new(_rutaDeArchivo);
            streamWriter.Write(json);
            return true;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
       
    }
}