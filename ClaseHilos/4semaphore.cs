using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaseHilos
{
    internal class _4semaphore
    {
        static Semaphore semaforo = new Semaphore(2, 2);
        static void HolaUAP()
        {
            semaforo.WaitOne();
            Console.WriteLine($"Proceso {Thread.CurrentThread.Name}");
            Thread.Sleep(4000);
            Console.WriteLine("Hola desde " + Thread.CurrentThread.Name);
            semaforo.Release();
        }
        internal static void Excecute()
        {
            Thread task1 = new Thread(HolaUAP);
            task1.Name = "Hilo 1";

            Thread task2 = new Thread(HolaUAP);
            task2.Name = "Hilo 2";

            Thread task3 = new Thread(() => Console.WriteLine("InlineAction"));

            //Excecute all tasks
            task1.Start();
            task2.Start();
            task3.Start();

            Console.WriteLine("Hola desde el hilo principal");
            Console.ReadLine();
        }

    }
}
