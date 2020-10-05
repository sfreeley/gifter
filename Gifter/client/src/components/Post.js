import React from "react";
import { Link } from "react-router-dom";
import { Card, CardImg, CardBody } from "reactstrap";

const Post = ({ post }) => {
    return (
        <Card className="m-4">
            <p className="text-left px-2">Posted by: {post.userProfile.name}</p>
            <CardImg top src={post.imageUrl} alt={post.title} />
            <CardBody>
                <p>
                    <Link to={`/posts/${post.id}`}>
                        <strong>{post.title}</strong>
                    </Link>
                </p>
                <p>{post.caption}</p>
                {post.comments && post.comments.length === 0 ? null : <h4>Comments</h4>}
                {post.comments && post.comments.map(aPostWithComments => {
                    return <p>{aPostWithComments.message}</p>
                })}
            </CardBody>
        </Card>
    );
};

export default Post;