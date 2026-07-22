using System;
using System.Linq;
using GestionPedidos.Data;
using GestionPedidos.Models;
using GestionPedidos.Models.Enums;

namespace GestionPedidos.Services
{
  
    public class RegistroService
    {
        private readonly PedidoRepository _repositorio;

        public RegistroService(PedidoRepository repositorio)
        {
            _repositorio = repositorio;
        }

        // ---------------------------------------------------------
        // OPCIÓN 1: REGISTRAR
        // ---------------------------------------------------------
        public void RegistrarPedido()
        {
            Console.WriteLine("\n--- Registrar nuevo pedido ---");

            string codigo = LeerTextoNoVacio("Código del pedido: ");
            if (_repositorio.Pedidos.Any(p => p.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Error: ya existe un pedido con ese código.");
                return;
            }

            string nombre = LeerTextoNoVacio("Cliente: ");
            string producto = LeerTextoNoVacio("Producto: ");
            int cantidad = LeerEnteroPositivo("Cantidad: ");
            decimal precio = LeerDecimalPositivo("Precio unitario: ");
            TipoEntrega tipoEntrega = LeerTipoEntrega();
            DateTime fecha = LeerFecha();

            // El constructor de Pedido ya pone EstadoPedido.Pendiente por defecto
            var nuevoPedido = new Pedido(codigo, nombre, producto, cantidad, precio, tipoEntrega, fecha);

            if (_repositorio.Agregar(nuevoPedido)) 
            { 
                Console.WriteLine($"Pedido '{codigo}' registrado correctamente. Total: {CalcularTotal(nuevoPedido):C}"); 
            } else 
            {
                Console.WriteLine("Error al agregar.");
            }

        }

        // ---------------------------------------------------------
        // OPCIÓN 4: MODIFICAR
        // ---------------------------------------------------------
        public void ModificarPedido()
        {
            Console.WriteLine("\n--- Modificar pedido existente ---");

            string codigo = LeerTextoNoVacio("Código del pedido a modificar: ");
            var pedido = _repositorio.Pedidos
                .FirstOrDefault(p => p.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));

            if (pedido == null)
            {
                Console.WriteLine("Error: no se encontró un pedido con ese código.");
                return;
            }

            Console.WriteLine("Deje en blanco un campo si no desea modificarlo.");

            Console.Write($"Cliente [{pedido.Nombre}]: ");
            string nombreTexto = Console.ReadLine();
            string nombre = string.IsNullOrWhiteSpace(nombreTexto) ? pedido.Nombre : nombreTexto.Trim();

            Console.Write($"Producto [{pedido.Producto}]: ");
            string productoTexto = Console.ReadLine();
            string producto = string.IsNullOrWhiteSpace(productoTexto) ? pedido.Producto : productoTexto.Trim();

            Console.Write($"Cantidad [{pedido.Cantidad}]: ");
            string cantidadTexto = Console.ReadLine();
            int cantidad = (!string.IsNullOrWhiteSpace(cantidadTexto) &&
                             int.TryParse(cantidadTexto, out int nuevaCantidad) && nuevaCantidad > 0)
                ? nuevaCantidad
                : pedido.Cantidad;

            Console.Write($"Precio unitario [{pedido.PrecioUnitario}]: ");
            string precioTexto = Console.ReadLine();
            decimal precio = (!string.IsNullOrWhiteSpace(precioTexto) &&
                               decimal.TryParse(precioTexto, out decimal nuevoPrecio) && nuevoPrecio > 0)
                ? nuevoPrecio
                : pedido.PrecioUnitario;

            Console.Write($"¿Cambiar tipo de entrega actual ({pedido.TipoEntrega})? (S/N): ");
            TipoEntrega tipoEntrega = Console.ReadLine()?.Trim().ToUpper() == "S"
                ? LeerTipoEntrega()
                : pedido.TipoEntrega;

            Console.Write($"¿Cambiar fecha actual ({pedido.FechaPedido:dd/MM/yyyy})? (S/N): ");
            DateTime fecha = Console.ReadLine()?.Trim().ToUpper() == "S"
                ? LeerFecha()
                : pedido.FechaPedido;

            // El código no se modifica y el estado se preserva
            // (los cambios de estado se manejan en la Opción 5, no aquí).
            var pedidoActualizado = new Pedido(
                pedido.Codigo, nombre, producto, cantidad, precio,
                tipoEntrega, fecha, pedido.EstadoPedido);

            if(_repositorio.Reemplazar(pedidoActualizado))
            {
                Console.WriteLine($"Pedido '{codigo}' actualizado. Nuevo total: {CalcularTotal(pedidoActualizado):C}");
            }
            else
            {
                Console.WriteLine("Error al actualizar");
            }

            
        }

