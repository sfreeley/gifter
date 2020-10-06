import React, { useContext, useEffect } from "react";
import { PostContext } from "../providers/PostProvider";
import Post from "./Post";
import PostSearch from "./PostSearch";


const PostList = () => {
    const { posts, getAllPosts, getAllPostsWithComments } = useContext(PostContext);

    useEffect(() => {
        getAllPostsWithComments();
    }, []);

    return (
        <>
            <PostSearch />
            <div className="container">
                <div className="row justify-content-center">
                    <div className="cards-column">
                        {posts.map((post) => {
                            return <Post key={post.id} post={post} />
                        })}

                    </div>
                </div>
            </div>
        </>
    );
};

export default PostList;