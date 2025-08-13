import React from "react";

export default function Tabs({ activeTab, setActiveTab }) {
  return (
    <div className="mb-4 border-b border-gray-200">
      <nav className="flex space-x-4">
        <button
          onClick={() => setActiveTab("description")}
          className={`py-2 px-1 border-b-2 font-medium text-sm ${
            activeTab === "description"
              ? "border-blue-500 text-blue-600"
              : "border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300"
          }`}
        >
          Açıklama
        </button>
        <button
          onClick={() => setActiveTab("comments")}
          className={`py-2 px-1 border-b-2 font-medium text-sm ${
            activeTab === "comments"
              ? "border-blue-500 text-blue-600"
              : "border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300"
          }`}
        >
          Yorumlar
        </button>
        <button
          onClick={() => setActiveTab("makes")}
          className={`py-2 px-1 border-b-2 font-medium text-sm ${
            activeTab === "makes"
              ? "border-blue-500 text-blue-600"
              : "border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300"
          }`}
        >
          Makes
        </button>
      </nav>
    </div>
  );
}
