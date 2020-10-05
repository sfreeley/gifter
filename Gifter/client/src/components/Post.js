import React from "react";
import { Card, CardImg, CardBody } from "reactstrap";

const Post = ({ post, postWithComments }) => {
    return (
        <Card className="m-4">
            <p className="text-left px-2">Posted by: {post.userProfile.name}</p>
            <CardImg top src={post.imageUrl} alt={post.title} />
            <CardBody>
                <p>
                    <strong>{post.title}</strong>
                </p>
                <p>{post.caption}</p>
                <h4>Comments</h4>
                {postWithComments.map(post => {
                    return <p>{post.message}</p>
                })}
            </CardBody>
        </Card>
    );
};

export default Post;