class Program
{
    static async Task Main(string[] args)
    {
        string basePath = Path.Combine(Directory.GetCurrentDirectory(), "Music");
        if (!Directory.Exists(basePath))
        {
            Directory.CreateDirectory(basePath);
        }

        while (true)
        {
            Console.WriteLine("YouTube Music Manager");
            Console.WriteLine("1. Download Music");
            Console.WriteLine("1p. Download Music and open file");
            Console.WriteLine("2. Create Playlist");
            Console.WriteLine("3. Add to Playlist");
            Console.WriteLine("4. Remove from Playlist");
            Console.WriteLine("5. List Playlists");
            Console.WriteLine("6. Exit");
            Console.Write("Select an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await Music.Download(basePath);
                    break;
                case "1p":
                    await Music.Download(basePath, true);
                    break;
                case "2":
                    Playlist.Create(basePath);
                    break;
                case "3":
                    Playlist.AddMusic(basePath);
                    break;
                case "4":
                    Playlist.RemoveMusic(basePath);
                    break;
                case "5":
                    Playlist.Show(basePath);
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }












}
