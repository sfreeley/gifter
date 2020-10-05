import React from "react";
import { Switch, Route } from "react-router-dom";
import PostList from "./PostList";
import PostForm from "./PostForm";
import PostDetails from "./PostDetails";

const ApplicationViews = () => {
    return (
        //Swtich component is going to look at the url and render the first route that is a match
        // exact = only want to render this component when it matches exactly /
        <Switch>
            <Route path="/" exact>
                <PostList />
            </Route>

            {/* if a url matches the value of this path attribute
            the children of that <Route> will be rendered */}
            <Route path="/posts/add">
                <PostForm />
            </Route>

            {/* the : will tell the react router that this will be some id parameter */}
            <Route path="/posts/:id">
                <PostDetails />
            </Route>
        </Switch>
    );
};

export default ApplicationViews;