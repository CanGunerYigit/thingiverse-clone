import React from "react";

export default function MakeItemForm({ formData, onChange, onFileChange, onSubmit, loading }) {
  return (
    <form onSubmit={onSubmit} className="flex flex-col gap-4">
      <input
        type="text"
        placeholder="Name"
        value={formData.name}
        onChange={(e) => onChange("name", e.target.value)}
        className="border p-2 rounded"
        required
      />
      <input
        type="text"
        placeholder="Thumbnail URL"
        value={formData.thumbnail}
        onChange={(e) => onChange("thumbnail", e.target.value)}
        className="border p-2 rounded"
      />
      <textarea
        placeholder="Description"
        value={formData.description}
        onChange={(e) => onChange("description", e.target.value)}
        className="border p-2 rounded"
      />
      <input
        type="text"
        placeholder="Preview Image URL"
        value={formData.previewImage}
        onChange={(e) => onChange("previewImage", e.target.value)}
        className="border p-2 rounded"
      />
      <input
        type="file"
        multiple
        onChange={(e) => onFileChange([...e.target.files])}
        className="border p-2 rounded"
      />
      <button
        type="submit"
        disabled={loading}
        className="px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700 disabled:opacity-70"
      >
        {loading ? "Oluşturuluyor..." : "Oluştur"}
      </button>
    </form>
  );
}
