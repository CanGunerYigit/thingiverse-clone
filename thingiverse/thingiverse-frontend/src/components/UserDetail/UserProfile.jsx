import { useState, useEffect } from "react";
import defaultAvatar from "../../assets/default-avatar.png";

export default function UserProfile({ user, commentsCount, likesCount, isCurrentUser, onProfileUpdated }) {
  const [username, setUsername] = useState(user.UserName || "");
  const [profileImage, setProfileImage] = useState(null);
  const [loading, setLoading] = useState(false);
  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [passwordLoading, setPasswordLoading] = useState(false);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(null);
const [openDropdown, setOpenDropdown] = useState(false);

  useEffect(() => {
    setUsername(user.UserName || "");
  }, [user]);

  const getImageUrl = () => {
    if (!user?.ProfileImageUrl) return defaultAvatar;
    
    // URL zaten tam ise
    if (user.ProfileImageUrl.startsWith('http')) return user.ProfileImageUrl;
    
    // Base URL ve cache busting
    return `https://localhost:7267${
      user.ProfileImageUrl.startsWith('/') 
        ? user.ProfileImageUrl 
        : `/${user.ProfileImageUrl}`
    }?t=${Date.now()}`;
  };

  const handleFileChange = (e) => {
    if (e.target.files?.[0]) {
      const file = e.target.files[0];
      
      // Dosya validasyonu
      const validTypes = ['image/jpeg', 'image/png', 'image/jpg'];
      if (!validTypes.includes(file.type)) {
        setError('Sadece JPEG/PNG dosyaları yükleyebilirsiniz');
        return;
      }
      
      if (file.size > 5 * 1024 * 1024) {
        setError('Dosya boyutu 5MB\'den büyük olamaz');
        return;
      }
      
      setProfileImage(file);
      setError(null);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    setSuccess(null);

    try {
      const formData = new FormData();
      formData.append("UserName", username);
      if (profileImage) formData.append("ProfileImage", profileImage);

      const token = localStorage.getItem("token");
      const res = await fetch("https://localhost:7267/api/Account/update-profile", {
        method: "PUT",
        headers: { 
          Authorization: `Bearer ${token}`,
        },
        body: formData,
      });

      const data = await res.json();

      if (!res.ok) {
        throw new Error(data.message || "Profil güncellenemedi");
      }

      // Backend'den gelen veriyi normalize et
      const updatedUser = {
        ...user,
        UserName: data.UserName,
        ProfileImageUrl: data.ProfileImageUrl
      };

      localStorage.setItem("user", JSON.stringify(updatedUser));
      window.dispatchEvent(new Event("userUpdated"));
      onProfileUpdated();
      setSuccess("Profil başarıyla güncellendi!");
      
      // Resmi önbellekten temizle
      if (data.ProfileImageUrl) {
        new Image().src = getImageUrl();
      }
    } catch (err) {
      console.error("Profil güncelleme hatası:", err);
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const handlePasswordChange = async (e) => {
    e.preventDefault();
    setPasswordLoading(true);
    setError(null);
    setSuccess(null);

    try {
      if (newPassword !== confirmPassword) {
        throw new Error("Yeni şifre ve onay şifresi aynı olmalı");
      }

      const token = localStorage.getItem("token");
      const res = await fetch("https://localhost:7267/api/Account/change-password", {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          CurrentPassword: currentPassword,
          NewPassword: newPassword,
          ConfirmPassword: confirmPassword
        }),
      });

      const data = await res.json();

      if (!res.ok) {
        throw new Error(data.message || "Şifre güncellenemedi");
      }

      setCurrentPassword("");
      setNewPassword("");
      setConfirmPassword("");
      setSuccess(data.message || "Şifre başarıyla güncellendi!");
    } catch (err) {
      console.error("Şifre güncelleme hatası:", err);
      setError(err.message);
    } finally {
      setPasswordLoading(false);
    }
  };

  return (
    <div className="max-w-3xl mx-auto space-y-8">
      {/* Profil Kartı */}
      <div className="bg-white shadow-lg rounded-2xl p-6 flex flex-col md:flex-row items-center md:items-start space-y-4 md:space-y-0 md:space-x-6">
        {user?.ProfileImageUrl ? (
  <img
    src={getImageUrl()}
    alt="Profile"
    className="w-28 h-28 rounded-full object-cover "
    onError={(e) => { 
      e.target.onerror = null; 
      e.target.src = defaultAvatar;
    }}
  />
) : (
  <div className="w-28 h-28 rounded-full flex items-center justify-center bg-gray-300">
    <span className="text-3xl font-bold text-white">
      {user?.UserName?.[0]?.toUpperCase() || "?"}
    </span>
  </div>
)}


        <div className="flex-1">
          <h2 className="text-2xl font-bold">{user.UserName}</h2>
          <p className="text-gray-600">{user.Email}</p>
          <p className="text-gray-500 mt-2">{commentsCount} Comments · {likesCount} Likes</p>
        </div>
      </div>

      {/* Hata ve Başarı Mesajları */}
      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative">
          {error}
        </div>
      )}
      {success && (
        <div className="bg-green-100 border border-green-400 text-green-700 px-4 py-3 rounded relative">
          {success}
        </div>
      )}

      {/* Profil Güncelleme Dropdown */}
{isCurrentUser && (
  <div className="bg-white shadow-lg rounded-2xl p-6 space-y-6">
    <button
      onClick={() => setOpenDropdown(!openDropdown)}
      className="w-full flex justify-between items-center bg-gray-100 px-4 py-2 rounded-lg text-gray-700 font-semibold "
    >
      Profil Bilgilerini Güncelle
      <svg
        className={`w-5 h-5 transform transition-transform ${
          openDropdown ? "rotate-180" : ""
        }`}
        fill="none"
        stroke="currentColor"
        viewBox="0 0 24 24"
        xmlns="http://www.w3.org/2000/svg"
      >
        <path
          strokeLinecap="round"
          strokeLinejoin="round"
          strokeWidth={2}
          d="M19 9l-7 7-7-7"
        />
      </svg>
    </button>

    {openDropdown && (
      <div className="mt-4 space-y-6">
        {/* Profil Güncelleme Formu */}
        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700">
              Kullanıcı Adı
            </label>
            <input
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              className="mt-1 block w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-blue-500 focus:border-blue-500"
              required
              minLength={3}
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700">
              Profil Fotoğrafı
            </label>
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
          <h3 className="text-xl font-semibold text-gray-700 mb-4">
            Şifre Güncelle
          </h3>
          <form onSubmit={handlePasswordChange} className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-700">
                Mevcut Şifre
              </label>
              <input
                type="password"
                value={currentPassword}
                onChange={(e) => setCurrentPassword(e.target.value)}
                className="mt-1 block w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-red-500 focus:border-red-500"
                required
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700">
                Yeni Şifre
              </label>
              <input
                type="password"
                value={newPassword}
                onChange={(e) => setNewPassword(e.target.value)}
                className="mt-1 block w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-red-500 focus:border-red-500"
                required
                minLength={8}
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700">
                Yeni Şifre (Tekrar)
              </label>
              <input
                type="password"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                className="mt-1 block w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-red-500 focus:border-red-500"
                required
                minLength={8}
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
)}

    </div>
  );
}