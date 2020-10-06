import React, { useContext, useEffect, useState } from "react";
import { UserContext } from "../providers/UserProvider";
import { ListGroup, Card, CardImg, CardBody } from "reactstrap";
import { useParams } from "react-router-dom";


const UserPosts = () => {
    const { getUserPosts } = useContext(UserContext);
    const [userWithPosts, setUserWithPosts] = useState();
    const { id } = useParams();

    useEffect(() => {
        getUserPosts(id).then(setUserWithPosts);
    }, [])

    if (!userWithPosts) {
        return null;
    }

    return (
        <div className="container">
            <div className="row justify-content-center">
                <div className="col-sm-12 col-lg-6">
                    <h4> Posts by: {userWithPosts.name}</h4>
                    <ListGroup>
                        {userWithPosts.posts.map((post) => (
                            <Card className="m-4">

                                <CardImg top src={post.imageUrl} alt={post.title} />
                                <CardBody>
                                    <p>{post.title}</p>
                                    <p>{post.caption}</p>

                                </CardBody>
                            </Card>
                        ))}
                    </ListGroup>
                </div>
            </div>
        </div>
    );
};

export default UserPosts;