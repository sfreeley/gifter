import React from "react";
import { Link } from "react-router-dom";
import { Card, CardImg, CardBody } from "reactstrap";

const Post = ({ post }) => {
    return (
        <Card className="m-4">
            <p className="text-left px-2">Posted by:
            <Link to={`/users/${post.userProfileId}`}>
                    {post.userProfile.name}
                </Link>
            </p>
            <CardImg top src={post.imageUrl} alt={post.title} />
            <CardBody>
                <p>
                    <Link to={`/posts/${post.id}`}>
                        <strong>{post.title}</strong>
                    </Link>
                </p>
                <p>{post.caption}</p>
                {post.comments && post.comments.length === 0 ? null :
                    <>
                        <h5>Comments</h5>
                        <p>
                            {post.comments.map(comment => {
                                return <p>{comment.message}</p>
                            })}
                        </p>

                    </>}

            </CardBody>
        </Card>
    );
};

export default Post;