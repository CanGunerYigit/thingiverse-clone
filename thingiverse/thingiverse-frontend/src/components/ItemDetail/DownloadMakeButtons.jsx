import React, { useState } from "react";
import { Link } from "react-router-dom";
import { saveAs } from "file-saver";

export default function DownloadMakeButtons({ token, item }) {
  const [loading, setLoading] = useState(false);

  const handleDownload = async () => {
    setLoading(true);
    try {
      const res = await fetch("https://localhost:7267/api/Download/zip", {
  method: "POST",
  headers: {
    "Content-Type": "application/json",
    Accept: "application/zip",
    ...(token ? { Authorization: `Bearer ${token}` } : {}),
  },
  body: JSON.stringify({ thingId: item.id }),
});


      if (!res.ok) {
        const text = await res.text();
        console.error("Zip oluşturma hatası:", text);
        alert(`İndirme hatası: ${text}`);
        setLoading(false);
        return;
      }

      // dosya adı
    const contentDisposition = res.headers.get("content-disposition");
      let fileName = `${item.name || "thing"}_${item.id}.zip`;
      if (contentDisposition) {
        const match = contentDisposition.match(/filename="?(.+)"?/);
        if (match && match[1]) fileName = match[1];
      }

      const blob = await res.blob();
      saveAs(blob, fileName);
    } catch (e) {
      console.error("İndirme hatası:", e);
      alert("İndirme sırasında bir hata oluştu.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex items-center space-x-3 mb-6">
      {/* Make Butonu */}
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

      {/* Download Butonu */}
      <button
        onClick={handleDownload}
        className={`px-4 py-2 rounded-lg text-white ${
          loading ? "bg-gray-400 cursor-not-allowed" : "bg-blue-600 hover:bg-blue-700"
        }`}
        disabled={loading}
      >
        {loading ? "İndiriliyor..." : "Download"}
      </button>
    </div>
  );
}
