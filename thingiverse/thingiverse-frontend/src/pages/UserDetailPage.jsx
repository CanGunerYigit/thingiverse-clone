import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import UserProfile from "../components/UserDetail/UserProfile";
import UserComments from "../components/UserDetail/UserComments";
import UserLikes from "../components/UserDetail/UserLikes";

export default function UserDetail() {
  const { id } = useParams();
  const [user, setUser] = useState(null);
  const [comments, setComments] = useState([]);
  const [likes, setLikes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [notFound, setNotFound] = useState(false);

  const currentUserId = localStorage.getItem("userId"); 

  const fetchUserData = async () => {
    setLoading(true);
    setNotFound(false);

    // Burada userIdToFetch'i tanımlıyoruz
    const userIdToFetch = id === currentUserId ? currentUserId : id;

    try {
      const userRes = await fetch(`https://localhost:7267/api/User/${userIdToFetch}`);
      if (!userRes.ok) throw new Error("User not found");
      const userData = await userRes.json();
      setUser(userData);

      // Navbar username'i güncelle (eğer prop geçtiyse)
      if (currentUserId === userData.id && typeof setUsername === "function") {
        setUsername(userData.userName);
      }

      const [commentsRes, likesRes] = await Promise.all([
        fetch(`https://localhost:7267/api/Comment/user/${userIdToFetch}`),
        fetch(`https://localhost:7267/api/Like/user/${userIdToFetch}`)
      ]);

      const commentsData = commentsRes.ok ? await commentsRes.json() : [];
      const likesData = likesRes.ok ? await likesRes.json() : [];

      setComments(commentsData);
      setLikes(likesData);

    } catch (err) {
      console.error(err);
      setNotFound(true);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUserData();
  }, [id]);

  if (loading) return <div className="flex justify-center items-center h-screen">
    <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-blue-500"></div>
  </div>;

  if (notFound) return <div className="flex flex-col items-center justify-center h-screen text-red-500">
    <h1 className="text-2xl font-bold">User not found</h1>
  </div>;

  // Şu satır çok önemli: string karşılaştırması kesinlikle eşleşmeli
const isCurrentUser = user?.id?.toString() === currentUserId?.toString();

  return (
    
  <div className="container mx-auto px-4 py-8 max-w-6xl">
    {console.log(
      "isCurrentUser:",
      currentUserId === user.id,
      "user.id:",
      user.id,
      "localStorageId:",
      localStorage.getItem("userId")
    )}
    <UserProfile
  user={user}
  commentsCount={comments.length}
  likesCount={likes.length}
  isCurrentUser={currentUserId === user.id}
  onProfileUpdated={fetchUserData}
/>
    <UserComments comments={comments} />
    <UserLikes likes={likes} />
  </div>
);

  
}
