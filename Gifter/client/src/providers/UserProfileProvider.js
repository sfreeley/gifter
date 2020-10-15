import React, { useState } from "react";

//the context object gets created and can accept data from the provider
export const UserProfileContext = React.createContext();

export const UserProfileProvider = (props) => {


    const getUserPosts = (userId) => {
        return fetch(`/api/userprofile/getwithposts/${userId}`)
            .then((res) => res.json())

    }

    return (
        <UserProfileContext.Provider value={{ getUserPosts }}>
            {/* used to display whatever you include in between the opening and closing tags when invoking a component */}
            {props.children}
        </UserProfileContext.Provider>
    )
}