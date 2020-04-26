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
                    Console.WriteLine("1) Display All Blogs");
                    Console.WriteLine("2) Create Blog");
                    Console.WriteLine("3) Create Post");
                    Console.WriteLine("4) Display Posts");
                    Console.WriteLine("Anything Else To Exit");
                    choice = Console.ReadLine();
                    logger.Info("User Choice:"+choice);
                    if (choice == "1")
                    {
                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        Console.WriteLine(query.Count() + " Blogs Returned");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Name);
                        }
                    }
                    if (choice.Equals("2"))
                    {
                        // Create and save a new Blog
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();
                        if (name.Length == 0)
                        {
                            logger.Error("Blog Not Added-Blog Needs A Name");
                        }
                        else
                        {
                            var blog = new Blog { Name = name };

                            db.AddBlog(blog);
                            logger.Info("Blog added - {name}", name);
                        }
                    }
                    if (choice.Equals("3"))
                    {
                        var query = db.Blogs.OrderBy(b => b.BlogId);

                        Console.WriteLine("Choose blog to write to:");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.BlogId + " - " + item.Name);
                        }
                        try
                        {
                            int blogid = int.Parse(Console.ReadLine());
                            if (blogid > query.Count() || blogid <= 0)
                            {
                                logger.Error("Post Not Added-Input Was Not a Valid Blog ID");
                            }
                            else
                            {
                                Blog myBlog = query.ToList()[blogid];
                                Console.WriteLine("Title For Post:");
                                string title = Console.ReadLine();
                                if (title.Length == 0)
                                {
                                    logger.Error("Post Not Added-Post Must Have A Title");
                                }
                                Console.WriteLine("Content of Post:");
                                string content = Console.ReadLine();
                                Post post = new Post { Title = title, Content = content, BlogId = blogid, Blog = myBlog };
                                db.AddPost(post);
                            }
                        }
                        catch
                        {
                            logger.Error("Post Not Added-Input Was Not A Valid Number");
                        }

                    }
                    if (choice == "4")
                    {
                        var query = db.Blogs.OrderBy(b => b.BlogId);

                        Console.WriteLine("Choose blog to Display Posts:");
                        Console.WriteLine("0 - All Blogs");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.BlogId + " - " + item.Name);
                        }
                        int blogid = 0;
                        try
                        {
                            blogid=int.Parse(Console.ReadLine());
                            if (blogid < 0 || blogid > query.Count())
                            {
                                logger.Info("Input Was Not a Valid Blog ID-Displaying All Posts");
                                blogid = 0;
                            }
                        }
                        catch
                        {
                            logger.Info("Input Was Not a Valid Number-Displaying All Posts");
                        }
                        var posts = db.Posts.Where(p => p.BlogId == blogid);
                        if (blogid == '0')
                        {
                            posts = db.Posts.OrderBy(p => p.BlogId);
                        }
                        Console.WriteLine(posts.Count() + " Posts Returned");
                        foreach(var post in posts)
                        {
                            Console.WriteLine($"Blog: {post.Blog}\nTitle: {post.Title}\nContent: {post.Content}");
                        }
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