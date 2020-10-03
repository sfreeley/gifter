import React, { useState, useContext } from "react";
import { Input, Button } from "reactstrap";
import { PostContext } from "../providers/PostProvider";

const PostSearch = (props) => {
    const { searchPosts, searchPostsArray } = useContext(PostContext);
    const [searchTerm, setSearchTerm] = useState("");
    const [sortDesc, setSortDesc] = useState(true);
    // const [searchPostsArray, setSearchPostsArray] = useState([]);
    let searchInput;


    const handleSearchField = (event) => {
        searchInput = event.target
        let { name, value } = searchInput
        setSearchTerm(searchInput.value);
        console.log(name, value)

    }

    const searchAllPosts = () => {
        searchPosts(searchTerm, sortDesc)

    }

    return (
        <>

            <Input type="text" name="searchTerm" value={searchTerm} placeholder="Search Posts" className="form-control searchBar" id="searchTerm" onChange={handleSearchField}> </Input>

            <Button className="submitSearch" type="button" color="success" onClick={searchAllPosts}>
                {'Search'}
            </Button>

            {searchPostsArray.map(post => {
                return post.Title
            })}

        </>
    )

}
export default PostSearch;

