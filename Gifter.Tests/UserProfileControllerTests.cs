using Gifter.Controllers;
using Gifter.Models;
using Gifter.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Gifter.Tests
{
   public class UserProfileControllerTests
    {
        [Fact]
        public void Get_Returns_All_UserProfiles()
        {
            // Arrange --creating any variables, objects or resources needed to run the tests
            var userProfileCount = 20;
           
            var userProfiles = CreateTestUserProfiles(userProfileCount);

            var repo = new InMemoryUserProfileRepository(userProfiles);

            var controller = new UserProfileController(repo);

            // Act  --run the tests (system under test)
            var result = controller.Get();

        
            var okResult = Assert.IsType<OkObjectResult>(result);
             
            var actualUserProfiles = Assert.IsType<List<UserProfile>>(okResult.Value);

            Assert.Equal(userProfileCount, actualUserProfiles.Count);
            Assert.Equal(userProfiles, actualUserProfiles);
        }

        [Fact]
        public void Get_By_Id_Returns_NotFound_When_Given_Unknown_Id()
        {
            // Arrange 
            var userProfile = new List<UserProfile>();

            var repo = new InMemoryUserProfileRepository(userProfile);
            var controller = new UserProfileController(repo);

            // Act
            var result = controller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Get_By_Id_Returns_UserProfile_With_Given_Id()
        {
            // Arrange
            var testUserProfileId = 99;
            var userProfiles = CreateTestUserProfiles(3);
            userProfiles[0].Id = testUserProfileId; 

            var repo = new InMemoryUserProfileRepository(userProfiles);
            var controller = new UserProfileController(repo);

            // Act
            var result = controller.Get(testUserProfileId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualUserProfile = Assert.IsType<UserProfile>(okResult.Value);

            Assert.Equal(testUserProfileId, actualUserProfile.Id);
        }

        [Fact]
        public void Post_Method_Adds_A_New_UserProfile()
        {
            // Arrange 
            var userProfileCount = 20;
            var userProfiles = CreateTestUserProfiles(userProfileCount);

            var repo = new InMemoryUserProfileRepository(userProfiles);
            var controller = new UserProfileController(repo);

            // Act
            var newUserProfile = new UserProfile()
            {
                Name = "Name",
                Email = "Email",
                ImageUrl = "http://some.image.url",
                Bio = "Bio",
                DateCreated = DateTime.Today, 
                FirebaseUserId = "123456789",
               
            };

            controller.Post(newUserProfile);

            // Assert
            Assert.Equal(userProfileCount + 1, repo.InternalData.Count);
        }

        [Fact]
        public void Put_Method_Returns_BadRequest_When_Ids_Do_Not_Match()
        {
            // Arrange
            var testUserProfileId = 99;
            var userProfiles = CreateTestUserProfiles(3);
            userProfiles[0].Id = testUserProfileId; 


            var repo = new InMemoryUserProfileRepository(userProfiles);
            var controller = new UserProfileController(repo);

            var userProfileToUpdate = new UserProfile()
            {
                Id = testUserProfileId,
                FirebaseUserId = "12345",
                Name = "Name",
                Email = "Email",
                ImageUrl = "http://some.image.url",
                Bio = "Bio",
                DateCreated = DateTime.Now
            };
            var someOtherUserProfileId = testUserProfileId + 1; // make sure they aren't the same

            // Act
            var result = controller.Put(someOtherUserProfileId, userProfileToUpdate);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Put_Method_Updates_A_UserProfile()
        {
            // Arrange
            var testUserProfileId = 99;
            var userProfiles = CreateTestUserProfiles(3);
            userProfiles[0].Id = testUserProfileId; 


            var repo = new InMemoryUserProfileRepository(userProfiles);
            var controller = new UserProfileController(repo);

            var userProfileToUpdate = new UserProfile()
            {
                Id = testUserProfileId,
                FirebaseUserId = "12345",
                Name = "Updated!",
                Email = "Updated!",
                ImageUrl = "http://some.image.url",
                Bio = "Updated!",
                DateCreated = DateTime.Now
            };

            // Act
            controller.Put(testUserProfileId, userProfileToUpdate);

            // Assert
            var userProfileFromDb = repo.InternalData.FirstOrDefault(p => p.Id == testUserProfileId);
            Assert.NotNull(userProfileFromDb);

            Assert.Equal(userProfileToUpdate.FirebaseUserId,userProfileFromDb.FirebaseUserId);
            Assert.Equal(userProfileToUpdate.Name,userProfileFromDb.Name);
            Assert.Equal(userProfileToUpdate.Email,userProfileFromDb.Email);
            Assert.Equal(userProfileToUpdate.ImageUrl,userProfileFromDb.ImageUrl);
            Assert.Equal(userProfileToUpdate.Bio,userProfileFromDb.Bio);
            Assert.Equal(userProfileToUpdate.DateCreated,userProfileFromDb.DateCreated);
        }

        [Fact]
        public void Delete_Method_Removes_A_UserProfile()
        {
            // Arrange
            var testUserProfileId = 99;
            var userProfiles = CreateTestUserProfiles(5);
            userProfiles[0].Id = testUserProfileId;

            var repo = new InMemoryUserProfileRepository(userProfiles);
            var controller = new UserProfileController(repo);

            // Act
            controller.Delete(testUserProfileId);

            // Assert
            var userProfileFromDb = repo.InternalData.FirstOrDefault(p => p.Id == testUserProfileId);
            Assert.Null(userProfileFromDb);
        }

        private List<UserProfile> CreateTestUserProfiles(int count)
        {
            var userProfiles = new List<UserProfile>();
            for (var i = 1; i <= count; i++)
            {
                userProfiles.Add(new UserProfile()
                {
                    Id = i,
                    FirebaseUserId = $"123456789",
                    Name = $"User {i}",
                    Email = $"user{i}@example.com",
                    ImageUrl = $"http://user.image.url/{i}",
                    Bio = $"Bio {i}",
                    DateCreated = DateTime.Today.AddDays(-i),
                    
                   
                });
            }
            return userProfiles;
        }

        //private UserProfile CreateTestUserProfile(int id)
        //{
        //    return new UserProfile()
        //    {
        //        Id = id,
        //        Name = $"User {id}",
        //        Email = $"user{id}@example.com",
        //        Bio = $"Bio {id}",
        //        DateCreated = DateTime.Today.AddDays(-id),
        //        ImageUrl = $"http://user.image.url/{id}",
        //    };
        //}

       
    }
}
