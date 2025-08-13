import React from "react";

export default function MakeGallery({ make }) {
  if (!make.imagePaths || make.imagePaths.length === 0) return null;

  return (
    <div className="mb-12">
      <h2 className="text-xl font-semibold text-gray-800 mb-4 pb-2 border-b border-gray-200">
        Görseller
      </h2>
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
        {make.imagePaths.map((path, i) => (
          <div
            key={i}
            className="bg-white rounded-lg border border-gray-200 overflow-hidden shadow-sm hover:shadow-md transition-shadow"
          >
            <img
              src={`https://localhost:7267${path}`}
              alt={`${make.name} image ${i + 1}`}
              className="w-full h-48 object-cover"
            />
            <div className="p-3">
              <p className="text-sm text-gray-500">Görsel {i + 1}</p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
