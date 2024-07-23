class Playlist
{

    public static void Create(string basePath)
    {
        Console.Write("Enter playlist name: ");
        string playlistName = Console.ReadLine();
        string playlistPath = Path.Combine(basePath, playlistName);

        if (!Directory.Exists(playlistPath))
        {
            Directory.CreateDirectory(playlistPath);
            Console.WriteLine("Playlist created.");
        }
        else
        {
            Console.WriteLine("Playlist already exists.");
        }
    }

    public static void AddMusic(string basePath)
    {
        Console.Write("Enter playlist name: ");
        string playlistName = Console.ReadLine();
        string playlistPath = Path.Combine(basePath, playlistName);

        if (Directory.Exists(playlistPath))
        {
            Console.Write("Enter music file name to add: ");
            string fileName = Console.ReadLine();
            string filePath = Path.Combine(basePath, fileName);

            if (File.Exists(filePath))
            {
                File.Copy(filePath, Path.Combine(playlistPath, fileName), true);
                Console.WriteLine("File added to playlist.");
            }
            else
            {
                Console.WriteLine("File does not exist.");
            }
        }
        else
        {
            Console.WriteLine("Playlist does not exist.");
        }
    }


    public static void RemoveMusic(string basePath)
    {
        Console.Write("Enter playlist name: ");
        string playlistName = Console.ReadLine();
        string playlistPath = Path.Combine(basePath, playlistName);

        if (Directory.Exists(playlistPath))
        {
            Console.Write("Enter music file name to remove: ");
            string fileName = Console.ReadLine();
            string filePath = Path.Combine(playlistPath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Console.WriteLine("File removed from playlist.");
            }
            else
            {
                Console.WriteLine("File does not exist.");
            }
        }
        else
        {
            Console.WriteLine("Playlist does not exist.");
        }
    }


    public static void Show(string basePath)
    {
        var directories = Directory.GetDirectories(basePath);
        if (directories.Length == 0)
        {
            Console.WriteLine("No playlists found.");
        }
        else
        {
            Console.WriteLine("Playlists:");
            foreach (var dir in directories)
            {
                Console.WriteLine(Path.GetFileName(dir));
            }
        }
    }

}