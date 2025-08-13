export default function UserProfile({ user, commentsCount, likesCount }) {
  return (
    <div className="bg-white rounded-lg shadow-md p-6 mb-8">
      <div className="flex items-center space-x-6">
        <div className="bg-gray-200 rounded-full w-20 h-20 flex items-center justify-center text-2xl font-bold text-gray-600">
          {user?.userName.charAt(0).toUpperCase()}
        </div>
        <div>
          <h1 className="text-3xl font-bold text-gray-800">{user?.userName}</h1>
          <p className="text-gray-600">{user?.email}</p>
          <div className="flex space-x-4 mt-2">
            <span className="text-sm bg-blue-100 text-blue-800 px-3 py-1 rounded-full">
              {commentsCount} comments
            </span>
            <span className="text-sm bg-green-100 text-green-800 px-3 py-1 rounded-full">
              {likesCount} likes
            </span>
          </div>
        </div>
      </div>
    </div>
  );
}
