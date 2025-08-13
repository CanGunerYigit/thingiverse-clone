import React from "react";

export default function DescriptionTab({ description }) {
  return (
    <div className="p-3 sm:p-4 bg-white rounded-lg">
      <h2 className="text-lg sm:text-xl font-semibold mb-2 sm:mb-3">
        Açıklama
      </h2>
      <p className="text-sm sm:text-base text-gray-700 whitespace-pre-wrap leading-relaxed">
        {description}
      </p>
    </div>
  );
}