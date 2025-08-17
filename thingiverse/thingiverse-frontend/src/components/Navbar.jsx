import React, { useState, useEffect, useRef } from "react";
import { Link, useNavigate } from "react-router-dom"; 
import SearchBar from "./SearchBar";
import { CirclePlus, Menu, X, ChevronLeft } from "lucide-react";
import thingsHeader from "../assets/page-header__things.jpg";
import thingsCreator from "../assets/page-header__creators.jpg";

export default function Navbar() {
  const [username, setUsername] = useState(null);
  const [profileImage, setProfileImage] = useState(null);
  const [showDropdown, setShowDropdown] = useState(false);
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);
  const [showLoginPanel, setShowLoginPanel] = useState(false);

  const dropdownRef = useRef(null);
  const navigate = useNavigate();

  // Backend’den kullanıcıyı fetch et
  useEffect(() => {
    const fetchUser = async () => {
      const storedUser = localStorage.getItem("user");
      if (!storedUser) return;

      try {
        const parsedUser = JSON.parse(storedUser);
        const res = await fetch(`https://localhost:7267/api/User/${parsedUser.id}`);
        if (res.ok) {
          const data = await res.json();
          setUsername(data.userName);
          setProfileImage(data.profileImageUrl ? `https://localhost:7267${data.profileImageUrl}` : null);
        } else {
          setUsername(parsedUser.userName);
          setProfileImage(null);
        }
      } catch {
        setUsername(null);
        setProfileImage(null);
      }
    };

    fetchUser();
    window.addEventListener("userUpdated", fetchUser);
    return () => window.removeEventListener("userUpdated", fetchUser);
  }, []);

  // dışarı tıklayınca dropdown kapat
  useEffect(() => {
    const handleClickOutside = (event) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
        setShowDropdown(false);
      }
    };
    if (showDropdown) {
      document.addEventListener("mousedown", handleClickOutside);
    } else {
      document.removeEventListener("mousedown", handleClickOutside);
    }
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, [showDropdown]);

  const toggleDropdown = () => setShowDropdown(!showDropdown);
  const toggleLoginPanel = () => setShowLoginPanel(!showLoginPanel);

  const handleLogout = () => {
    localStorage.removeItem("user");
    localStorage.removeItem("token");
    setUsername(null);
    setProfileImage(null);
    setShowDropdown(false);
    setMobileMenuOpen(false);
  };

  const goBack = () => navigate(-1);

  return (
    <div className="relative bg-[#2b52fe]">
      <div className="flex items-center justify-between px-4 py-3 md:px-6 text-white">
        {/* Sol taraf - Logo + Geri Butonu */}
        <div className="flex items-center space-x-4">
          {location.pathname !== "/" && (
            <button onClick={goBack} className="px-3 py-1 rounded-md">
              <ChevronLeft/>
            </button>
          )}
          <Link to="/" className="text-xl font-bold">Thingiverse</Link>
        </div>

        {/* Search Bar */}
        <div className="hidden md:flex flex-1 justify-center px-4">
          <SearchBar />
        </div>

        {/* Desktop Menü */}
        <div className="hidden md:flex items-center space-x-3">
          {username ? (
            <>
              <Link to="/create-item">
                <button className="bg-white border text-black px-3 py-1 rounded-xl flex items-center hover:text-blue-400 hover:border-blue-400">
                  <CirclePlus size={16}/> <span className="ml-1">Create</span>
                </button>
              </Link>

              {/* Dropdown + Profil Foto veya Harf */}
              <div className="relative flex items-center" ref={dropdownRef}>
                {profileImage ? (
                  <img 
                    src={profileImage} 
                    alt="avatar" 
                    className="w-8 h-8 rounded-full mr-2 object-cover"
                  />
                ) : (
                  <div className="w-8 h-8 rounded-full bg-gray-300 flex items-center justify-center text-sm font-bold text-gray-700 mr-2">
                    {username ? username.charAt(0).toUpperCase() : "?"}
                  </div>
                )}
                <button onClick={toggleDropdown} className="px-3 py-1 hover:bg-blue-500 rounded-xl">
                  {username} ▾
                </button>
                              {showDropdown && (
                <div className="absolute right-0 mt-[150px] w-40 bg-white rounded-lg shadow-lg text-black ">
                  <button
                    onClick={() => {
                      const storedUser = localStorage.getItem("user");
                      if (storedUser) {
                        const parsedUser = JSON.parse(storedUser);
                        navigate(`/user/${parsedUser.id}`);
                        setShowDropdown(false); // dropdown kapansın
                      }
                    }}
                    className="block w-full text-left px-4 py-2 hover:bg-gray-100"
                  >
                    Profile
                  </button>
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
            <button
              onClick={toggleLoginPanel}
              className="hover:bg-blue-500 rounded-xl px-3 py-1"
            >
              Log In
            </button>
          )}
        </div>

        {/* Mobil Menü Butonu */}
        <button className="md:hidden" onClick={() => setMobileMenuOpen(!mobileMenuOpen)}>
          {mobileMenuOpen ? <X size={28} /> : <Menu size={28} />}
        </button>
      </div>

      {/* Login Panel (desktop) */}
      {!username && showLoginPanel && (
        <div className="absolute right-0 mt-2 w-[350px] bg-white text-black rounded-lg shadow-lg p-4 z-50 md:right-4">
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

      {/* Mobil Menü İçerik */}
      {mobileMenuOpen && (
        <div className="md:hidden bg-[#2B52FE] px-4 pb-4 space-y-3">
          <div className="flex justify-center mb-2">
            <SearchBar />
          </div>
          <Link to="/" className="block hover:underline text-white">Explore</Link>

          {username ? (
            <>
              <Link to="/create-item" className="block bg-green-600 text-white px-3 py-1 rounded-xl text-center">
                Create
              </Link>
              <button
                onClick={handleLogout}
                className="block w-full text-left px-3 py-1 hover:bg-blue-600 rounded text-white"
              >
                Logout
              </button>
            </>
          ) : (
            <button
              onClick={toggleLoginPanel}
              className="hover:bg-blue-500 rounded-xl px-3 py-1 text-white"
            >
              Log In
            </button>
          )}
        </div>
      )}
    </div>
  );
}
