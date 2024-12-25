using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Xml.Xsl;

public class Day23
{


    private List<Link> _connections = [];
    private HashSet<string> _uniqueNames = [];
    public Day23()
    {
        var lines = File.ReadAllLines("Day23\\input.txt");



        foreach (var line in lines)
        {
            var splits = line.Split("-");

            var computerNameA = splits[0];
            var computerNameB = splits[1];

            _uniqueNames.Add(computerNameA);
            _uniqueNames.Add(computerNameB);

            _connections.Add(new Link()
            {
                ConnectionString = line,
                Left = computerNameA,
                Right = computerNameB,
            });

        }


    }

    public void Run()
    {

        // Worst p1 ever :)
        List<string> matches = [];
        foreach (var link in _uniqueNames)
        {
            var activeConnections = _connections.Where(l => l.ConnectionString.Contains(link)).ToList();

            var others = new HashSet<string>();
            var rights = activeConnections.Where(l => l.Left == link).Select(l => l.Right);
            var lefts = activeConnections.Where(l => l.Right == link).Select(l => l.Left);
            others.UnionWith(lefts);
            others.UnionWith(rights);

            foreach (var other in others)
            {
                var notThisOthers = others.Where(o => o != other);
                foreach (var otherother in notThisOthers)
                {
                    if (_connections.Any(c => c.ConnectionString.Contains(other) && c.ConnectionString.Contains(otherother)))

                    {
                        matches.Add($",{link},{other},{otherother}");
                    }

                }
            }

        }

        var filter = matches.Where(m => m.Contains(",t")).ToList();
        Console.WriteLine(filter.Count / 6);
        //     var computers = new List<Computer>();
        //     foreach (var name in _uniqueNames)
        //     {
        //         var computer = new Computer() { Name = name };
        //         computers.Add(computer);
        //     }

        //     foreach (var computer in computers)
        //     {
        //         var uniqueConnections = new HashSet<string>();
        //         var computerConnections = _connections.Where(c => c.ConnectionString.Contains(computer.Name));

        //         uniqueConnections.UnionWith(computerConnections.Where(cc => cc.Right == computer.Name).Select(cc => cc.Left));
        //         uniqueConnections.UnionWith(computerConnections.Where(cc => cc.Left == computer.Name).Select(cc => cc.Right));

        //         var connectionsToAdd = computers.Where(c => uniqueConnections.Contains(c.Name)).ToList();

        //         computer.ConnectedComputers = connectionsToAdd.ToHashSet();
        //     }

        //     var start = "";
        //     var maxDepth = 0;
        //     string ignores = "";
        //     foreach (var computer in computers)
        //     {
        //         ignores += $"|{computer.Name}|";
        //         if (computer.Name == "ka")
        //             Console.WriteLine();
        //         var depth = computer.FindDepth(ignores, computer.Name, 1);
        //         if (depth > maxDepth)
        //         {
        //             maxDepth = depth;
        //             start = computer.Name;
        //         }
        //         else if (depth == maxDepth)
        //         {
        //             start += "," + computer.Name;
        //         }
        //     }

        // }


    }

    public class Link
    {
        public Link()
        {

        }
        public required string ConnectionString { get; set; }
        public required string Left { get; set; }
        public required string Right { get; set; }

    }

    public class Computer
    {
        public HashSet<Computer> ConnectedComputers = [];
        public required string Name { get; set; }

        internal int FindDepth(string ignores, string name, int level)
        {
            if (Name == name && level != 1)
                return level;

            int maxDepth = -1;
            Console.WriteLine("Standing in: " + Name);
            var connectedNames = string.Join(',', ConnectedComputers.Select(c => c.Name));
            Console.WriteLine("Lookin in: " + connectedNames);
            foreach (var computer in ConnectedComputers)
            {
                if (computer.Name == name)
                {
                    maxDepth = level;
                    Console.WriteLine(computer.Name);
                }
                if (ignores.Contains(computer.Name))
                    continue;
                else
                {
                    ignores += $"|{Name}|";
                    var depth = computer.FindDepth(ignores, name, level + 1);
                    if (depth > maxDepth)
                        maxDepth = depth;
                }
            }
            if (maxDepth > 0)
                return maxDepth;
            return maxDepth;
        }
    }
}