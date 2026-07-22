using System;
using GestorPedidos.Models;
using GestorPedidos.Persistence;

namespace GestorPedidos.Data;

public class PedidoRepository
{
    private readonly GestorJSON gestorJSON = new();
    public List<Pedido> ObtenerPedidos() => gestorJSON.LeerPedidos();
}