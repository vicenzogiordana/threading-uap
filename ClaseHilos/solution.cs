using System.Threading;

namespace ClaseHilos
{
    internal class Producto
    {
        public string Nombre { get; set; }
        public decimal PrecioUnitarioDolares { get; set; }
        public int CantidadEnStock { get; set; }

        public Producto(string nombre, decimal precioUnitario, int cantidadEnStock)
        {
            Nombre = nombre;
            PrecioUnitarioDolares = precioUnitario;
            CantidadEnStock = cantidadEnStock;
        }
    }
    internal class Solution //reference: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/lock
    {
        static Barrier barrera = new Barrier(4, (b) =>
        {
        });
        public static Barrier barrera_
        {
            get { return barrera; }
        }

        static Mutex mutex = new Mutex();

        static List<Producto> productos = new List<Producto>
        {
            new Producto("Camisa", 10, 50),
            new Producto("Pantalón", 8, 30),
            new Producto("Zapatilla/Champión", 7, 20),
            new Producto("Campera", 25, 100),
            new Producto("Gorra", 16, 10)
        };

        static int precio_dolar = 500;

        static void Tarea1()
        {
            Console.WriteLine("Tarea 1 in line");
            mutex.WaitOne();
            Console.WriteLine("Tarea 1 has entered mutex");
            for (int i = 0; i < productos.Count; i++)
            {
                productos[i].CantidadEnStock += 10;
            }
            Console.WriteLine("Stock updated, leaving mutex");

            mutex.ReleaseMutex();
            barrera.SignalAndWait();

        }
        static void Tarea2()
        {
            Console.WriteLine("Tarea 2 in Line");
            mutex.WaitOne();
            Console.WriteLine("Tarea 2 has entered mutex");
            precio_dolar += 5;
            Console.WriteLine("Dolar updated, leaving mutex");
            mutex.ReleaseMutex();
            barrera.SignalAndWait();
        }
        static void Tarea3()
        {
            lock (productos)
            {
                Console.WriteLine("Thread 3 entering");
                decimal totalPrice = 0;
                for (int i = 0; i < productos.Count; i++)
                {
                    totalPrice += productos[i].PrecioUnitarioDolares;
                }
                foreach (Producto producto in productos)
                {
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("Producto:\t" + producto.Nombre);
                    Console.WriteLine("Stock:\t\t" + producto.CantidadEnStock);
                    Console.WriteLine("Precio (USD):\t" + producto.PrecioUnitarioDolares);
                    Console.WriteLine("Precio (ARS):\t" + producto.PrecioUnitarioDolares * precio_dolar);
                }
                Console.WriteLine("--------------------------------------------");

                Console.WriteLine(totalPrice);


            }
            barrera.SignalAndWait();
        }

        static void Tarea4()
        {
            lock (productos)
            {
                Console.WriteLine("Thread 4 entering");
                decimal totalPrice = 0;
                barrera.SignalAndWait();

                for (int i = 0; i < productos.Count; i++)
                {
                    productos[i].PrecioUnitarioDolares *= (decimal)1.1;
                }
                for (int i = 0; i < productos.Count; i++)
                {
                    totalPrice += productos[i].PrecioUnitarioDolares;
                }
                Console.WriteLine("===========================================Updated Price=====================================");
                foreach (Producto producto in productos)
                {
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("Producto:\t" + producto.Nombre);
                    Console.WriteLine("Stock:\t\t" + producto.CantidadEnStock);
                    Console.WriteLine("Precio (USD):\t" + producto.PrecioUnitarioDolares);
                    Console.WriteLine("In pesos:\t" + producto.PrecioUnitarioDolares * precio_dolar);

                }
                Console.WriteLine("--------------------------------------------");

                Console.WriteLine(totalPrice);


            }
        }

        internal static void Excecute()
        {
            Thread hilo = new Thread(new ThreadStart(Tarea1));
            Thread hilo2 = new Thread(new ThreadStart(Tarea2));
            Thread hilo3 = new Thread(new ThreadStart(Tarea3));
            Thread hilo4 = new Thread(new ThreadStart(Tarea4));

            hilo.Start();
            hilo2.Start();
            hilo3.Start();
            hilo4.Start();

            Thread hilo5 = new Thread(new ThreadStart(Tarea3));



        }
    }
}