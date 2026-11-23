using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SistemaAlmacen
{
    class Program
    {
        static string linea = new string('=', 85);
        static string linea2 = new string('-', 85);

        static string archivoInventario = "Inventario.txt";
        static string archivoPedidos = "Pedidos.txt";

        static List<string> productos = new List<string>();
        static List<int> stocks = new List<int>();
        static List<decimal> precios = new List<decimal>();

        static List<string> nombres = new List<string>();
        static List<string> numeros = new List<string>();
        static List<string> pedProductos = new List<string>();
        static List<string> pedStocks = new List<string>();
        static List<string> pedPrecios = new List<string>();

        static void Main(string[] args)
        {
            CargarInventario();
            CargarPedidos();
            Inicio();
        }
        static void Titulo()
        {
            Console.WriteLine(linea);
            Console.WriteLine("\t\tSISTEMA DIGITAL DE REGISTRO Y GESTION DE DOCUMENTOS\n\t\t\tEMPRESA DE PRODUCTOS CAPILARES");
            Console.WriteLine(linea);
        }

        #region Inicio
        static void Inicio()
        {
            int opcion = 0;
            while (true)
            {
                Console.Clear();
                Titulo();
                Console.WriteLine(opcion == 0 ? "\x1b[7m[1] Registrar nuevo pedido\x1b[0m" : "[1] Registrar nuevo pedido");
                Console.WriteLine(opcion == 1 ? "\x1b[7m[2] Gestionar inventario\x1b[0m" : "[2] Gestionar inventario");
                Console.WriteLine(opcion == 2 ? "\x1b[7m[3] Gestionar pedidos\x1b[0m" : "[3] Gestionar pedidos");
                Console.WriteLine(opcion == 3 ? "\x1b[7m[4] Salir\x1b[0m" : "[4] Salir");
                Console.WriteLine(linea2);
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.UpArrow || tecla.Key == ConsoleKey.W)
                {
                    opcion--;
                    if (opcion < 0) opcion = 3;
                }
                else if (tecla.Key == ConsoleKey.DownArrow || tecla.Key == ConsoleKey.S)
                {
                    opcion++;
                    if (opcion > 3) opcion = 0;
                }
                else if (tecla.Key == ConsoleKey.Enter)
                {
                    switch (opcion)
                    {
                        case 0: Registrar(); return;
                        case 1: Inventario(); return;
                        case 2: Pedidos(); return;
                        case 3:
                            Console.WriteLine("Saliendo del sistema...");
                            Environment.Exit(0);
                            break;
                    }
                }
            }
        }
        #endregion

        #region Registro
        static void Registrar()
        {
            Console.Clear();
            Titulo();
            Console.WriteLine("Registro de pedido");
            Console.WriteLine(linea);

            Console.Write("Ingrese el nombre del cliente:\n>/ ");
            string nombre = Console.ReadLine();

            Console.Write("Ingrese el número de contacto del cliente:\n>/ ");
            string numero = Console.ReadLine();

            List<string> pedProductoLocal = new List<string>();
            List<int> pedStockLocal = new List<int>();
            List<decimal> pedPrecioLocal = new List<decimal>();

            int opcion = 0;

            while (true)
            {
                while (true)
                {
                    Console.Clear();
                    Titulo();
                    Console.WriteLine("Seleccione producto (presione \"Escape\" para cancelar pedido)");
                    Console.WriteLine(linea2);
                    Console.WriteLine($"{"Nro",-5}{"|Producto",-61}{"|Stock",-9}{"|Precio",-10}");
                    Console.WriteLine(linea2);
                    for (int i = 0; i < productos.Count; i++)
                    {
                        Console.WriteLine(opcion == i ? $"\u001b[7m{i + 1,-5}|{productos[i],-60}|{stocks[i],-8}|{precios[i],-10:C}\u001b[0m" : $"{i + 1,-5}|{productos[i],-60}|{stocks[i],-8}|{precios[i],-10:C}");
                    }
                    Console.WriteLine(linea2);
                    ConsoleKeyInfo tecla = Console.ReadKey(true);
                    if (tecla.Key == ConsoleKey.UpArrow || tecla.Key == ConsoleKey.W)
                    {
                        opcion--;
                        if (opcion < 0) opcion = productos.Count - 1;
                    }
                    else if (tecla.Key == ConsoleKey.DownArrow || tecla.Key == ConsoleKey.S)
                    {
                        opcion++;
                        if (opcion > productos.Count - 1) opcion = 0;
                    }
                    else if (tecla.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                    else if (tecla.Key == ConsoleKey.Escape)
                    {
                        Inicio();
                    }
                }
                if (opcion <= productos.Count)
                {
                    Console.Write($"Ingrese la cantidad de {productos[opcion]} que desea agregar al pedido:\n>/ ");
                    if (!int.TryParse(Console.ReadLine(), out int cantidad))
                    {
                        Console.WriteLine("Cantidad inválida.");
                        continue;
                    }
                    if (cantidad <= stocks[opcion])
                    {
                        pedProductoLocal.Add(productos[opcion]);
                        pedStockLocal.Add(cantidad);
                        decimal subtotal = precios[opcion] * cantidad;
                        pedPrecioLocal.Add(subtotal);

                        stocks[opcion] -= cantidad;
                        GuardarInventario();
                        Console.WriteLine($"{cantidad} unidades de {productos[opcion]} agregadas al pedido.");
                        Console.Write("¿Desea agregar otro producto al pedido? (s/n):\n>/ ");
                        string respuesta = Console.ReadLine();
                        if (respuesta?.ToLower() != "s") break;
                    }
                    else
                    {
                        Console.WriteLine($"No hay suficiente stock de {productos[opcion]}. Stock disponible: {stocks[opcion]} unidades.");
                        Console.WriteLine("Presione Enter para continuar...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Opción inválida. Por favor, seleccione un producto válido.");
                }
            }
            //Console.WriteLine("Desea agregar otro producto al pedido?");
            if (pedProductoLocal.Count > 0)
            {
                nombres.Add(nombre);
                numeros.Add(numero);
                pedProductos.Add(string.Join(",", pedProductoLocal));
                pedStocks.Add(string.Join(",", pedStockLocal));
                pedPrecios.Add(string.Join(",", pedPrecioLocal));
                GuardarPedidos();
                Console.WriteLine("Pedido registrado exitosamente.");
            }
            else
            {
                Console.WriteLine("No se agregaron productos al pedido. Registro cancelado.");
            }

            Console.Write("Presione Enter para continuar...");
            Console.ReadLine();
            Console.Clear();
            Inicio();
        }
        #endregion

        #region Inventario
        static void CargarInventario()
        {
            if (File.Exists(archivoInventario))
            {
                foreach (var lineaTexto in File.ReadAllLines(archivoInventario))
                {
                    if (string.IsNullOrWhiteSpace(lineaTexto)) continue;
                    var partes = lineaTexto.Split('/');
                    if (partes.Length == 3)
                    {
                        productos.Add(partes[0]);
                        stocks.Add(int.Parse(partes[1]));
                        precios.Add(decimal.Parse(partes[2], System.Globalization.CultureInfo.InvariantCulture));
                    }
                }
            }
        }
        static void GuardarInventario()
        {
            using (StreamWriter archivo = new StreamWriter(archivoInventario))
            {
                for (int i = 0; i < productos.Count; i++)
                {
                    archivo.WriteLine($"{productos[i]}/{stocks[i]}/{precios[i].ToString(System.Globalization.CultureInfo.InvariantCulture)}");
                }
            }
        }
        static void Inventario()
        {
            int opcion = 0;
            while (true)
            {
                Console.Clear();
                Titulo();
                Console.WriteLine("Inventario disponible");
                Console.WriteLine(linea2);
                Console.WriteLine($"{"Nro",-5}{"|Producto",-61}{"|Stock",-9}{"|Precio",-10}");
                Console.WriteLine(linea2);
                for (int i = 0; i < productos.Count; i++)
                {
                    Console.WriteLine($"{i + 1,-5}|{productos[i],-60}|{stocks[i],-8}|{precios[i],-10:C}");
                }
                Console.WriteLine(linea2);
                Console.WriteLine(opcion == 0 ? "\x1b[7m[1] Editar producto\x1b[0m" : "[1] Editar producto");
                Console.WriteLine(opcion == 1 ? "\x1b[7m[2] Agregar producto\x1b[0m" : "[2] Agregar producto");
                Console.WriteLine(opcion == 2 ? "\x1b[7m[3] Eliminar producto\x1b[0m" : "[3] Eliminar producto");
                Console.WriteLine(opcion == 3 ? "\x1b[7m[4] Volver\x1b[0m" : "[4] Volver");
                Console.WriteLine(linea2);
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.UpArrow || tecla.Key == ConsoleKey.W)
                {
                    opcion--;
                    if (opcion < 0) opcion = 3;
                }
                else if (tecla.Key == ConsoleKey.DownArrow || tecla.Key == ConsoleKey.S)
                {
                    opcion++;
                    if (opcion > 3) opcion = 0;
                }
                else if (tecla.Key == ConsoleKey.Enter)
                {
                    switch (opcion)
                    {
                        case 0: EditarProducto(); return;
                        case 1: AgregarProducto(); return;
                        case 2: EliminarProducto(); return;
                        case 3:
                            Inicio();
                            break;
                    }
                }
            }
        }
        static void EditarProducto()
        {
            int opcion = 0;
            while (true)
            {
                Console.Clear();
                Titulo();
                Console.WriteLine("Editar producto");
                Console.WriteLine(linea2);
                Console.WriteLine($"{"Nro",-5}{"|Producto",-61}{"|Stock",-9}{"|Precio",-10}");
                Console.WriteLine(linea2);
                for (int i = 0; i < productos.Count; i++)
                {
                    Console.WriteLine(opcion == i ? $"\u001b[7m{i + 1,-5}|{productos[i],-60}|{stocks[i],-8}|{precios[i],-10:C}\u001b[0m" : $"{i + 1,-5}|{productos[i],-60}|{stocks[i],-8}|{precios[i],-10:C}");
                }
                Console.WriteLine(linea2);
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.UpArrow || tecla.Key == ConsoleKey.W)
                {
                    opcion--;
                    if (opcion < 0) opcion = productos.Count - 1;
                }
                else if (tecla.Key == ConsoleKey.DownArrow || tecla.Key == ConsoleKey.S)
                {
                    opcion++;
                    if (opcion > productos.Count - 1) opcion = 0;
                }
                else if (tecla.Key == ConsoleKey.Enter)
                {
                    break;
                }

            }
            Console.Write($"Ingrese el nuevo nombre del producto (actual: {productos[opcion]}):\n>/ ");
            string nuevoNombre = Console.ReadLine();
            Console.Write($"Ingrese el nuevo stock del producto (actual: {stocks[opcion]}):\n>/ ");
            int nuevoStock = int.Parse(Console.ReadLine());
            Console.Write($"Ingrese el nuevo precio del producto (actual: {precios[opcion]:C}):\n>/ ");
            decimal nuevoPrecio = decimal.Parse(Console.ReadLine());
            productos[opcion] = nuevoNombre;
            stocks[opcion] = nuevoStock;
            precios[opcion] = nuevoPrecio;
            GuardarInventario();
            Console.WriteLine("Producto editado exitosamente...");
            Console.ReadKey();
            Inventario();
        }

        static void AgregarProducto()
        {
            Console.Clear();
            Titulo();
            Console.WriteLine("Agregar producto");
            Console.WriteLine(linea2);

            Console.Write("Ingrese el nombre del nuevo producto:\n>/ ");
            string nuevoNombre = Console.ReadLine();

            Console.Write("Ingrese el stock del nuevo producto:\n>/ ");
            int.TryParse(Console.ReadLine(), out int nuevoStock);

            Console.Write("Ingrese el precio del nuevo producto:\n>/ ");
            decimal.TryParse(Console.ReadLine(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal nuevoPrecio);

            productos.Add(nuevoNombre);
            stocks.Add(nuevoStock);
            precios.Add(nuevoPrecio);
            GuardarInventario();

            Console.WriteLine("Producto agregado exitosamente.");
            Console.Write("Presione Enter para continuar...");
            Console.ReadLine();
            Inventario();
        }

        static void EliminarProducto()
        {
            int opcion = 0;
            while (true)
            {
                Console.Clear();
                Titulo();
                Console.WriteLine("Editar producto");
                Console.WriteLine(linea2);
                Console.WriteLine($"{"Nro",-5}{"|Producto",-61}{"|Stock",-9}{"|Precio",-10}");
                Console.WriteLine(linea2);
                for (int i = 0; i < productos.Count; i++)
                {
                    Console.WriteLine(opcion == i ? $"\u001b[7m{i + 1,-5}|{productos[i],-60}|{stocks[i],-8}|{precios[i],-10:C}\u001b[0m" : $"{i + 1,-5}|{productos[i],-60}|{stocks[i],-8}|{precios[i],-10:C}");
                }
                Console.WriteLine(linea2);
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.UpArrow || tecla.Key == ConsoleKey.W)
                {
                    opcion--;
                    if (opcion < 0) opcion = productos.Count - 1;
                }
                else if (tecla.Key == ConsoleKey.DownArrow || tecla.Key == ConsoleKey.S)
                {
                    opcion++;
                    if (opcion > productos.Count - 1) opcion = 0;
                }
                else if (tecla.Key == ConsoleKey.Enter)
                {
                    break;
                }

            }
            productos.RemoveAt(opcion);
            stocks.RemoveAt(opcion);
            precios.RemoveAt(opcion);
            GuardarInventario();
            Console.WriteLine("Producto eliminado exitosamente.");
            Console.WriteLine(linea);
            Console.Write("Presione Enter para continuar...");
            Console.ReadLine();
            Inventario();
        }
        #endregion

        #region Pedidos
        static void CargarPedidos()
        {
            if (File.Exists(archivoPedidos))
            {
                foreach (var lineaTexto in File.ReadAllLines(archivoPedidos))
                {
                    if (string.IsNullOrWhiteSpace(lineaTexto)) continue;
                    var partes = lineaTexto.Split('/');
                    if (partes.Length == 5)
                    {
                        nombres.Add(partes[0]);
                        numeros.Add(partes[1]);
                        pedProductos.Add(partes[2]);
                        pedStocks.Add(partes[3]);
                        pedPrecios.Add(partes[4]);
                    }
                }
            }
        }
        static void GuardarPedidos()
        {
            using (StreamWriter archivo = new StreamWriter(archivoPedidos))
            {
                for (int i = 0; i < nombres.Count; i++)
                {
                    archivo.WriteLine($"{nombres[i]}/{numeros[i]}/{pedProductos[i]}/{pedStocks[i]}/{pedPrecios[i]}");
                }
            }
        }
        static void Pedidos()
        {
            int opcion = 0;
            while (true)
            {
                Console.Clear();
                Titulo();
                Console.WriteLine("Pedidos registrados");
                Console.WriteLine(linea);
                if (nombres.Count == 0)
                {
                    Console.WriteLine("No hay pedidos registrados.");
                }
                else
                {
                    Console.WriteLine($"{"Nro", -5} {"|Nombre", -21} {"|Total",-11} {"|Estado"}");
                    Console.WriteLine(linea2);
                    for (int i = 0; i < nombres.Count; i++)
                    {
                        decimal total = pedPrecios[i].Split(',').Sum(x => decimal.Parse(x, System.Globalization.CultureInfo.InvariantCulture));
                        Console.WriteLine($"{i + 1, -5} |{nombres[i], -20} |{total, -10:F2} |Activo");
                    }
                }
                Console.WriteLine(linea2);
                Console.WriteLine(opcion == 0 ? "\x1b[7m[1] Generar boleta\x1b[0m" : "[1] Generar boleta");
                Console.WriteLine(opcion == 1 ? "\x1b[7m[2] Editar datos del cliente\x1b[0m" : "[2] Editar datos del cliente");
                Console.WriteLine(opcion == 2 ? "\x1b[7m[3] Anular pedido\x1b[0m" : "[3] Anular pedido");
                Console.WriteLine(opcion == 3 ? "\x1b[7m[4] Volver\x1b[0m" : "[4] Volver");
                Console.WriteLine(linea2);
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.UpArrow || tecla.Key == ConsoleKey.W)
                {
                    opcion--;
                    if (opcion < 0) opcion = 3;
                }
                else if (tecla.Key == ConsoleKey.DownArrow || tecla.Key == ConsoleKey.S)
                {
                    opcion++;
                    if (opcion > 3) opcion = 0;
                }
                else if (tecla.Key == ConsoleKey.Enter)
                {
                    switch (opcion)
                    {
                        case 0: GenerarBoleta(); return;
                        case 1: EditarDatos(); return;
                        case 2: AnularPedido(); return;
                        case 3:
                            Inicio();
                            break;
                    }
                }
            }
        }
        static void GenerarBoleta()
        {
            int opcion = 0;
            while (true)
            {
                Console.Clear();
                Titulo();
                Console.WriteLine("Generar y retirar boleta");
                Console.WriteLine(linea);
                if (nombres.Count == 0)
                {
                    Console.WriteLine("No hay pedidos registrados.");
                    Console.Write("Presione Enter para continuar...");
                    Console.ReadLine();
                    Pedidos();
                    return;
                }
                Console.WriteLine($"{"Nro",-5} {"|Nombre",-21} {"|Total",-11} {"|Estado"}");
                Console.WriteLine(linea2);
                for (int i = 0; i < nombres.Count; i++)
                {
                    decimal total = pedPrecios[i].Split(',').Sum(x => decimal.Parse(x, System.Globalization.CultureInfo.InvariantCulture));
                    Console.WriteLine(opcion == i ? $"\u001b[7m{i + 1,-5} |{nombres[i],-20} |{total,-10:F2} |Activo\u001b[0m" : $"{i + 1,-5} |{nombres[i],-20} |{total,-10:F2} |Activo");
                }
                Console.WriteLine(linea2);
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.UpArrow || tecla.Key == ConsoleKey.W)
                {
                    opcion--;
                    if (opcion < 0) opcion = productos.Count;
                }
                else if (tecla.Key == ConsoleKey.DownArrow || tecla.Key == ConsoleKey.S)
                {
                    opcion++;
                    if (opcion > productos.Count) opcion = 0;
                }
                else if (tecla.Key == ConsoleKey.Enter)
                {
                    break;
                }
            }
            var sepPro = pedProductos[opcion].Split(',');
            var sepSto = pedStocks[opcion].Split(',');
            var sepPre = pedPrecios[opcion].Split(',');
            Console.Clear();
            Console.WriteLine(linea);
            Console.WriteLine("BOLETA DE VENTA PRODUCTOS CAPILARES");
            Console.WriteLine(linea);
            Console.WriteLine($"Nro Documento: {opcion}");
            Console.WriteLine($"Cliente: {nombres[opcion]}");
            Console.WriteLine($"Telefono: {numeros[opcion]}");
            Console.WriteLine(linea2);
            Console.WriteLine("Cantidad | Producto | Subtotal");
            Console.WriteLine(linea2);
            for (int j = 0; j < sepPro.Length; j++)
            {
                decimal subVal = decimal.Parse(sepPre[j], System.Globalization.CultureInfo.InvariantCulture);
                Console.WriteLine($"{sepSto[j]}\t | {sepPro[j]} | S/.{subVal:F2}");
            }
            decimal totalGeneral = sepPre.Sum(x => decimal.Parse(x, System.Globalization.CultureInfo.InvariantCulture));
            Console.WriteLine(linea2);
            Console.WriteLine($"Total: S/.{totalGeneral:F2}");
            Console.WriteLine(linea);
            Console.WriteLine("Gracias por su compra. ¡Vuelva pronto!");
            Console.WriteLine(linea);
            Console.Write("\nPresione Enter para continuar...");
            Console.ReadLine();
            Pedidos();
        }
        static void EditarDatos()
        {
            int opcion = 0;
            while (true)
            {
                Console.Clear();
                Titulo();
                Console.WriteLine("Generar y retirar boleta");
                Console.WriteLine(linea);
                if (nombres.Count == 0)
                {
                    Console.WriteLine("No hay pedidos registrados.");
                    Console.Write("Presione Enter para continuar...");
                    Console.ReadLine();
                    Pedidos();
                    return;
                }
                Console.WriteLine($"{"Nro",-5} {"|Nombre",-21} {"|Total",-11} {"|Estado"}");
                Console.WriteLine(linea2);
                for (int i = 0; i < nombres.Count; i++)
                {
                    decimal total = pedPrecios[i].Split(',').Sum(x => decimal.Parse(x, System.Globalization.CultureInfo.InvariantCulture));
                    Console.WriteLine(opcion == i ? $"\u001b[7m{i + 1,-5} |{nombres[i],-20} |{total,-10:F2} |Activo\u001b[0m" : $"{i + 1,-5} |{nombres[i],-20} |{total,-10:F2} |Activo");
                }
                Console.WriteLine(linea2);
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.UpArrow || tecla.Key == ConsoleKey.W)
                {
                    opcion--;
                    if (opcion < 0) opcion = productos.Count;
                }
                else if (tecla.Key == ConsoleKey.DownArrow || tecla.Key == ConsoleKey.S)
                {
                    opcion++;
                    if (opcion > productos.Count) opcion = 0;
                }
                else if (tecla.Key == ConsoleKey.Enter)
                {
                    break;
                }
            }
            Console.Write("Ingrese el nuevo nombre (deje en blanco para mantener el actual):\n>/ ");
            string nuevoNombre = Console.ReadLine();
            Console.Write("Ingrese el nuevo número de contacto (deje en blanco para mantener el actual):\n>/ ");
            string nuevoNumero = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nuevoNombre)) nombres[opcion] = nuevoNombre;
            if (!string.IsNullOrWhiteSpace(nuevoNumero)) numeros[opcion] = nuevoNumero;
            GuardarPedidos();
            Console.WriteLine("Datos del cliente actualizados exitosamente.");
            Console.Write("Presione Enter para continuar...");
            Console.ReadLine();
            Pedidos();
        }
        static void AnularPedido()
        {
            int opcion = 0;
            while (true)
            {
                Console.Clear();
                Titulo();
                Console.WriteLine("Generar y retirar boleta");
                Console.WriteLine(linea);
                if (nombres.Count == 0)
                {
                    Console.WriteLine("No hay pedidos registrados.");
                    Console.Write("Presione Enter para continuar...");
                    Console.ReadLine();
                    Pedidos();
                    return;
                }
                Console.WriteLine($"{"Nro",-5} {"|Nombre",-21} {"|Total",-11} {"|Estado"}");
                Console.WriteLine(linea2);
                for (int i = 0; i < nombres.Count; i++)
                {
                    decimal total = pedPrecios[i].Split(',').Sum(x => decimal.Parse(x, System.Globalization.CultureInfo.InvariantCulture));
                    Console.WriteLine(opcion == i ? $"\u001b[7m{i + 1,-5} |{nombres[i],-20} |{total,-10:F2} |Activo\u001b[0m" : $"{i + 1,-5} |{nombres[i],-20} |{total,-10:F2} |Activo");
                }
                Console.WriteLine(linea2);
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.UpArrow || tecla.Key == ConsoleKey.W)
                {
                    opcion--;
                    if (opcion < 0) opcion = productos.Count;
                }
                else if (tecla.Key == ConsoleKey.DownArrow || tecla.Key == ConsoleKey.S)
                {
                    opcion++;
                    if (opcion > productos.Count) opcion = 0;
                }
                else if (tecla.Key == ConsoleKey.Enter)
                {
                    break;
                }
            }
            Console.Write($"¿Está seguro de que desea anular el pedido de {nombres[opcion]}? (s/n):\n>/ ");
            string confirmacion = Console.ReadLine();
            if (confirmacion?.ToLower() == "s")
            {
                var sepPro = pedProductos[opcion].Split(',');
                var sepSto = pedStocks[opcion].Split(',');

                for (int j = 0; j < sepPro.Length; j++)
                {
                    int index = productos.IndexOf(sepPro[j]);
                    if (index != -1)
                    {
                        stocks[index] += int.Parse(sepSto[j]);
                    }
                }
                nombres.RemoveAt(opcion);
                numeros.RemoveAt(opcion);
                pedProductos.RemoveAt(opcion);
                pedStocks.RemoveAt(opcion);
                pedPrecios.RemoveAt(opcion);
                GuardarPedidos();
                GuardarInventario();
                Console.WriteLine("Pedido anulado exitosamente.");
            }
            else
            {
                Console.WriteLine("Anulación de pedido cancelada.");
            }
            Console.Write("Presione Enter para continuar...");
            Console.ReadLine();
            Pedidos();
        }
        #endregion
    }
}
