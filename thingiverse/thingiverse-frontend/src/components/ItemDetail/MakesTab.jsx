import React from "react";
import { Link } from "react-router-dom";

export default function MakesTab({ makes, loading }) {
  return (
    <div className="p-4 bg-white rounded-lg">
      <h2 className="text-xl font-semibold mb-3">Makes</h2>
      {loading ? (
        <p>Makes yükleniyor...</p>
      ) : makes.length === 0 ? (
        <p>Henüz make eklenmemiş.</p>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          {makes.map((make) => (
            <Link
              key={make.id}
              to={`/makes/${make.id}`}
              className="block border rounded-lg overflow-hidden shadow-sm hover:shadow-md transition-shadow"
            >
              <div className="p-4">
                <div className="flex items-center gap-3 mb-3">
                  <Link
                    to={`/user/${make.userId}`}
                    className="font-medium text-blue-600 hover:underline"
                    onClick={(e) => e.stopPropagation()}
                  >
                    {make.userName}
                  </Link>
                  <span className="text-gray-500 text-sm">
                    {new Date(make.createdAt).toLocaleDateString("tr-TR")}
                  </span>
                </div>

                {make.description && <p className="text-gray-700 mb-3">{make.description}</p>}

                <div className="space-y-2">
                  {make.thumbnail && (
                    <div className="relative h-48 bg-gray-100 rounded overflow-hidden">
                      <img
                        src={make.thumbnail.startsWith("http") ? make.thumbnail : `https://localhost:7267${make.thumbnail}`}
                        alt={`${make.userName}'s make thumbnail`}
                        className="w-full h-full object-contain"
                        onError={(e) => {
                          e.target.src = "https://via.placeholder.com/300x200?text=Thumbnail+Not+Found";
                        }}
                      />
                    </div>
                  )}

                  {make.imagePaths && make.imagePaths.length > 0 && (
                    <div className="grid grid-cols-3 gap-2 mt-2">
                      {make.imagePaths.map((path, index) => (
                        <div key={index} className="relative h-24 bg-gray-100 rounded overflow-hidden">
                          <img
                            src={`https://localhost:7267${path}`}
                            alt={`Make image ${index + 1}`}
                            className="w-full h-full object-cover"
                            onError={(e) => {
                              e.target.src = "https://via.placeholder.com/150?text=Image+Not+Found";
                            }}
                          />
                        </div>
                      ))}
                    </div>
                  )}
                </div>
              </div>
            </Link>
          ))}
        </div>
      )}
    </div>
  );
}
