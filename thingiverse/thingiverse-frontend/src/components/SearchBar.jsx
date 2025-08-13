import { useState, useEffect, useRef } from "react";
import { useNavigate } from "react-router-dom";

export default function SearchBar() {
  const [query, setQuery] = useState("");
  const [searchType, setSearchType] = useState("Items");
  const [results, setResults] = useState([]);
  const [showDropdown, setShowDropdown] = useState(false);
  const [isMobile, setIsMobile] = useState(window.innerWidth < 640);
  const navigate = useNavigate();
  const dropdownRef = useRef();

  useEffect(() => {
    const handleResize = () => {
      setIsMobile(window.innerWidth < 640);
    };

    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
  }, []);

  useEffect(() => {
    if (query.trim().length < 2) {
      setResults([]);
      setShowDropdown(false);
      return;
    }

    const delayDebounce = setTimeout(() => {
      let url =
        searchType === "Items"
          ? `https://localhost:7267/api/Items/search/${encodeURIComponent(query)}`
          : `https://localhost:7267/api/User/search?query=${encodeURIComponent(query)}`;

      fetch(url)
        .then((res) => (res.ok ? res.json() : []))
        .then((data) => {
          setResults(data || []);
          setShowDropdown(true);
        })
        .catch(() => {
          setResults([]);
          setShowDropdown(false);
        });
    }, 300);

    return () => clearTimeout(delayDebounce);
  }, [query, searchType]);

  useEffect(() => {
    function handleClickOutside(event) {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
        setShowDropdown(false);
      }
    }

    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  const handleResultClick = (id) => {
    setShowDropdown(false);
    setQuery("");
    navigate(searchType === "Items" ? `/item/${id}` : `/user/${id}`);
  };

  return (
    <div className="relative w-full sm:w-80" ref={dropdownRef}>
      {/* Search tipi seç */}
      {isMobile ? (
        <div className="flex mb-2">
          <button
            onClick={() => setSearchType("Items")}
            className={`flex-1 py-1 px-3 rounded-l border ${
              searchType === "Items" 
                ? "bg-blue-600 text-white border-blue-600" 
                : "bg-white text-black border-gray-300"
            }`}
          >
            Items
          </button>
          <button
            onClick={() => setSearchType("Users")}
            className={`flex-1 py-1 px-3 rounded-r border ${
              searchType === "Users" 
                ? "bg-blue-600 text-white border-blue-600" 
                : "bg-white text-black border-gray-300"
            }`}
          >
            Users
          </button>
        </div>
      ) : (
        <select
          value={searchType}
          onChange={(e) => {
            setSearchType(e.target.value);
            setResults([]);
            setShowDropdown(false);
            setQuery("");
          }}
          className="absolute left-0 top-0 h-full rounded-l border border-gray-300 bg-white px-3 py-2 text-sm text-black focus:outline-none z-10"
          style={{ width: "90px" }}
        >
          <option value="Items">Items</option>
          <option value="Users">Users</option>
        </select>
      )}

      {/* input alanı */}
      <input
        type="text"
        placeholder={`Search ${searchType.toLowerCase()}...`}
        value={query}
        onChange={(e) => setQuery(e.target.value)}
        className={`w-full ${
          isMobile ? 'pl-3' : 'pl-24'
        } pr-4 py-2 rounded border border-gray-300 text-black focus:outline-none focus:ring focus:border-blue-300 bg-white`}
      />

      {/* Search dropdown */}
      {showDropdown && (
        <div className="absolute left-0 right-0 bg-white border border-gray-300 rounded mt-1 max-h-80 overflow-y-auto z-50 shadow-lg">
          {results.length > 0 ? (
            <ul>
              {results.map((res) => (
                <li
                  key={res.id}
                  className="py-2 px-4 cursor-pointer hover:bg-blue-100 text-black"
                  onMouseDown={(e) => e.preventDefault()}
                  onClick={() => handleResultClick(res.id)}
                >
                  <div className="flex items-center">
                    {searchType === "Items" && res.thumbnail && (
                      <img 
                        src={res.thumbnail} 
                        alt="" 
                        className="w-8 h-8 object-cover rounded mr-2"
                      />
                    )}
                    <span>{searchType === "Items" ? res.name : res.userName}</span>
                  </div>
                </li>
              ))}
            </ul>
          ) : (
            <div className="px-4 py-2 text-gray-500">No results found.</div>
          )}
        </div>
      )}
    </div>
  );
}