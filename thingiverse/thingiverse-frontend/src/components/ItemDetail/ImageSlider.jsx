import React from "react";
import { FaChevronLeft, FaChevronRight } from "react-icons/fa";

export default function ImageSlider({ images, selectedIndex, setSelectedIndex, itemName }) {
  const navigateImages = (direction) => {
    let newIndex;
    if (direction === "prev") {
      newIndex = selectedIndex === 0 ? images.length - 1 : selectedIndex - 1;
    } else {
      newIndex = selectedIndex === images.length - 1 ? 0 : selectedIndex + 1;
    }
    setSelectedIndex(newIndex);
  };

  return (
    <>
      <div className="flex-1 relative bg-gray-100 rounded-lg flex items-center justify-center overflow-hidden">
        <button
          onClick={() => navigateImages("prev")}
          className="absolute left-2 z-10 bg-black bg-opacity-50 text-white p-2 rounded-full hover:bg-opacity-70 transition"
        >
          <FaChevronLeft />
        </button>
        <img src={images[selectedIndex]} alt={itemName} className="max-w-full max-h-full object-contain" />
        <button
          onClick={() => navigateImages("next")}
          className="absolute right-2 z-10 bg-black bg-opacity-50 text-white p-2 rounded-full hover:bg-opacity-70 transition"
        >
          <FaChevronRight />
        </button>
      </div>

      <div className="w-28 flex flex-col items-center space-y-2">
        <div className="flex flex-col space-y-2 overflow-y-auto scrollbar-hide w-full">
          {images.map((img, index) => (
            <button
              key={index}
              onClick={() => setSelectedIndex(index)}
              className={`w-full h-20 rounded border-2 ${
                selectedIndex === index ? "border-blue-500" : "border-gray-200"
              }`}
            >
              <img src={img} alt={`${itemName} thumbnail ${index}`} className="w-full h-full object-cover" />
            </button>
          ))}
        </div>
      </div>
    </>
  );
}
