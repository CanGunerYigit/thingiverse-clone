import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import MakeHeader from "../components/MakeDetail/MakeHeader";
import MakeGallery from "../components/MakeDetail/MakeGallery";

export default function MakeDetail() {
  const { makeId } = useParams();
  const [make, setMake] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    async function fetchMake() {
      try {
        const res = await fetch(`https://localhost:7267/api/Makes/${makeId}`);
        if (!res.ok) throw new Error("Make bulunamadı");
        const data = await res.json();
        setMake(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }
    fetchMake();
  }, [makeId]);

  if (loading)
    return (
      <div className="flex justify-center items-center h-screen">
        <div className="animate-pulse flex space-x-4">
          <div className="rounded-full bg-gray-300 h-12 w-12"></div>
        </div>
      </div>
    );

  if (error)
    return (
      <div className="max-w-3xl mx-auto mt-8 p-4 bg-red-50 border border-red-200 rounded-lg">
        <div className="flex items-center">
          <svg
            className="w-6 h-6 text-red-500 mr-2"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
            />
          </svg>
          <h3 className="text-lg font-medium text-red-800">Yüklenirken hata oluştu</h3>
        </div>
        <p className="mt-2 text-red-600">{error}</p>
      </div>
    );

  return (
    <div className="max-w-6xl mx-auto px-4 py-8">
      <MakeHeader make={make} makeId={makeId} />
      <MakeGallery make={make} />
    </div>
  );
}
