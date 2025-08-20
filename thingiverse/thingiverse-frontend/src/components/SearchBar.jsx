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

  // Responsive kontrol
  useEffect(() => {
    const handleResize = () => setIsMobile(window.innerWidth < 640);
    window.addEventListener("resize", handleResize);
    return () => window.removeEventListener("resize", handleResize);
  }, []);

  // Arama logic
  useEffect(() => {
    if (query.trim().length < 2) {
      setResults([]);
      setShowDropdown(false);
      return;
    }

    const delayDebounce = setTimeout(async () => {
      const url =
        searchType === "Items"
          ? `https://localhost:7267/api/Items/search/${encodeURIComponent(query)}`
          : `https://localhost:7267/api/User/search?query=${encodeURIComponent(query)}`;

      let headers = {};
      if (searchType === "Users") {
        const token = localStorage.getItem("token");
        if (token) headers.Authorization = `Bearer ${token}`; // token varsa ekle
      }

      try {
        const res = await fetch(url, { headers });
        console.log("Search fetch response status:", res.status);
        const data = res.ok ? await res.json() : [];
        console.log("Search fetch data:", data);

        setResults(data || []);
        setShowDropdown(true);
      } catch (err) {
        console.error("Search fetch error:", err);
        setResults([]);
        setShowDropdown(false);
      }
    }, 300);

    return () => clearTimeout(delayDebounce);
  }, [query, searchType]);

  // Dropdown dışına tıklayınca kapat
  useEffect(() => {
    const handleClickOutside = (event) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
        setShowDropdown(false);
      }
    };
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
        className={`w-full ${isMobile ? "pl-3" : "pl-24"} pr-4 py-2 rounded border border-gray-300 text-black focus:outline-none focus:ring focus:border-blue-300 bg-white`}
      />

      {/* Search dropdown */}
      {showDropdown && (
        <div className="absolute left-0 right-0 bg-white border border-gray-300 rounded mt-1 max-h-80 overflow-y-auto z-50 shadow-lg">
          {results.length > 0 ? (
            <ul>
             {results.map((res, index) => (
  <li
    key={res.Id ?? res.id ?? index} // id varsa kullan, yoksa index
    className="py-2 px-4 cursor-pointer hover:bg-blue-100 text-black flex items-center"
    onMouseDown={(e) => e.preventDefault()}
    onClick={() => handleResultClick(res.Id ?? res.id)}
  >
    {searchType === "Items" && res.thumbnail && (
      <img
        src={res.thumbnail}
        alt=""
        className="w-8 h-8 object-cover rounded mr-2"
      />
    )}

    {searchType === "Users" && res.ProfileImageUrl && (
      <img
        src={`https://localhost:7267${res.ProfileImageUrl}`}
        alt=""
        className="w-8 h-8 object-cover rounded-full mr-2"
      />
    )}

    <span>{searchType === "Items" ? res.name : res.UserName}</span>
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
