import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import ItemHeader from "../components/ItemDetail/ItemHeader";
import ImageSlider from "../components/ItemDetail/ImageSlider";
import DownloadMakeButtons from "../components/ItemDetail/DownloadMakeButtons";
import Tabs from "../components/ItemDetail/Tabs";
import DescriptionTab from "../components/ItemDetail/DescriptionTab";
import CommentsTab from "../components/ItemDetail/CommentsTab";
import MakesTab from "../components/ItemDetail/MakesTab";

export default function ItemDetail() {
  const { id } = useParams();
  const [item, setItem] = useState(null);
  const [comments, setComments] = useState([]);
  const [makes, setMakes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [loadingComments, setLoadingComments] = useState(true);
  const [loadingMakes, setLoadingMakes] = useState(true);
  const [error, setError] = useState(false);
  const [selectedImageIndex, setSelectedImageIndex] = useState(0);
  const [newComment, setNewComment] = useState("");
  const [activeTab, setActiveTab] = useState("description");
  const token = localStorage.getItem("token");

  useEffect(() => {
    fetchItem();
    fetchComments();
    fetchMakes();
  }, [id]);

  const fetchItem = async () => {
    try {
      const res = await fetch(`https://localhost:7267/api/Items/${id}/with-images`);
      if (!res.ok) throw new Error();
      const data = await res.json();
      setItem(data);
      setSelectedImageIndex(0);
    } catch {
      setError(true);
    } finally {
      setLoading(false);
    }
  };

  const fetchComments = async () => {
    try {
      const res = await fetch(`https://localhost:7267/api/Comment/item/${id}`);
      if (!res.ok) throw new Error();
      const data = await res.json();
      setComments(data);
    } catch {
      console.error("Yorumlar alınamadı");
    } finally {
      setLoadingComments(false);
    }
  };

  const fetchMakes = async () => {
    try {
      const res = await fetch(`https://localhost:7267/api/Makes/item/${id}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      if (!res.ok) throw new Error();
      const data = await res.json();
      setMakes(data);
    } catch (error) {
      console.error("Makes alınamadı:", error);
    } finally {
      setLoadingMakes(false);
    }
  };

  const allImages = item
    ? [item.thumbnail, ...(item.images?.map(img => `https://localhost:7267/api/Items/image/${img.thingId}/${img.id}`) || [])]
    : [];

  if (loading) return <div className="p-6">Yükleniyor...</div>;
  if (error || !item) return <div className="p-6 text-red-500">Hata oluştu.</div>;

  return (
    <div className="max-w-4xl mx-auto p-6">
      <ItemHeader item={item} />
      <div className="mb-8 flex flex-row h-[400px] gap-4">
        <ImageSlider
          images={allImages}
          selectedIndex={selectedImageIndex}
          setSelectedIndex={setSelectedImageIndex}
          itemName={item.name}
        />
      </div>
      <DownloadMakeButtons token={token} item={item} />
      <Tabs activeTab={activeTab} setActiveTab={setActiveTab} />

      <div className="mb-6">
        {activeTab === "description" && <DescriptionTab description={item.description} />}
        {activeTab === "comments" && (
          <CommentsTab
            comments={comments}
            loading={loadingComments}
            token={token}
            newComment={newComment}
            setNewComment={setNewComment}
            itemId={id}
            refreshComments={fetchComments}
          />
        )}
        {activeTab === "makes" && <MakesTab makes={makes} loading={loadingMakes} />}
      </div>
    </div>
  );
}
