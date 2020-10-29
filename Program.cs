using System;
using System.Threading;

namespace ThreadSynchronization
{
    public class Program
    {
        static SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        static Semaphore _semaphore = new Semaphore(1, 1);
        static Mutex _mutex = new Mutex();
        static AutoResetEvent _are = new AutoResetEvent(false);
        static ManualResetEvent _mre = new ManualResetEvent(false);
        private static object _locker = new object();

        public static void Main(string[] args)
        {
            //starting writer thread
            //new Thread(Write).Start();

            for (int i = 0; i < 5; i++)
            {
                //new Thread(Read).Start();
                new Thread(WriteSemaphoreSlim).Start();
            }

            //Thread.Sleep(3000); //except
            //_mutex.ReleaseMutex(); //except

            //auto
            //Thread.Sleep(3000);
            //_are.Set();

            /* Lock and monitor
            for(int i = 0; i < 5; i++)
            {
                new Thread(Dowork).Start(); // Lock and Monitor
            }
            */

            Console.ReadKey();
        }

        
        public static void Dowork()
        {
            lock(_locker)
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} starting...");
                Thread.Sleep(2000);
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} completed...");
            }

            try
            {
                Monitor.Enter(_locker);
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} starting...");
                Thread.Sleep(2000);
                //throw new Exception();
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} completed...");
            } catch (Exception e)
            {
                //error logger
            } finally
            {
                Monitor.Exit(_locker);
            }
        }

        /* Semaphore Slim */
        public static void WriteSemaphoreSlim()
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Waiting...");
            _semaphoreSlim.Wait();
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Writing...");
            Thread.Sleep(2000);
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Writing Completed...");
            _semaphoreSlim.Release();
        }

        /* Semaphore */
        public static void WriteSemaphore()
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Waiting...");
            _semaphore.WaitOne();
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Writing...");
            Thread.Sleep(2000);
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Writing Completed...");
            _semaphore.Release();
        }

        /* Mutex */
        public static void WriteMutex()
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Waiting...");
            _mutex.WaitOne();
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Writing...");
            Thread.Sleep(2000);
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Writing Completed...");
            _mutex.ReleaseMutex();
        }

        /* Auto event reset */
        public static void WriteAuto()
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Waiting...");
            _are.WaitOne();
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Writing...");
            Thread.Sleep(2000);
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Writing Completed...");
            _are.Set();
        }

        /* Manual event reset */
        public static void Write()
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Writing...");
            _mre.Reset();
            Thread.Sleep(2000);
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Writing Completed...");
            _mre.Set();
        }

        public static void Read()
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Waiting...");
            _mre.WaitOne();
            Thread.Sleep(2000);
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Reading Completed...");
        }
    }
}