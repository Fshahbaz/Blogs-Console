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

                Console.WriteLine("1) Display All Blogs");
                Console.WriteLine("2) Add Blogs");
                Console.WriteLine("3) Create the Post");

                string choice = Console.ReadLine();

                BloggingContext db = new BloggingContext();

                if (choice == "1")
                {
                    // Display all Blogs from the database
                    IOrderedQueryable<Blog> query = db.Blogs.OrderBy(b => b.Name);

                    Console.WriteLine("All blogs in the database:");
                    foreach (Blog item in query)
                    {
                        Console.WriteLine(item.Name);

                    }
                } else if (choice == "2")
                {
                    // Create and save a new Blog
                    Console.Write("Enter a name for a new Blog: ");
                    string name = Console.ReadLine();

                    Blog blog = new Blog { Name = name };

                    db.AddBlog(blog);
                    logger.Info("Blog added - {name}", name);
                } else if (choice == "3")
                {
                    Console.WriteLine("Enter the name of the Blog you are posting to: ");
                    Boolean blogExists = false;
                    string userBlog = Console.ReadLine();
                    IOrderedQueryable<Blog> query = db.Blogs.OrderBy(b => b.Name);
                    Blog blog = new Blog();
                    foreach (Blog item in query)
                    {
                        if (item.Name == userBlog)
                        {
                            blogExists = true;
                            blog = item;
                        }
                    }
                    if (blogExists == true)
                    {
                        Console.WriteLine("Enter the Post Title: ");
                        string title = Console.ReadLine();
                        Console.WriteLine("Enter the Post Content: ");
                        string content = Console.ReadLine();
                        Post post = new Post { Title = title, Content = content, Blog = blog };
                        logger.Info("Post added - {title}", title);
                        db.AddPost(post);
                    } else
                    {
                        Console.WriteLine("ERROR: Entered blog does not exsist.");
                    }
                }
            }

            catch (Exception ex)
            {
                logger.Error(ex.Message);

            }
            logger.Info("Program ended");
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
