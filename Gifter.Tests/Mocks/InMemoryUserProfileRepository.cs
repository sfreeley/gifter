using Gifter.Models;
using Gifter.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gifter.Tests.Mocks
{
    class InMemoryUserProfileRepository : IUserProfileRepository
    {
        private readonly List<UserProfile> _data;

        public List<UserProfile> InternalData
        {
            get
            {
                return _data;
            }
        }

        public InMemoryUserProfileRepository(List<UserProfile> startingData)
        {
            _data = startingData;
        }

        public void Add(UserProfile userProfile)
        {

            var lastUserProfile = _data.Last();
            userProfile.Id = lastUserProfile.Id + 1;
            _data.Add(userProfile);
        }

        public void Delete(int id)
        {

            var userProfileToDelete = _data.FirstOrDefault(p => p.Id == id);

            if (userProfileToDelete == null)
            {
                return;
            }

        }

        public List<UserProfile> GetAll()
        {
            return _data;
        }

        public UserProfile GetById(int id)
        {
            return _data.FirstOrDefault(p => p.Id == id);
        }

        public void Update(UserProfile userProfile)
        {
            //get the first and unique post by post Id
            var currentUserProfile = _data.FirstOrDefault(p => p.Id == userProfile.Id);
            //if that post is null, return
            if (currentUserProfile == null)
            {
                return;
            }

            //when editing, the values of each property will be whatever the value of the object's properties are that is passed in the method
            currentUserProfile.FirebaseUserId = userProfile.FirebaseUserId;
            currentUserProfile.Name = userProfile.Name;
            currentUserProfile.Email = userProfile.Email;
            currentUserProfile.ImageUrl = userProfile.ImageUrl;
            currentUserProfile.Bio = userProfile.Bio;
            currentUserProfile.DateCreated = userProfile.DateCreated;
        }

        public UserProfile GetByFirebaseUserId(string firebaseUserId)
        {
            throw new NotImplementedException();
        }

        public UserProfile GetWithPosts(int id)
        {
            throw new NotImplementedException();
        }
    }
}
