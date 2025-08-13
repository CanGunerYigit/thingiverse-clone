import React from "react";
import { Link } from "react-router-dom";

export default function DownloadMakeButtons({ token, item }) {
  const handleDownload = async () => {
    try {
      const response = await fetch(item.thumbnail);
      const blob = await response.blob();
      const url = window.URL.createObjectURL(blob);

      const a = document.createElement("a");
      a.href = url;
      a.download = item.name || "thumbnail.jpg";
      document.body.appendChild(a);
      a.click();
      a.remove();
      window.URL.revokeObjectURL(url);
    } catch (error) {
      console.error("İndirme hatası:", error);
    }
  };

  return (
    <div className="flex items-center space-x-3 mb-6">
      <Link
        to={token ? `/make-item/${item.id}` : "#"}
        className={`px-4 py-2 rounded-lg text-white ${
          token ? "bg-green-600 hover:bg-green-700" : "bg-gray-400 cursor-not-allowed"
        }`}
        onClick={(e) => {
          if (!token) e.preventDefault();
        }}
      >
        Make
      </Link>

      <button
        onClick={handleDownload}
        className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
      >
        Download
      </button>
    </div>
  );
}
