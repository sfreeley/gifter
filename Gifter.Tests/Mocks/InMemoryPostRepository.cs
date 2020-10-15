using System;
using System.Collections.Generic;
using System.Linq;
using Gifter.Models;
using Gifter.Repositories;

namespace Gifter.Tests.Mocks
{
    //implements the IPostRepository (which holds all the methods that we will be testing)
    //this is the reason why we create both an interface and class for each repository so we have the flexibility to use mock repositories for unit tests
    class InMemoryPostRepository : IPostRepository
    {
        //this will hold our data which will be a list of posts (will not have to actually go to the db to get the posts)
        private readonly List<Post> _data;

        //this is how we access the readonly field
        public List<Post> InternalData
        {
            get
            {
                return _data;
            }
        }

        //constructor that accepts a list of posts, which then the field will be assigned this value
        public InMemoryPostRepository(List<Post> startingData)
        {
            _data = startingData;
        }

        public void Add(Post post)
        {
            //(mimicking what happens when creating a new object?)
            //gets last post in the list of posts
            var lastPost = _data.Last();
            //sets the post we are adding to the postId one more than the last post
            post.Id = lastPost.Id + 1;
            //adding the post 
            _data.Add(post);
        }

        public void Delete(int id)
        {
            //find the first match and only unique match that matches the post id you want to delete
            var postTodelete = _data.FirstOrDefault(p => p.Id == id);
            //if there isn't a post to delete, return
            if (postTodelete == null)
            {
                return;
            }

            //remove that post from the list of posts
            _data.Remove(postTodelete);
        }

        public List<Post> GetAll()
        {
            return _data;
        }

        public Post GetById(int id)
        {
            return _data.FirstOrDefault(p => p.Id == id);
        }

        public void Update(Post post)
        {
            //get the first and unique post by post Id
            var currentPost = _data.FirstOrDefault(p => p.Id == post.Id);
            //if that post is null, return
            if (currentPost == null)
            {
                return;
            }

            //when editing, the values of each property will be whatever the value of the object's properties are that is passed in the method
            currentPost.Caption = post.Caption;
            currentPost.Title = post.Title;
            currentPost.DateCreated = post.DateCreated;
            currentPost.ImageUrl = post.ImageUrl;
            currentPost.UserProfileId = post.UserProfileId;
        }

        public List<Post> Search(string criterion, bool sortDescending)
        {
            throw new NotImplementedException();
        }

        public List<Post> GetAllWithComments()
        {
            throw new NotImplementedException();
        }

        public Post GetPostByIdWithComments(int id)
        {
            throw new NotImplementedException();
        }

        public List<Post> Hottest(DateTime criterion, bool sortDescending)
        {
            throw new NotImplementedException();
        }
    }
}