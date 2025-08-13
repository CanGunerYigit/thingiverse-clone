import React from "react";

export default function MakeHeader({ make, makeId }) {
  return (
    <div className="flex flex-col md:flex-row gap-8 mb-8">
      <div className="md:w-2/3">
        <div className="bg-white rounded-lg border border-gray-200 overflow-hidden shadow-sm">
          <img
            src={make.thumbnail}
            alt={make.name}
            className="w-full h-auto max-h-96 object-contain p-4"
          />
        </div>
      </div>

      <div className="md:w-1/3">
        <div className="bg-white rounded-lg border border-gray-200 p-6 shadow-sm sticky top-4">
          <div className="flex items-center">
            <h1 className="text-2xl font-bold text-gray-900 mb-2">{make.name}</h1>

            <div className="flex items-center ml-3">
              <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800">
                Make #{makeId}
              </span>
            </div>
          </div>

          <div className="prose max-w-none text-gray-600 mb-6">
            <p>{make.description}</p>
          </div>
        </div>
      </div>
    </div>
  );
}
