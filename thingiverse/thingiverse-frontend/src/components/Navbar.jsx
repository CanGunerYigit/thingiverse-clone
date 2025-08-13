import React, { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom"; 
import SearchBar from "./SearchBar";
import thingsHeader from "../assets/page-header__things.jpg";
import thingsCreator from "../assets/page-header__creators.jpg";
import { ChevronLeft, CirclePlus, Menu, X } from "lucide-react"; 

export default function Navbar() {
  const [showLoginPanel, setShowLoginPanel] = useState(false);
  const [username, setUsername] = useState(null);
  const [showDropdown, setShowDropdown] = useState(false);
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);

  const navigate = useNavigate(); // navigate fonksiyonu

  useEffect(() => {
    const storedUser = localStorage.getItem("user");
    if (storedUser) {
      try {
        const parsedUser = JSON.parse(storedUser);
        setUsername(parsedUser.userName || parsedUser.username || parsedUser.UserName);
      } catch {
        setUsername(null);
      }
    }
  }, []);

  const toggleLoginPanel = () => setShowLoginPanel(!showLoginPanel);
  const toggleDropdown = () => setShowDropdown(!showDropdown);
  const handleLogout = () => {
    localStorage.removeItem("user");
    localStorage.removeItem("token");
    setUsername(null);
    setShowDropdown(false);
  };

  const goBack = () => {
    navigate(-1); // bir önceki sayfaya git
  };

  return (
    <div className="relative bg-[#2b52fe]">
      <div className="flex items-center justify-between px-4 py-3 md:px-6 text-white">
        
        {/* Sol taraf - Logo + Geri Butonu */}
        <div className="flex items-center space-x-4">
          {location.pathname !== "/" && ( // anasayfadaysak gösterme
          <button
            onClick={goBack}
            className="  px-3 py-1 rounded-md"
          >
            <ChevronLeft/>
          </button>
           )}
          <Link to="/" className="text-xl font-bold">Thingiverse</Link>
        </div>

        {/* searchbar */}
        <div className="hidden md:flex flex-1 justify-center px-4">
          <SearchBar />
        </div>

        {/* Sağ taraf */}
        <div className="hidden md:flex items-center space-x-3">
          {username ? (
            <>
              <Link to="/create-item">
                <button className="bg-white border border-solid  text-black px-3 py-1 rounded-xl flex items-center hover:text-blue-400 hover:border-blue-400">
                  <CirclePlus size={16}></CirclePlus>
                 <span className="ml-1">Create</span> 
                </button>
              </Link>
              <div className="relative">
                <button
                  onClick={toggleDropdown}
                  className="px-3 py-1 hover:bg-blue-500 rounded-xl"
                >
                  {username} ▾
                </button>
                {showDropdown && (
                  <div className="absolute right-0 mt-2 w-40 bg-white rounded-lg shadow-lg text-black">
                    <button
                      onClick={handleLogout}
                      className="block w-full text-left px-4 py-2 hover:bg-gray-100"
                    >
                      Logout
                    </button>
                  </div>
                )}
              </div>
            </>
          ) : (
            <>
              <button
                onClick={toggleLoginPanel}
                className="hover:bg-blue-500 rounded-xl px-3 py-1"
              >
                Log In
              </button>
              {showLoginPanel && (
                <div className="absolute right-0 mt-[50px] w-[350px] bg-white text-black rounded-lg shadow-lg p-4">
                  <h3 className="font-semibold text-center mb-2">Join the Community</h3>
                  <div className="flex items-center mb-3">
                    <img src={thingsHeader} alt="" className="w-[70px] h-[32px]" />
                    <span className="text-sm pl-2"><b>2M+</b> Things across all categories</span>
                  </div>
                  <div className="flex items-center mb-4">
                    <img src={thingsCreator} alt="" className="w-[70px] h-[32px]" />
                    <span className="text-sm pl-2"><b>70k+</b> Creators</span>
                  </div>
                  <div className="flex justify-center">
                    <Link to="/signup">
                      <button className="bg-blue-600 hover:bg-blue-800 text-white w-full py-2 rounded-md mb-2">
                        Sign up to Thingiverse!
                      </button>
                    </Link>
                  </div>
                  <div className="w-full border-t border-gray-200 my-4" />
                  <div className="flex justify-between items-center">
                    <p className="text-sm text-gray-600">Already have an account?</p>
                    <Link to="/login" className="text-[#2B52FE] font-medium border p-2 px-5 text-sm border-[#2B52FE] rounded-lg hover:bg-gray-100">
                      Log in
                    </Link>
                  </div>
                </div>
              )}
            </>
          )}
        </div>

        {/* Mobil sekmesi */}
        <button
          className="md:hidden"
          onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
        >
          {mobileMenuOpen ? <X size={28} /> : <Menu size={28} />}
        </button>
      </div>

      {/* Mobil Menü içerik */}
      {mobileMenuOpen && (
        <div className="md:hidden bg-[#2B52FE] px-4 pb-4 space-y-3">
          <div className="flex justify-center mb-2">
            <SearchBar />
          </div>
          <Link to="/" className="block hover:underline">Explore</Link>
          <a href="#" className="block hover:underline">Blog</a>

          {username ? (
            <>
              <Link to="/create-item" className="block bg-green-600 text-white px-3 py-1 rounded-xl text-center">
                Create
              </Link>
              <button
                onClick={handleLogout}
                className="block w-full text-left px-3 py-1 hover:bg-blue-600 rounded"
              >
                Logout
              </button>
            </>
          ) : (
            <div className="flex justify-center items-center flex-col-reverse">
              <Link to="/login" className="block px-3 py-1 hover:bg-blue-600 rounded text-white">Log In</Link>
              <Link to="/signup" className="block px-3 py-1 hover:bg-blue-600 rounded text-white">Sign Up</Link>
            </div>
          )}
        </div>
      )}
    </div>
  );
}
