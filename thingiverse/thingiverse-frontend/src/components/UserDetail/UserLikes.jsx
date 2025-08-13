export default function UserLikes({ likes }) {
  if (likes.length === 0) {
    return (
      <div className="bg-white rounded-lg shadow-md p-6 text-center py-8">
        <svg
          xmlns="http://www.w3.org/2000/svg"
          className="h-12 w-12 mx-auto text-gray-400"
          fill="none"
          viewBox="0 0 24 24"
          stroke="currentColor"
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            strokeWidth={2}
            d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z"
          />
        </svg>
        <p className="mt-2 text-gray-500">This user hasn't liked any items yet</p>
      </div>
    );
  }

  return (
    <div className="bg-white rounded-lg shadow-md p-6">
      <h2 className="text-2xl font-bold text-gray-800 mb-6 pb-2 border-b border-gray-200">
        Liked Items
        <span className="ml-2 text-sm font-normal text-gray-500">({likes.length})</span>
      </h2>

      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
        {likes.map((like) => (
          <div
            key={like.id}
            className="border border-gray-200 rounded-lg overflow-hidden hover:shadow-lg transition-shadow cursor-pointer"
            onClick={() => (window.location.href = `/item/${like.item.id}`)}
          >
            <div className="aspect-square bg-gray-100">
              <img
                src={like.item.thumbnail}
                alt={like.item.name}
                className="w-full h-full object-cover"
              />
            </div>
            <div className="p-3">
              <h3 className="font-medium text-gray-800 truncate">{like.item.name}</h3>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
