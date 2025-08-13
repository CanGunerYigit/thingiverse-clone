import React, { useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import MakeItemForm from "../components/MakeItem/MakeItemForm";

export default function MakeItem() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [loading, setLoading] = useState(false);
  const [formData, setFormData] = useState({
    name: "",
    thumbnail: "",
    description: "",
    previewImage: "",
    images: [],
  });

  const handleChange = (field, value) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      const form = new FormData();
      form.append("Name", formData.name);
      form.append("Thumbnail", formData.thumbnail);
      form.append("Description", formData.description);
      form.append("PreviewImage", formData.previewImage);

      formData.images.forEach((img) => {
        form.append("Images", img);
      });

      const res = await fetch(`https://localhost:7267/api/Makes/${id}/makes`, {
        method: "POST",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
        body: form,
      });

      if (res.ok) {
        alert("Make başarıyla oluşturuldu!");
        navigate(`/items/${id}`);
      } else {
        const errText = await res.text();
        alert("Hata: " + errText);
      }
    } catch (err) {
      console.error(err);
      alert("Sunucu hatası.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="p-8 max-w-xl mx-auto">
      <h1 className="text-2xl font-bold mb-4">Make Item</h1>
      <MakeItemForm
        formData={formData}
        onChange={handleChange}
        onFileChange={(files) => handleChange("images", files)}
        onSubmit={handleSubmit}
        loading={loading}
      />
    </div>
  );
}
