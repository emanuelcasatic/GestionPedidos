using System;
using GestionPedidos.Models;
using GestionPedidos.Persistence;

namespace GestionPedidos.Data;

public class PedidoRepository
{
    private readonly GestorJSON gestorJSON = new();
    public List<Pedido> ObtenerPedidos() => gestorJSON.LeerPedidos();
}