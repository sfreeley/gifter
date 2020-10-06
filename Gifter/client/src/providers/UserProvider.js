import React, { useState } from "react";

//the context object gets created and can accept data from the provider
export const UserContext = React.createContext();

export const UserProvider = (props) => {


    const getUserPosts = (userId) => {
        return fetch(`/api/userprofile/getwithposts/${userId}`)
            .then((res) => res.json())

    }

    return (
        <UserContext.Provider value={{ getUserPosts }}>
            {/* used to display whatever you include in between the opening and closing tags when invoking a component */}
            {props.children}
        </UserContext.Provider>
    )
}