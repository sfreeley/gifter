using Gifter.Models;
using Gifter.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Gifter.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }

        public UserProfile GetByFirebaseUserId(string firebaseUserId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT up.Id, Up.FirebaseUserId, up.Name AS UserProfileName, up.Email, up.UserTypeId,
                               ut.Name AS UserTypeName
                        FROM UserProfile up
                        WHERE FirebaseUserId = @FirebaseuserId";

                    DbUtils.AddParameter(cmd, "@FirebaseUserId", firebaseUserId);

                    UserProfile userProfile = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            FirebaseUserId = DbUtils.GetString(reader, "FirebaseUserId"),
                            Name = DbUtils.GetString(reader, "UserProfileName"),
                            Email = DbUtils.GetString(reader, "Email")
                        };
                    }
                    reader.Close();

                    return userProfile;
                }
            }
        }

        public List<UserProfile> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT Id, Name, Email, ImageUrl, Bio, DateCreated 
                    FROM UserProfile
                    ";

                    var reader = cmd.ExecuteReader();
                    var userProfiles = new List<UserProfile>();

                    while (reader.Read())
                    {
                        userProfiles.Add(new UserProfile()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            Name = DbUtils.GetString(reader, "Name"),
                            Email = DbUtils.GetString(reader, "Email"),
                            DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                            ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                            Bio = DbUtils.GetString(reader, "Bio")
                        });
                    }
                    reader.Close();
                    return userProfiles;
                }
            }
        }

        public UserProfile GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT Id, Name, Email, ImageUrl, Bio, DateCreated 
                    FROM UserProfile
                    WHERE Id = @id
                    ";
                    DbUtils.AddParameter(cmd, "@id", id);

                    var reader = cmd.ExecuteReader();

                    UserProfile userProfile = null;

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = id,
                            Name = DbUtils.GetString(reader, "Name"),
                            Email = DbUtils.GetString(reader, "Email"),
                            ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                            DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                            Bio = DbUtils.GetString(reader, "Bio")

                        };
                    }
                    reader.Close();
                    return userProfile;
                }
                
            }
        }

        //getting one user with their id and all their posts including those posts comments
        public UserProfile GetWithPosts(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    
                    SELECT up.Name, up.Bio, up.Email, up.DateCreated AS UserProfileDateCreated,
                    up.ImageUrl AS UserProfileImageUrl,

                    p.Id AS PostId, p.Title, p.Caption, p.DateCreated AS PostDateCreated,
                    p.ImageUrl AS PostImageUrl, p.UserProfileId AS PostUserProfileId,

                    c.Id AS CommentId, c.Message, c.UserProfileId AS CommentUserProfileId
                    
                    FROM Post p
                    LEFT JOIN Comment c ON c.PostId = p.Id
                    LEFT JOIN UserProfile up ON p.UserProfileId = up.Id
                    WHERE up.Id = @id
                    ";

                    //adding the id parameter in order to search by userprofileId
                    DbUtils.AddParameter(cmd, "@Id", id);
                    var reader = cmd.ExecuteReader();

                    //initially userProfile will be null
                    UserProfile userProfile = null;

                    //while there is data in the database...
                    while (reader.Read())
                    {
                       //because initially userProfile will be null, create new instance of UserProfile
                        if (userProfile == null)
                        {
                            userProfile = new UserProfile()
                            {
                                Id = id,
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
                                ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
                                Bio = DbUtils.GetString(reader, "Bio"),
                                //this will be list of posts that belong to that user
                                Posts = new List<Post>()
                            };

                        }

                        //because of one to many relationship, one post can have many comments,
                        //if we don't get the unique ones the postId and that post entry will show up every time there is a comment
                        //get the id of the post
                        var postId = DbUtils.GetInt(reader, "PostId");
                        //this will return a post that has a post Id that is equivalent to postId, BUT ONLY
                        //if it doesn't already exist in the list of posts
                        var existingPost = userProfile.Posts.FirstOrDefault(p => p.Id == postId);

                        //we need to check and make sure that the value for the column PostId is not null
                        if (DbUtils.IsNotDbNull(reader, "PostId"))
                        {
                            //if there is no post with that postId, the post will be null and we set its value to a new post
                            if (existingPost == null)
                            {
                                existingPost = new Post()
                                {
                                    Id = postId,
                                    Title = DbUtils.GetString(reader, "Title"),
                                    Caption = DbUtils.GetString(reader, "Caption"),
                                    DateCreated = DbUtils.GetDateTime(reader, "PostDateCreated"),
                                    ImageUrl = DbUtils.GetString(reader, "PostImageUrl"),
                                    UserProfileId = DbUtils.GetInt(reader, "PostUserProfileId"),
                                    //this will be list of comments that belong to that post
                                    Comments = new List<Comment>()

                                };
                                //add the post to the userprofile list of posts
                                userProfile.Posts.Add(existingPost);
                            
                            }

                        }
                        //then must get all the comments for this post;
                        //because we want to get all the comments for that specific post, we have to make sure
                        //there are actually comments for that post

                        if (DbUtils.IsNotDbNull(reader, "CommentId"))
                        {
                            //if the column is not null, create new comment 
                             Comment comment = new Comment()
                            {
                                Id = DbUtils.GetInt(reader, "CommentId"),
                                Message = DbUtils.GetString(reader, "Message"),
                                PostId = postId,
                                UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId")
                            };
                            //add it to the comments list that belong to that post
                            existingPost.Comments.Add(comment);

                        }
                    }
                    reader.Close();
                    return userProfile;
                }
            }
        }

        public void Add(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO UserProfile (Name, Email, ImageUrl, Bio, DateCreated)
                    OUTPUT INSERTED.ID
                    VALUES (@name, @email, @imageUrl, @bio, @dateCreated)
                     ";

                    DbUtils.AddParameter(cmd, "@name", userProfile.Name);
                    DbUtils.AddParameter(cmd, "@email", userProfile.Email);
                    DbUtils.AddParameter(cmd, "@imageUrl", userProfile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@dateCreated", userProfile.DateCreated);
                    DbUtils.AddParameter(cmd, "@bio", userProfile.Bio);

                    userProfile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE UserProfile
                           SET Name = @name,
                               Email = @email,
                               DateCreated = @dateCreated,
                               ImageUrl = @imageUrl,
                               Bio = @bio
                         WHERE Id = @id";

                    DbUtils.AddParameter(cmd, "@name", userProfile.Name);
                    DbUtils.AddParameter(cmd, "@email", userProfile.Email);
                    DbUtils.AddParameter(cmd, "@dateCreated", userProfile.DateCreated);
                    DbUtils.AddParameter(cmd, "@imageUrl", userProfile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@bio", userProfile.Bio);
                    DbUtils.AddParameter(cmd, "@id", userProfile.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM UserProfile WHERE Id = @id";
                    DbUtils.AddParameter(cmd, "@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
