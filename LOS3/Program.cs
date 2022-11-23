using System;
using System.ComponentModel;
using System.Threading;


class Conveyor
{
    public int timer = 100;

    private Queue<int> queue = new Queue<int>();
    private Random rnd = new Random();
    private object locker = new();

    private bool sleep = false;

    private Thread manufacturer1;
    private Thread manufacturer2;
    private Thread manufacturer3;
    private Thread consumer1;
    private Thread consumer2;


    // Добавляет элемент в очередь и выводит информацию об этой операции в консоль.
    // Является методом производителя.
    private void Work(string name)
    {
        if (queue.Count >= 100) sleep = true;
        if (queue.Count <= 80) sleep = false;

        if (!sleep)
        {
            lock (locker)
            {
                int temp = rnd.Next(1, 101);
                Console.WriteLine("Thread {0} adding element: {1}\t\tQueue count: {2}", name, temp, queue.Count + 1);
                queue.Enqueue(temp);
            }
        }
        
    }

    // Выводит элемент из очереди и выводит информацию об этой операции в консоль.
    // Является методом потребителя.
    private void GetWork(string name)
    {
        lock (locker)
        {
            if (queue.Count > 0)
            {
                int temp = queue.Dequeue();
                Console.WriteLine("Thread {0} getting element: {1}\t\tQueue count: {2}", name, temp, queue.Count);
            }
        }
    }


    public void Start()
    {
        manufacturer1 = new Thread(() => {
            while (true)
            {
                Work("Manufacturer First");
                Thread.Sleep(timer);
            }
        });
        manufacturer2 = new Thread(() => {
            while (true)
            {
                Work("Manufacturer Second");
                Thread.Sleep(timer);
            }
        });
        manufacturer3 = new Thread(() => {
            while (true)
            {
                Work("Manufacturer Third");
                Thread.Sleep(timer);
            }
        });
        consumer1 = new Thread(() => {
            while (true)
            {
                GetWork("Consumer First");
                Thread.Sleep(timer);
            }
        });
        consumer2 = new Thread(() => {
            while (true)
            {
                GetWork("Consumer Second");
                Thread.Sleep(timer);
            }
        });


        manufacturer1.Start();
        manufacturer2.Start();
        manufacturer3.Start();
        consumer1.Start();
        consumer2.Start();

    }
}



class Program
{

    static void Main()
    {

        Conveyor con = new Conveyor();

        con.timer = 10;
        con.Start();
        
        while (true)
        {
            string end = Console.ReadLine();
            if (end == "q") Environment.Exit(0);
        }

    }
}