import { useState, useEffect } from "react";

export default function UserProfile({ user, commentsCount, likesCount, isCurrentUser, onProfileUpdated }) {
  const [username, setUsername] = useState(user.userName || "");
  const [profileImage, setProfileImage] = useState(null);
  const [loading, setLoading] = useState(false);

  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [passwordLoading, setPasswordLoading] = useState(false);

  useEffect(() => {
    setUsername(user.userName || "");
  }, [user]);

  const handleFileChange = (e) => setProfileImage(e.target.files[0]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      const formData = new FormData();
      formData.append("UserName", username);
      if (profileImage) formData.append("ProfileImage", profileImage);

      const token = localStorage.getItem("token");
      const res = await fetch("https://localhost:7267/api/Account/update-profile", {
        method: "PUT",
        headers: { Authorization: `Bearer ${token}` },
        body: formData,
      });

      if (!res.ok) throw new Error("Profil güncellenemedi");

      const updatedUser = await res.json();
      localStorage.setItem("user", JSON.stringify(updatedUser));
      window.dispatchEvent(new Event("userUpdated"));
      onProfileUpdated();
    } catch (err) {
      console.error(err);
      alert("Profil güncellenemedi");
    } finally {
      setLoading(false);
    }
  };

  const handlePasswordChange = async (e) => {
    e.preventDefault();
    if (newPassword !== confirmPassword) {
      alert("Yeni şifre ve onay şifresi uyuşmuyor!");
      return;
    }

    setPasswordLoading(true);
    try {
      const token = localStorage.getItem("token");
      const payload = {
        CurrentPassword: currentPassword,
        NewPassword: newPassword,
        ConfirmPassword: confirmPassword
      };

      const res = await fetch("https://localhost:7267/api/Account/change-password", {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`
        },
        body: JSON.stringify(payload),
      });

      if (!res.ok) {
        const errData = await res.json().catch(() => null);
        console.error("Backend validation error:", errData);
        throw new Error(errData?.message || "Şifre güncellenemedi");
      }

      alert("Şifre başarıyla güncellendi!");
      setCurrentPassword("");
      setNewPassword("");
      setConfirmPassword("");
    } catch (err) {
      console.error(err);
      alert(err.message);
    } finally {
      setPasswordLoading(false);
    }
  };

  return (
    <div className="max-w-3xl mx-auto space-y-8">
      {/* Profil Kartı */}
      <div className="bg-white shadow-lg rounded-2xl p-6 flex flex-col md:flex-row items-center md:items-start space-y-4 md:space-y-0 md:space-x-6">
        {user.profileImageUrl ? (
          <img
            src={`https://localhost:7267${user.profileImageUrl}`}
            alt="Profile"
            className="w-28 h-28 rounded-full object-cover border-2 border-blue-500"
          />
        ) : (
          <div className="w-28 h-28 rounded-full bg-blue-500 flex items-center justify-center text-white text-4xl font-bold border-2 border-blue-500">
            {user.userName ? user.userName.charAt(0).toUpperCase() : "?"}
          </div>
        )}

        <div className="flex-1">
          <h2 className="text-2xl font-bold">{user.userName}</h2>
          <p className="text-gray-600">{user.email}</p>
          <p className="text-gray-500 mt-2">{commentsCount} Comments · {likesCount} Likes</p>
        </div>
      </div>

      {/* Profil Güncelleme Formu */}
      {isCurrentUser && (
        <div className="bg-white shadow-lg rounded-2xl p-6 space-y-6">
          <h3 className="text-xl font-semibold text-gray-700">Profil Bilgilerini Güncelle</h3>
          <form onSubmit={handleSubmit} className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-700">Kullanıcı Adı</label>
              <input
                type="text"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                className="mt-1 block w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-blue-500 focus:border-blue-500"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700">Profil Fotoğrafı</label>
              <input
                type="file"
                accept="image/*"
                onChange={handleFileChange}
                className="mt-1 block w-full text-gray-600"
              />
            </div>

            <button
              type="submit"
              disabled={loading}
              className="bg-blue-500 text-white px-4 py-2 rounded-lg hover:bg-blue-600 disabled:opacity-50"
            >
              {loading ? "Güncelleniyor..." : "Profili Güncelle"}
            </button>
          </form>

          {/* Şifre Güncelleme */}
          <div className="mt-8 border-t border-gray-200 pt-6">
            <h3 className="text-xl font-semibold text-gray-700 mb-4">Şifre Güncelle</h3>
            <form onSubmit={handlePasswordChange} className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700">Mevcut Şifre</label>
                <input
                  type="password"
                  value={currentPassword}
                  onChange={(e) => setCurrentPassword(e.target.value)}
                  className="mt-1 block w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-red-500 focus:border-red-500"
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700">Yeni Şifre</label>
                <input
                  type="password"
                  value={newPassword}
                  onChange={(e) => setNewPassword(e.target.value)}
                  className="mt-1 block w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-red-500 focus:border-red-500"
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700">Yeni Şifre (Tekrar)</label>
                <input
                  type="password"
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                  className="mt-1 block w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-red-500 focus:border-red-500"
                />
              </div>

              <button
                type="submit"
                disabled={passwordLoading}
                className="bg-red-500 text-white px-4 py-2 rounded-lg hover:bg-red-600 disabled:opacity-50"
              >
                {passwordLoading ? "Güncelleniyor..." : "Şifreyi Güncelle"}
              </button>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
