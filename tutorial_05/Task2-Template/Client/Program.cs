using System;
using System.Collections.Generic;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Press any key to request the Sort service...");
        Console.ReadKey();
        SortSevice service = new SortSevice();
        var resut = service.Sort(new int[] {42, 5, 13, 1, 10, 21, 7, 3});
        Array.ForEach(resut, Console.WriteLine);
        Console.ReadKey();
    }
}
