import { useState, useRef, useEffect } from "react";
import { ChevronDown, Filter, Zap, Trophy, Layers } from "lucide-react";

const buttons = [
  { id: "Popular", icon: <Zap size={16} />, label: "Popular" },
  { id: "Newest", icon: <Trophy size={16} />, label: "Newest" },
  { id: "MostMakes", icon: <Layers size={16} />, label: "Most Makes" },
];

const timeRanges = ["All Time", "10 Months", "3 Years"];

export default function SortBar({ active, setActive, timeRange, setTimeRange }) {
  const [showDropdown, setShowDropdown] = useState(false);
  const dropdownRef = useRef(null);

  useEffect(() => {
    function handleClickOutside(event) {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
        setShowDropdown(false);
      }
    }
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  return (
    <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between px-4 sm:px-6 py-4 bg-[#f8f9fb]">
      {/* Butonlar */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:space-x-3 space-y-2 sm:space-y-0 w-full sm:w-auto">
        {buttons.map(({ id, icon, label }) => (
          <button
            key={id}
            onClick={() => setActive(id)}
            className={`flex items-center justify-center sm:justify-start space-x-1 px-3 sm:px-4 py-2 rounded-full text-sm font-medium bg-white border w-full sm:w-auto ${
              active === id
                ? "border-blue-500 text-blue-600"
                : "border-gray-300 text-gray-700"
            }`}
          >
            {icon}
            <span>{label}</span>
          </button>
        ))}

        {/* Zaman filtreleri */}
        {active !== "Newest" && active !== "MostMakes" && (
          <div className="relative sm:ml-3 w-full sm:w-auto" ref={dropdownRef}>
            <button
              onClick={() => setShowDropdown((prev) => !prev)}
              className="flex items-center justify-between sm:justify-start space-x-1 border px-3 sm:px-4 py-2 rounded-full text-sm text-gray-700 bg-white w-full sm:w-auto"
            >
              <span>{timeRange}</span>
              <ChevronDown size={14} />
            </button>

            {showDropdown && (
              <div className="absolute mt-2 w-full sm:w-40 bg-white border rounded-md shadow-md z-50">
                {timeRanges.map((range) => (
                  <div
                    key={range}
                    className={`px-4 py-2 hover:bg-gray-100 cursor-pointer ${
                      timeRange === range ? "font-semibold text-blue-600" : ""
                    }`}
                    onClick={() => {
                      setTimeRange(range);
                      setShowDropdown(false);
                    }}
                  >
                    {range}
                  </div>
                ))}
              </div>
            )}
          </div>
        )}
      </div>

      {/* Filtre */}
      <div className="flex items-center justify-end space-x-1 text-blue-600 cursor-pointer text-sm font-medium mt-3 sm:mt-0">
        <span>Filters</span>
        <Filter size={16} />
      </div>
    </div>
  );
}
