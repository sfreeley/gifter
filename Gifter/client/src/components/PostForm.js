import React, { useState, useContext } from "react";
import { Form, FormGroup, Label, Input, Button } from "reactstrap";
import { PostContext } from "../providers/PostProvider";
import { useHistory } from "react-router-dom";


const PostForm = () => {
    // Use this hook to allow us to programatically redirect users
    const history = useHistory();
    const { addPost, getAllPosts } = useContext(PostContext);

    const [isLoading, setIsLoading] = useState(false)

    //new post that will be added into state
    const [newPost, setNewPost] = useState({
        title: "",
        imageUrl: "",
        caption: "",
        dateCreated: "",
        userProfileId: 2,
    })


    //handling input field for posting new post
    const handleFieldChange = (event) => {
        const stateToChange = { ...newPost };
        stateToChange[event.target.id] = event.target.value;
        setNewPost(stateToChange);

    };

    const addNewPost = () => {
        setIsLoading(true);
        //do we need another .then?
        addPost(newPost).then(() => getAllPosts());
        setIsLoading(false);
        setNewPost({
            title: "",
            imageUrl: "",
            caption: "",
            dateCreated: "",
            userProfileId: 2,
        });
        //navigate the user back to the home route
        history.push("/");

    }


    return (
        <>
            <Form>
                <FormGroup>
                    <Label htmlFor="title"><strong>Title</strong></Label>
                    <Input className="p-2 bd-highlight justify-content-center"
                        value={newPost.title}
                        onChange={handleFieldChange}
                        type="text"
                        name="title"
                        defaultValue=""
                        id="title"
                        required=""
                    />
                </FormGroup>
                <FormGroup>
                    <Label htmlFor="imageUrl"><strong>Image URL</strong></Label>
                    <Input className="p-2 bd-highlight justify-content-center"
                        value={newPost.imageUrl}
                        onChange={handleFieldChange}
                        type="text"
                        name="imageUrl"
                        id="imageUrl"
                        required=""
                    />
                </FormGroup>
                <FormGroup>
                    <Label htmlFor="caption"><strong>Caption</strong></Label>
                    <Input className="p-2 bd-highlight justify-content-center"
                        value={newPost.caption}
                        onChange={handleFieldChange}
                        type="text"
                        name="caption"
                        id="caption"

                    />
                </FormGroup>
                <FormGroup>
                    <Label htmlFor="dateCreated"><strong>Date Created</strong></Label>
                    <Input className="p-2 bd-highlight justify-content-center"
                        value={newPost.dateCreated}
                        onChange={handleFieldChange}
                        type="date"
                        name="dateCreated"
                        id="dateCreated"

                    />
                </FormGroup>
                {/* <FormGroup>
                    <Label htmlFor="userProfileId"><strong>User Profile Id</strong></Label>
                    <Input className="p-2 bd-highlight justify-content-center"
                        value={parseInt(newPost.userProfileId)}
                        onChange={handleFieldChange}
                        type="number"
                        name="userProfileId"
                        id="userProfileId"

                    />
                </FormGroup> */}
            </Form >
            <Button className="submitPost" type="button" color="success" isLoading={isLoading} onClick={addNewPost}>
                {'Add Post'}
            </Button>
        </>
    )

}
export default PostForm;