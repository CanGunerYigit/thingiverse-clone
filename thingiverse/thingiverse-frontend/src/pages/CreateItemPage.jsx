import React, { useState } from "react";
import axios from "axios";
import InputField from "../components/CreateItem/InputField";

export default function CreateItem() {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [thumbnailFile, setThumbnailFile] = useState(null);
  const [thumbnailPreview, setThumbnailPreview] = useState("");
  const [previewImageFile, setPreviewImageFile] = useState(null);
  const [previewImagePreview, setPreviewImagePreview] = useState("");
  const [images, setImages] = useState([]);
  const [imagePreviews, setImagePreviews] = useState([]);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(false);

  const handleThumbnailChange = (e) => {
    const file = e.target.files[0];
    setThumbnailFile(file);
    setThumbnailPreview(file ? URL.createObjectURL(file) : "");
  };

  const handlePreviewImageChange = (e) => {
    const file = e.target.files[0];
    setPreviewImageFile(file);
    setPreviewImagePreview(file ? URL.createObjectURL(file) : "");
  };

  const handleImagesChange = (e) => {
    const files = Array.from(e.target.files);
    setImages(files);
    setImagePreviews(files.map((file) => URL.createObjectURL(file)));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsSubmitting(true);
    setError(null);
    setSuccess(false);

    try {
      const formData = new FormData();
      formData.append("Name", name);
      formData.append("Description", description);

      if (thumbnailFile) {
        formData.append("ThumbnailFile", thumbnailFile);
      }

      if (previewImageFile) {
        formData.append("PreviewImageFile", previewImageFile);
        formData.append("PreviewImage", previewImageFile.name);
      } else {
        formData.append("PreviewImage", "");
      }

      images.forEach((file) => {
        formData.append("Images", file);
      });

      const token = localStorage.getItem("token");

      const res = await axios.post(
        "https://localhost:7267/api/Items/create",
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
            Authorization: `Bearer ${token}`,
          },
        }
      );

      console.log("Başarılı:", res.data);
      setSuccess(true);

      // form reset
      setName("");
      setDescription("");
      setThumbnailFile(null);
      setThumbnailPreview("");
      setPreviewImageFile(null);
      setPreviewImagePreview("");
      setImages([]);
      setImagePreviews([]);
    } catch (err) {
      if (err.response) {
        console.error("API Hatası:", err.response.data);
        setError(err.response.data.message || "Bir hata oluştu");
      } else {
        console.error("İstek hatası:", err);
        setError("Bağlantı hatası oluştu");
      }
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="max-w-4xl mx-auto p-4 sm:p-6">
      <h1 className="text-2xl sm:text-3xl font-bold mb-6">Yeni Öğe Oluştur</h1>

      <form onSubmit={handleSubmit} className="space-y-6">
        {/* Name Field */}
        <InputField
          id="name"
          label="İsim"
          placeholder="Öğe ismi girin"
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
        />

        {/* Description Field */}
        <InputField
          id="description"
          label="Açıklama"
          type="textarea"
          placeholder="Açıklama girin"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          required
          rows={5}
        />

        {/* Thumbnail Upload */}
        <div>
          <label htmlFor="thumbnail" className="block text-sm font-medium text-gray-700 mb-1">
            Küçük Resim (Thumbnail)*
          </label>
          <input
            id="thumbnail"
            type="file"
            accept="image/*"
            onChange={handleThumbnailChange}
            required
            className="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-md file:border-0 
                       file:text-sm file:font-semibold file:bg-blue-50 file:text-blue-700 hover:file:bg-blue-100"
          />
          {thumbnailPreview && (
            <div className="mt-2">
              <img src={thumbnailPreview} alt="Thumbnail preview" className="h-32 object-contain rounded-md" />
            </div>
          )}
        </div>

        {/* Preview Image Upload */}
        <div>
          <label htmlFor="previewImage" className="block text-sm font-medium text-gray-700 mb-1">
            Önizleme Resmi
          </label>
          <input
            id="previewImage"
            type="file"
            accept="image/*"
            onChange={handlePreviewImageChange}
            className="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-md file:border-0 
                       file:text-sm file:font-semibold file:bg-blue-50 file:text-blue-700 hover:file:bg-blue-100"
          />
          {previewImagePreview && (
            <div className="mt-2">
              <img src={previewImagePreview} alt="Preview image" className="h-32 object-contain rounded-md" />
            </div>
          )}
        </div>

        {/* Additional Images Upload */}
        <div>
          <label htmlFor="images" className="block text-sm font-medium text-gray-700 mb-1">
            Ek Resimler
          </label>
          <input
            id="images"
            type="file"
            accept="image/*"
            multiple
            onChange={handleImagesChange}
            className="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-md file:border-0 
                       file:text-sm file:font-semibold file:bg-blue-50 file:text-blue-700 hover:file:bg-blue-100"
          />
          {imagePreviews.length > 0 && (
            <div className="mt-2 grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-2">
              {imagePreviews.map((preview, index) => (
                <img
                  key={index}
                  src={preview}
                  alt={`Preview ${index + 1}`}
                  className="h-24 w-full object-cover rounded-md"
                />
              ))}
            </div>
          )}
        </div>

        {/* Status Messages */}
        {error && (
          <div className="p-4 bg-red-50 text-red-700 rounded-md">{error}</div>
        )}
        {success && (
          <div className="p-4 bg-green-50 text-green-700 rounded-md">
            Öğe başarıyla oluşturuldu!
          </div>
        )}

        {/* Submit Button */}
        <button
          type="submit"
          disabled={isSubmitting}
          className={`px-6 py-3 rounded-md text-white font-medium ${
            isSubmitting ? "bg-blue-400" : "bg-blue-600 hover:bg-blue-700"
          }`}
        >
          {isSubmitting ? "Oluşturuluyor..." : "Oluştur"}
        </button>
      </form>
    </div>
  );
}
