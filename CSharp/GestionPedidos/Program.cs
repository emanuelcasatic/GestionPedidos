using System;
using GestionPedidos.Data;

namespace GestionPedidos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var pedidoRepo = new PedidoRepository();
            var lista = pedidoRepo.ObtenerPedidos();
            foreach(var l in lista)
            {
                Console.WriteLine(l.Codigo);
            }
            
        }
    }
}
