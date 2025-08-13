import React from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  useLocation,
} from "react-router-dom";

import Navbar from "./components/Navbar";
import Hero from "./components/Hero";
import SortBar from "./pages/MainPage";
import ItemDetail from "./pages/ItemDetailPage";
import UserDetail from "./pages/UserDetailPage";
import RegisterPage from "./pages/RegisterPage";
import LoginPage from "./pages/LoginPage";
import CreateItem from "./pages/CreateItemPage"; 
import MakeItem from "./pages/MakeItemPage";  
import MakeDetail from "./pages/MakeDetailPage";
import MainPage from "./pages/MainPage";

function LayoutWithNavbar() {
  const location = useLocation();
  const hideNavbarRoutes = ["/signup", "/login"];

  const shouldShowNavbar = !hideNavbarRoutes.includes(location.pathname);

  return (
    <>
      {shouldShowNavbar && (
        <div style={{ backgroundColor: "#2b52fe" }}>
          <Navbar />
        </div>
      )}

      <Routes>
        <Route
          path="/"
          element={
            <>
              <Hero />
              <MainPage />
            </>
          }
        />
        <Route path="/item/:id" element={<ItemDetail />} />
        <Route path="/user/:id" element={<UserDetail />} />
        <Route path="/signup" element={<RegisterPage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/create-item" element={<CreateItem />} />
        
        
        <Route path="/make-item/:id" element={<MakeItem />} />
        <Route path="/makes/:makeId" element={<MakeDetail />} />
      </Routes>
    </>
  );
}

function App() {
  return (
    <Router>
      <LayoutWithNavbar />
    </Router>
  );
}

export default App;
