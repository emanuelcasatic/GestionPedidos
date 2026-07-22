using System;
using System.Diagnostics;
using System.Reflection.Emit;

namespace GestionPedidos.UI
{
        class MenuConsola
    {
        public static void Menu()
        {
            bool salir = false;
            int opcion = 10;

            while(salir != true)
            {
                Console.Write("============Menu de Gestor de Pedidos============\n"
                +"1. Registrar Pedido\n"
                +"2. Mostrar todos los pedidos\n"
                +"3. Buscar pedido\n"
                +"4. Modificar pedido\n"
                +"5. Cambiar estado\n"
                +"6. Eliminar pedido\n"
                +"7. Filtrar pedidos\n"
                +"8. Mostrar Estadisticas\n"
                +"9. Mostrar ranking de pedidos\n"
                +"10. Salir\n");
                Console.Write("Seleccione una opcion: ");
                opcion = Convert.ToInt32(Console.ReadLine());

                switch (opcion)
                {
                    case 1:
                    break;
                    case 2:
                    break;
                    case 3:
                    break;
                    case 4:
                    break;
                    case 5:
                    break;
                    case 6:
                    break;
                    case 7:
                    break;
                    case 8:
                    break;
                    case 9:
                    break;
                    case 10:
                    break;
                    
                }
            }
        }
    }
}