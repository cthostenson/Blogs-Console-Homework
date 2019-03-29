using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            var programIsRunning = true;
            while (programIsRunning == true)
            {
                Console.WriteLine("1) Display All Blogs");
                Console.WriteLine("2) Add Blogs");
                Console.WriteLine("3) Create Post");
                Console.WriteLine("4) Exit");
                int response = Convert.ToInt32(Console.ReadLine());
                if (response == 1)
                {
                    try
                    {
                        var db = new BloggingContext();

                        var query = db.Blogs.OrderBy(b => b.Name);

                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                    }
                }
                else if (response == 2)
                {
                    try
                    {
                        // Create and save a new Blog
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();

                        var blog = new Blog { Name = name };

                        var db = new BloggingContext();
                        db.AddBlog(blog);
                        logger.Info("Blog added - {name}", name);

                        // Display all Blogs from the database
                        var query = db.Blogs.OrderBy(b => b.Name);

                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                    }
                }
                else if (response == 3)
                {
                    try
                    {
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        List<int> blogIds = new List<int> { };
                        Console.WriteLine("All blogs in the database:");

                        foreach (var item in query)
                        {
                            Console.WriteLine(item.BlogId.ToString() + ") " + item.Name);
                            blogIds.Add(item.BlogId);
                        }

                        var waitingForValidAnswer = true;
                        while (waitingForValidAnswer == true)
                        {
                            Console.WriteLine("\nWhich Blog Would You Like To Add To?");
                            int answer = Convert.ToInt32(Console.ReadLine());
                            int chosenBlogId = 0;
                            for (int i = 0; i < blogIds.Count; i++)
                            {
                                if (answer == blogIds.ElementAt(i))
                                {
                                    waitingForValidAnswer = false;
                                    chosenBlogId = blogIds.IndexOf(blogIds.ElementAt(i));
                                }
                            }

                            if (waitingForValidAnswer == true)
                            {
                                Console.WriteLine("Please Enter A Valid ID");
                            }
                            else
                            {
                                try
                                {
                                    // Create and save a new Blog
                                    Console.Write("Enter a title for a new Post:");
                                    var title = Console.ReadLine();
                                    Console.WriteLine("Enter The Post's Content:");
                                    var content = Console.ReadLine();

                                    var newPostId = 1;

                                    try
                                    {
                                        newPostId = db.Posts.Select(p => p.PostId).Last() + 1;
                                    }
                                    catch// (Exception ex)
                                    {
                                        Console.WriteLine("No Posts In Database");
                                    }

                                    var post = new Post { Title = title, Content = content, BlogId = chosenBlogId + 1, PostId = newPostId };

                                    db.AddPost(post);
                                    logger.Info("Post added - {title}", post);

                                    // Display all Blogs from the database
                                    var query2 = db.Posts.OrderBy(b => b.Title);

                                    Console.WriteLine("All posts in the database:");
                                    foreach (var item in query2)
                                    {
                                        Console.WriteLine(item.Title);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.Error(ex.Message);
                                }

                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                    }
                }
                else if (response == 4)
                {
                    programIsRunning = false;
                }

            }

            logger.Info("Program ended");
        }
    }
}
