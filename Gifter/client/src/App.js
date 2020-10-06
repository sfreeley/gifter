import React from "react";
import { BrowserRouter as Router } from "react-router-dom";
import "./App.css";
import { PostProvider } from "./providers/PostProvider";
import { UserProvider } from "./providers/UserProvider";
import ApplicationViews from "./components/ApplicationViews";
import Header from "./components/Header";

//now we import the applicationviews component into app.js and wrap it in PostProvider and the Router
function App() {
  return (
    <div className="App">
      <Router>
        <UserProvider>
          <PostProvider>
            <Header />
            <ApplicationViews />
          </PostProvider>
        </UserProvider>
      </Router>
    </div>
  );
}

export default App;