using System;
using System.Collections.Generic;
using System.Text;

namespace GestionPedidos.Exceptions
{
    public class PedidoException(string msg) : Exception(msg)
    {
        public static PedidoException CodigoRepetidoException(string codigo) => new ($"El código {codigo} está repetido.");
        public static PedidoException EstadoInvalidoException() => new ("El estado del pedido es inválido.");
    }


}
