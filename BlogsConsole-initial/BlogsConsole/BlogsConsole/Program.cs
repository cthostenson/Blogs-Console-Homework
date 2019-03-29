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
                Console.WriteLine("4) Display Posts");
                Console.WriteLine("5) Exit");
                int response = Convert.ToInt32(Console.ReadLine());
                if (response == 1)
                {
                    Console.WriteLine("\n\n\n\n\n\n\n\n");
                    logger.Info("Option 1 Selected");
                    try
                    {
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.Name);
                        var counter = db.Blogs.Count();

                        if (counter > 0)
                        {
                            Console.WriteLine("-- " + counter + " blog(s) found --");
                            Console.WriteLine("All blogs in the database:");
                            foreach (var item in query)
                            {
                                Console.WriteLine(item.Name);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No Blogs In Database");
                            logger.Info("No Blogs In Database");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                    }
                }
                else if (response == 2)
                {
                    Console.WriteLine("\n\n\n\n\n\n\n\n");
                    logger.Info("Option 2 Selected");
                    try
                    {

                        // Create and save a new Blog
                        var waitingForBlogName = true;
                        var name = "";
                        while (waitingForBlogName == true)
                        {
                            Console.Write("Enter a name for a new Blog: ");
                            name = Console.ReadLine();

                            if (name.Trim() == "")
                            {
                                logger.Info("Empty Blog Name Attempt");
                                Console.WriteLine("Please Enter A Valid Blog Name");
                            }
                            else
                            {
                                waitingForBlogName = false;
                            }
                        }

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
                    Console.WriteLine("\n\n\n\n\n\n\n\n");
                    logger.Info("Option 3 Selected");
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
                                    var waitingForValidPostTitle = true;
                                    var title = "";
                                    while (waitingForValidPostTitle == true)
                                    {
                                        Console.Write("Enter a title for a new Post:");
                                        title = Console.ReadLine();
                                        if (title.Trim() == "")
                                        {
                                            logger.Info("Attempt To Add Post Without Title");
                                            Console.WriteLine("Please Enter A Post Title");
                                        }
                                        else
                                        {
                                            waitingForValidPostTitle = false;
                                        }
                                    }

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
                    Console.WriteLine("\n\n\n\n\n\n\n\n");
                    logger.Info("Option 4 Selected");
                    var waitingForValidPostResponse = true;
                    while (waitingForValidPostResponse == true)
                    {

                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        List<int> blogIds = new List<int> { };

                        Console.WriteLine("What Would You Like To Display:");
                        Console.WriteLine("0) Display All Posts");

                        foreach (var item in query)
                        {
                            Console.WriteLine(item.BlogId.ToString() + ") " + item.Name);
                            blogIds.Add(item.BlogId);
                        }

                        var postResponse = Convert.ToInt32(Console.ReadLine());
                        if (postResponse == 0)
                        {
                            var allPostQuery = db.Posts.OrderBy(b => b.Title);
                            var counter = db.Blogs.Count();

                            if (counter > 0)
                            {
                                Console.WriteLine("-- " + counter + " post(s) found --");
                                Console.WriteLine("All posts in the database:");
                                foreach (var item in allPostQuery)
                                {
                                    Console.WriteLine(item.PostId + ") " + item.Title + " - " + item.Content);
                                }
                            }
                            else
                            {
                                Console.WriteLine("No Posts In Database");
                                logger.Info("No Posts In Database");
                            }

                            waitingForValidPostResponse = false;

                        }
                        else
                        {
                            int postChosenBlogId = 0;
                            for (int i = 0; i < blogIds.Count; i++)
                            {
                                if (postResponse == blogIds.ElementAt(i))
                                {
                                    waitingForValidPostResponse = false;
                                    postChosenBlogId = blogIds.IndexOf(blogIds.ElementAt(i));
                                }
                            }

                            if (waitingForValidPostResponse == true)
                            {
                                Console.WriteLine("Please Enter A Valid ID");
                            }
                            else
                            {
                                var specificPostsInBlogQuery = db.Posts.OrderBy(b => b.Title).Where(b => b.BlogId == postChosenBlogId);
                                foreach (var item in specificPostsInBlogQuery)
                                {
                                    Console.WriteLine(item.PostId + ") " + item.Title + " - " + item.Content);
                                }

                            }

                        }



                    }
                }
                else if (response == 5)
                {
                    logger.Info("Option 5 Selected");
                    programIsRunning = false;
                }

            }

            logger.Info("Program ended");
        }
    }

}
