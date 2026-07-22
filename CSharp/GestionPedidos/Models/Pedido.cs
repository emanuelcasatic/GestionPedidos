using System;
using GestionPedidos.Models.Enums;

namespace GestionPedidos.Models;


public class Pedido
{
    public string Codigo { get; private set; }
    public string Nombre { get; private set; }
    public string Producto { get; private set; }
    public int Cantidad { get; private set; }
    public decimal PrecioUnitario { get; private set; }
    public TipoEntrega TipoEntrega { get; private set; }
    public DateTime FechaPedido { get; private set; } 
    public EstadoPedido EstadoPedido { get; private set;}

    public Pedido(string codigo, string nombre, string producto, int cantidad, decimal precioUnitario, TipoEntrega tipoEntrega, DateTime fechaPedido, EstadoPedido estadoPedido = EstadoPedido.Pendiente) 
    {
        Codigo = codigo;
        Nombre = nombre;
        Producto = producto;
        Cantidad = cantidad;
        PrecioUnitario = precioUnitario;
        TipoEntrega = tipoEntrega;
        FechaPedido = fechaPedido;
        EstadoPedido = estadoPedido;
    }

}