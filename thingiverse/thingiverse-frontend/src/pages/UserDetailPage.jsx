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

  useEffect(() => {
    setLoading(true);
    setNotFound(false);

    Promise.all([
      fetch(`https://localhost:7267/api/User/${id}`).then((res) => {
        if (!res.ok) throw new Error("User not found");
        return res.json();
      }),
      fetch(`https://localhost:7267/api/Comment/user/${id}`).then((res) => {
        if (!res.ok) throw new Error("Comments not found");
        return res.json();
      }),
      fetch(`https://localhost:7267/api/Like/user/${id}`).then((res) => {
        if (!res.ok) throw new Error("Likes not found");
        return res.json();
      }),
    ])
      .then(([userData, commentsData, likesData]) => {
        setUser(userData);
        setComments(commentsData);
        setLikes(likesData);
      })
      .catch((err) => {
        console.error(err);
        setNotFound(true);
      })
      .finally(() => setLoading(false));
  }, [id]);

  if (loading)
    return (
      <div className="flex justify-center items-center h-screen">
        <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-blue-500"></div>
      </div>
    );

  if (notFound)
    return (
      <div className="flex flex-col items-center justify-center h-screen text-red-500">
        <svg
          xmlns="http://www.w3.org/2000/svg"
          className="h-16 w-16 mb-4"
          fill="none"
          viewBox="0 0 24 24"
          stroke="currentColor"
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            strokeWidth={2}
            d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
          />
        </svg>
        <h1 className="text-2xl font-bold">User not found</h1>
        <p className="text-gray-600 mt-2">The requested user does not exist</p>
      </div>
    );

  if (!user) return <div className="text-center mt-10">Loading user data...</div>;

  return (
    <div className="container mx-auto px-4 py-8 max-w-6xl">
      <UserProfile user={user} commentsCount={comments.length} likesCount={likes.length} />
      <UserComments comments={comments} />
      <UserLikes likes={likes} />
    </div>
  );
}
