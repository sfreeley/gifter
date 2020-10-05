import React, { useState } from "react";

//the context object gets created and can accept data from the provider
export const PostContext = React.createContext();

export const PostProvider = (props) => {
    const [posts, setPosts] = useState([]);



    const getAllPosts = () => {
        return fetch("/api/post")
            .then((res) => res.json())
            .then(setPosts);
    };

    const addPost = (post) => {
        return fetch("/api/post", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(post),
        });
    };

    const searchPosts = (searchInput, isSortDesc) => {
        return fetch(`/api/post/search?q=${searchInput}&sortDesc=${isSortDesc}`)
            .then((res) => res.json())
            .then(setPosts);
    }

    const getAllPostsWithComments = () => {
        return fetch("/api/post/getwithcomments")
            .then((res) => res.json())
            .then(setPosts);
    }

    return (
        //this provides the state value of the posts array, the functions to fetch all posts and add a new post
        <PostContext.Provider value={{ posts, getAllPosts, addPost, searchPosts, getAllPostsWithComments }}>
            {props.children}
        </PostContext.Provider>
    );
};
