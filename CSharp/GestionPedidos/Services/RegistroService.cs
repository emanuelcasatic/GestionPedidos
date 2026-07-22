using System;
using GestorPedidos.Models;
using GestorPedidos.Persistence;

namespace GestorPedidos.Services;

public class RegistroService
{
    private readonly GestorJSON gestorJSON = new();
    public List<Pedido> ObtenerPedidos() => gestorJSON.LeerPedidos();
}