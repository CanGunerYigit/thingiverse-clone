import React from "react";
import { Link } from "react-router-dom";

export default function ItemHeader({ item }) {
  return (
    <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center mb-4">
      <div>
        <h1 className="text-3xl font-bold">{item.name}</h1>
        <div className="flex items-center gap-1 mt-1">
          <span className="text-gray-600">by</span>
          <Link
            to={item.creatorUrl || "#"}
            target="_blank"
            rel="noopener noreferrer"
            className="text-blue-600 hover:underline"
          >
            {item.creatorName}
          </Link>
          <div className="text-gray-500 text-sm ml-2">
            {new Date(item.createdAt).toLocaleDateString("en-EN", {
              month: "long",
              day: "numeric",
              year: "numeric",
            })}
          </div>
        </div>
      </div>
    </div>
  );
}