        // ---------------------------------------------------------
        // OPCIÓN 6: ELIMINAR
        // ---------------------------------------------------------
        public void EliminarPedido()
        {
            Console.WriteLine("\n--- Eliminar pedido ---");

            string codigo = LeerTextoNoVacio("Código del pedido a eliminar: ");
            var pedido = _repositorio.Pedidos
                .FirstOrDefault(p => p.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));

            if (pedido == null)
            {
                Console.WriteLine("Error: no se encontró un pedido con ese código.");
                return;
            }

            Console.WriteLine($"Cliente: {pedido.Nombre} | Producto: {pedido.Producto} | Total: {CalcularTotal(pedido):C}");
            Console.Write("¿Confirma eliminar este pedido? (S/N): ");
            string confirmacion = Console.ReadLine();

            if (confirmacion?.Trim().ToUpper() == "S")
            {
                if(_repositorio.Eliminar(pedido))
                {
                    Console.WriteLine("Pedido eliminado correctamente.");
                }
                else
                {
                    Console.WriteLine("Error al eliminar.");
                }
                
            }
            else
            {
                Console.WriteLine("Eliminación cancelada.");
            }
        }

        // ---------------------------------------------------------
        // CÁLCULO DE TOTALES
        // ---------------------------------------------------------
        private decimal ObtenerCostoEntrega(TipoEntrega tipo) => tipo switch
        {
            TipoEntrega.Tienda => 0.00m,
            TipoEntrega.Estandar => 2.50m,
            TipoEntrega.Rapida => 5.00m,
            _ => 0.00m
        };

        private decimal CalcularTotal(Pedido pedido)
        {
            decimal subtotal = pedido.Cantidad * pedido.PrecioUnitario;
            return subtotal + ObtenerCostoEntrega(pedido.TipoEntrega);
        }

              // MÉTODOS AUXILIARES DE VALIDACIÓN

        private string LeerTextoNoVacio(string mensaje)
        {
            string texto;
            do
            {
                Console.Write(mensaje);
                texto = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(texto))
                    Console.WriteLine("Este campo no puede estar vacío.");
            } while (string.IsNullOrWhiteSpace(texto));

            return texto.Trim();
        }

        private int LeerEnteroPositivo(string mensaje)
        {
            int valor;
            while (true)
            {
                Console.Write(mensaje);
                if (int.TryParse(Console.ReadLine(), out valor) && valor > 0)
                    return valor;
                Console.WriteLine("Ingrese un número entero mayor a 0.");
            }
        }

        private decimal LeerDecimalPositivo(string mensaje)
        {
            decimal valor;
            while (true)
            {
                Console.Write(mensaje);
                if (decimal.TryParse(Console.ReadLine(), out valor) && valor > 0)
                    return valor;
                Console.WriteLine("Ingrese un monto numérico mayor a 0.");
            }
        }

        private TipoEntrega LeerTipoEntrega()
        {
            while (true)
            {
                Console.WriteLine("Tipo de entrega:");
                Console.WriteLine("  1. Retiro en tienda ($0.00)");
                Console.WriteLine("  2. Entrega estándar ($2.50)");
                Console.WriteLine("  3. Entrega rápida ($5.00)");
                Console.Write("Seleccione una opción: ");

                switch (Console.ReadLine()?.Trim())
                {
                    case "1": return TipoEntrega.Tienda;
                    case "2": return TipoEntrega.Estandar;
                    case "3": return TipoEntrega.Rapida;
                    default:
                        Console.WriteLine("Opción no válida. Intente de nuevo.");
                        break;
                }
            }
        }

        private DateTime LeerFecha()
        {
            DateTime fecha;
            while (true)
            {
                Console.Write("Fecha del pedido (dd/MM/yyyy): ");
                string texto = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(texto))
                {
                    Console.WriteLine("La fecha no puede estar vacía.");
                    continue;
                }

                if (DateTime.TryParseExact(texto.Trim(), "dd/MM/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out fecha))
                {
                    return fecha;
                }

                Console.WriteLine("Formato inválido. Use dd/MM/yyyy (ej. 20/07/2026).");
            }
        }
    }
}