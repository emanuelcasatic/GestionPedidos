using System;
using GestionPedidos.Models;
using GestionPedidos.Persistence;

namespace GestionPedidos.Data;

public class PedidoRepository
{
    private readonly GestorJSON _gestorJSON;
    public List<Pedido> Pedidos { get; private set; }

    public PedidoRepository(GestorJSON gestorJSON)
    {
        _gestorJSON = gestorJSON;
        Pedidos = _gestorJSON.LeerPedidos();
    }

    public bool Agregar(Pedido pedido)
    {
        Pedidos.Add(pedido);
        return _gestorJSON.GuardarPedidos(Pedidos);
    }

    public bool Reemplazar(Pedido pedido)
    {
        Pedido? pedidoAReemplazar = null;
        if((pedidoAReemplazar = Pedidos.Find(p => p.Codigo.Equals(pedido.Codigo, StringComparison.OrdinalIgnoreCase))) != null)
        {
            int index = Pedidos.IndexOf(pedidoAReemplazar);
            if(index >= 0)
            {
                Pedidos[index] = pedido;
                return _gestorJSON.GuardarPedidos(Pedidos);
            }
        }

        return false;
    }

    public bool Eliminar(Pedido pedido)
    {
        if (Pedidos.Remove(pedido))
        {
           return _gestorJSON.GuardarPedidos(Pedidos);
        }

        return false;
    }
}