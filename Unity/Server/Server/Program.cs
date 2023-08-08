// See https://aka.ms/new-console-template for more information

// Console.WriteLine("123");
//
// Console.WriteLine("Hello, World!");
// string str = Console.ReadLine()??"";
// Console.WriteLine(str);
// Console.ReadLine();


Thread netThread = new Thread(() =>
{
	NetManager.StartLoop(8888);
});
netThread.IsBackground = true;
netThread.Start();




//updata启动方案
while (true)
{
	Thread.Sleep(1);
	try
	{
		NetManager.Updata();
	}
	catch (Exception e)
	{
		Console.WriteLine(e);
	}
}
Console.WriteLine("结束");
// netThread.Abort();