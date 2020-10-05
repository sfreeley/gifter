import React, { useEffect, useContext, useState } from "react";
import { ListGroup, ListGroupItem } from "reactstrap";
import { PostContext } from "../providers/PostProvider";
//accesses the route parameters
import { useParams } from "react-router-dom";
import Post from "./Post";

const PostDetails = () => {
    //putting that one individual post into state
    const [post, setPost] = useState();
    //using the getPost function that the context provided for us
    const { getPostWithComments } = useContext(PostContext);
    //using the route parameter
    const { id } = useParams();

    useEffect(() => {
        getPostWithComments(id)
            .then(setPost);
    }, []);

    if (!post) {
        return null;
    }

    return (
        <div className="container">
            <div className="row justify-content-center">
                <div className="col-sm-12 col-lg-6">
                    <Post post={post} />
                    <ListGroup>
                        {post.comments.map((c) => (
                            <ListGroupItem>{c.message}</ListGroupItem>
                        ))}
                    </ListGroup>
                </div>
            </div>
        </div>
    );
};

export default PostDetails;