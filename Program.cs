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
            Boolean programRunning = true;
            logger.Info("Program started");
            try
            {
                while (programRunning)
                {
                    Console.WriteLine("1) Display All Blogs");
                    Console.WriteLine("2) Add Blogs");
                    Console.WriteLine("3) Create the Post");
                    Console.WriteLine("4) Display Posts");
                    Console.WriteLine("Enter q to quit");

                    string choice = Console.ReadLine();

                    BloggingContext db = new BloggingContext();

                    Console.Clear();
                    if (choice == "1")
                    {
                        logger.Info("Option 1 selected");
                        // Display all Blogs from the database
                        IOrderedQueryable<Blog> query = db.Blogs.OrderBy(b => b.Name);
                        int blogCount = db.Blogs.Count();
                        Console.WriteLine(blogCount + " Blogs returned");
                        foreach (Blog item in query)
                        {
                            Console.WriteLine(item.Name);
                        }
                        Console.WriteLine();
                    }
                    else if (choice == "2")
                    {
                        logger.Info("Option 2 selected");
                        // Create and save a new Blog
                        Console.Write("Enter a name for a new Blog: ");

                        string name = Console.ReadLine();

                        if (name.Length == 0)
                        {
                            logger.Info("Blog name cannot be null");
                        }
                        else
                        {
                            Blog blog = new Blog { Name = name };
                            db.AddBlog(blog);
                            logger.Info("Blog added - {name}", name);
                        }
                        Console.WriteLine();
                    }
                    else if (choice == "3")
                    {
                        logger.Info("Option 3 selected");
                        int blogCount = db.Blogs.Count();
                        if (blogCount > 0) {
                            Console.WriteLine("Select the blog you would like to post to: ");
                            IOrderedQueryable<Blog> query = db.Blogs.OrderBy(b => b.BlogId);
                            foreach (Blog item in query)
                            {
                                Console.WriteLine(item.BlogId+")" + " " + item.Name);
                            }
                            string userOption = Console.ReadLine();
                            int val = 0;
                            if (!(int.TryParse(userOption, out val)))
                            {
                                logger.Info("Invalid Blog Id");
                            }
                            else 
                            {
                                int id = int.Parse(userOption);
                                int blogIDMatchCount = query.Where(b => b.BlogId.Equals(id)).Count();
                                if (blogIDMatchCount > 0)
                                {
                                    Console.WriteLine("Enter the Post Title: ");
                                    string title = Console.ReadLine();
                                    if (title.Length == 0)
                                    {
                                        logger.Info("Post title cannot be null");
                                    } else
                                    {
                                        Console.WriteLine("Enter the Post Content: ");
                                        string content = Console.ReadLine();
                                        Post post = new Post { Title = title, Content = content, BlogId = id };
                                        logger.Info("Post added - {title}", title);
                                        db.AddPost(post);
                                    }
                                }
                                else
                                {
                                    logger.Info("There are no Blogs saved with that Id");
                                }
                            }
                        } else
                        {
                            logger.Info("No blogs found. Use option 2 to add a blog.");
                        }
                        Console.WriteLine();
                    }
                    else if (choice == "4")
                    {

                        logger.Info("Option 4 selected");
                        Console.WriteLine("Select the blog's posts to display: ");
                        IOrderedQueryable<Blog> blogQuery = db.Blogs.OrderBy(b => b.BlogId);
                        Console.WriteLine("0) Posts from all blogs");
                        foreach (Blog item in blogQuery)
                        {
                            Console.WriteLine(item.BlogId + ")" + " Posts from " + item.Name);
                        }
                        string userOption = Console.ReadLine();
                        if (userOption == "0")
                        {
                            int postCount = db.Posts.Count();
                            Console.WriteLine(postCount + " post(s) returned");
                            IOrderedQueryable<Post> postQuery = db.Posts.OrderBy(b => b.PostId);
                            foreach (Post item in postQuery)
                            {
                                Console.WriteLine("Blog: " + item.Blog.Name);
                                Console.WriteLine("Title: " + item.Title);
                                Console.WriteLine("Content: " + item.Content);
                                Console.WriteLine();
                            }
                        }
                        int val = 0;
                        if ((int.TryParse(userOption, out val)))
                        {
                            int optionInt = int.Parse(userOption);
                            IQueryable<Post> postQuery = db.Posts.Where(p => p.BlogId.Equals(optionInt));
                            int postCount = postQuery.Count();
                            Console.WriteLine(postCount + " post(s) returned");
                            if (postCount > 0)
                            {
                                foreach (Post item in postQuery)
                                {
                                    Console.WriteLine("Blog: " + item.Blog.Name);
                                    Console.WriteLine("Title: " + item.Title);
                                    Console.WriteLine("Content: " + item.Content);
                                    Console.WriteLine();
                                }
                            }
                        }
                        else
                        {
                            logger.Info("Invalid Blog Id");
                        }
                    }
                    else if (choice == "q")
                    {
                        programRunning = false;
                    }
                    else
                    {
                        logger.Info("Invalid option");
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
