using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;

public class Day9
{

    private int _xLen;
    private int?[] _disk;
    private int[] map;
    private int map_size;
    private int disk_size;

    public Day9()
    {
        var diskstring = File.ReadAllText("Day9\\input.txt");

        map = diskstring.Select(c => int.Parse(c.ToString())).ToArray();


        disk_size = map.Sum();

        _disk = new int?[disk_size];

        var map_len = map.Length;
        var disk_pos = 0;
        var file_id = 0;
        for (int i = 0; i < map_len; i++)
        {
            var map_val = map[i];
            if (i % 2 == 0)
            {
                for (int j = 0; j < map_val; j++)
                {
                    _disk[disk_pos] = file_id;
                    disk_pos++;
                }
                file_id++;
            }
            else
            {
                for (int j = 0; j < map_val; j++)
                {
                    _disk[disk_pos] = null;
                    disk_pos++;
                }
            }
        }

    }

    public void Run()
    {
        //P1
        // var moved = true;
        // var disk_pos = disk_size - 1;
        // while (moved)
        // {
        //     var tmp = _disk[disk_pos];

        //     if (tmp == null)
        //     {
        //         disk_pos--;
        //         continue;
        //     }
        //     var firstempty = Array.IndexOf(_disk, null);
        //     if (firstempty > disk_pos)
        //     {
        //         break;
        //     }

        //     _disk[firstempty] = tmp;
        //     _disk[disk_pos] = null;
        //     moved = true;

        // }
        // var sum = CheckSum();

        // Console.WriteLine(sum.ToString());


        //P2
     
        var movedFiles = new List<int>();
        var moved = true;
        var disk_pos = disk_size - 1;
        while (disk_pos > 0)
        {
            var tmp = _disk[disk_pos];

            if (tmp == null)
            {
                disk_pos--;
                continue;
            }
            if (movedFiles.Contains(tmp.Value))
            {
                disk_pos--;
                continue;
            }
            movedFiles.Add(tmp.Value);
            var size = FindItemsInARow(disk_pos, tmp);
            var firstempty = FindNullsInARow(size);
            if (firstempty != -1 && firstempty < disk_pos - size)
            {

                for (int i = 0; i < size; i++)
                {
                    _disk[firstempty + i] = tmp;
                    _disk[disk_pos - i] = null;
                }
            }
            disk_pos -= size;
     
        }
        var sum = CheckSum();

        Console.WriteLine(sum.ToString());

    }

    public long CheckSum()
    {
        long sum = 0;
        for (int i = 0; i < disk_size; i++)
        {
            var val = _disk[i];
            if (val == null)
                continue;
            else
                sum += val.Value * i;
        }

        return sum;
    }

    public int FindEmptyIndex(int size)
    {
        for (int i = 1; i < map_size; i += 2)
        {
            if (map[i] >= size)
                return i;
        }
        return -1;
    }
    public int FindNullsInARow(int size)
    {

        var index = 0;
        var nullsInARow = 0;
        var segmentStart = 0;
        while (index < disk_size)
        {
            if (_disk[index] == null)
            {
                segmentStart = index;
                while (index < disk_size && _disk[index] == null)
                {
                    nullsInARow++;
                    index++;
                    if (nullsInARow == size)
                        return segmentStart;
                }
                nullsInARow = 0;

            }
            index++;
        }
        return -1;
    }
    public int FindItemsInARow(int index, int? whatToLookFor)
    {
        var noOfItems = 0;
        while (index >= 0 && _disk[index] == whatToLookFor)
        {
            noOfItems++;
            index--;
        }
        return noOfItems;
    }

    public void Print()
    {

        var sb = new StringBuilder();

        foreach (var space in _disk)
        {
            if (space == null)
                sb.Append('.');
            else
                sb.Append(space.ToString());
        }
        Console.WriteLine(sb.ToString());

    }
}
