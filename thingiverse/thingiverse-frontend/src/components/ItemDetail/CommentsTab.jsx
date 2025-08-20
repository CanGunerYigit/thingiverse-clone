import React from "react";
import { Link } from "react-router-dom";

export default function CommentsTab({
  comments,
  loading,
  token,
  newComment,
  setNewComment,
  itemId,
  refreshComments,
}) {
  const handleAddComment = async (e) => {
    e.preventDefault();
    if (!newComment.trim()) return;

    const token = localStorage.getItem("token");
    try {
      const res = await fetch(`https://localhost:7267/api/Comment/AddComment`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          itemId: parseInt(itemId),
          message: newComment,
        }),
      });

      if (!res.ok) {
        const errorMsg = await res.text();
        alert(`Yorum eklenemedi: ${errorMsg}`);
        return;
      }

      setNewComment("");
      refreshComments();
    } catch (err) {
      console.error("Yorum ekleme hatası:", err);
    }
  };

  return (
    <div className="p-4 bg-white rounded-lg">
      <h2 className="text-xl font-semibold mb-3">Yorumlar</h2>
      {loading ? (
        <p>Yorumlar yükleniyor...</p>
      ) : comments.length === 0 ? (
        <p>Henüz yorum yapılmamış.</p>
      ) : (
        <ul className="space-y-3">
          {comments.map((comment) => {
            const key = comment.Id || `${comment.UserId}_${comment.CreatedAt}`;
            return (
              <li
                key={key}
                className="border p-3 rounded-lg bg-gray-50 hover:bg-gray-100 transition"
              >
                <Link
                  to={`/user/${comment.UserId}`}
                  className="font-medium text-blue-600 hover:underline"
                >
                  {comment.UserName || "Unknown"}
                </Link>
                <p className="text-gray-700 mt-1">{comment.Message || ""}</p>
                <small className="text-gray-500">
                  {comment.CreatedAt
                    ? new Date(comment.CreatedAt).toLocaleString("tr-TR")
                    : "Tarih yok"}
                </small>
              </li>
            );
          })}
        </ul>
      )}

      {token ? (
        <form onSubmit={handleAddComment} className="mt-6 flex gap-2">
          <input
            type="text"
            value={newComment}
            onChange={(e) => setNewComment(e.target.value)}
            placeholder="Yorumunuzu yazın..."
            className="flex-1 border rounded-lg p-2"
          />
          <button
            type="submit"
            className="bg-blue-600 text-white px-4 rounded-lg hover:bg-blue-700"
          >
            Gönder
          </button>
        </form>
      ) : (
        <p className="mt-4 text-gray-500">Yorum yapmak için giriş yapmalısınız.</p>
      )}
    </div>
  );
}
