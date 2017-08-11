using System;

public class Class1
{
	public Class1()
	{
        string[][] Array = new string[100][];

        for (int i = 0; i < 100; i++)
            Array[i] = new string[2] { "ROLI:" + i, "DEVESH:" + i };

        var data = (from arr in Array select new { fileName = arr[0], filePath = arr[1] });

        dataGridView1.ItemsSource = data.ToList();
    }
}
