using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            try
            {
                var db = new BloggingContext();
                string choice = "";
                do
                {
                    Console.WriteLine("1) Create Blog");
                    Console.WriteLine("2) Create Post");
                    Console.WriteLine("Anything Else To Exit");
                    choice = Console.ReadLine();
                    logger.Info("User Choice:"+choice);
                    if (choice.Equals("1"))
                    {
                        // Create and save a new Blog
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();

                        var blog = new Blog { Name = name };

                        db.AddBlog(blog);
                        logger.Info("Blog added - {name}", name);
                    }
                    if (choice.Equals("2"))
                    {
                        // Display all Blogs from the database
                        var query = db.Blogs.OrderBy(b => b.BlogId);

                        Console.WriteLine("Choose blog to write to:");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.BlogId + " - " + item.Name);
                        }
                        int blogid = int.Parse(Console.ReadLine());
                        Blog myBlog = query.ToList()[blogid];
                        Console.WriteLine("Title For Post:");
                        string title = Console.ReadLine();
                        Console.WriteLine("Content of Post:");
                        string content = Console.ReadLine();
                        Post post = new Post { Title = title, Content = content, BlogId = blogid, Blog = myBlog };
                        db.AddPost(post);

                    }
                } while (choice == "1" || choice == "2");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
    }
}